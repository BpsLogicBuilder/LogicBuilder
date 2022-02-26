using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class FragmentListInitializer : IFragmentListInitializer
    {
        private readonly ILoadFragments _loadFragments;
        private readonly IFragmentDictionaryBuilder _fragmentDictionaryBuilder;
        private readonly IFragmentTreeFolderBuilder _fragmentTreeFolderBuilder;

        public FragmentListInitializer(ILoadFragments loadFragments, IFragmentDictionaryBuilder fragmentDictionaryBuilder, IFragmentTreeFolderBuilder fragmentTreeFolderBuilder)
        {
            _loadFragments = loadFragments;
            _fragmentDictionaryBuilder = fragmentDictionaryBuilder;
            _fragmentTreeFolderBuilder = fragmentTreeFolderBuilder;
        }

        public FragmentList InitializeList()
        {
            XmlDocument xmlDocument = _loadFragments.Load();
            return new FragmentList
            (
                _fragmentDictionaryBuilder.GetDictionary(xmlDocument),
                _fragmentTreeFolderBuilder.GetTreeFolder(xmlDocument)
            );
        }
    }
}
