using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace TelerikLogicBuilder.FormsPreviewer
{
    internal class MockMdiParent : System.Windows.Forms.Form, IMDIParent
    {
        public RadCommandBar CommandBar => throw new NotImplementedException();

        public CommandBarButton CommandBarButtonSave => throw new NotImplementedException();

        public IDocumentEditor? EditControl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IMessages Messages => new MockMessages();

        public RadMenuItem RadMenuItemDelete => throw new NotImplementedException();

        public RadMenuItem RadMenuItemSave => throw new NotImplementedException();

        public RadMenuItem RadMenuItemUndo => throw new NotImplementedException();

        public RadMenuItem RadMenuItemRedo => throw new NotImplementedException();

        public RadMenuItem RadMenuItemIndexInformation => throw new NotImplementedException();

        public RadMenuItem RadMenuItemChaining => throw new NotImplementedException();

        public RadMenuItem RadMenuItemToggleActivateAll => throw new NotImplementedException();

        public RadMenuItem RadMenuItemToggleReevaluateAll => throw new NotImplementedException();

        public RadMenuItem RadMenuItemFullChaining => throw new NotImplementedException();

        public RadMenuItem RadMenuItemNoneChaining => throw new NotImplementedException();

        public RadMenuItem RadMenuItemUpdateOnlyChaining => throw new NotImplementedException();

        public SplitPanel SplitPanelMessages => throw new NotImplementedException();

        public SplitPanel SplitPanelExplorer => throw new NotImplementedException();

        public IProjectExplorer ProjectExplorer => throw new NotImplementedException();

        public void AddTableControl(IDocumentEditor documentEditor)
        {
            throw new NotImplementedException();
        }

        public void AddVisioControl(IDocumentEditor documentEditor)
        {
            throw new NotImplementedException();
        }

        public void ChangeCursor(System.Windows.Forms.Cursor cursor)
        {
        }

        public void ClearProgressBar()
        {
        }

        public void CloseProject()
        {
            throw new NotImplementedException();
        }

        public void OpenProject(string projectFileFullname)
        {
            throw new NotImplementedException();
        }

        public void RemoveEditControl()
        {
            throw new NotImplementedException();
        }

        public Task RunAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task)
        {
            throw new NotImplementedException();
        }

        public Task RunLoadContextAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task)
        {
            throw new NotImplementedException();
        }

        public Task RunLoadContextAsync(Func<CancellationTokenSource, Task> task)
        {
            throw new NotImplementedException();
        }

        public void SetButtonStates(bool projectOpen)
        {
            throw new NotImplementedException();
        }

        public void SetEditControlMenuStates(bool visioOpen, bool tableOpen)
        {
            throw new NotImplementedException();
        }

        public void UpdateApplicationMenuItems()
        {
            throw new NotImplementedException();
        }
    }
}
