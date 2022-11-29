using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers
{
    internal class OpenInTextViewerCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;
        private readonly RadLabel _radLabelMessages;

        public OpenInTextViewerCommand(IMainWindow mainWindow, RadLabel radLabelMessages)
        {
            _mainWindow = mainWindow;
            _radLabelMessages = radLabelMessages;
        }

        public override void Execute()
        {
            if (string.IsNullOrEmpty(_radLabelMessages.Text))
                return;

            using IScopedDisposableManager<TextViewer> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<TextViewer>>();
            TextViewer textViewer = disposableManager.ScopedService;
            textViewer.SetText(_radLabelMessages.Text);
            textViewer.ShowDialog(_mainWindow.Instance);
        }
    }
}
