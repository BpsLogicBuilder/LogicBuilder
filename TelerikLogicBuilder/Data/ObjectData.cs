using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class ObjectData
    {
        public ObjectData(XmlElement childElement, XmlElement objectElement, IEnumHelper enumHelper)
        {
            ChildElement = childElement;
            ChildElementCategory = enumHelper.GetObjectCategory(ChildElement.Name);
            ObjectElement = objectElement;
        }

        internal XmlElement ChildElement { get; }
        internal ObjectCategory ChildElementCategory { get; }
        internal XmlElement ObjectElement { get; }
    }
}
