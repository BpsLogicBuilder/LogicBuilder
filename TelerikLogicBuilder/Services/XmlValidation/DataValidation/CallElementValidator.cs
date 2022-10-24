using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class CallElementValidator : ICallElementValidator
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public CallElementValidator(
            IExceptionHelper exceptionHelper,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _exceptionHelper = exceptionHelper;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private IConstructorElementValidator? _constructorElementValidator;
		private IConstructorElementValidator ConstructorElementValidator => _constructorElementValidator ??= _xmlElementValidatorFactory.GetConstructorElementValidator();
        private IFunctionElementValidator? _functionElementValidator;
		private IFunctionElementValidator FunctionElementValidator => _functionElementValidator ??= _xmlElementValidatorFactory.GetFunctionElementValidator();
        private ILiteralListElementValidator? _literalListElementValidator;
		private ILiteralListElementValidator LiteralListElementValidator => _literalListElementValidator ??= _xmlElementValidatorFactory.GetLiteralListElementValidator();
        private IObjectListElementValidator? _objectListElementValidator;
		private IObjectListElementValidator ObjectListElementValidator => _objectListElementValidator ??= _xmlElementValidatorFactory.GetObjectListElementValidator();
        private IVariableElementValidator? _variableElementValidator;
		private IVariableElementValidator VariableElementValidator => _variableElementValidator ??= _xmlElementValidatorFactory.GetVariableElementValidator();

        public void Validate(XmlElement callElement, Type assignedTo, ApplicationTypeInfo application, List<string> validationErrors)
        {
            switch (callElement.Name)
            {
                case XmlDataConstants.CONSTRUCTORELEMENT:
                    ConstructorElementValidator.Validate(callElement, assignedTo, application, validationErrors);
                    break;
                case XmlDataConstants.FUNCTIONELEMENT:
                    FunctionElementValidator.Validate(callElement, assignedTo, application, validationErrors);
                    break;
                case XmlDataConstants.VARIABLEELEMENT:
                    VariableElementValidator.Validate(callElement, assignedTo, application, validationErrors);
                    break;
                case XmlDataConstants.LITERALLISTELEMENT:
                    LiteralListElementValidator.Validate(callElement, assignedTo, application, validationErrors);
                    break;
                case XmlDataConstants.OBJECTLISTELEMENT:
                    ObjectListElementValidator.Validate(callElement, assignedTo, application, validationErrors);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{781FCE1F-69DE-412D-BF75-6924AE7B0B71}");
            }
        }
    }
}
