using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFromDomain
{
    internal partial class SelectFromDomainForm : Telerik.WinControls.UI.RadForm, ISelectFromDomainForm
    {
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IFormInitializer _formInitializer;

        private readonly IList<string> domain;

        public SelectFromDomainForm(
            IDialogFormMessageControl dialogFormMessageControl,
            IFormInitializer formInitializer,
            IList<string> domain, 
            string comments)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;
            _formInitializer = formInitializer;
            _dialogFormMessageControl.SetMessage(comments);
            this.domain = domain;
            Initialize();
        }

        public string SelectedItem => (string)radListControlDomain.SelectedValue;

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            ControlsLayoutUtility.LayoutGroupBox(radPanelFill, radGroupBoxDomainList);

            radListControlDomain.SelectedIndexChanged += RadListControlDomain_SelectedIndexChanged;
            radListControlDomain.SelectionMode = SelectionMode.One;

            _formInitializer.SetFormDefaults(this, 559);

            btnCancel.CausesValidation = false;
            btnOk.Enabled = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            LoadListBox();
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void LoadListBox()
        {
            if (this.domain.Count < 1)
                return;

            radListControlDomain.Items.AddRange(this.domain.OrderBy(i => i).Select(i => new RadListDataItem(i, i)));
            radListControlDomain.SelectedIndex = 0;
        }

        private void RadListControlDomain_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e) 
            => btnOk.Enabled = radListControlDomain.SelectedIndex != -1;
    }
}
