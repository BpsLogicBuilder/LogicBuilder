using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        //private readonly fields were injected into XmlElementValidator

        public FunctionElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _functionDataParser = xmlElementValidator.FunctionDataParser;
            _functionGenericsConfigrationValidator = xmlElementValidator.FunctionGenericsConfigrationValidator;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
            _configurationService = xmlElementValidator.ContextProvider.ConfigurationService;
            _enumHelper = xmlElementValidator.ContextProvider.EnumHelper;
            _typeHelper = xmlElementValidator.ContextProvider.TypeHelper;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor.
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
        }
    }
}
