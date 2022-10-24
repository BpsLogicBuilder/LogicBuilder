using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralListParameterElementValidator : ILiteralListParameterElementValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlElementValidatorFactory _xmlElementValidatorFactory;

        public LiteralListParameterElementValidator(
            IEnumHelper enumHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlElementValidatorFactory xmlElementValidatorFactory)
        {
            _enumHelper = enumHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlElementValidatorFactory = xmlElementValidatorFactory;
        }

        //Element validators cannot be injected because of cyclic dependencies.
        private ICallElementValidator? _callElementValidator;
		private ICallElementValidator CallElementValidator => _callElementValidator ??= _xmlElementValidatorFactory.GetCallElementValidator();

        public void Validate(XmlElement parameterElement, ListOfLiteralsParameter parameter, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (parameter.LiteralType == LiteralParameterType.Any)
                return;

            CallElementValidator.Validate
            (
                _xmlDocumentHelpers.GetSingleChildElement(parameterElement),
                _enumHelper.GetSystemType
                (
                    parameter.ListType,
                    _enumHelper.GetSystemType
                    (
                        parameter.LiteralType
                    )
                ), 
                application, 
                validationErrors
            );
        }
    }
}
