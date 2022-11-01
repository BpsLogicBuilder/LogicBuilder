using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal interface IConfigureProjectPropertiesContextMenuCommandFactory
    {
        AddApplicationCommand GetAddApplicationCommand(IConfigureProjectProperties configureProjectProperties);
        DeleteApplicationCommand GetDeleteApplicationCommand(IConfigureProjectProperties configureProjectProperties);
    }
}
