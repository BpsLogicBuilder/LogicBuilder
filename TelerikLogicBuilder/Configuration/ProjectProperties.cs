using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Globalization;
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

            var directoryInfo = contextProvider.FileIOHelper.GetNewDirectoryInfo(projectPath);
            if (!directoryInfo.Exists)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.projectPathDoesNotExistFormat, projectPath));

            if (directoryInfo.Parent == null)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.projectPathCannotBeTheRootFolderFormat, projectPath));
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
                        foreach (var qh in new string[] { "Form", "Row", "Column", "Question", "Answer" })
                        {
                            xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTTYPESGROUPELEMENT);
                            xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, qh);
                            //foreach (string cons in this.QuestionsHierarchyObjectTypes[qh])
                                //xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, cons);
                            xmlTextWriter.WriteEndElement();
                        }
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteStartElement(XmlDataConstants.INPUTQUESTIONSHIERARCHYOBJECTTYPESELEMENT);
                        foreach (var qh in new string[] { "Form", "Row", "Column", "Question" })
                        {
                            xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTTYPESGROUPELEMENT);
                            xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, qh);
                            //foreach (string cons in this.InputQuestionsHierarchyObjectTypes[qh])
                                //xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, cons);
                            xmlTextWriter.WriteEndElement();
                        }
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
