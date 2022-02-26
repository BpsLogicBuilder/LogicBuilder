using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class ConstructorListInitializer : IConstructorListInitializer
    {
        private readonly ILoadConstructors _loadConstructors;
        private readonly IConstructorDictionaryBuilder _constructorDictionaryBuilder;
        private readonly IConstructorTreeFolderBuilder _constructorTreeFolderBuilder;

        public ConstructorListInitializer(ILoadConstructors loadConstructors, IConstructorDictionaryBuilder constructorDictionaryBuilder, IConstructorTreeFolderBuilder constructorTreeFolderBuilder)
        {
            _loadConstructors = loadConstructors;
            _constructorDictionaryBuilder = constructorDictionaryBuilder;
            _constructorTreeFolderBuilder = constructorTreeFolderBuilder;
        }

        public ConstructorList InitializeList()
        {
            XmlDocument xmlDocument = _loadConstructors.Load();
            return new ConstructorList
            (
                _constructorDictionaryBuilder.GetDictionary(xmlDocument), 
                _constructorTreeFolderBuilder.GetTreeFolder(xmlDocument)
            );
        }
    }
}
