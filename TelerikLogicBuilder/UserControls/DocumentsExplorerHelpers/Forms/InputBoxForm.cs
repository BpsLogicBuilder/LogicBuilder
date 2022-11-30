using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms
{
    internal partial class InputBoxForm : Telerik.WinControls.UI.RadForm, IInputBoxForm
    {
        private readonly IFormInitializer _formInitializer;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;

        public InputBoxForm(IFormInitializer formInitializer, IDialogFormMessageControl dialogFormMessageControl)
        {
            _formInitializer = formInitializer;
            _dialogFormMessageControl = dialogFormMessageControl;
            InitializeComponent();
            Initialize();
        }

        private void InitializeDialogFormMessageControl()
        {
            ((ISupportInitialize)(this.radPanelMessages)).BeginInit();
            this.radPanelMessages.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new System.Drawing.Point(0, 0);
            this.radPanelMessages.Controls.Add((Control)_dialogFormMessageControl);

            ((ISupportInitialize)(this.radPanelMessages)).EndInit();
            this.radPanelMessages.ResumeLayout(true);
        }

        private string regularExpression = string.Empty;

        public string Input
        {
            get { return radTextBoxInput.Text; }
            set { radTextBoxInput.Text = value; }
        }

        public void SetTitles(string regularExpression, string caption, string prompt)
        {
            this.regularExpression = regularExpression;
            this.Text = caption;
            this.radGroupBoxPrompt.Text = prompt;
        }

        private void Initialize()
        {
            InitializeDialogFormMessageControl();

            radGroupBoxPrompt.Anchor = AnchorConstants.AnchorsLeftTopRightBottom;
            radTextBoxInput.Anchor = AnchorConstants.AnchorsLeftTopRight;

            _formInitializer.SetFormDefaults(this, 264);

            radButtonOk.Enabled = false;
            radButtonOk.DialogResult = DialogResult.OK;
            radButtonCancel.DialogResult = DialogResult.Cancel;

            this.ActiveControl = radTextBoxInput;
            this.AcceptButton = radButtonOk;
        }

        private void RadTextBoxInput_TextChanged(object sender, EventArgs e)
        {
            if (radTextBoxInput.Text.Trim().Length == 0)
            {
                radButtonOk.Enabled = false;
            }
            else if (regularExpression.Length != 0 && !Regex.IsMatch(radTextBoxInput.Text, regularExpression))
            {
                _dialogFormMessageControl.SetErrorMessage(Strings.invalidInputMessage);
                radButtonOk.Enabled = false;
            }
            else
            {
                _dialogFormMessageControl.ClearMessage();
                radButtonOk.Enabled = true;
            }
        }
    }
}
