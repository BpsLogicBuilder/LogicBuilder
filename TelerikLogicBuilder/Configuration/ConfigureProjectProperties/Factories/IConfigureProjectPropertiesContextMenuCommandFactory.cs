using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories
{
    internal interface IConfigureProjectPropertiesContextMenuCommandFactory
    {
        AddApplicationCommand GetAddApplicationCommand(IConfigureProjectPropertiesForm configureProjectProperties);
        DeleteApplicationCommand GetDeleteApplicationCommand(IConfigureProjectPropertiesForm configureProjectProperties);
    }
}
