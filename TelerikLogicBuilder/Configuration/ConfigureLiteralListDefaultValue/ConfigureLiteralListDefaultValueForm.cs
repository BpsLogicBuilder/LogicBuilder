using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue
{
    internal partial class ConfigureLiteralListDefaultValueForm : RadForm, IConfigureLiteralListDefaultValueForm
    {
        private readonly IFormInitializer _formInitializer;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IConfigureLiteralListDefaultValueControl _configureLiteralListDefaultValueControl;
        private readonly IList<string> existingDefaultValueItems;

        public ConfigureLiteralListDefaultValueForm(
            IConfigureLiteralListDefaultValueControlFactory configureLiteralListDefaultValueControlFactory,
            IFormInitializer formInitializer,
            IDialogFormMessageControl dialogFormMessageControl,
            IList<string> existingDefaultValueItems,
            Type type)
        {
            InitializeComponent();
            _formInitializer = formInitializer;
            _dialogFormMessageControl = dialogFormMessageControl;
            this.existingDefaultValueItems = existingDefaultValueItems;
            Type = type;
            _configureLiteralListDefaultValueControl = configureLiteralListDefaultValueControlFactory.GetConfigureLiteralListDefaultValueControl(this);
            Initialize();
        }

        public Type Type { get; }

        public IList<string> DefaultValueItems => _configureLiteralListDefaultValueControl.GetDefaultValueItems();

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
            _formInitializer.SetToConfigSize(this);

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
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
            InitializeControl(this.radPanelFill, (Control)_configureLiteralListDefaultValueControl);
        }

        private void LoadListBox()
        {
            _configureLiteralListDefaultValueControl.SetDefaultValueItems(existingDefaultValueItems);
        }
    }
}
