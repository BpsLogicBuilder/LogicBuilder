using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralParameterElementValidator : ILiteralParameterElementValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public LiteralParameterElementValidator(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private ILiteralElementValidator? _literalElementValidator;
		private ILiteralElementValidator LiteralElementValidator => _literalElementValidator ??= _xmlElementValidatorFactory.GetLiteralElementValidator();

        public void Validate(XmlElement parameterElement, LiteralParameter parameter, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (parameterElement.Name != XmlDataConstants.LITERALPARAMETERELEMENT)
                throw _exceptionHelper.CriticalException("{E3BC3B76-B0BF-4936-B4AF-6C8F99B0F157}");

            LiteralElementValidator.Validate
            (
                parameterElement,
                _enumHelper.GetSystemType(parameter.LiteralType),
                application,
                validationErrors
            );
        }
    }
}
