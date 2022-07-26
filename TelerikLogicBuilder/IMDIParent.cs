using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal interface IMDIParent
    {
        RadCommandBar CommandBar { get; }
        CommandBarButton CommandBarButtonSave { get; }
        IDocumentEditor? EditControl { get; set; }
        IMessages Messages { get; }
        RadMenuItem RadMenuItemDelete { get; }
        RadMenuItem RadMenuItemSave { get; }
        RadMenuItem RadMenuItemUndo { get; }
        RadMenuItem RadMenuItemRedo { get; }
        RadMenuItem RadMenuItemIndexInformation { get; }
        RadMenuItem RadMenuItemChaining { get; }
        RadMenuItem RadMenuItemToggleActivateAll { get; }
        RadMenuItem RadMenuItemToggleReevaluateAll { get; }
        RadMenuItem RadMenuItemFullChaining { get; }
        RadMenuItem RadMenuItemNoneChaining { get; }
        RadMenuItem RadMenuItemUpdateOnlyChaining { get; }

        void AddTableControl(IDocumentEditor documentEditor);
        void AddVisioControl(IDocumentEditor documentEditor);
        void CloseProject();
        void ChangeCursor(Cursor cursor);
        void RemoveEditControl();
        Task RunLoadContextAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task);
        Task RunLoadContextAsync(Func<CancellationTokenSource, Task> task);
        void OpenProject(string projectFileFullname);
        void SetEditControlMenuStates(bool visioOpen, bool tableOpen);

        event FormClosingEventHandler FormClosing;
    }
}
