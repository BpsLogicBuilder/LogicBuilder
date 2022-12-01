using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain
{
    internal partial class ConfigureLiteralDomainForm : RadForm, IConfigureLiteralDomainForm
    {
        private readonly IFormInitializer _formInitializer;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IConfigureLiteralDomainControl _configureLiteralDomainControl;

        private readonly IList<string> existingDomainItems;

        public ConfigureLiteralDomainForm(
            IConfigureLiteralDomainControlFactory configureLiteralDomainControlFactory,
            IFormInitializer formInitializer,
            IDialogFormMessageControl dialogFormMessageControl,
            IList<string> existingDomainItems,
            Type type)
        {
            InitializeComponent();
            _formInitializer = formInitializer;
            _dialogFormMessageControl = dialogFormMessageControl;
            this.existingDomainItems = existingDomainItems;
            Type = type;
            _configureLiteralDomainControl = configureLiteralDomainControlFactory.GetConfigureLiteralDomainControl(this);
            Initialize();
        }

        public IList<string> DomainItems => _configureLiteralDomainControl.GetDomainItems();

        public Type Type { get; }

        public void ClearMessage()
        {
            _dialogFormMessageControl.ClearMessage();
        }

        public void DisableControlsDuringEdit(bool disable)
        {
            radPanelButtons.Enabled = !disable;
        }

        public void SetErrorMessage(string message)
        {
            _dialogFormMessageControl.SetErrorMessage(message);
        }

        public void SetMessage(string message, string title = "")
        {
            _dialogFormMessageControl.SetMessage(message, title);
        }

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
            InitializeControl(this.radPanelFill, (Control)_configureLiteralDomainControl);
        }

        private void LoadListBox()
        {
            _configureLiteralDomainControl.SetDomainItems(existingDomainItems);
        }
    }
}
