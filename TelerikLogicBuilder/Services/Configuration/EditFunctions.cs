using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class EditFunctions : IEditFunctions
    {
        private readonly IMainWindow _mainWindow;

        public EditFunctions(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void Edit()
        {
            DisplayMessage.Show(_mainWindow.Instance, "EditFunctions", _mainWindow.RightToLeft);
        }
    }
}
