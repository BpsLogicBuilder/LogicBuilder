using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class ShowAboutCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;

        public ShowAboutCommand(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            using IScopedDisposableManager<ILogicBuilderAboutBox> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<ILogicBuilderAboutBox>>();
            ILogicBuilderAboutBox aboutBox = disposableManager.ScopedService;
            ((Form)aboutBox).ShowDialog(_mainWindow.Instance);
        }
    }
}
