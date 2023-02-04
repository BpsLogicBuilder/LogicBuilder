using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class FunctionListInitializer : IFunctionListInitializer
    {
        private readonly ILoadFunctions _loadFunctions;
        private readonly IFunctionDictionaryBuilder _functionDictionaryBuilder;
        private readonly IFunctionTreeFolderBuilder _functionTreeFolderBuilder;

        public FunctionListInitializer(ILoadFunctions loadFunctions, IFunctionDictionaryBuilder functionDictionaryBuilder, IFunctionTreeFolderBuilder functionTreeFolderBuilder)
        {
            _loadFunctions = loadFunctions;
            _functionDictionaryBuilder = functionDictionaryBuilder;
            _functionTreeFolderBuilder = functionTreeFolderBuilder;
        }

        public FunctionList InitializeList()
        {
            XmlDocument xmlDocument = _loadFunctions.Load();
            return new FunctionList
            (
                _functionDictionaryBuilder.GetDictionary(xmlDocument),
                _functionDictionaryBuilder.GetVoidFunctionDictionary(xmlDocument),
                _functionDictionaryBuilder.GetBooleanFunctionDictionary(xmlDocument),
                _functionDictionaryBuilder.GetValueFunctionDictionary(xmlDocument),
                _functionDictionaryBuilder.GetDialogFunctionDictionary(xmlDocument),
                _functionDictionaryBuilder.GetTableFunctionDictionary(xmlDocument),
                _functionTreeFolderBuilder.GetVoidFunctionsTreeFolder(xmlDocument),
                _functionTreeFolderBuilder.GetBooleanFunctionsTreeFolder(xmlDocument),
                _functionTreeFolderBuilder.GetValueFunctionsTreeFolder(xmlDocument),
                _functionTreeFolderBuilder.GetDialogFunctionsTreeFolder(xmlDocument),
                _functionTreeFolderBuilder.GetTableFunctionsTreeFolder(xmlDocument),
                _functionTreeFolderBuilder.GetBuiltInVoidFunctionsTreeFolder(xmlDocument),
                _functionTreeFolderBuilder.GetBuiltInBooleanFunctionsTreeFolder(xmlDocument),
                _functionTreeFolderBuilder.GetBuiltInValueFunctionsTreeFolder(xmlDocument),
                _functionTreeFolderBuilder.GetBuiltInDialogFunctionsTreeFolder(xmlDocument),
                _functionTreeFolderBuilder.GetBuiltInTableFunctionsTreeFolder(xmlDocument)
            );
        }
    }
}
