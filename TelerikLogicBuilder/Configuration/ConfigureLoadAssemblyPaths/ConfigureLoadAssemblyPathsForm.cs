using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths
{
    internal partial class ConfigureLoadAssemblyPathsForm : Telerik.WinControls.UI.RadForm, IConfigureLoadAssemblyPathsForm
    {
        private readonly IFormInitializer _formInitializer;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IConfigureLoadAssemblyPathsControl _loadAssemblyPathsControl;

        private readonly IList<string> existingPaths;

        public ConfigureLoadAssemblyPathsForm(
            IConfigureLoadAssemblyPathsControlFactory configurationLoadAssemblyPathsControlFactory,
            IFormInitializer formInitializer,
            IDialogFormMessageControl dialogFormMessageControl,
            IList<string> existingPaths)
        {
            InitializeComponent();
            _formInitializer = formInitializer;
            _dialogFormMessageControl = dialogFormMessageControl;
            _loadAssemblyPathsControl = configurationLoadAssemblyPathsControlFactory.GetLoadAssemblyPathsControl(this);
            this.existingPaths = existingPaths;
            Initialize();
        }

        public IList<string> Paths => _loadAssemblyPathsControl.GetPaths();

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
            CollapsePanelBorder(radPanelFill);
            InitializeControls();
            _formInitializer.SetFormDefaults(this, 408);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            LoadListBox();
        }

        private void InitializeControls()
        {
            ((ISupportInitialize)this.radPanelBottom).BeginInit();
            this.radPanelBottom.SuspendLayout();
            ((ISupportInitialize)this.radPanelMessages).BeginInit();
            this.radPanelMessages.SuspendLayout();
            ((ISupportInitialize)this.radPanelFill).BeginInit();
            this.radPanelFill.SuspendLayout();
            ((ISupportInitialize)this).BeginInit();
            this.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new Point(0, 0);
            this.radPanelMessages.Controls.Add((Control)_dialogFormMessageControl);

            Control loadAssemblyPathsControl = (Control)_loadAssemblyPathsControl;
            loadAssemblyPathsControl.Dock = DockStyle.Fill;
            loadAssemblyPathsControl.Location = new Point(0, 0);
            this.radPanelFill.Controls.Add(loadAssemblyPathsControl);

            ((ISupportInitialize)this.radPanelBottom).EndInit();
            this.radPanelBottom.ResumeLayout(false);
            ((ISupportInitialize)this.radPanelMessages).EndInit();
            this.radPanelMessages.ResumeLayout(false);
            ((ISupportInitialize)this.radPanelFill).EndInit();
            this.radPanelFill.ResumeLayout(false);
            ((ISupportInitialize)this).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadListBox()
        {
            _loadAssemblyPathsControl.SetPaths(existingPaths);
        }
    }
}
