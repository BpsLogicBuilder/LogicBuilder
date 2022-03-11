using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class ObjectListVariableData
    {
        public ObjectListVariableData(XmlElement childElement, XmlElement objectListVariableElement, IEnumHelper enumHelper)
        {
            ChildElement = childElement;
            ChildElementCategory = enumHelper.GetObjectCategory(ChildElement.Name);
            ObjectListVariableElement = objectListVariableElement;
        }

        /// <summary>
        /// Child element of <objectListParameter></objectListParameter> (variable, function, constructor, literalList, objectList)
        /// (Usually objectList)
        /// </summary>
        internal XmlElement ChildElement { get; }

        /// <summary>
        /// ChildElementCategory depends on the child element (variable, function, constructor, literalList, or objectList)
        /// </summary>
        internal ObjectCategory ChildElementCategory { get; }

        /// <summary>
        /// <objectListParameter></objectListParameter> element
        /// </summary>
        internal XmlElement ObjectListVariableElement { get; }
    }
}
