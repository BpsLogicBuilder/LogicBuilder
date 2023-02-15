using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal partial class EditConstructorForm : Telerik.WinControls.UI.RadForm, IEditConstructorForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfigurationService _configurationService;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEditingControlFactory _editingControlFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;

        private ApplicationTypeInfo _application;
        private readonly Type assignedTo;

        public EditConstructorForm(
            IConfigurationService configurationService,
            IDialogFormMessageControl dialogFormMessageControl,
            IEditingControlFactory editingControlFactory,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IServiceFactory serviceFactory,
            Type assignedTo)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _editingControlFactory = editingControlFactory;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            this.assignedTo = assignedTo;
            Initialize();
        }

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{0C223B16-511C-4019-A272-7AB8CEC6E297}");

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void RequestDocumentUpdate()
        {
        }

        public void SetErrorMessage(string message) => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => _dialogFormMessageControl.SetMessage(message, title);

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.Enabled = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            radButton1.Click += RadButton1_Click;
            radButton2.Click += RadButton2_Click;
            splitPanelLeft.Click += SplitPanelLeft_Click;
        }

        private void SplitPanelLeft_Click(object? sender, EventArgs e)
        {
            radPanelFields.Controls.Clear();
        }

        private void RadButton2_Click(object? sender, EventArgs e)
        {
            //var constructorName = "DateTime_yy_mm_dd";
            //var constructorName = "ColumnSettingsParameters";
            //var constructorName = "RequestDetailsParameters";
            //var constructorName = "DetailFieldSettingParameters";
            var constructorName = "AggregateDefinitionParameters";
            var constructor = _configurationService.ConstructorList.Constructors[constructorName];
            Navigate((Control)_editingControlFactory.GetEditConstructorControl(this, constructor, assignedTo));
        }

        private void RadButton1_Click(object? sender, EventArgs e)
        {
            //var constructorName = "DropDownItemBindingParameters";
            //var constructorName = "CommandButtonParameters";
            var constructorName = "DirectiveDescriptionParameters";
            var constructor = _configurationService.ConstructorList.Constructors[constructorName];
            Navigate((Control)_editingControlFactory.GetEditConstructorControl(this, constructor, assignedTo));
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

        private void Navigate(Control newControl)
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            ((ISupportInitialize)radPanelFields).BeginInit();
            radPanelFields.SuspendLayout();

            ClearFieldControls();
            newControl.Dock = DockStyle.Fill;
            newControl.Location = new Point(0, 0);
            radPanelFields.Controls.Add(newControl);

            ((ISupportInitialize)radPanelFields).EndInit();
            radPanelFields.ResumeLayout(false);
            radPanelFields.PerformLayout();

            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);

            void ClearFieldControls()
            {
                foreach (Control control in radPanelFields.Controls)
                    control.Visible = false;

                radPanelFields.Controls.Clear();
            }
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }
        #endregion Event Handlers
    }
}
