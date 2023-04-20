using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class FragmentTreeFolderBuilder : IFragmentTreeFolderBuilder
    {
        private readonly IEmptyTreeFolderRemover _emptyTreeFolderRemover;
        private readonly IExceptionHelper _exceptionHelpers;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public FragmentTreeFolderBuilder(
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
                throw _exceptionHelpers.CriticalException("{08493F64-EBD5-4B73-8B4B-7F331BAFBEEA}");

            XmlElement xnodRoot = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, $"/{XmlDataConstants.FOLDERELEMENT}");

            TreeFolder treeFolder = new(Strings.fragmentsRootNodeText, new List<string>(), new List<TreeFolder>());
            GetFolderChildren(xnodRoot, treeFolder);
            _emptyTreeFolderRemover.RemoveEmptyFolders(treeFolder);
            return treeFolder;
        }

        private void GetFolderChildren(XmlNode xmlNode, TreeFolder treeFolder)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode,
                e => e.Name == XmlDataConstants.FRAGMENTELEMENT,
                e => e.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)
            )
            .ForEach
            (
                fragmentNode => treeFolder.FileNames.Add(fragmentNode.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)
            );

            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode,
                e => e.Name == XmlDataConstants.FOLDERELEMENT,
                en => en.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)
            )
            .ForEach(folderNode =>
            {
                TreeFolder childFolder = new
                (
                    folderNode.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                    new List<string>(),
                    new List<TreeFolder>()
                );
                treeFolder.FolderNames.Add(childFolder);
                GetFolderChildren(folderNode, childFolder);
            });
        }
    }
}
