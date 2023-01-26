using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories
{
    internal interface IConfigureFragmentsFactory
    {
        IConfigureFragmentsDragDropHandler GetConfigureFragmentsDragDropHandler(IConfigureFragmentsForm configureFragmentsForm);
        ConfigureFragmentsTreeView GetConfigureFragmentsTreeView(IConfigureFragmentsForm configureFragmentsForm);
    }
}
