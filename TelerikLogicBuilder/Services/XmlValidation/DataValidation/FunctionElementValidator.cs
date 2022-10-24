using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class FunctionElementValidator : IFunctionElementValidator
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionGenericsConfigrationValidator _functionGenericsConfigrationValidator;
        private readonly IGenericFunctionHelper _genericFunctionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariableHelper _variableHelper;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public FunctionElementValidator(
            IConfigurationService configurationService,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IFunctionGenericsConfigrationValidator functionGenericsConfigrationValidator,
            IGenericFunctionHelper genericFunctionHelper,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IVariableHelper variableHelper,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _configurationService = configurationService;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _functionGenericsConfigrationValidator = functionGenericsConfigrationValidator;
            _genericFunctionHelper = genericFunctionHelper;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            _variableHelper = variableHelper;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private IBinaryOperatorFunctionElementValidator? _binaryOperatorFunctionElementValidator;
		private IBinaryOperatorFunctionElementValidator BinaryOperatorFunctionElementValidator => _binaryOperatorFunctionElementValidator ??= _xmlElementValidatorFactory.GetBinaryOperatorFunctionElementValidator();
        private IRuleChainingUpdateFunctionElementValidator? _ruleChainingUpdateFunctionElementValidator;
		private IRuleChainingUpdateFunctionElementValidator RuleChainingUpdateFunctionElementValidator => _ruleChainingUpdateFunctionElementValidator ??= _xmlElementValidatorFactory.GetRuleChainingUpdateFunctionElementValidator();
        private IParametersElementValidator? _parametersElementValidator;
		private IParametersElementValidator ParametersElementValidator => _parametersElementValidator ??= _xmlElementValidatorFactory.GetParametersElementValidator();

        public void Validate(XmlElement functionElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (functionElement.Name != XmlDataConstants.NOTELEMENT && functionElement.Name != XmlDataConstants.FUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{CBDCCFFF-F0B2-43F7-901E-CA6BD4AEB0C6}");

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

            //validate the parameters.
            switch (function.FunctionCategory)
            {
                case FunctionCategories.BinaryOperator:
                    BinaryOperatorFunctionElementValidator.Validate(function, functionData.ParameterElementsList, application, validationErrors);
                    break;
                case FunctionCategories.RuleChainingUpdate:
                    RuleChainingUpdateFunctionElementValidator.Validate(function, functionData.ParameterElementsList, validationErrors);
                    break;
                default:
                    ParametersElementValidator.Validate(function.Parameters, functionData.ParameterElementsList, application, validationErrors);
                    //In case the configured variable has been modified or removed (reference definitions are not applicable for BinaryOperator or RuleChainingUpdate:)
                    ValidateVariableReferenceDefinitions(function, validationErrors);
                    break;
            }
        }

        void ValidateVariableReferenceDefinitions(Function function, List<string> validationErrors)
        {
            for (int i = 0; i < function.ReferenceDefinition.Count; i++)
            {
                if (function.ReferenceDefinition[i] == ValidIndirectReference.VariableKeyIndexer && !_configurationService.VariableList.Variables.ContainsKey(function.ReferenceName[i]))
                {
                    validationErrors.Add
                    (
                        string.Format
                        (
                            Strings.variableKeyReferenceIsInvalidFormat3,
                            _enumHelper.GetVisibleEnumText(function.ReferenceDefinition[i]), 
                            function.ReferenceName[i], 
                            function.Name
                        )
                    );
                }

                if (function.ReferenceDefinition[i] == ValidIndirectReference.VariableArrayIndexer)
                {
                    string[] indexes = function.ReferenceName[i].Trim().Split(new char[] { MiscellaneousConstants.COMMA }, StringSplitOptions.RemoveEmptyEntries);
                    //Since arrays can be multidimensional, allow the variableArrayIndex to combine variables and integer literals as indexes
                    foreach (string index in indexes)
                    {
                        if (_configurationService.VariableList.Variables.TryGetValue(index, out VariableBase? variable) && _variableHelper.CanBeInteger(variable))
                        {
                        }
                        else if (int.TryParse(index, out int arrayIndex) && arrayIndex > -1)//parsed integer must be positive
                        {
                        }
                        else
                        {
                            validationErrors.Add
                            (
                                string.Format
                                (
                                    Strings.variableArrayKeyReferenceIsInvalidFormat3,
                                    _enumHelper.GetVisibleEnumText(function.ReferenceDefinition[i]),
                                    function.ReferenceName[i],
                                    function.Name
                                )
                            );
                        }
                    }
                }
            }
        }
    }
}
