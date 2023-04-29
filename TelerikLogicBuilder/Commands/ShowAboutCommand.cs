using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            using IScopedDisposableManager<LogicBuilderAboutBox> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<LogicBuilderAboutBox>>();
            LogicBuilderAboutBox aboutBox = disposableManager.ScopedService;
            aboutBox.ShowDialog(_mainWindow.Instance);
        }
    }
}
