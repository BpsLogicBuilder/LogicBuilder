using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ListBox.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class ManagedListBoxControl : UserControl, IManagedListBoxControl
    {
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
            InitializeHButtonCommand(btnCancel, new ListBoxManagerCancelCommand(radListBoxManager));
            InitializeHButtonCommand(btnCopy, new ListBoxManagerCopyCommand(radListBoxManager));
            InitializeHButtonCommand(btnEdit, new ListBoxManagerEditCommand(radListBoxManager));
            InitializeHButtonCommand(btnRemove, new ListBoxManagerRemoveCommand(radListBoxManager));
            InitializeHButtonCommand(btnUp, new ListBoxManagerMoveUpCommand(radListBoxManager));
            InitializeHButtonCommand(btnDown, new ListBoxManagerMoveDownCommand(radListBoxManager));
        }

        public void DisableControls() => Enable(false);

        public void EnableControls() => Enable(true);

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
            CollapsePanelBorder(radPanelListBox);
            CollapsePanelBorder(radPanelUpDownButtons);
            CollapsePanelBorder(radPanelEditButtons);
        }

        private static void InitializeHButtonCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }
    }
}
