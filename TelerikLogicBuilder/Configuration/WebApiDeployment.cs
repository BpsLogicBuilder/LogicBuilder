using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class WebApiDeployment
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        internal WebApiDeployment(
            IXmlDocumentHelpers xmlDocumentHelpers,
            string postFileDataUrl,
            string postVariablesMetaUrl,
            string deleteRulesUrl,
            string deleteAllRulesUrl)
        {
            this.PostFileDataUrl = postFileDataUrl;
            this.PostVariablesMetaUrl = postVariablesMetaUrl;
            this.DeleteRulesUrl = deleteRulesUrl;
            this.DeleteAllRulesUrl = deleteAllRulesUrl;
            this._xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public string PostFileDataUrl { get; private set; }
        public string PostVariablesMetaUrl { get; private set; }
        public string DeleteRulesUrl { get; private set; }
        public string DeleteAllRulesUrl { get; private set; }
        public string ToXml => BuildXml();

        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.WEBAPIDEPLOYMENTELEMENT);
                    xmlTextWriter.WriteElementString(XmlDataConstants.POSTFILEDATAURLELEMENT, this.PostFileDataUrl);
                    xmlTextWriter.WriteElementString(XmlDataConstants.POSTVARIABLESMETADATAURLELEMENT, this.PostVariablesMetaUrl);
                    xmlTextWriter.WriteElementString(XmlDataConstants.DELETERULESURLELEMENT, this.DeleteRulesUrl);
                    xmlTextWriter.WriteElementString(XmlDataConstants.DELETEALLRULESURLELEMENT, this.DeleteAllRulesUrl);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
            return stringBuilder.ToString();
        }
    }
}
