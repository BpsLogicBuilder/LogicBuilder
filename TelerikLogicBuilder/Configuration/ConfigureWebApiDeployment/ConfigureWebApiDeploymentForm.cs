using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;
using System.ComponentModel;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment
{
    internal partial class ConfigureWebApiDeploymentForm : Telerik.WinControls.UI.RadForm, IConfigureWebApiDeploymentForm
    {
        private readonly IFormInitializer _formInitializer;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IWebApiDeploymentItemFactory _webApiDeploymentItemFactory;

        public ConfigureWebApiDeploymentForm(
            IFormInitializer formInitializer,
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            IWebApiDeploymentItemFactory webApiDeploymentItemFactory,
            WebApiDeployment webApiDeployment)
        {
            InitializeComponent();
            _webApiDeploymentItemFactory = webApiDeploymentItemFactory;
            _formInitializer = formInitializer;
            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();

            WebApiDeployment = webApiDeployment;
            Initialize();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WebApiDeployment WebApiDeployment { get; private set; }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeTableLayoutPanel();
            InitializeDialogFormMessageControl();

            this.FormClosing += ConfigureWebApiDeployment_FormClosing;

            CollapsePanelBorder(radPanelUrls);
            CollapsePanelBorder(radPanelTableParent);

            Padding groupBoxPadding = PerFontSizeConstants.GroupBoxPadding;
            _formInitializer.SetFormDefaults
            (
                this,
                this.Size.Height - this.ClientSize.Height
                    + groupBoxPadding.Top
                    + groupBoxPadding.Bottom
                    + (int)(2 * PerFontSizeConstants.BoundarySize)
                    + (int)(3 * PerFontSizeConstants.SeparatorLineHeight)
                    + (int)(3 * PerFontSizeConstants.SingleLineHeight)
                    + (int)PerFontSizeConstants.BottomPanelHeight
            );

            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.CausesValidation = false;

            SetControlValues();
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void InitializeTableLayoutPanel()
        {
            ControlsLayoutUtility.LayoutControls
            (
                radGroupBoxUrls,
                radPanelUrls,
                radPanelTableParent,
                tableLayoutPanel,
                3
            );
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
            List<string> errors = [];
            ValidateUrlInputBox(txtPostFileDataUrl.Text, lblPostFileDataUrl.Text, errors);
            ValidateUrlInputBox(txtDeleteRulesUrl.Text, lblDeleteRulesUrl.Text, errors);
            ValidateUrlInputBox(txtDeleteAllRulesUrl.Text, lblDeleteAllRulesUrl.Text, errors);
            return errors;
        }

        private static void ValidateUrlInputBox(string input, string label, List<string> errors)
        {
            if (!Uri.IsWellFormedUriString(input, UriKind.Absolute))
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidUrlInputFormat, label));
            else if (!UrlRegex().IsMatch(input))
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidUrlInputFormat, label));
        }

        #region Event Handlers
        private void ConfigureWebApiDeployment_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                _dialogFormMessageControl.ClearMessage();
                List<string> errors = ValidateFields();
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

        [GeneratedRegex(RegularExpressions.URL)]
        private static partial Regex UrlRegex();
        #endregion Event Handlers
    }
}
