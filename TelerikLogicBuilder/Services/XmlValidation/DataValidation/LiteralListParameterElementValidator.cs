using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class LiteralListParameterElementValidator : ILiteralListParameterElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;
        private readonly IEnumHelper _enumHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public LiteralListParameterElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
            _enumHelper = xmlElementValidator.ContextProvider.EnumHelper;
            _xmlDocumentHelpers = xmlElementValidator.ContextProvider.XmlDocumentHelpers;
        }

        //ElementValidator properties are created in the XmlElementValidator constructor and may be null in the constructor
        private ICallElementValidator CallElementValidator => _xmlElementValidator.CallElementValidator;

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
