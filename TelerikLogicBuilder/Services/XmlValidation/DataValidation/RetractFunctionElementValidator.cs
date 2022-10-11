using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
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
    internal class RetractFunctionElementValidator : IRetractFunctionElementValidator
    {
        private readonly IConfigurationService _configurationService;
        private readonly ITypeHelper _typeHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRetractFunctionDataParser _retractFunctionDataParser;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariableDataParser _variableDataParser;

        public RetractFunctionElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _configurationService = xmlElementValidator.ConfigurationService;
            _typeHelper = xmlElementValidator.TypeHelper;
            _exceptionHelper = xmlElementValidator.ExceptionHelper;
            _retractFunctionDataParser = xmlElementValidator.RetractFunctionDataParser;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
            _variableDataParser = xmlElementValidator.VariableDataParser;
        }

        public void Validate(XmlElement functionElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (functionElement.Name != XmlDataConstants.RETRACTFUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{D3603215-F74A-46A4-9E7E-C5C2217D92D7}");

            RetractFunctionData functionData = _retractFunctionDataParser.Parse(functionElement);
            if (!_configurationService.FunctionList.Functions.TryGetValue(functionData.Name, out Function? function))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, functionData.Name));
                return;
            }

            if (function.FunctionCategory != FunctionCategories.Retract)
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidFunctionCategoryFormat, function.Name, Enum.GetName(typeof(FunctionCategories), function.FunctionCategory), functionElement.Name));
                return;
            }

            ValidateVariable
            (
                _variableDataParser.Parse(functionData.VariableElement),
                application,
                validationErrors
            );
        }

        private void ValidateVariable(VariableData variableData, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (!_configurationService.VariableList.Variables.TryGetValue(variableData.Name, out VariableBase? variable))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotEvaluateVariableFormat, variableData.Name));
                return;
            }

            if (!_typeLoadHelper.TryGetSystemType(variable, application, out Type? variableType))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForVariableFormat, variable.ObjectTypeString, variable.Name));
                return;
            }

            if (variableType.IsValueType && !_typeHelper.IsNullable(variableType))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotSetValueTypedVariableToNullFormat, variable.Name));
                return;
            }
        }
    }
}
