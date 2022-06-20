using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class SetSelectedApplicationCommand : ClickCommandBase
    {
        private readonly ICheckSelectedApplication _checkSelectedApplication;
        private readonly IConfigurationService _configurationService;
        private readonly RadMenuItem selectApplicationMenuItem;
        private readonly string applicationName;

        public SetSelectedApplicationCommand(ICheckSelectedApplication checkSelectedApplication, IConfigurationService configurationService, RadMenuItem selectApplicationMenuItem, string applicationName)
        {
            _checkSelectedApplication = checkSelectedApplication;
            _configurationService = configurationService;
            this.selectApplicationMenuItem = selectApplicationMenuItem;
            this.applicationName = applicationName;
        }

        public override void Execute()
        {
            _configurationService.SetSelectedApplication(applicationName);
            _checkSelectedApplication.CheckSelectedItem(selectApplicationMenuItem.Items);
        }
    }
}
