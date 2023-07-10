using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ListBox.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class ManagedListBoxControl : UserControl, IManagedListBoxControl
    {
        private EventHandler? btnCancelClickHandler;
        private EventHandler? btnCopyClickHandler;
        private EventHandler? btnEditClickHandler;
        private EventHandler? btnRemoveClickHandler;
        private EventHandler? btnUpClickHandler;
        private EventHandler? btnDownClickHandler;

        public ManagedListBoxControl()
        {
            InitializeComponent();
            Initialize();
        }

        public RadButton BtnCancel => btnCancel;

        public RadButton BtnCopy => btnCopy;

        public RadButton BtnEdit => btnEdit;

        public RadButton BtnRemove => btnRemove;

        public RadButton BtnUp => btnUp;

        public RadButton BtnDown => btnDown;

        public RadListControl ListBox => listBox;

        public void CreateCommands(IRadListBoxManager radListBoxManager)
        {
            btnCancelClickHandler = InitializeHButtonCommand(new ListBoxManagerCancelCommand(radListBoxManager));
            btnCopyClickHandler = InitializeHButtonCommand(new ListBoxManagerCopyCommand(radListBoxManager));
            btnEditClickHandler = InitializeHButtonCommand(new ListBoxManagerEditCommand(radListBoxManager));
            btnRemoveClickHandler = InitializeHButtonCommand(new ListBoxManagerRemoveCommand(radListBoxManager));
            btnUpClickHandler = InitializeHButtonCommand(new ListBoxManagerMoveUpCommand(radListBoxManager));
            btnDownClickHandler = InitializeHButtonCommand(new ListBoxManagerMoveDownCommand(radListBoxManager));
            AddClickCommands();
        }

        public void DisableControls() => Enable(false);

        public void EnableControls(IRadListBoxManager radListBoxManager) => radListBoxManager.RestoreEnabledControls();

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnCancel.Click += btnCancelClickHandler;
            btnCopy.Click += btnCopyClickHandler;
            btnEdit.Click += btnEditClickHandler;
            btnRemove.Click += btnRemoveClickHandler;
            btnUp.Click += btnUpClickHandler;
            btnDown.Click += btnDownClickHandler;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
        {
            ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;
        }

        private void Enable(bool enable)
        {
            ListBox.Enabled = enable;
            btnCancel.Enabled = enable;
            btnCopy.Enabled = enable;
            btnDown.Enabled = enable;
            btnEdit.Enabled = enable;
            btnRemove.Enabled = enable;
            btnUp.Enabled = enable;
        }

        private void Initialize()
        {
            ControlsLayoutUtility.LayoutManagedListBoxEditButtons(radPanelEditButtons, radPanelTableParent, tableLayoutPanel);
            CollapsePanelBorder(radPanelListBox);
            CollapsePanelBorder(radPanelUpDownButtons);
            CollapsePanelBorder(radPanelEditButtons);
            CollapsePanelBorder(radPanelTableParent);
            this.Disposed += ManagedListBoxControl_Disposed;
        }

        private static EventHandler InitializeHButtonCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void RemoveClickCommands()
        {
            btnCancel.Click -= btnCancelClickHandler;
            btnCopy.Click -= btnCopyClickHandler;
            btnEdit.Click -= btnEditClickHandler;
            btnRemove.Click -= btnRemoveClickHandler;
            btnUp.Click -= btnUpClickHandler;
            btnDown.Click -= btnDownClickHandler;
        }

        #region Event Handlers
        private void ManagedListBoxControl_Disposed(object? sender, EventArgs e)
        {
            RemoveClickCommands();
        }
        #endregion Event Handlers
    }
}
