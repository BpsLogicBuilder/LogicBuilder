using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class FunctionTreeFolderBuilder : IFunctionTreeFolderBuilder
    {
        private readonly IFunctionXmlParser _functionXmlParser;
        private readonly IContextProvider _contextProvider;
        private static readonly string FUNCTIONSROOTXPATHFORMAT = $"/forms/form[@name='{XmlDataConstants.FUNCTIONSFORMROOTNODENAME}']/folder[@name='{XmlDataConstants.FUNCTIONSROOTFOLDERNAMEATTRIBUTE}']";
        private static readonly string BUILTINFUNCTIONSROOTXPATHFORMAT = $"/forms/form[@name='{XmlDataConstants.BUILTINFUNCTIONSFORMROOTNODENAME}']/folder[@name='{XmlDataConstants.BUILTINFUNCTIONSROOTFOLDERNAMEATTRIBUTE}']";

        public FunctionTreeFolderBuilder(IContextProvider contextProvider, IFunctionXmlParser functionXmlParser)
        {
            _contextProvider = contextProvider;
            _functionXmlParser = functionXmlParser;
        }

        public TreeFolder GetBooleanFunctionsTreeFolder(XmlDocument xmlDocument) 
            => new FunctionListFolderBuilderUtility
            (
                _contextProvider,
                FUNCTIONSROOTXPATHFORMAT,
                Strings.functionsRootFolderText,
                e => IsBoolFunction(e)
            )
            .GetTreeFolder(xmlDocument);

        public TreeFolder GetBuiltInBooleanFunctionsTreeFolder(XmlDocument xmlDocument)
            => new FunctionListFolderBuilderUtility
            (
                _contextProvider,
                BUILTINFUNCTIONSROOTXPATHFORMAT,
                Strings.builtInFunctionsRootFolderText,
                e => IsBoolFunction(e)
            )
            .GetTreeFolder(xmlDocument);

        public TreeFolder GetBuiltInDialogFunctionsTreeFolder(XmlDocument xmlDocument)
            => new FunctionListFolderBuilderUtility
            (
                _contextProvider,
                BUILTINFUNCTIONSROOTXPATHFORMAT,
                Strings.builtInFunctionsRootFolderText,
                e => IsDialogFunction(e)
            )
            .GetTreeFolder(xmlDocument);

        public TreeFolder GetBuiltInTableFunctionsTreeFolder(XmlDocument xmlDocument)
            => new FunctionListFolderBuilderUtility
            (
                _contextProvider,
                BUILTINFUNCTIONSROOTXPATHFORMAT,
                Strings.builtInFunctionsRootFolderText,
                e => IsTableFunction(e)
            )
            .GetTreeFolder(xmlDocument);

        public TreeFolder GetBuiltInValueFunctionsTreeFolder(XmlDocument xmlDocument)
            => new FunctionListFolderBuilderUtility
            (
                _contextProvider,
                BUILTINFUNCTIONSROOTXPATHFORMAT,
                Strings.builtInFunctionsRootFolderText,
                e => IsValueFunction(e)
            )
            .GetTreeFolder(xmlDocument);

        public TreeFolder GetBuiltInVoidFunctionsTreeFolder(XmlDocument xmlDocument)
            => new FunctionListFolderBuilderUtility
            (
                _contextProvider,
                BUILTINFUNCTIONSROOTXPATHFORMAT,
                Strings.builtInFunctionsRootFolderText,
                e => IsVoidFunction(e)
            )
            .GetTreeFolder(xmlDocument);

        public TreeFolder GetDialogFunctionsTreeFolder(XmlDocument xmlDocument)
            => new FunctionListFolderBuilderUtility
            (
                _contextProvider,
                FUNCTIONSROOTXPATHFORMAT,
                Strings.functionsRootFolderText,
                e => IsDialogFunction(e)
            )
            .GetTreeFolder(xmlDocument);

        public TreeFolder GetTableFunctionsTreeFolder(XmlDocument xmlDocument)
            => new FunctionListFolderBuilderUtility
            (
                _contextProvider,
                FUNCTIONSROOTXPATHFORMAT,
                Strings.functionsRootFolderText,
                e => IsTableFunction(e)
            )
            .GetTreeFolder(xmlDocument);

        public TreeFolder GetValueFunctionsTreeFolder(XmlDocument xmlDocument)
            => new FunctionListFolderBuilderUtility
            (
                _contextProvider,
                FUNCTIONSROOTXPATHFORMAT,
                Strings.functionsRootFolderText,
                e => IsValueFunction(e)
            )
            .GetTreeFolder(xmlDocument);

        public TreeFolder GetVoidFunctionsTreeFolder(XmlDocument xmlDocument)
            => new FunctionListFolderBuilderUtility
            (
                _contextProvider,
                FUNCTIONSROOTXPATHFORMAT,
                Strings.functionsRootFolderText,
                e => IsVoidFunction(e)
            )
            .GetTreeFolder(xmlDocument);

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
