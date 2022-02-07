using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class ProjectProperties
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ProjectProperties(string projectName, string projectPath, Dictionary<string, Application> applicationList, HashSet<string> connectorObjectTypes, IContextProvider contextProvider)
        {
            ProjectName = projectName;
            ProjectPath = projectPath;
            ProjectFileFullName = $"{contextProvider.PathHelper.CombinePaths(projectPath, projectName)}{FileExtensions.PROJECTFILEEXTENSION}";
            ApplicationList = applicationList;
            ConnectorObjectTypes = connectorObjectTypes;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        internal string ProjectName { get; set; }
        internal string ProjectPath { get; set; }
        internal string ProjectFileFullName { get; set; }
        internal Dictionary<string, Application> ApplicationList { get; set; }
        internal HashSet<string> ConnectorObjectTypes { get; set; }
        internal string ToXml => BuildXml();

        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateFormattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.PROJECTPROPERTIESELEMENT);

                    xmlTextWriter.WriteElementString(XmlDataConstants.USESHAREPOINTELEMENT, "false");
                    xmlTextWriter.WriteElementString(XmlDataConstants.WEBELEMENT, string.Empty);
                    xmlTextWriter.WriteElementString(XmlDataConstants.DOCUMENTLIBRARYELEMENT, string.Empty);
                    xmlTextWriter.WriteElementString(XmlDataConstants.USEDEFAULTCREDENTIALSELEMENT, "false");
                    xmlTextWriter.WriteElementString(XmlDataConstants.USERNAMEELEMENT, string.Empty);
                    xmlTextWriter.WriteElementString(XmlDataConstants.DOMAINELEMENT, string.Empty);

                    xmlTextWriter.WriteStartElement(XmlDataConstants.APPLICATIONSELEMENT);
                        foreach (Application application in this.ApplicationList.Values)
                        {
                            xmlTextWriter.WriteRaw(application.ToXml);
                        }
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteStartElement(XmlDataConstants.QUESTIONSHIERARCHYOBJECTTYPESELEMENT);
                        //foreach (QuestionsHierarchy qh in this.QuestionsHierarchyObjectTypes.Keys)
                        //{
                        //    xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTTYPESGROUPELEMENT);
                        //    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, Enum.GetName(typeof(QuestionsHierarchy), qh));
                        //    foreach (string cons in this.QuestionsHierarchyObjectTypes[qh])
                        //        xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, cons);
                        //    xmlTextWriter.WriteEndElement();
                        //}
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteStartElement(XmlDataConstants.INPUTQUESTIONSHIERARCHYOBJECTTYPESELEMENT);
                        //foreach (InputQuestionsHierarchy qh in this.InputQuestionsHierarchyObjectTypes.Keys)
                        //{
                        //    xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTTYPESGROUPELEMENT);
                        //    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, Enum.GetName(typeof(InputQuestionsHierarchy), qh));
                        //    foreach (string cons in this.InputQuestionsHierarchyObjectTypes[qh])
                        //        xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, cons);
                        //    xmlTextWriter.WriteEndElement();
                        //}
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteStartElement(XmlDataConstants.CONNECTOROBJECTTYPESELEMENT);
                    foreach (string cons in this.ConnectorObjectTypes)
                        xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, cons);
                    xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
            return stringBuilder.ToString();
        }
    }
}
