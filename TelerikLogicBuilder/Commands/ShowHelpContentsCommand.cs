using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class ShowHelpContentsCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;

        public ShowHelpContentsCommand(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public async override void Execute()
        {
            _mainWindow.MDIParent.ChangeCursor(Cursors.WaitCursor);
            Uri uri = new($"logicbuilderhelp://ads?adFree={bool.TrueString}");
            await Windows.System.Launcher.LaunchUriAsync(uri, new Windows.System.LauncherOptions { });
            _mainWindow.MDIParent.ChangeCursor(Cursors.Default);
        }
    }
}
