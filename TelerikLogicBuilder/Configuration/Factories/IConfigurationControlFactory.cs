using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal interface IConfigurationControlFactory
    {
        IApplicationControl GetApplicationControl(IConfigureProjectProperties configureProjectProperties);
        ILoadAssemblyPathsControl GetLoadAssemblyPathsControl(IConfigureLoadAssemblyPaths configureLoadAssemblyPaths);
    }
}
