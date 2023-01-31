using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureLiteralReturnType
{
    internal partial class ConfigureLiteralReturnTypeControl : UserControl, IConfigureLiteralReturnTypeControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IReturnTypeFactory _returnTypeFactory;
        private readonly IConfigureReturnTypeForm configureReturnTypeForm;

        public ConfigureLiteralReturnTypeControl(
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

        public ReturnTypeBase ReturnType => _returnTypeFactory.GetLiteralReturnType((LiteralFunctionReturnType)cmbLiLiteralType.SelectedValue);

        public void SetValues(ReturnTypeBase returnType)
        {
            if (returnType is not LiteralReturnType literalReturnType)
                throw _exceptionHelper.CriticalException("{978062A1-B132-46DF-BBB2-CC3FA3B41D91}");

            RemoveEventHandlers();
            cmbLiLiteralType.SelectedValue = literalReturnType.ReturnType;
            AddEventHandlers();
            ValidateOk();
        }

        void AddEventHandlers()
        {
            cmbLiLiteralType.SelectedIndexChanged += CmbLiLiteralType_SelectedIndexChanged;
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
            helpProvider.SetHelpString(this.cmbLiLiteralType, Strings.retTypeLiteralTypeHelp);
            toolTip.SetToolTip(this.lblLiLiteralType, Strings.retTypeLiteralTypeHelp);
        }

        private void InitializeTableLayoutPanel()
        {
            float size_20 = 20F / 76 * 100;
            float size_30 = 30F / 76 * 100;
            float size_6 = 6F / 76 * 100;

            ((ISupportInitialize)this.radPanelTableParent).BeginInit();
            this.radPanelTableParent.SuspendLayout();

            this.tableLayoutPanel.RowStyles[0] = new RowStyle(SizeType.Percent, size_20);
            this.tableLayoutPanel.RowStyles[1] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[2] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[3] = new RowStyle(SizeType.Percent, size_20);

            ((ISupportInitialize)this.radPanelTableParent).EndInit();
            this.radPanelTableParent.ResumeLayout(true);
        }

        private void LoadDropDowns()
        {
            _radDropDownListHelper.LoadComboItems<LiteralFunctionReturnType>(cmbLiLiteralType);
        }

        private void RemoveEventHandlers()
        {
            cmbLiLiteralType.SelectedIndexChanged -= CmbLiLiteralType_SelectedIndexChanged;
        }

        private void SetDefaultValues()
        {
            RemoveEventHandlers();
            cmbLiLiteralType.SelectedValue = LiteralFunctionReturnType.String;
            AddEventHandlers();
        }

        private void ValidateFields()
        {
            configureReturnTypeForm.ClearMessage();

            List<string> errors = new();
            if (cmbLiLiteralType.SelectedIndex < 0)
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.returnTypeFieldNotSelectedFormat, lblLiLiteralType.Text));
            if (errors.Count > 0)
                configureReturnTypeForm.SetErrorMessage(string.Join(Environment.NewLine, errors));

            ValidateOk();
        }

        private void ValidateOk()
        {
            configureReturnTypeForm.SetOkValidation(cmbLiLiteralType.SelectedIndex > -1);
        }

        #region Event Handlers
        private void CmbLiLiteralType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            ValidateFields();
        }
        #endregion Event Handlers
    }
}
