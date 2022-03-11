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

        /// <summary>
        /// Fully qualified type name for the element object
        /// </summary>
        internal string ObjectType { get; }

        /// <summary>
        /// Enum representing the list type (listType attribute)
        /// </summary>
        internal ListType ListType { get; }

        /// <summary>
        /// VisibleText attribute of <objectList></objectList> element
        /// </summary>
        internal string VisibleText { get; }

        /// <summary>
        /// Collection of child <object></object> elements.
        /// </summary>
        internal List<XmlElement> ChildElements { get; }

        /// <summary>
        /// The <objectList></objectList> element
        /// </summary>
        internal XmlElement ObjectListElement { get; }
    }
}
