using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class FunctionTreeFolderBuilder : IFunctionTreeFolderBuilder
    {
        private readonly IEmptyTreeFolderRemover _emptyTreeFolderRemover;
        private readonly IExceptionHelper _exceptionHelpers;
        private readonly IFunctionListMatcher _functionListMatcher;
        private readonly IFunctionXmlParser _functionXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private static readonly string FUNCTIONSROOTXPATHFORMAT = $"/forms/form[@name='{XmlDataConstants.FUNCTIONSFORMROOTNODENAME}']/folder[@name='{XmlDataConstants.FUNCTIONSROOTFOLDERNAMEATTRIBUTE}']";
        private static readonly string BUILTINFUNCTIONSROOTXPATHFORMAT = $"/forms/form[@name='{XmlDataConstants.BUILTINFUNCTIONSFORMROOTNODENAME}']/folder[@name='{XmlDataConstants.BUILTINFUNCTIONSROOTFOLDERNAMEATTRIBUTE}']";

        public FunctionTreeFolderBuilder(
            IEmptyTreeFolderRemover emptyTreeFolderRemover,
            IExceptionHelper exceptionHelpers,
            IFunctionListMatcher functionListMatcher,
            IFunctionXmlParser functionXmlParser,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _emptyTreeFolderRemover = emptyTreeFolderRemover;
            _exceptionHelpers = exceptionHelpers;
            _functionListMatcher = functionListMatcher;
            _functionXmlParser = functionXmlParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public TreeFolder GetBooleanFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetFunctionsTreeFolder(xmlDocument, _functionListMatcher.IsBoolFunction);

        public TreeFolder GetBuiltInBooleanFunctionsTreeFolder(XmlDocument xmlDocument) 
            => GetBuiltInFunctionsTreeFolder(xmlDocument, _functionListMatcher.IsBoolFunction);

        public TreeFolder GetBuiltInDialogFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetBuiltInFunctionsTreeFolder(xmlDocument, _functionListMatcher.IsDialogFunction);

        public TreeFolder GetBuiltInTableFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetBuiltInFunctionsTreeFolder(xmlDocument, _functionListMatcher.IsTableFunction);

        public TreeFolder GetBuiltInValueFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetBuiltInFunctionsTreeFolder(xmlDocument, _functionListMatcher.IsValueFunction);

        public TreeFolder GetBuiltInVoidFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetBuiltInFunctionsTreeFolder(xmlDocument, _functionListMatcher.IsVoidFunction);

        public TreeFolder GetDialogFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetFunctionsTreeFolder(xmlDocument, _functionListMatcher.IsDialogFunction);

        public TreeFolder GetTableFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetFunctionsTreeFolder(xmlDocument, _functionListMatcher.IsTableFunction);

        public TreeFolder GetValueFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetFunctionsTreeFolder(xmlDocument, _functionListMatcher.IsValueFunction);

        public TreeFolder GetVoidFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetFunctionsTreeFolder(xmlDocument, _functionListMatcher.IsVoidFunction);

        private TreeFolder GetBuiltInFunctionsTreeFolder(XmlDocument xmlDocument, Func<Function, bool> functionsFilter)
            => GetTreeFolder(xmlDocument, BUILTINFUNCTIONSROOTXPATHFORMAT, Strings.builtInFunctionsRootFolderText, functionsFilter);

        private TreeFolder GetFunctionsTreeFolder(XmlDocument xmlDocument, Func<Function, bool> functionsFilter)
            => GetTreeFolder(xmlDocument, FUNCTIONSROOTXPATHFORMAT, Strings.functionsRootFolderText, functionsFilter);

        private void GetFolderChildren(XmlNode xmlNode, TreeFolder treeFolder, Func<Function, bool> functionsFilter)
        {
            treeFolder.FileNames.AddRange
            (
                _xmlDocumentHelpers.GetChildElements
                (
                    xmlNode,
                    e => e.Name == XmlDataConstants.FUNCTIONELEMENT,
                    en => en.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)
                )
                .Select(_functionXmlParser.Parse)
                .Where(functionsFilter)
                .Select(f => f.Name)
            );

            _xmlDocumentHelpers.GetChildElements
            (
                xmlNode,
                e => e.Name == XmlDataConstants.FOLDERELEMENT,
                e => e.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)
            )
            .ForEach
            (
                folderNode =>
                {
                    TreeFolder childFolder = new
                    (
                        folderNode.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                        new List<string>(),
                        new List<TreeFolder>()
                    );
                    treeFolder.FolderNames.Add(childFolder);
                    GetFolderChildren(folderNode, childFolder, functionsFilter);
                }
            );
        }

        private TreeFolder GetTreeFolder(XmlDocument xmlDocument, string rootFolderXPath, string rootFolderText, Func<Function, bool> functionsFilter)
        {
            if (xmlDocument == null)
                throw _exceptionHelpers.CriticalException("{0DAB5714-3982-4BE3-979E-3CED2DDEB4A4}");

            XmlElement xnodRoot = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, rootFolderXPath);

            TreeFolder treeFolder = new(rootFolderText, new List<string>(), new List<TreeFolder>());
            GetFolderChildren(xnodRoot, treeFolder, functionsFilter);
            _emptyTreeFolderRemover.RemoveEmptyFolders(treeFolder);
            return treeFolder;
        }
    }
}
