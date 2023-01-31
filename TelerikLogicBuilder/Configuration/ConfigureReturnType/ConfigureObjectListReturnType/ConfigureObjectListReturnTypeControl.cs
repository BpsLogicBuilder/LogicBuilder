using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureObjectListReturnType
{
    internal partial class ConfigureObjectListReturnTypeControl : UserControl, IConfigureObjectListReturnTypeControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IReturnTypeFactory _returnTypeFactory;
        private readonly IConfigureReturnTypeForm configureReturnTypeForm;

        public ConfigureObjectListReturnTypeControl(
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

        public ReturnTypeBase ReturnType => _returnTypeFactory.GetListOfObjectsReturnType(cmbCoLiObjectType.Text, (ListType)cmbCoLiListType.SelectedValue);

        public void SetValues(ReturnTypeBase returnType)
        {
            if (returnType is not ListOfObjectsReturnType listOfObjectsReturnType)
                throw _exceptionHelper.CriticalException("{85F2A971-D813-4785-99F8-30E34519C60F}");

            RemoveEventHandlers();
            cmbCoLiObjectType.Text = listOfObjectsReturnType.ObjectType;
            cmbCoLiListType.SelectedValue = listOfObjectsReturnType.ListType;
            AddEventHandlers();
            ValidateOk();
        }

        private void AddEventHandlers()
        {
            cmbCoLiObjectType.TextChanged += CmbCoLiObjectType_TextChanged;
            cmbCoLiListType.SelectedIndexChanged += CmbCoLiListType_SelectedIndexChanged;
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
            helpProvider.SetHelpString(cmbCoLiObjectType, Strings.retTypeObjectHelp);
            helpProvider.SetHelpString(cmbCoLiListType, Strings.retTypeListTypeHelp);
            toolTip.SetToolTip(lblCoLiObjectType, Strings.retTypeObjectHelp);
            toolTip.SetToolTip(lblCoLiListType, Strings.retTypeListTypeHelp);
        }

        private void InitializeTableLayoutPanel()
        {
            float size_20 = 20F / 112 * 100;
            float size_30 = 30F / 112 * 100;
            float size_6 = 6F / 112 * 100;

            ((ISupportInitialize)this.radPanelTableParent).BeginInit();
            this.radPanelTableParent.SuspendLayout();

            this.tableLayoutPanel.RowStyles[0] = new RowStyle(SizeType.Percent, size_20);
            this.tableLayoutPanel.RowStyles[1] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[2] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[3] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[4] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[5] = new RowStyle(SizeType.Percent, size_20);

            ((ISupportInitialize)this.radPanelTableParent).EndInit();
            this.radPanelTableParent.ResumeLayout(true);
        }

        private void LoadDropDowns()
        {
            _radDropDownListHelper.LoadComboItems<ListType>(cmbCoLiListType);
        }

        private void RemoveEventHandlers()
        {
            cmbCoLiObjectType.TextChanged -= CmbCoLiObjectType_TextChanged;
            cmbCoLiListType.SelectedIndexChanged -= CmbCoLiListType_SelectedIndexChanged;
        }

        private void SetDefaultValues()
        {
            RemoveEventHandlers();
            cmbCoLiObjectType.Text = MiscellaneousConstants.DEFAULT_OBJECT_TYPE;
            cmbCoLiListType.SelectedValue = ListType.GenericList;
            AddEventHandlers();
        }

        private void ValidateFields()
        {
            configureReturnTypeForm.ClearMessage();

            List<string> errors = new();
            if (!FullyQualifiedClassNameRegex().IsMatch(cmbCoLiObjectType.Text))
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidClassNameFormat, lblCoLiObjectType.Text));
            if (cmbCoLiListType.SelectedIndex < 0)
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.returnTypeFieldNotSelectedFormat, lblCoLiListType.Text));
            if (errors.Count > 0)
                configureReturnTypeForm.SetErrorMessage(string.Join(Environment.NewLine, errors));

            ValidateOk();
        }

        private void ValidateOk()
        {
            configureReturnTypeForm.SetOkValidation
            (
                FullyQualifiedClassNameRegex().IsMatch(cmbCoLiObjectType.Text)
                && cmbCoLiListType.SelectedIndex > -1
            );
        }

        #region Event Handlers
        private void CmbCoLiListType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            ValidateFields();
        }

        private void CmbCoLiObjectType_TextChanged(object? sender, EventArgs e)
        {
            ValidateFields();
        }
        #endregion Event Handlers

        [GeneratedRegex(RegularExpressions.FULLYQUALIFIEDCLASSNAME)]
        private static partial Regex FullyQualifiedClassNameRegex();
    }
}
