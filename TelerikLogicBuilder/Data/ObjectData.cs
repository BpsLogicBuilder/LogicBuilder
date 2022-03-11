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

        /// <summary>
        /// Child element of <object></object> (variable, function, constructor, literalList, objectList)
        /// (Usually constructor)
        /// </summary>
        internal XmlElement ChildElement { get; }

        /// <summary>
        /// ChildElementCategory depends on the child element (variable, function, constructor, literalList, or objectList)
        /// </summary>
        internal ObjectCategory ChildElementCategory { get; }

        /// <summary>
        /// <object></object> element
        /// </summary>
        internal XmlElement ObjectElement { get; }
    }
}
