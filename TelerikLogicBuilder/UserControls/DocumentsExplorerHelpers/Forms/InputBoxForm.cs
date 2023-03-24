using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
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
            InitializeGroupBoxPrompt();

            _formInitializer.SetFormDefaults
            (
                this,
                this.Size.Height - this.ClientSize.Height
                    + PerFontSizeConstants.SingleRowGroupBoxHeight
                    + (int)(5 * PerFontSizeConstants.SeparatorLineHeight)
                    + (int)(4 * PerFontSizeConstants.SingleLineHeight)
            );

            this.Size = new System.Drawing.Size(this.Width, this.MinimumSize.Height);

            radButtonOk.Enabled = false;
            radButtonOk.DialogResult = DialogResult.OK;
            radButtonCancel.DialogResult = DialogResult.Cancel;

            this.ActiveControl = radTextBoxInput;
            this.AcceptButton = radButtonOk;
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelCommandButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void InitializeGroupBoxPrompt()
        {
            ((ISupportInitialize)(this.radPanelFill)).BeginInit();
            this.radPanelFill.SuspendLayout();
            ((ISupportInitialize)radGroupBoxPrompt).BeginInit();
            radGroupBoxPrompt.SuspendLayout();

            radGroupBoxPrompt.Margin = new Padding(0);
            radGroupBoxPrompt.Padding = PerFontSizeConstants.SingleRowGroupBoxPadding;

            ((ISupportInitialize)radGroupBoxPrompt).EndInit();
            radGroupBoxPrompt.ResumeLayout(false);
            ((ISupportInitialize)this.radPanelFill).EndInit();
            this.radPanelFill.ResumeLayout(true);
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
