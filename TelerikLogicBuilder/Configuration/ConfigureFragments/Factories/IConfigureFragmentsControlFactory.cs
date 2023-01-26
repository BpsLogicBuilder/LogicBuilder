using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.ConfigureFragment;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.ConfigureFragmentsFolder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.ConfigureFragmentsRootNode;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories
{
    internal interface IConfigureFragmentsControlFactory
    {
        IConfigureFragmentControl GetConfigureFragmentControl(IConfigureFragmentsForm configureFragmentsForm);
        IConfigureFragmentsFolderControl GetConfigureFragmentsFolderControl(IConfigureFragmentsForm configureFragmentsForm);
        IConfigureFragmentsRootNodeControl GetConfigureFragmentsRootNodeControl();
    }
}
