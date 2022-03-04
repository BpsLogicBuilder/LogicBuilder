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

            XmlElement xnodRoot = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, $"/{XmlDataConstants.FORMELEMENT}");

            TreeFolder treeFolder = new(Strings.constructorsRootFolderText, new List<string>(), new List<TreeFolder>());
            GetFolderChildren(xnodRoot, treeFolder);
            RemoveEmptyFolders(treeFolder);
            return treeFolder;
        }

        private void GetFolderChildren(XmlNode xmlNode, TreeFolder treeFolder)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode,
                e => e.Name == XmlDataConstants.CONSTRUCTORELEMENT,
                e => e.OrderBy(i => i.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            )
            .ForEach
            (
                fileNode => treeFolder.FileNames.Add(fileNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            );

            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode,
                e => e.Name == XmlDataConstants.FOLDERELEMENT,
                e => e.OrderBy(i => i.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            )
            .ForEach
            (
                folderNode =>
                {
                    TreeFolder childFolder = new
                    (
                        folderNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
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
