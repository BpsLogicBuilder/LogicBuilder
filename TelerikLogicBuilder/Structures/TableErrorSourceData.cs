using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class TableErrorSourceData
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public TableErrorSourceData(string fileFullName, int rowIndex, int columnIndex, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            FileFullName = fileFullName;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Properties
        /// <summary>
        /// fileFullName attribute of <tableErrorSource /> element
        /// </summary>
        internal string FileFullName { get; }

        /// <summary>
        /// rowIndex attribute of <tableErrorSource /> element
        /// </summary>
        internal int RowIndex { get; }

        /// <summary>
        /// columnIndex attribute of <tableErrorSource /> element
        /// </summary>
        internal int ColumnIndex { get; }

        internal string ToXml => BuildXml();
        #endregion Properties

        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.TABLEERRORSOURCE);
                xmlTextWriter.WriteAttributeString(XmlDataConstants.FILEFULLNAMEATTRIBUTE, FileFullName);
                xmlTextWriter.WriteAttributeString(XmlDataConstants.ROWINDEXATTRIBUTE, RowIndex.ToString(CultureInfo.InvariantCulture));
                xmlTextWriter.WriteAttributeString(XmlDataConstants.COLUMNINDEXATTRIBUTE, ColumnIndex.ToString(CultureInfo.InvariantCulture));
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
            return stringBuilder.ToString();
        }
    }
}
