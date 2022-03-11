using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class ObjectVariableData
    {
        public ObjectVariableData(XmlElement childElement, XmlElement objectVariableElement, IEnumHelper enumHelper)
        {
            ChildElement = childElement;
            ChildElementCategory = enumHelper.GetObjectCategory(ChildElement.Name);
            ObjectVariableElement = objectVariableElement;
        }

        /// <summary>
        /// Child element of <objectVariable></objectVariable> (variable, function, constructor, literalList, objectList)
        /// (Typically constructor)
        /// </summary>
        internal XmlElement ChildElement { get; }

        /// <summary>
        /// ChildElementCategory depends on the child element (variable, function, constructor, literalList, or objectList)
        /// </summary>
        internal ObjectCategory ChildElementCategory { get; }

        /// <summary>
        /// <objectVariable></objectVariable> element
        /// </summary>
        internal XmlElement ObjectVariableElement { get; }
    }
}
