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
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class AssertFunctionElementValidator : IAssertFunctionElementValidator
    {
        private readonly IAssertFunctionDataParser _assertFunctionDataParser;
        private readonly IConfigurationService _configurationService;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IVariableValueDataParser _variableValueDataParser;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public AssertFunctionElementValidator(
            IAssertFunctionDataParser assertFunctionDataParser,
            IConfigurationService configurationService,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            ITypeLoadHelper typeLoadHelper,
            IVariableDataParser variableDataParser,
            IVariableValueDataParser variableValueDataParser,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _assertFunctionDataParser = assertFunctionDataParser;
            _configurationService = configurationService;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _typeLoadHelper = typeLoadHelper;
            _variableDataParser = variableDataParser;
            _variableValueDataParser = variableValueDataParser;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private ILiteralVariableElementValidator? _literalVariableElementValidator;
		private ILiteralVariableElementValidator LiteralVariableElementValidator => _literalVariableElementValidator ??= _xmlElementValidatorFactory.GetLiteralVariableElementValidator();
        private IObjectVariableElementValidator? _objectVariableElementValidator;
		private IObjectVariableElementValidator ObjectVariableElementValidator => _objectVariableElementValidator ??= _xmlElementValidatorFactory.GetObjectVariableElementValidator();
        private ILiteralListVariableElementValidator? _literalListVariableElementValidator;
		private ILiteralListVariableElementValidator LiteralListVariableElementValidator => _literalListVariableElementValidator ??= _xmlElementValidatorFactory.GetLiteralListVariableElementValidator();
        private IObjectListVariableElementValidator? _objectListVariableElementValidator;
		private IObjectListVariableElementValidator ObjectListVariableElementValidator => _objectListVariableElementValidator ??= _xmlElementValidatorFactory.GetObjectListVariableElementValidator();

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

            if (_enumHelper.GetVariableTypeCategory(variableValueData.ChildElement.Name) != variable.VariableTypeCategory)
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidVariableElementFormat, variable.Name, _enumHelper.GetVisibleEnumText(variable.VariableTypeCategory)));
                return;
            }

            switch (variable)
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
