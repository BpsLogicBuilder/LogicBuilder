using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class ObjectListParameterElementValidator : IObjectListParameterElementValidator
    {
        private readonly IXmlElementValidator _xmlElementValidator;

        public ObjectListParameterElementValidator(IXmlElementValidator xmlElementValidator)
        {
            _xmlElementValidator = xmlElementValidator;
        }

        public void Validate(XmlElement parameterElement, ListOfObjectsParameter parameter, ApplicationTypeInfo application, List<string> validationErrors)
        {
            //throw new System.NotImplementedException();
        }
    }
}
