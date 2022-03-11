using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class LiteralListVariableData
    {
        public LiteralListVariableData(XmlElement childElement, XmlElement literalListVariableElement, IEnumHelper enumHelper)
        {
            ChildElement = childElement;
            ChildElementCategory = enumHelper.GetObjectCategory(ChildElement.Name);
            LiteralListVariableElement = literalListVariableElement;
        }

        /// <summary>
        /// Child element of <literalListVariable></literalListVariable> (variable, function, constructor, literalList, objectList)
        /// (Usually literalList.)
        /// </summary>
        internal XmlElement ChildElement { get; }

        /// <summary>
        /// ChildElementCategory depends on the child element (variable, function, constructor, literalList, or objectList)
        /// </summary>
        internal ObjectCategory ChildElementCategory { get; }

        /// <summary>
        /// <literalListVariable></literalListVariable> element.
        /// </summary>
        internal XmlElement LiteralListVariableElement { get; }
    }
}
