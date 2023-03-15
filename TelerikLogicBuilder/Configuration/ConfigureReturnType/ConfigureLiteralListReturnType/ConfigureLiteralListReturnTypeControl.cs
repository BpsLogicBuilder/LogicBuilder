using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureLiteralListReturnType
{
    internal partial class ConfigureLiteralListReturnTypeControl : UserControl, IConfigureLiteralListReturnTypeControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IReturnTypeFactory _returnTypeFactory;
        private readonly IConfigureReturnTypeForm configureReturnTypeForm;

        public ConfigureLiteralListReturnTypeControl(
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

        public ReturnTypeBase ReturnType => _returnTypeFactory.GetListOfLiteralsReturnType((LiteralFunctionReturnType)cmbLiLiLiteralType.SelectedValue, (ListType)cmbLiLiListType.SelectedValue);

        public void SetValues(ReturnTypeBase returnType)
        {
            if (returnType is not ListOfLiteralsReturnType listOfLiteralsReturnType)
                throw _exceptionHelper.CriticalException("{EE1E566E-487A-487E-9F12-834C00D4CC5A}");

            RemoveEventHandlers();
            cmbLiLiLiteralType.SelectedValue = listOfLiteralsReturnType.UnderlyingLiteralType;
            cmbLiLiListType.SelectedValue = listOfLiteralsReturnType.ListType;
            AddEventHandlers();
            ValidateOk();
        }

        void AddEventHandlers()
        {
            cmbLiLiLiteralType.SelectedIndexChanged += CmbLiLiLiteralType_SelectedIndexChanged;
            cmbLiLiListType.SelectedIndexChanged += CmbLiLiListType_SelectedIndexChanged;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeTableLayoutPanel();
            CollapsePanelBorder(radScrollablePanelReturnType);
            CollapsePanelBorder(radPanelTableParent);
            InitializeReturnTypeControls();
            LoadDropDowns();
            SetDefaultValues();
            ValidateOk();
        }

        private void InitializeReturnTypeControls()
        {
            helpProvider.SetHelpString(this.cmbLiLiLiteralType, Strings.retTypeLiteralTypeHelp);
            helpProvider.SetHelpString(this.cmbLiLiListType, Strings.retTypeListTypeHelp);
            toolTip.SetToolTip(this.lblLiLiLiteralType, Strings.retTypeLiteralTypeHelp);
            toolTip.SetToolTip(this.lblLiLiListType, Strings.retTypeListTypeHelp);
        }

        private void InitializeTableLayoutPanel()
        {
            ControlsLayoutUtility.LayoutControls
            (
                groupBoxReturnType,
                radScrollablePanelReturnType,
                radPanelTableParent,
                tableLayoutPanel,
                2
            );
        }

        private void LoadDropDowns()
        {
            _radDropDownListHelper.LoadComboItems<LiteralFunctionReturnType>(cmbLiLiLiteralType);
            _radDropDownListHelper.LoadComboItems<ListType>(cmbLiLiListType);
        }

        private void RemoveEventHandlers()
        {
            cmbLiLiLiteralType.SelectedIndexChanged -= CmbLiLiLiteralType_SelectedIndexChanged;
            cmbLiLiListType.SelectedIndexChanged -= CmbLiLiListType_SelectedIndexChanged;
        }

        private void SetDefaultValues()
        {
            RemoveEventHandlers();
            cmbLiLiLiteralType.SelectedValue = LiteralFunctionReturnType.String;
            cmbLiLiListType.SelectedValue = ListType.GenericList;
            AddEventHandlers();
        }

        private void ValidateFields()
        {
            configureReturnTypeForm.ClearMessage();

            List<string> errors = new();
            if (cmbLiLiLiteralType.SelectedIndex < 0)
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.returnTypeFieldNotSelectedFormat, lblLiLiLiteralType.Text));
            if (cmbLiLiListType.SelectedIndex < 0)
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.returnTypeFieldNotSelectedFormat, lblLiLiListType.Text));
            if (errors.Count > 0)
                configureReturnTypeForm.SetErrorMessage(string.Join(Environment.NewLine, errors));

            ValidateOk();
        }

        private void ValidateOk()
        {
            configureReturnTypeForm.SetOkValidation(cmbLiLiLiteralType.SelectedIndex > -1 && cmbLiLiListType.SelectedIndex > -1);
        }

        #region Event Handlers
        private void CmbLiLiListType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            ValidateFields();
        }

        private void CmbLiLiLiteralType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            ValidateFields();
        }
        #endregion Event Handlers
    }
}
