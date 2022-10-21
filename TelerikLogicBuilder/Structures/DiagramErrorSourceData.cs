using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class DiagramErrorSourceData
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DiagramErrorSourceData(IXmlDocumentHelpers xmlDocumentHelpers, string fileFullName, int pageIndex, int shapeIndex, int pageId, int shapeId)
        {
            FileFullName = fileFullName;
            PageIndex = pageIndex;
            ShapeIndex = shapeIndex;
            PageId = pageId;
            ShapeId = shapeId;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Properties
        /// <summary>
        /// fileFullName attribute of <diagramErrorSource /> element
        /// </summary>
        internal string FileFullName { get; }

        /// <summary>
        /// pageIndex attribute of <diagramErrorSource /> element
        /// </summary>
        internal int PageIndex { get; }

        /// <summary>
        /// shapeIndex attribute of <diagramErrorSource /> element
        /// </summary>
        internal int ShapeIndex { get; }

        /// <summary>
        /// pageId attribute of <diagramErrorSource /> element
        /// </summary>
        internal int PageId { get; }

        /// <summary>
        /// shapeId attribute of <diagramErrorSource /> element
        /// </summary>
        internal int ShapeId { get; }

        internal string ToXml => BuildXml();
        #endregion Properties

        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.DIAGRAMERRORSOURCE);
                xmlTextWriter.WriteAttributeString(XmlDataConstants.FILEFULLNAMEATTRIBUTE, FileFullName);
                xmlTextWriter.WriteAttributeString(XmlDataConstants.PAGEINDEXATTRIBUTE, PageIndex.ToString(CultureInfo.InvariantCulture));
                xmlTextWriter.WriteAttributeString(XmlDataConstants.SHAPEINDEXATTRIBUTE, ShapeIndex.ToString(CultureInfo.InvariantCulture));
                xmlTextWriter.WriteAttributeString(XmlDataConstants.PAGEIDATTRIBUTE, PageId.ToString(CultureInfo.InvariantCulture));
                xmlTextWriter.WriteAttributeString(XmlDataConstants.SHAPEIDATTRIBUTE, ShapeId.ToString(CultureInfo.InvariantCulture));
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
            return stringBuilder.ToString();
        }
    }
}
