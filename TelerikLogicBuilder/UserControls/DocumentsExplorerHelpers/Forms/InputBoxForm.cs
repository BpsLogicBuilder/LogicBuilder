using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms
{
    internal partial class InputBoxForm : Telerik.WinControls.UI.RadForm
    {
        private readonly IFormInitializer _formInitializer;

        public InputBoxForm(IFormInitializer formInitializer)
        {
            _formInitializer = formInitializer;
            InitializeComponent();
            Initialize();
        }

        private string regularExpression = string.Empty;

        internal string Input
        {
            get { return radTextBoxInput.Text; }
            set { radTextBoxInput.Text = value; }
        }

        internal void SetTitles(string regularExpression, string caption, string prompt)
        {
            this.regularExpression = regularExpression;
            this.Text = caption;
            this.radGroupBoxPrompt.Text = prompt;
        }

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 264);
            
            radGroupBoxPrompt.Anchor = AnchorConstants.AnchorsLeftTopRightBottom;
            radTextBoxInput.Anchor = AnchorConstants.AnchorsLeftRightBottom;

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
                dialogFormMessageControl1.SetErrorMessage(Strings.invalidInputMessage);
                radButtonOk.Enabled = false;
            }
            else
            {
                dialogFormMessageControl1.ClearMessage();
                radButtonOk.Enabled = true;
            }
        }
    }
}
