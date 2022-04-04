using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class CallElementValidator : ICallElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IExceptionHelper _exceptionHelper;

        public CallElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private IConstructorElementValidator ConstructorElementValidator => _xmlElementValidator.ConstructorElementValidator;
        private IFunctionElementValidator FunctionElementValidator => _xmlElementValidator.FunctionElementValidator;
        private ILiteralListElementValidator LiteralListElementValidator => _xmlElementValidator.LiteralListElementValidator;
        private IObjectListElementValidator ObjectListElementValidator => _xmlElementValidator.ObjectListElementValidator;
        private IVariableElementValidator VariableElementValidator => _xmlElementValidator.VariableElementValidator;

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
