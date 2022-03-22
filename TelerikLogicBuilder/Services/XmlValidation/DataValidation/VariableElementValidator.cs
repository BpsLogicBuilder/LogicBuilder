using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
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
    internal class VariableElementValidator : IVariableElementValidator
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IConfigurationService _configurationService;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariableDataParser _variableDataParser;

        public VariableElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _variableDataParser = xmlElementValidator.VariableDataParser;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
            _configurationService = xmlElementValidator.ContextProvider.ConfigurationService;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _typeHelper = xmlElementValidator.ContextProvider.TypeHelper;
        }

        public void Validate(XmlElement variableElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (variableElement.Name != XmlDataConstants.VARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{0649E098-8CDC-4551-9577-40A735EF35B9}");

            Validate
            (
                _variableDataParser.Parse(variableElement),
                assignedTo,
                application,
                validationErrors
            );
        }

        private void Validate(VariableData variableData, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (!_configurationService.VariableList.Variables.TryGetValue(variableData.Name, out VariableBase? variable))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotEvaluateVariableFormat, variableData.Name));
                return;
            }

            _typeLoadHelper.TryGetSystemType(variable, application, out Type? variableType);

            if (variable.CastVariableAs != MiscellaneousConstants.TILDE
                && !string.IsNullOrEmpty(variable.CastVariableAs)
                && variableType == null)
            {
                validationErrors.Add
                (
                    string.Format
                    (
                        Strings.cannotLoadCastAsVariableTypeFormat, 
                        variable.CastVariableAs, 
                        variable.Name
                    )
                );
                return;
            }

            if (variableType == null || !_typeHelper.AssignableFrom(assignedTo, variableType))
            {
                validationErrors.Add
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.variableNotAssignableFormat,
                        variable.Name,
                        assignedTo.ToString()
                    )
                );
            }
        }
    }
}
