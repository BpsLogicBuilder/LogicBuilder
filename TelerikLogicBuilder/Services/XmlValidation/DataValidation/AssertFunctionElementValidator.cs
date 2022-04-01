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
    internal class AssertFunctionElementValidator : IAssertFunctionElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IAssertFunctionDataParser _assertFunctionDataParser;
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IVariableValueDataParser _variableValueDataParser;

        public AssertFunctionElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _configurationService = xmlElementValidator.ContextProvider.ConfigurationService;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _assertFunctionDataParser = xmlElementValidator.AssertFunctionDataParser;
            _typeLoadHelper = xmlElementValidator.TypeLoadHelper;
            _variableDataParser = xmlElementValidator.VariableDataParser;
            _variableValueDataParser = xmlElementValidator.VariableValueDataParser;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor.
        //These fields may be null in the constructor i.e. when new AssertFunctionElementValidator((XmlElementValidator)this) runs
        //therefore they must be properties.
        private ILiteralVariableElementValidator LiteralVariableElementValidator => _xmlElementValidator.LiteralVariableElementValidator;
        private IObjectVariableElementValidator ObjectVariableElementValidator => _xmlElementValidator.ObjectVariableElementValidator;
        private ILiteralListVariableElementValidator LiteralListVariableElementValidator => _xmlElementValidator.LiteralListVariableElementValidator;
        private IObjectListVariableElementValidator ObjectListVariableElementValidator => _xmlElementValidator.ObjectListVariableElementValidator;

        public void Validate(XmlElement functionElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (functionElement.Name != XmlDataConstants.ASSERTFUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{CAB00483-7D5A-43D1-A2D5-D6CD6FFF0954}");

            AssertFunctionData functionData = _assertFunctionDataParser.Parse(functionElement);
            if (!_configurationService.FunctionList.Functions.TryGetValue(functionData.Name, out Function? function))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, functionData.Name));
                return;
            }

            if (function.FunctionCategory != FunctionCategories.Assert)
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidFunctionCategoryFormat, function.Name, Enum.GetName(typeof(FunctionCategories), function.FunctionCategory), functionElement.Name));
                return;
            }

            ValidateVariable
            (
                _variableDataParser.Parse(functionData.VariableElement),
                _variableValueDataParser.Parse(functionData.VariableValueElement),
                application,
                validationErrors
            );
        }

        private void ValidateVariable(VariableData variableData, VariableValueData variableValueData, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (!_configurationService.VariableList.Variables.TryGetValue(variableData.Name, out VariableBase? variable))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotEvaluateVariableFormat, variableData.Name));
                return;
            }

            if (!_typeLoadHelper.TryGetSystemType(variable, application, out Type? _))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForVariableFormat, variable.ObjectTypeString, variable.Name));
                return;
            }

            switch(variable)
            {
                case LiteralVariable literalVariable:
                    LiteralVariableElementValidator.Validate(variableValueData.ChildElement, literalVariable, application, validationErrors);
                    break;
                case ObjectVariable objectVariable:
                    ObjectVariableElementValidator.Validate(variableValueData.ChildElement, objectVariable, application, validationErrors);
                    break;
                case ListOfLiteralsVariable listOfLiteralsVariable:
                    LiteralListVariableElementValidator.Validate(variableValueData.ChildElement, listOfLiteralsVariable, application, validationErrors);
                    break;
                case ListOfObjectsVariable listOfObjectsVariable:
                    ObjectListVariableElementValidator.Validate(variableValueData.ChildElement, listOfObjectsVariable, application, validationErrors);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{C7F29579-F6BF-46E2-A2D4-95D141A03A66}");
            }
        }
    }
}
