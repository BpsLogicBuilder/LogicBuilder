using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment
{
    internal partial class ConfigureWebApiDeploymentForm : Telerik.WinControls.UI.RadForm
    {
        private readonly IFormInitializer _formInitializer;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IWebApiDeploymentItemFactory _webApiDeploymentItemFactory;

        public ConfigureWebApiDeploymentForm(
            IFormInitializer formInitializer,
            IDialogFormMessageControl dialogFormMessageControl,
            IWebApiDeploymentItemFactory webApiDeploymentItemFactory,
            WebApiDeployment webApiDeployment)
        {
            InitializeComponent();
            _webApiDeploymentItemFactory = webApiDeploymentItemFactory;
            _formInitializer = formInitializer;
            _dialogFormMessageControl = dialogFormMessageControl;

            WebApiDeployment = webApiDeployment;
            Initialize();
        }

        internal WebApiDeployment WebApiDeployment { get; private set;  }

        private void Initialize()
        {
            InitializeDialogFormMessageControl();

            this.FormClosing += ConfigureWebApiDeployment_FormClosing;

            radPanelUrls.AutoScroll = true;
            ((BorderPrimitive)radPanelUrls.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;
            ((BorderPrimitive)radPanelTableParent.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

            _formInitializer.SetFormDefaults(this, 351);

            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.CausesValidation = false;

            SetControlValues();
        }

        private void InitializeDialogFormMessageControl()
        {
            ((ISupportInitialize)this.radPanelBottom).BeginInit();
            this.radPanelBottom.SuspendLayout();
            ((ISupportInitialize)this.radPanelMessages).BeginInit();
            this.radPanelMessages.SuspendLayout();
            ((ISupportInitialize)this).BeginInit();
            this.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new Point(0, 0);
            this.radPanelMessages.Controls.Add((Control)_dialogFormMessageControl);

            ((ISupportInitialize)this.radPanelBottom).EndInit();
            this.radPanelBottom.ResumeLayout(false);
            ((ISupportInitialize)this.radPanelMessages).EndInit();
            this.radPanelMessages.ResumeLayout(false);
            ((ISupportInitialize)this).EndInit();
            this.ResumeLayout(false);
        }

        private void SetControlValues()
        {
            txtPostFileDataUrl.Text = WebApiDeployment.PostFileDataUrl;
            txtDeleteRulesUrl.Text = WebApiDeployment.DeleteRulesUrl;
            txtDeleteAllRulesUrl.Text = WebApiDeployment.DeleteAllRulesUrl;
        }

        private void UpdateFields()
        {
            WebApiDeployment = _webApiDeploymentItemFactory.GetWebApiDeployment
            (
                txtPostFileDataUrl.Text,
                Strings.defaultPostVariableMetaDataUrl,
                txtDeleteRulesUrl.Text,
                txtDeleteAllRulesUrl.Text
            );
        }

        private List<string> ValidateFields()
        {
            List<string> errors = new();
            ValidateUrlInputBox(txtPostFileDataUrl.Text, lblPostFileDataUrl.Text, errors);
            ValidateUrlInputBox(txtDeleteRulesUrl.Text, lblDeleteRulesUrl.Text, errors);
            ValidateUrlInputBox(txtDeleteAllRulesUrl.Text, lblDeleteAllRulesUrl.Text, errors);
            return errors;
        }

        private static void ValidateUrlInputBox(string input, string label, List<string> errors)
        {
            if (!Uri.IsWellFormedUriString(input, UriKind.Absolute))
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidUrlInputFormat, label));
            else if (!Regex.IsMatch(input, RegularExpressions.URL))
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidUrlInputFormat, label));
        }

        #region Event Handlers
        private void ConfigureWebApiDeployment_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                _dialogFormMessageControl.ClearMessage();
                IList<string> errors = ValidateFields();
                if (errors.Count > 0)
                {
                    _dialogFormMessageControl.SetErrorMessage(string.Join(Environment.NewLine, errors));
                    e.Cancel = true;
                    return;
                }
                else
                {
                    UpdateFields();
                }

            }
        }
        #endregion Event Handlers
    }
}
