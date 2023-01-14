using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Factories
{
    internal interface IConfigureConstructorsFactory
    {
        IConfigureConstructorsDragDropHandler GetConfigureConstructorsDragDropHandler(IConfigureConstructorsForm configureConstructorsForm);
        ConfigureConstructorsTreeView GetConfigureConstructorsTreeView(IConfigureConstructorsForm configureConstructorsForm);
    }
}
