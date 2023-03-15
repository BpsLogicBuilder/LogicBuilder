using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType
{
    internal partial class ConfigureReturnTypeForm : Telerik.WinControls.UI.RadForm, IConfigureReturnTypeForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfigureReturnTypeControlFactory _configureReturnTypeControlFactory;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IRadDropDownListHelper _radDropDownListHelper;

        private readonly ReturnTypeBase initialReturnType;
        private ApplicationTypeInfo _application;

        public ConfigureReturnTypeForm(
            IConfigureReturnTypeControlFactory configureReturnTypeControlFactory,
            IDialogFormMessageControl dialogFormMessageControl,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            IList<string> genericArguments,
            ReturnTypeBase returnType)
        {
            InitializeComponent();
            _configureReturnTypeControlFactory = configureReturnTypeControlFactory;
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _radDropDownListHelper = radDropDownListHelper;
            initialReturnType = returnType;
            GenericArguments = genericArguments;
            Initialize();
        }

        private IConfigureReturnTypeControl CurrentReturnTypeControl
        {
            get
            {
                if (radPanelFields.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{C78ECDB0-FEB8-4D66-9270-649EB1361734}");

                return (IConfigureReturnTypeControl)radPanelFields.Controls[0];
            }
        }

        public IList<string> GenericArguments { get; }

        public ReturnTypeBase ReturnType => CurrentReturnTypeControl.ReturnType;

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{DE4E8F34-A743-403A-9397-5049EF1FF0DB}");

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void SetErrorMessage(string message)
           => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _dialogFormMessageControl.SetMessage(message, title);

        public void SetOkValidation(bool isValid)
        {
            btnOk.Enabled = isValid;
        }

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            radDropDownListCategory.SelectedIndexChanged += RadDropDownListCategory_SelectedIndexChanged;

            _formInitializer.SetFormDefaults(this, 448);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            LoadCategoryDropDown();
            SetInitialReturnType();
        }

        private void LoadCategoryDropDown()
        {
            _radDropDownListHelper.LoadComboItems<ReturnTypeCategory>(radDropDownListCategory);
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

        private void Navigate(Control newEditingControl)
        {
            NavigationUtility.Navigate(this.Handle, radPanelFields, newEditingControl);
        }

        private void Navigate()
        {
            ClearMessage();
            switch ((ReturnTypeCategory)radDropDownListCategory.SelectedValue)
            {
                case ReturnTypeCategory.GenericList:
                    Navigate((Control)_configureReturnTypeControlFactory.GetConfigureGenericListReturnTypeControl(this));
                    break;
                case ReturnTypeCategory.Generic:
                    Navigate((Control)_configureReturnTypeControlFactory.GetConfigureGenericReturnTypeControl(this));
                    break;
                case ReturnTypeCategory.LiteralList:
                    Navigate((Control)_configureReturnTypeControlFactory.GetConfigureLiteralListReturnTypeControl(this));
                    break;
                case ReturnTypeCategory.Literal:
                    Navigate((Control)_configureReturnTypeControlFactory.GetConfigureLiteralReturnTypeControl(this));
                    break;
                case ReturnTypeCategory.ObjectList:
                    Navigate((Control)_configureReturnTypeControlFactory.GetConfigureObjectListReturnTypeControl(this));
                    break;
                case ReturnTypeCategory.Object:
                    Navigate((Control)_configureReturnTypeControlFactory.GetConfigureObjectReturnTypeControl(this));
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{D25FBA49-55A5-4DDF-B638-C9958B0605DD}");
            }
        }

        private void SetInitialReturnType()
        {
            radDropDownListCategory.SelectedIndexChanged -= RadDropDownListCategory_SelectedIndexChanged;
            radDropDownListCategory.SelectedValue = initialReturnType.ReturnTypeCategory;
            Navigate();
            CurrentReturnTypeControl.SetValues(initialReturnType);
            radDropDownListCategory.SelectedIndexChanged += RadDropDownListCategory_SelectedIndexChanged;
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void RadDropDownListCategory_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            Navigate();
        }
        #endregion Event Handlers
    }
}
