using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class ObjectListData
    {
        public ObjectListData(string objectType, ListType listType, string visibleText, List<XmlElement> childElements, XmlElement objectListElement)
        {
            ObjectType = objectType;
            ListType = listType;
            VisibleText = visibleText;
            ChildElements = childElements;
            ObjectListElement = objectListElement;
        }

        internal string ObjectType { get; }
        internal ListType ListType { get; }
        internal string VisibleText { get; }
        internal List<XmlElement> ChildElements { get; }
        internal XmlElement ObjectListElement { get; }
    }
}
