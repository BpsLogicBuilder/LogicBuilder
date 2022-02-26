using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class VariableListInitializer : IVariableListInitializer
    {
        private readonly ILoadVariables _loadVariables;
        private readonly IVariableDictionaryBuilder _variableDictionaryBuilder;
        private readonly IVariableTreeFolderBuilder _variableTreeFolderBuilder;

        public VariableListInitializer(ILoadVariables loadVariables, IVariableDictionaryBuilder variableDictionaryBuilder, IVariableTreeFolderBuilder variableTreeFolderBuilder)
        {
            _loadVariables = loadVariables;
            _variableDictionaryBuilder = variableDictionaryBuilder;
            _variableTreeFolderBuilder = variableTreeFolderBuilder;
        }

        public VariableList InitializeList()
        {
            XmlDocument xmlDocument = _loadVariables.Load();
            return new VariableList
            (
                _variableDictionaryBuilder.GetDictionary(xmlDocument),
                _variableTreeFolderBuilder.GetTreeFolder(xmlDocument)
            );
        }
    }
}
