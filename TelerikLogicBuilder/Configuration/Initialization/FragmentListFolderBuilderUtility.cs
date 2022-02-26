﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
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

            XmlNode xnodRoot = xmlDocument.SelectSingleNode($"/{XmlDataConstants.FOLDERELEMENT}");

            if (xnodRoot != null)
            {
                TreeFolder treeFolder = new(Strings.fragmentsRootNodeText, new List<string>(), new List<TreeFolder>());
                GetFolderChildren(xnodRoot, treeFolder);
                RemoveEmptyFolders(treeFolder);
                return treeFolder;
            }
            else
            {
                throw _exceptionHelpers.CriticalException("{BD2789C7-631F-4035-928E-ACBF71E34402}");
            }
        }

        private void GetFolderChildren(XmlNode xmlNode, TreeFolder treeFolder)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode,
                e => e.Name == XmlDataConstants.FRAGMENTELEMENT,
                e => e.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value)
            )
            .ForEach
            (
                fragmentNode => treeFolder.FileNames.Add(fragmentNode.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value)
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
