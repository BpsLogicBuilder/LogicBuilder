using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class LiteralListData
    {
        public LiteralListData(LiteralParameterType literalType, ListType listType, string visibleText, List<XmlElement> childElements, XmlElement literalListElement)
        {
            LiteralType = literalType;
            ListType = listType;
            VisibleText = visibleText;
            ChildElements = childElements;
            LiteralListElement = literalListElement;
        }

        /// <summary>
        /// Enum representing the literal type (literalType attribute)
        /// </summary>
        internal LiteralParameterType LiteralType { get; }

        /// <summary>
        /// Enum representing the list type (listType attribute)
        /// </summary>
        internal ListType ListType { get; }

        /// <summary>
        /// VisibleText attribute of <literalList></literalList> element
        /// </summary>
        internal string VisibleText { get; }

        /// <summary>
        /// Collection of child <literal></literal> elements.
        /// </summary>
        internal List<XmlElement> ChildElements { get; }

        /// <summary>
        /// <literalList></literalList> element
        /// </summary>
        internal XmlElement LiteralListElement { get; }
    }
}
