using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator
{
    internal class TableErrorSourceData
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public TableErrorSourceData(string fileFullName, int rowIndex, int columnIndex, IContextProvider contextProvider)
        {
            FileFullName = fileFullName;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
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

        internal string ToXml => this.BuildXml();
        #endregion Properties

        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
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
