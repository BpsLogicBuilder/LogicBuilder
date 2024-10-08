﻿using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal interface IMDIParent : IWin32Window
    {
        RadCommandBar CommandBar { get; }
        CommandBarButton CommandBarButtonSave { get; }
        IDocumentEditor? EditControl { get; set; }
        IMessages Messages { get; }
        IProjectExplorer ProjectExplorer { get; }
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

        RightToLeft RightToLeft { get; }

        SplitPanel SplitPanelMessages { get; }
        SplitPanel SplitPanelExplorer { get; }

        void AddTableControl(IDocumentEditor documentEditor);
        void AddVisioControl(IDocumentEditor documentEditor);
        void ClearProgressBar();
        void Close();
        void CloseProject();
        void ChangeCursor(Cursor cursor);
        void CreateNewProject(string projectFileFullName);
        void RemoveEditControl();
        Task RunAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task);
        Task RunLoadContextAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task);
        Task RunLoadContextAsync(Func<CancellationTokenSource, Task> task);
        void OpenProject(string projectFileFullName);
        void SetButtonStates(bool projectOpen);
        void SetEditControlMenuStates(bool visioOpen, bool tableOpen);
        void UpdateApplicationMenuItems();

        event FormClosingEventHandler FormClosing;
    }
}
