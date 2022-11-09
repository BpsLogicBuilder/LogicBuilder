using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Forms
{
    internal partial class ConfigureLoadAssemblyPaths : Telerik.WinControls.UI.RadForm, IConfigureLoadAssemblyPaths
    {
        private readonly IConfigurationItemFactory _configurationItemFactory;
        private readonly IFormInitializer _formInitializer;
        private readonly DialogFormMessageControl _dialogFormMessageControl;
        private readonly ILoadAssemblyPathsControl _loadAssemblyPathsControl;

        public RadListControl ListBox => _loadAssemblyPathsControl.ListBox;

        public IList<string> Paths => ListBox.Items.Select(i => ((AssemblyPath)i.Value).Path).ToArray();

        public ConfigureLoadAssemblyPaths(
            IConfigurationItemFactory configurationItemFactory,
            IConfigurationControlFactory configurationControlFactory,
            IFormInitializer formInitializer,
            DialogFormMessageControl dialogFormMessageControl,
            IList<string> existingPaths)
        {
            InitializeComponent();
            _configurationItemFactory = configurationItemFactory;
            _formInitializer = formInitializer;
            _dialogFormMessageControl = dialogFormMessageControl;
            _loadAssemblyPathsControl = configurationControlFactory.GetLoadAssemblyPathsControl(this);
            this.existingPaths = existingPaths;
            Initialize();
        }

        private readonly IList<string> existingPaths;

        public void ClearMessage()
        {
            _dialogFormMessageControl.ClearMessage();
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

        private void LoadListBox()
        {
            ListBox.Items.AddRange
            (
                existingPaths
                    .Select(p => _configurationItemFactory.GetAssemblyPath(p.Trim()))
                    .Select(ap => new RadListDataItem(ap.ToString(), ap))
            );
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
            this.radPanelMessages.Controls.Add(_dialogFormMessageControl);

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
    }
}
