using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IMetaObjectElementValidator
    {
        void Validate(XmlElement metaObjectElement, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
