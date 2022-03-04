using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Initialization
{
    internal class FragmentListFolderBuilderUtility : ConfigurationItemFolderBuilderUtility
    {
        private readonly IExceptionHelper _exceptionHelpers;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public FragmentListFolderBuilderUtility(IContextProvider contextProvider)
        {
            _exceptionHelpers = contextProvider.ExceptionHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        internal override TreeFolder GetTreeFolder(XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
                throw _exceptionHelpers.CriticalException("{08493F64-EBD5-4B73-8B4B-7F331BAFBEEA}");

            XmlElement xnodRoot = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, $"/{XmlDataConstants.FOLDERELEMENT}");

            TreeFolder treeFolder = new(Strings.fragmentsRootNodeText, new List<string>(), new List<TreeFolder>());
            GetFolderChildren(xnodRoot, treeFolder);
            RemoveEmptyFolders(treeFolder);
            return treeFolder;
        }

        private void GetFolderChildren(XmlNode xmlNode, TreeFolder treeFolder)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode,
                e => e.Name == XmlDataConstants.FRAGMENTELEMENT,
                e => e.OrderBy(i => i.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            )
            .ForEach
            (
                fragmentNode => treeFolder.FileNames.Add(fragmentNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            );

            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode,
                e => e.Name == XmlDataConstants.FOLDERELEMENT,
                en => en.OrderBy(i => i.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            )
            .ForEach(folderNode =>
            {
                TreeFolder childFolder = new
                (
                    folderNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                    new List<string>(), 
                    new List<TreeFolder>()
                );
                treeFolder.FolderNames.Add(childFolder);
                GetFolderChildren(folderNode, childFolder);
            });
        }
    }
}
