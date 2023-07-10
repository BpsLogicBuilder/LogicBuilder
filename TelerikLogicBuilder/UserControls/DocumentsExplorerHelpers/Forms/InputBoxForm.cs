using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms
{
    internal partial class InputBoxForm : Telerik.WinControls.UI.RadForm, IInputBoxForm
    {
        private readonly IFormInitializer _formInitializer;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;

        public InputBoxForm(IFormInitializer formInitializer, IDialogFormMessageControlFactory dialogFormMessageControlFactory)
        {
            _formInitializer = formInitializer;
            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();
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

            radTextBoxInput.AutoSize = false;
            radTextBoxInput.Dock = DockStyle.Fill;
            InitializeGroupBoxPrompt();

            radPanelFill.Size = new System.Drawing.Size(radPanelFill.Width, 0);
            radPanelFill.Margin = new Padding(0);
            _formInitializer.SetFormDefaults
            (
                this,
                this.Size.Height - this.ClientSize.Height
                    + radPanelTop.Height
                    + radPanelBottom.Height
            );
            this.Size = new System.Drawing.Size(this.Width, 0);

            ControlsLayoutUtility.CollapsePanelBorder(radPanelTop);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelFill);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelBottom);

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
            ControlsLayoutUtility.LayoutSingleRowGroupBox(radPanelTop, radGroupBoxPrompt);
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
