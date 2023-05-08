using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment
{
    internal partial class SelectFragmentForm : Telerik.WinControls.UI.RadForm, ISelectFragmentForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfiguredItemControlFactory _configuredItemControlFactory;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;

        private ApplicationTypeInfo _application;

        public SelectFragmentForm(
            IConfiguredItemControlFactory configuredItemControlFactory,
            IDialogFormMessageControl dialogFormMessageControl,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IServiceFactory serviceFactory)
        {
            InitializeComponent();
            _configuredItemControlFactory = configuredItemControlFactory;
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            Initialize();
        }

        private ISelectFragmentControl SelectFragmentControl
        {
            get
            {
                if (radPanelFragment.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{27058E38-910D-47D2-9C5B-F5782E6FD54C}");

                return (ISelectFragmentControl)radPanelFragment.Controls[0];
            }
        }

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{7D150B3D-BDA3-4930-B95C-BDA400114EB4}");

        public string FragmentName => SelectFragmentControl.FragmentName ?? throw _exceptionHelper.CriticalException("{4F2AF920-68DD-4AFF-971E-354E89B9184F}");

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void SetFragment(string fragmentName)
            => SelectFragmentControl.SetFragment(fragmentName);

        public void SetErrorMessage(string message) => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => _dialogFormMessageControl.SetMessage(message, title);

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();
            InitializeFragmentControl();

            CollapsePanelBorder(radPanelFragment);

            radPanelFragment.Padding = new Padding(10, 0, 0, 10);
            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            FormClosing += SelectFragmentForm_FormClosing;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.Enabled = SelectFragmentControl.IsValid;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }

        private void InitializeApplicationDropDownList()
        {
            ControlsLayoutUtility.LayoutApplicationGroupBox(this, radPanelApplication, radGroupBoxApplication, _applicationDropDownList);
        }

        private void InitializeFragmentControl()
        {
            ((ISupportInitialize)this.radPanelFragment).BeginInit();
            this.radPanelFragment.SuspendLayout();
            ISelectFragmentControl selectFragmentControl = _configuredItemControlFactory.GetSelectFragmentControl(this);
            selectFragmentControl.Dock = DockStyle.Fill;
            selectFragmentControl.Location = new Point(0, 0);
            this.radPanelFragment.Controls.Add((Control)selectFragmentControl);
            selectFragmentControl.Changed += SelectFragmentControl_Changed;

            ((ISupportInitialize)this.radPanelFragment).EndInit();
            this.radPanelFragment.ResumeLayout(true);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void SelectFragmentControl_Changed(object? sender, EventArgs e)
        {
            btnOk.Enabled = SelectFragmentControl.IsValid;
        }

        private void SelectFragmentForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK && !SelectFragmentControl.IsValid)
                e.Cancel = true;
        }
        #endregion Event Handlers
    }
}
