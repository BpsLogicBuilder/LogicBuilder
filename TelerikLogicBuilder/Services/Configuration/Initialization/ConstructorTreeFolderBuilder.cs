using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class ConstructorTreeFolderBuilder : IConstructorTreeFolderBuilder
    {
        private readonly IEmptyTreeFolderRemover _emptyTreeFolderRemover;
        private readonly IExceptionHelper _exceptionHelpers;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConstructorTreeFolderBuilder(
            IEmptyTreeFolderRemover emptyTreeFolderRemover,
            IExceptionHelper exceptionHelpers,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _emptyTreeFolderRemover = emptyTreeFolderRemover;
            _exceptionHelpers = exceptionHelpers;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public TreeFolder GetTreeFolder(XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
                throw _exceptionHelpers.CriticalException("{F6EA4AD2-DE48-4698-8689-193D1CD472A2}");

            XmlElement xnodRoot = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, $"/{XmlDataConstants.FORMELEMENT}");

            TreeFolder treeFolder = new(Strings.constructorsRootFolderText, new List<string>(), new List<TreeFolder>());
            GetFolderChildren(xnodRoot, treeFolder);
            _emptyTreeFolderRemover.RemoveEmptyFolders(treeFolder);
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
