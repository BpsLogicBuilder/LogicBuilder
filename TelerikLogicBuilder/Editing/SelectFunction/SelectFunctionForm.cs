using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
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
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction
{
    internal partial class SelectFunctionForm : RadForm, ISelectFunctionForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfiguredItemControlFactory _configuredItemControlFactory;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;

        private readonly Type assignedTo;
        private ApplicationTypeInfo _application;

        public SelectFunctionForm(
            IConfiguredItemControlFactory configuredItemControlFactory,
            IDialogFormMessageControl dialogFormMessageControl,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IServiceFactory serviceFactory,
            Type assignedTo,
            IDictionary<string, Function> functionDictionary,
            IList<TreeFolder> treeFolders)
        {
            InitializeComponent();
            _configuredItemControlFactory = configuredItemControlFactory;
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            this.assignedTo = assignedTo;
            FunctionDictionary = functionDictionary;
            TreeFolders = treeFolders;
            Initialize();
        }

        private ISelectFunctionControl SelectFunctionControl
        {
            get
            {
                if (radPanelFunction.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{8ED3F6E1-CD64-49A4-9385-02139C555F0A}");

                return (ISelectFunctionControl)radPanelFunction.Controls[0];
            }
        }

        public IList<TreeFolder> TreeFolders { get; }

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{78F829E4-B44E-42F7-97F5-216ADBF96595}");

        public IDictionary<string, Function> FunctionDictionary { get; }

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
            InitializeFunctionControl();

            CollapsePanelBorder(radPanelFunction);

            radPanelFunction.Padding = new Padding(10, 0, 0, 10);
            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            FormClosing += SelectFunctionForm_FormClosing;

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

        private void InitializeFunctionControl()
        {
            ((ISupportInitialize)this.radPanelFunction).BeginInit();
            this.radPanelFunction.SuspendLayout();
            ISelectFunctionControl selectFunctionControl = _configuredItemControlFactory.GetSelectFunctionControl(this, assignedTo);
            selectFunctionControl.Dock = DockStyle.Fill;
            selectFunctionControl.Location = new Point(0, 0);
            this.radPanelFunction.Controls.Add((Control)selectFunctionControl);
            selectFunctionControl.Changed += SelectFunctionControl_Changed;

            ((ISupportInitialize)this.radPanelFunction).EndInit();
            this.radPanelFunction.ResumeLayout(true);
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

        private void SelectFunctionControl_Changed(object? sender, EventArgs e)
        {
            btnOk.Enabled = SelectFunctionControl.IsValid;
        }

        private void SelectFunctionForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK && !SelectFunctionControl.IsValid)
                e.Cancel = true;
        }
        #endregion Event Handlers
    }
}
