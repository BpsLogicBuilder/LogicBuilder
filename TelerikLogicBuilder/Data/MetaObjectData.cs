using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class MetaObjectData
    {
        public MetaObjectData(string objectType, XmlElement childElement, XmlElement metaObjectElement, IEnumHelper enumHelper)
        {
            ObjectType = objectType;
            ChildElement = childElement;
            ChildElementCategory = enumHelper.GetObjectCategory(ChildElement.Name);
            MetaObjectElement = metaObjectElement;
        }

        internal string ObjectType { get; }
        internal XmlElement ChildElement { get; }
        internal ObjectCategory ChildElementCategory { get; }
        internal XmlElement MetaObjectElement { get; }
    }
}
