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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor
{
    internal partial class SelectConstructorForm : Telerik.WinControls.UI.RadForm, ISelectConstructorForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfiguredItemControlFactory _configuredItemControlFactory;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;

        private readonly Type assignedTo;
        private ApplicationTypeInfo _application;

        public SelectConstructorForm(
            IConfiguredItemControlFactory configuredItemControlFactory,
            IDialogFormMessageControl dialogFormMessageControl,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IServiceFactory serviceFactory,
            Type assignedTo)
        {
            InitializeComponent();
            _configuredItemControlFactory = configuredItemControlFactory;
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            this.assignedTo = assignedTo;
            Initialize();
        }

        private ISelectConstructorControl SelectConstructorControl
        {
            get
            {
                if (radPanelConstructor.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{1A50EE4A-4816-4196-A1C2-F83C4B32A036}");

                return (ISelectConstructorControl)radPanelConstructor.Controls[0];
            }
        }

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{9A5B3D2D-8E16-422A-98E9-E28462FBDC9E}");

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void SetErrorMessage(string message) => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => _dialogFormMessageControl.SetMessage(message, title);

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();
            InitializeConstructorControl();

            CollapsePanelBorder(radPanelConstructor);

            radPanelConstructor.Padding = new Padding(10, 0, 0, 10);
            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            FormClosing += SelectConstructorForm_FormClosing;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.Enabled = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }

        private void InitializeApplicationDropDownList()
        {
            ControlsLayoutUtility.LayoutApplicationGroupBox(this, radPanelApplication, radGroupBoxApplication, _applicationDropDownList);
        }

        private void InitializeConstructorControl()
        {
            ((ISupportInitialize)this.radPanelConstructor).BeginInit();
            this.radPanelConstructor.SuspendLayout();
            ISelectConstructorControl selectConstructorControl = _configuredItemControlFactory.GetSelectConstructorControl(this, assignedTo);
            selectConstructorControl.Dock = DockStyle.Fill;
            selectConstructorControl.Location = new Point(0, 0);
            this.radPanelConstructor.Controls.Add((Control)selectConstructorControl);
            selectConstructorControl.Changed += SelectConstructorControl_Changed;

            ((ISupportInitialize)this.radPanelConstructor).EndInit();
            this.radPanelConstructor.ResumeLayout(true);
        }

        private void InitializeDialogFormMessageControl()
        {
            ((ISupportInitialize)this.radPanelMessages).BeginInit();
            this.radPanelMessages.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new Point(0, 0);
            this.radPanelMessages.Controls.Add((Control)_dialogFormMessageControl);

            ((ISupportInitialize)this.radPanelMessages).EndInit();
            this.radPanelMessages.ResumeLayout(true);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void SelectConstructorControl_Changed(object? sender, EventArgs e)
        {
            btnOk.Enabled = SelectConstructorControl.IsValid;
        }

        private void SelectConstructorForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK && !SelectConstructorControl.IsValid)
                e.Cancel = true;
        }
        #endregion Event Handlers
    }
}
