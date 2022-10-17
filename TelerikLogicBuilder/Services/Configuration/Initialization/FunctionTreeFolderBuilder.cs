using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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
        private readonly IFunctionXmlParser _functionXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private static readonly string FUNCTIONSROOTXPATHFORMAT = $"/forms/form[@name='{XmlDataConstants.FUNCTIONSFORMROOTNODENAME}']/folder[@name='{XmlDataConstants.FUNCTIONSROOTFOLDERNAMEATTRIBUTE}']";
        private static readonly string BUILTINFUNCTIONSROOTXPATHFORMAT = $"/forms/form[@name='{XmlDataConstants.BUILTINFUNCTIONSFORMROOTNODENAME}']/folder[@name='{XmlDataConstants.BUILTINFUNCTIONSROOTFOLDERNAMEATTRIBUTE}']";

        public FunctionTreeFolderBuilder(
            IEmptyTreeFolderRemover emptyTreeFolderRemover,
            IExceptionHelper exceptionHelpers,
            IFunctionXmlParser functionXmlParser,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _emptyTreeFolderRemover = emptyTreeFolderRemover;
            _exceptionHelpers = exceptionHelpers;
            _functionXmlParser = functionXmlParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public TreeFolder GetBooleanFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetFunctionsTreeFolder(xmlDocument, IsBoolFunction);

        public TreeFolder GetBuiltInBooleanFunctionsTreeFolder(XmlDocument xmlDocument) 
            => GetBuiltInFunctionsTreeFolder(xmlDocument, IsBoolFunction);

        public TreeFolder GetBuiltInDialogFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetBuiltInFunctionsTreeFolder(xmlDocument, IsDialogFunction);

        public TreeFolder GetBuiltInTableFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetBuiltInFunctionsTreeFolder(xmlDocument, IsTableFunction);

        public TreeFolder GetBuiltInValueFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetBuiltInFunctionsTreeFolder(xmlDocument, IsValueFunction);

        public TreeFolder GetBuiltInVoidFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetBuiltInFunctionsTreeFolder(xmlDocument, IsVoidFunction);

        public TreeFolder GetDialogFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetFunctionsTreeFolder(xmlDocument, IsDialogFunction);

        public TreeFolder GetTableFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetFunctionsTreeFolder(xmlDocument, IsTableFunction);

        public TreeFolder GetValueFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetFunctionsTreeFolder(xmlDocument, IsValueFunction);

        public TreeFolder GetVoidFunctionsTreeFolder(XmlDocument xmlDocument)
            => GetFunctionsTreeFolder(xmlDocument, IsVoidFunction);

        private TreeFolder GetBuiltInFunctionsTreeFolder(XmlDocument xmlDocument, Func<XmlElement, bool> functionsFilter)
            => GetTreeFolder(xmlDocument, BUILTINFUNCTIONSROOTXPATHFORMAT, Strings.builtInFunctionsRootFolderText, functionsFilter);

        private TreeFolder GetFunctionsTreeFolder(XmlDocument xmlDocument, Func<XmlElement, bool> functionsFilter)
            => GetTreeFolder(xmlDocument, FUNCTIONSROOTXPATHFORMAT, Strings.functionsRootFolderText, functionsFilter);

        private void GetFolderChildren(XmlNode xmleNode, TreeFolder treeFolder, Func<XmlElement, bool> functionsFilter)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                xmleNode,
                functionsFilter,
                en => en.OrderBy(i => i.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            )
            .ForEach
            (
                functionNode => treeFolder.FileNames.Add(functionNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            );

            _xmlDocumentHelpers.GetChildElements
            (
                xmleNode,
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
                    GetFolderChildren(folderNode, childFolder, functionsFilter);
                }
            );
        }

        private TreeFolder GetTreeFolder(XmlDocument xmlDocument, string rootFolderXPath, string rootFolderText, Func<XmlElement, bool> functionsFilter)
        {
            if (xmlDocument == null)
                throw _exceptionHelpers.CriticalException("{0DAB5714-3982-4BE3-979E-3CED2DDEB4A4}");

            XmlElement xnodRoot = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, rootFolderXPath);

            TreeFolder treeFolder = new(rootFolderText, new List<string>(), new List<TreeFolder>());
            GetFolderChildren(xnodRoot, treeFolder, functionsFilter);
            _emptyTreeFolderRemover.RemoveEmptyFolders(treeFolder);
            return treeFolder;
        }

        bool IsVoidFunction(XmlElement functionElement)
        {
            if (functionElement.Name != XmlDataConstants.FUNCTIONELEMENT)
                return false;

            Function function = _functionXmlParser.Parse(functionElement);

            return IsLiteralMatch(function, LiteralFunctionReturnType.Void) 
                && function.FunctionCategory != FunctionCategories.DialogForm 
                && function.FunctionCategory != FunctionCategories.RuleChainingUpdate;
        }

        private bool IsBoolFunction(XmlElement functionElement) 
            => functionElement.Name == XmlDataConstants.FUNCTIONELEMENT 
            && IsLiteralMatch(_functionXmlParser.Parse(functionElement), LiteralFunctionReturnType.Boolean);

        private bool IsValueFunction(XmlElement functionElement) => functionElement.Name == XmlDataConstants.FUNCTIONELEMENT 
            && !IsLiteralMatch(_functionXmlParser.Parse(functionElement), LiteralFunctionReturnType.Void);

        private bool IsDialogFunction(XmlElement functionElement)
        {
            if (functionElement.Name != XmlDataConstants.FUNCTIONELEMENT)
                return false;

            Function function = _functionXmlParser.Parse(functionElement);

            return IsLiteralMatch(function, LiteralFunctionReturnType.Void)
                && function.FunctionCategory == FunctionCategories.DialogForm;
        }

        private bool IsTableFunction(XmlElement functionElement)
        {
            if (functionElement.Name != XmlDataConstants.FUNCTIONELEMENT)
                return false;

            Function function = _functionXmlParser.Parse(functionElement);

            return IsLiteralMatch(function, LiteralFunctionReturnType.Void)
                && function.FunctionCategory != FunctionCategories.DialogForm;
        }

        static bool IsLiteralMatch(Function function, LiteralFunctionReturnType literalType) 
            => function.ReturnType is LiteralReturnType literalReturnType
                && literalReturnType.ReturnType == literalType;


    }
}
