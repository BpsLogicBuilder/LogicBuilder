using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Initialization
{
    internal class ConstructorListFolderBuilderUtility : ConfigurationItemFolderBuilderUtility
    {
        private readonly IExceptionHelper _exceptionHelpers;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConstructorListFolderBuilderUtility(IContextProvider contextProvider)
        {
            _exceptionHelpers = contextProvider.ExceptionHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        internal override TreeFolder GetTreeFolder(XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
                throw _exceptionHelpers.CriticalException("{F6EA4AD2-DE48-4698-8689-193D1CD472A2}");

            XmlNode? xnodRoot = xmlDocument.SelectSingleNode($"/{XmlDataConstants.FORMELEMENT}");

            if (xnodRoot != null)
            {
                TreeFolder treeFolder = new(Strings.constructorsRootFolderText, new List<string>(), new List<TreeFolder>());
                GetFolderChildren(xnodRoot, treeFolder);
                RemoveEmptyFolders(treeFolder);
                return treeFolder;
            }
            else
            {
                throw _exceptionHelpers.CriticalException("{1018F2C6-CBF2-4E17-9604-5BF579AF5F6A}");
            }
        }

        private void GetFolderChildren(XmlNode xmlNode, TreeFolder treeFolder)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode,
                e => e.Name == XmlDataConstants.CONSTRUCTORELEMENT,
                e => e.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)/*Attribute is required by schema definition*/
            )
            .ForEach
            (
                fileNode => treeFolder.FileNames.Add(fileNode.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)/*Attribute is required by schema definition*/
            );

            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode,
                e => e.Name == XmlDataConstants.FOLDERELEMENT,
                e => e.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)/*Attribute is required by schema definition*/
            )
            .ForEach
            (
                folderNode =>
                {
                    TreeFolder childFolder = new
                    (
                        folderNode.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,/*Attribute is required by schema definition*/
                        new List<string>(), 
                        new List<TreeFolder>()
                    );
                    treeFolder.FolderNames.Add(childFolder);
                    GetFolderChildren(folderNode, childFolder);
                }
            );
        }
    }
}
