using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureGenericReturnType
{
    internal partial class ConfigureGenericReturnTypeControl : UserControl, IConfigureGenericReturnTypeControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IReturnTypeFactory _returnTypeFactory;
        private readonly IConfigureReturnTypeForm configureReturnTypeForm;

        public ConfigureGenericReturnTypeControl(
            IExceptionHelper exceptionHelper,
            IRadDropDownListHelper radDropDownListHelper,
            IReturnTypeFactory returnTypeFactory,
            IConfigureReturnTypeForm configureReturnTypeForm)
        {
            InitializeComponent();
            _exceptionHelper = exceptionHelper;
            _radDropDownListHelper = radDropDownListHelper;
            _returnTypeFactory = returnTypeFactory;
            this.configureReturnTypeForm = configureReturnTypeForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public ReturnTypeBase ReturnType => _returnTypeFactory.GetGenericReturnType(cmbGeGenericArgument.SelectedItem.Text);

        public void SetValues(ReturnTypeBase returnType)
        {
            if (returnType is not GenericReturnType genericReturnType)
                throw _exceptionHelper.CriticalException("{7C3EA453-EE5B-4E43-A3DD-EFF9DEC2DCD3}");

            RemoveEventHandlers();
            cmbGeGenericArgument.SelectedValue = genericReturnType.GenericArgumentName;
            AddEventHandlers();
            ValidateOk();
        }

        void AddEventHandlers()
        {
            cmbGeGenericArgument.SelectedIndexChanged += CmbGeGenericArgument_SelectedIndexChanged;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            AddEventHandlers();
            CollapsePanelBorder(radScrollablePanelReturnType);
            CollapsePanelBorder(radPanelTableParent);
            InitializeReturnTypeControls();
            LoadDropDowns();
            SetDefaultValues();
            ValidateOk();
        }

        private void InitializeReturnTypeControls()
        {
            helpProvider.SetHelpString(cmbGeGenericArgument, Strings.retTypeGenericArgumentHelp);
            toolTip.SetToolTip(lblGeGenericArgument, Strings.retTypeGenericArgumentHelp);
        }

        private void LoadDropDowns()
        {
            _radDropDownListHelper.LoadTextItems(cmbGeGenericArgument, configureReturnTypeForm.GenericArguments);
        }

        private void RemoveEventHandlers()
        {
            cmbGeGenericArgument.SelectedIndexChanged -= CmbGeGenericArgument_SelectedIndexChanged;
        }

        private void SetDefaultValues()
        {
            RemoveEventHandlers();
            if (configureReturnTypeForm.GenericArguments.Any())
                cmbGeGenericArgument.SelectedValue = configureReturnTypeForm.GenericArguments.First();
            AddEventHandlers();
        }

        private void ValidateFields()
        {
            configureReturnTypeForm.ClearMessage();

            List<string> errors = new();
            if (cmbGeGenericArgument.SelectedIndex < 0)
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.returnTypeFieldNotSelectedFormat, lblGeGenericArgument.Text));
            if (errors.Count > 0)
                configureReturnTypeForm.SetErrorMessage(string.Join(Environment.NewLine, errors));

            ValidateOk();
        }

        private void ValidateOk()
        {
            configureReturnTypeForm.SetOkValidation(cmbGeGenericArgument.SelectedIndex > -1);
        }

        #region Event Handlers
        private void CmbGeGenericArgument_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            ValidateFields();
        }
        #endregion Event Handlers

    }
}
