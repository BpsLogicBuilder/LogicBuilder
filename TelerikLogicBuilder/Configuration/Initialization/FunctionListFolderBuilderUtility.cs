using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Initialization
{
    internal class FunctionListFolderBuilderUtility : ConfigurationItemFolderBuilderUtility
    {
        private readonly IExceptionHelper _exceptionHelpers;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly string rootFolderXPath;
        private readonly string rootFolderText;
        private readonly Func<XmlElement, bool> functionsFilter;

        public FunctionListFolderBuilderUtility(IContextProvider contextProvider, string rootFolderXPath, string rootFolderText, Func<XmlElement, bool> functionsFilter)
        {
            _exceptionHelpers = contextProvider.ExceptionHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            this.rootFolderXPath = rootFolderXPath;
            this.rootFolderText = rootFolderText;
            this.functionsFilter = functionsFilter;
        }

        internal override TreeFolder GetTreeFolder(XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
                throw _exceptionHelpers.CriticalException("{0DAB5714-3982-4BE3-979E-3CED2DDEB4A4}");

            XmlNode? xnodRoot = xmlDocument.SelectSingleNode(this.rootFolderXPath);

            if (xnodRoot != null)
            {
                TreeFolder treeFolder = new(rootFolderText, new List<string>(), new List<TreeFolder>());
                GetFolderChildren(xnodRoot, treeFolder);
                RemoveEmptyFolders(treeFolder);
                return treeFolder;
            }
            else
            {
                throw _exceptionHelpers.CriticalException("{6DA0770E-20DF-4ADA-AAAF-09B9911E1776}");
            }
        }

        private void GetFolderChildren(XmlNode xmleNode, TreeFolder treeFolder)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                xmleNode, 
                e => functionsFilter(e),
                en => en.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)/*Attribute is required by schema definition*/
            )
            .ForEach
            (
                functionNode => treeFolder.FileNames.Add(functionNode.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)/*Attribute is required by schema definition*/
            );

            _xmlDocumentHelpers.GetChildElements
            (
                xmleNode, 
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
