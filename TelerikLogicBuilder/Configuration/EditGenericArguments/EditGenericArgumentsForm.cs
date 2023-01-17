using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments
{
    internal partial class EditGenericArgumentsForm : Telerik.WinControls.UI.RadForm, IEditGenericArgumentsForm
    {
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IFormInitializer _formInitializer;

        private readonly IEditGenericArgumentsControl editGenericArgumentsControl;
        private readonly IList<string> existingArguments;

        public EditGenericArgumentsForm(
            IDialogFormMessageControl dialogFormMessageControl,
            IEditGenericArgumentsControlFactory editGenericArgumentsControlFactory,
            IFormInitializer formInitializer,
            IList<string> existingArguments)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;
            _formInitializer = formInitializer;
            this.existingArguments = existingArguments;
            editGenericArgumentsControl = editGenericArgumentsControlFactory.GetEditGenericArgumentsControl(this);
            Initialize();
        }

        public IList<string> Arguments => editGenericArgumentsControl.GetArguments();

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void DisableControlsDuringEdit(bool disable) => radPanelButtons.Enabled = !disable;

        public void SetErrorMessage(string message) => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => _dialogFormMessageControl.SetMessage(message, title);

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelFill);
            InitializeControls();
            _formInitializer.SetFormDefaults(this, 408);

            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            LoadListBox();
        }

        private static void InitializeControl(RadPanel radPanel, Control control)
        {
            ((ISupportInitialize)radPanel).BeginInit();
            radPanel.SuspendLayout();

            control.Dock = DockStyle.Fill;
            control.Location = new Point(0, 0);
            radPanel.Controls.Add(control);

            ((ISupportInitialize)radPanel).EndInit();
            radPanel.ResumeLayout(true);
        }

        private void InitializeControls()
        {
            InitializeControl(this.radPanelMessages, (Control)_dialogFormMessageControl);
            InitializeControl(this.radPanelFill, (Control)editGenericArgumentsControl);
        }

        private void LoadListBox()
        {
            editGenericArgumentsControl.SetArguments(existingArguments);
        }
    }
}
