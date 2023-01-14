using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructorsFolder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructorsRootNode;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Factories
{
    internal interface IConfigureConstructorsControlFactory
    {
        IConfigureConstructorControl GetConfigureConstructorControl(IConfigureConstructorsForm configureConstructorsForm);
        IConfigureConstructorsFolderControl GetConfigureConstructorsFolderControl(IConfigureConstructorsForm configureConstructorsForm);
        IConfigureConstructorsRootNodeControl GetConfigureConstructorsRootNodeControl();
    }
}
