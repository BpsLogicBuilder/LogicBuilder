using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class LiteralListData
    {
        public LiteralListData(LiteralListElementType literalType, ListType listType, string visibleText, List<XmlElement> childElements, XmlElement literalListElement)
        {
            LiteralType = literalType;
            ListType = listType;
            VisibleText = visibleText;
            ChildElements = childElements;
            LiteralListElement = literalListElement;
        }

        internal LiteralListElementType LiteralType { get; }
        internal ListType ListType { get; }
        internal string VisibleText { get; }
        internal List<XmlElement> ChildElements { get; }
        internal XmlElement LiteralListElement { get; }
    }
}
