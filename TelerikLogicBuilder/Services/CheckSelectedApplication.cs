using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class CheckSelectedApplication : ICheckSelectedApplication
    {
        private readonly IConfigurationService _configurationService;

        public CheckSelectedApplication(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public void CheckSelectedItem(RadItemOwnerCollection applicationMenuItems)
        {
            Application selectedApplication = _configurationService.GetSelectedApplication();
            foreach (RadMenuItem menuItem in applicationMenuItems)
                menuItem.IsChecked = (string)menuItem.Tag == selectedApplication.Name;
        }
    }
}
