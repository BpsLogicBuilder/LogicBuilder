using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable
{
    internal partial class SelectVariableForm : Telerik.WinControls.UI.RadForm, ISelectVariableForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly ISelectEditingControlFactory _selectEditingControlFactory;

        private readonly Type assignedTo;
        private ApplicationTypeInfo _application;

        public SelectVariableForm(
            IDialogFormMessageControl dialogFormMessageControl,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            ISelectEditingControlFactory selectEditingControlFactory,
            IServiceFactory serviceFactory,
            Type assignedTo)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _selectEditingControlFactory = selectEditingControlFactory;
            this.assignedTo = assignedTo;
            Initialize();
        }

        private ISelectVariableControl SelectVariableControl
        {
            get
            {
                if (radPanelVariable.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{20D1696E-3FB1-45A3-8CAF-47C9D5CD236A}");

                return (ISelectVariableControl)radPanelVariable.Controls[0];
            }
        }

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{7E9B95D2-1270-4D33-A41A-218242610278}");

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void RequestDocumentUpdate()
        {
        }

        public void SetErrorMessage(string message) => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => _dialogFormMessageControl.SetMessage(message, title);

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();
            InitializeVariableControl();

            CollapsePanelBorder(radPanelVariable);

            radPanelVariable.Padding = new Padding(10, 0, 0, 10);
            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            FormClosing += SelectVariableForm_FormClosing;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.Enabled = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }

        private void InitializeApplicationDropDownList()
        {
            ((ISupportInitialize)this.radGroupBoxApplication).BeginInit();
            this.radGroupBoxApplication.SuspendLayout();

            _applicationDropDownList.Dock = DockStyle.Fill;
            _applicationDropDownList.Location = new Point(0, 0);
            this.radGroupBoxApplication.Controls.Add((Control)_applicationDropDownList);

            ((ISupportInitialize)this.radGroupBoxApplication).EndInit();
            this.radGroupBoxApplication.ResumeLayout(true);
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

        private void InitializeVariableControl()
        {
            ((ISupportInitialize)this.radPanelVariable).BeginInit();
            this.radPanelVariable.SuspendLayout();
            ISelectVariableControl selectVariableControl = _selectEditingControlFactory.GetSelectVariableControl(this, assignedTo);
            selectVariableControl.Dock = DockStyle.Fill;
            selectVariableControl.Location = new Point(0, 0);
            this.radPanelVariable.Controls.Add((Control)selectVariableControl);
            selectVariableControl.Changed += SelectVariableControl_Changed;

            ((ISupportInitialize)this.radPanelVariable).EndInit();
            this.radPanelVariable.ResumeLayout(true);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void SelectVariableControl_Changed(object? sender, EventArgs e)
        {
            btnOk.Enabled = SelectVariableControl.IsValid;
        }

        private void SelectVariableForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK && !SelectVariableControl.IsValid)
                e.Cancel = true;
        }
        #endregion Event Handlers
    }
}
