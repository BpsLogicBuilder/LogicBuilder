using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Initialization
{
    internal class VariableListFolderBuilderUtility : ConfigurationItemFolderBuilderUtility
    {
        private readonly IExceptionHelper _exceptionHelpers;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public VariableListFolderBuilderUtility(IContextProvider contextProvider)
        {
            _exceptionHelpers = contextProvider.ExceptionHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        internal override TreeFolder GetTreeFolder(XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
                throw _exceptionHelpers.CriticalException("{451DB316-8A0D-405E-9E75-B1FE6C8B4EA8}");

            XmlNode xnodRoot = xmlDocument.SelectSingleNode($"/{XmlDataConstants.FOLDERELEMENT}");

            if (xnodRoot != null)
            {
                TreeFolder treeFolder = new(Strings.variablesRootNodeText, new List<string>(), new List<TreeFolder>());
                GetFolderChildren(xnodRoot, treeFolder);
                RemoveEmptyFolders(treeFolder);
                return treeFolder;
            }
            else
            {
                throw _exceptionHelpers.CriticalException("{582FF9B1-D5CE-4B03-9009-A6FD8D225062}");
            }
        }

        private void GetFolderChildren(XmlNode xmlNode, TreeFolder treeFolder)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode, 
                e => new HashSet<string> { XmlDataConstants.LITERALVARIABLEELEMENT, XmlDataConstants.OBJECTVARIABLEELEMENT, XmlDataConstants.LITERALLISTVARIABLEELEMENT, XmlDataConstants.OBJECTLISTVARIABLEELEMENT }.Contains(e.Name),
                e => e.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value)
            )
            .ForEach
            (
                variableNode => treeFolder.FileNames.Add(variableNode.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value)
            );

            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode, 
                e => e.Name == XmlDataConstants.FOLDERELEMENT,
                en => en.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value)
            )
            .ForEach(folderNode =>
            {
                TreeFolder childFolder = new(folderNode.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value, new List<string>(), new List<TreeFolder>());
                treeFolder.FolderNames.Add(childFolder);
                GetFolderChildren(folderNode, childFolder);
            });
        }
    }
}
