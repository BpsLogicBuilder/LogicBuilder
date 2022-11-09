using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    public partial class ManagedListBoxControl : UserControl, IManagedListBoxControl
    {
        public ManagedListBoxControl()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            CollapsePanelBorder(radPanelListBox);
            CollapsePanelBorder(radPanelUpDownButtons);
            CollapsePanelBorder(radPanelEditButtons);
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
        {
            ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;
        }

        public RadButton BtnUpdate => btnUpdate;

        public RadButton BtnCancel => btnCancel;

        public RadButton BtnCopy => btnCopy;

        public RadButton BtnEdit => btnEdit;

        public RadButton BtnRemove => btnRemove;

        public RadButton BtnUp => btnUp;

        public RadButton BtnDown => btnDown;

        public RadListControl ListBox => listBox;
    }
}
