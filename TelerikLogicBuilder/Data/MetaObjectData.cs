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

        /// <summary>
        /// Fully qualified type name for the object
        /// </summary>
        internal string ObjectType { get; }

        /// <summary>
        /// Child element of <metaObject></metaObject> (variable, function, constructor, literalList, objectList)
        /// (Usually constructor)
        /// </summary>
        internal XmlElement ChildElement { get; }

        /// <summary>
        /// ChildElementCategory depends on the child element (variable, function, constructor, literalList, or objectList)
        /// </summary>
        internal ObjectCategory ChildElementCategory { get; }

        /// <summary>
        /// <metaObject></metaObject> element
        /// </summary>
        internal XmlElement MetaObjectElement { get; }
    }
}
