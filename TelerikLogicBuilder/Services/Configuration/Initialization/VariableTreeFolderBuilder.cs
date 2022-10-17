using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class VariableTreeFolderBuilder : IVariableTreeFolderBuilder
    {
        private readonly IEmptyTreeFolderRemover _emptyTreeFolderRemover;
        private readonly IExceptionHelper _exceptionHelpers;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public VariableTreeFolderBuilder(
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
                throw _exceptionHelpers.CriticalException("{451DB316-8A0D-405E-9E75-B1FE6C8B4EA8}");

            XmlElement xnodRoot = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, $"/{XmlDataConstants.FOLDERELEMENT}");

            TreeFolder treeFolder = new(Strings.variablesRootNodeText, new List<string>(), new List<TreeFolder>());
            GetFolderChildren(xnodRoot, treeFolder);
            _emptyTreeFolderRemover.RemoveEmptyFolders(treeFolder);
            return treeFolder;
        }

        private void GetFolderChildren(XmlNode xmlNode, TreeFolder treeFolder)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode,
                e => new HashSet<string> { XmlDataConstants.LITERALVARIABLEELEMENT, XmlDataConstants.OBJECTVARIABLEELEMENT, XmlDataConstants.LITERALLISTVARIABLEELEMENT, XmlDataConstants.OBJECTLISTVARIABLEELEMENT }.Contains(e.Name),
                e => e.OrderBy(i => i.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            )
            .ForEach
            (
                variableNode => treeFolder.FileNames.Add(variableNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            );

            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode,
                e => e.Name == XmlDataConstants.FOLDERELEMENT,
                en => en.OrderBy(i => i.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            )
            .ForEach(folderNode =>
            {
                TreeFolder childFolder = new(folderNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), new List<string>(), new List<TreeFolder>());
                treeFolder.FolderNames.Add(childFolder);
                GetFolderChildren(folderNode, childFolder);
            });
        }
    }
}
