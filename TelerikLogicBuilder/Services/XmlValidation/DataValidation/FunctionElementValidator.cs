using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class FunctionElementValidator : IFunctionElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionGenericsConfigrationValidator _functionGenericsConfigrationValidator;
        private readonly IConfigurationService _configurationService;
        private readonly IEnumHelper _enumHelper;
        private readonly IGenericFunctionHelper _genericFunctionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        //private readonly fields were injected into XmlElementValidator

        public FunctionElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _functionDataParser = xmlElementValidator.FunctionDataParser;
            _functionGenericsConfigrationValidator = xmlElementValidator.FunctionGenericsConfigrationValidator;
            _genericFunctionHelper = xmlElementValidator.GenericFunctionHelper;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
            _configurationService = xmlElementValidator.ContextProvider.ConfigurationService;
            _enumHelper = xmlElementValidator.ContextProvider.EnumHelper;
            _typeHelper = xmlElementValidator.ContextProvider.TypeHelper;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor.
        //These fields may be null in the constructor i.e. when new FunctionElementValidator((XmlElementValidator)this)
        //therefore they must be properties.
        private IParameterElementValidator ParameterElementValidator => _xmlElementValidator.ParameterElementValidator;

        public void Validate(XmlElement functionElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            Validate
            (
                _functionDataParser.Parse(functionElement),
                assignedTo,
                application,
                validationErrors
            );
        }

        private void Validate(FunctionData functionData, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (!_configurationService.FunctionList.Functions.TryGetValue(functionData.Name, out Function? function))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, functionData.Name));
                return;
            }

            //ensure function data is consistent with the function with regard to generics and
            //make sure the function is a static function and a generic class if function has generic arguments
            if (!_functionGenericsConfigrationValidator.Validate(function, functionData.GenericArguments, application, validationErrors))
                return;

            //now if applicable, convert the generic types to non-genric types
            if (function.HasGenericArguments)
            {
                foreach (GenericConfigBase ga in functionData.GenericArguments)
                {
                    if (!_typeLoadHelper.TryGetSystemType(ga, application, out Type? _))
                    {
                        validationErrors.Add
                        (
                            string.Format
                            (
                                Strings.cannotLoadTypeForGenericArgumentForFunctionFormat,
                                ga.GenericArgumentName, 
                                functionData.Name
                            )
                        );

                        return;
                    };
                }

                function = _genericFunctionHelper.ConvertGenericTypes(function, functionData.GenericArguments, application);
            }

            if (!_typeLoadHelper.TryGetSystemType(function.ReturnType, Array.Empty<GenericConfigBase>(), application, out Type? functionType))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForFunctionFormat, function.ReturnType.Description, function.Name));
                return;
            }

            //finally check that the function is assignable to the object it is being assigned to
            if (!_typeHelper.AssignableFrom(assignedTo, functionType))
            {
                validationErrors.Add
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.functionNotAssignableFormat,
                        function.Name,
                        assignedTo.ToString()
                    )
                );

                return;
            }
        }

        private void ValidateParameters(Function function, List<XmlElement> parameterElementsList, ApplicationTypeInfo application, List<string> validationErrors)
        {
            Dictionary<string, XmlElement> elements = parameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE));

            function.Parameters.ForEach(par =>
            {
                if (!elements.TryGetValue(par.Name, out XmlElement? pElement))
                {
                    if (!par.IsOptional)
                        validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.parameterNotOptionalFormat, par.Name, function.Name));
                    return;
                }

                switch (par.ParameterCategory)
                {
                    case ParameterCategory.Object:
                    case ParameterCategory.LiteralList:
                    case ParameterCategory.ObjectList:
                        if (!pElement.HasChildNodes)//this should never happen.  The UI will always add a child node.  If editing XML then the schema validator should fail.
                        {
                            validationErrors.Add
                            (
                                string.Format
                                (
                                    CultureInfo.CurrentCulture, 
                                    Strings.invalidParameterElementFormat, 
                                    par.Name, 
                                    _enumHelper.GetVisibleEnumText(par.ParameterCategory)
                                )
                            );
                            return;
                        }
                        break;
                    default:
                        break;
                }

                if (_enumHelper.GetParameterCategory(pElement.Name) != par.ParameterCategory)
                {
                    validationErrors.Add
                    (
                        string.Format
                        (
                            CultureInfo.CurrentCulture, 
                            Strings.invalidParameterElementFormat, 
                            par.Name, 
                            _enumHelper.GetVisibleEnumText(par.ParameterCategory)
                        )
                    );
                    return;
                }

                ParameterElementValidator.Validate(pElement, par, application, validationErrors);
            });
        }
    }
}
