using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralParameterElementValidator : ILiteralParameterElementValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ILiteralElementValidator _literalElementValidator;

        public LiteralParameterElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _enumHelper = xmlElementValidator.ContextProvider.EnumHelper;
            _exceptionHelper = xmlElementValidator.ContextProvider.ExceptionHelper;
            _literalElementValidator = xmlElementValidator.LiteralElementValidator;
        }

        public void Validate(LiteralParameter parameter, XmlElement parameterElement, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (parameterElement.Name != XmlDataConstants.LITERALPARAMETERELEMENT)
                throw _exceptionHelper.CriticalException("{E3BC3B76-B0BF-4936-B4AF-6C8F99B0F157}");

            _literalElementValidator.Validate
            (
                parameterElement,
                _enumHelper.GetSystemType(parameter.LiteralType),
                application,
                validationErrors
            );
        }
    }
}
