using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            ((ISupportInitialize)this.radPanelMessages).BeginInit();
            this.radPanelMessages.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new Point(0, 0);
            this.radPanelMessages.Controls.Add((Control)_dialogFormMessageControl);

            ((ISupportInitialize)this.radPanelMessages).EndInit();
            this.radPanelMessages.ResumeLayout(true);
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
