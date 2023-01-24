using ABIS.LogicBuilder.FlowBuilder.Enums;
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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureGenericListReturnType
{
    internal partial class ConfigureGenericListReturnTypeControl : UserControl, IConfigureGenericListReturnTypeControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IReturnTypeFactory _returnTypeFactory;
        private readonly IConfigureReturnTypeForm configureReturnTypeForm;

        public ConfigureGenericListReturnTypeControl(
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

        public ReturnTypeBase ReturnType => _returnTypeFactory.GetListOfGenericsReturnType(cmbGeLiGenericArgument.SelectedItem.Text, (ListType)cmbGeLiListType.SelectedValue);

        public void SetValues(ReturnTypeBase returnType)
        {
            if (returnType is not ListOfGenericsReturnType listOfGenericsReturnType)
                throw _exceptionHelper.CriticalException("{26EC2AC6-6A42-4CE2-840B-245318C88C2D}");

            RemoveEventHandlers();
            cmbGeLiGenericArgument.SelectedValue = listOfGenericsReturnType.GenericArgumentName;
            cmbGeLiListType.SelectedValue = listOfGenericsReturnType.ListType;
            AddEventHandlers();
            ValidateOk();
        }

        void AddEventHandlers()
        {
            cmbGeLiGenericArgument.SelectedIndexChanged += CmbGeLiGenericArgument_SelectedIndexChanged;
            cmbGeLiListType.SelectedIndexChanged += CmbGeLiListType_SelectedIndexChanged;
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
            helpProvider.SetHelpString(this.cmbGeLiGenericArgument, Strings.retTypeGenericArgumentHelp);
            helpProvider.SetHelpString(this.cmbGeLiListType, Strings.retTypeListTypeHelp);
            toolTip.SetToolTip(this.lblGeLiGenericArgument, Strings.retTypeGenericArgumentHelp);
            toolTip.SetToolTip(this.lblGeLiListType, Strings.retTypeListTypeHelp);
        }

        private void LoadDropDowns()
        {
            _radDropDownListHelper.LoadTextItems(cmbGeLiGenericArgument, configureReturnTypeForm.GenericArguments);
            _radDropDownListHelper.LoadComboItems<ListType>(cmbGeLiListType);
        }

        void RemoveEventHandlers()
        {
            cmbGeLiGenericArgument.SelectedIndexChanged -= CmbGeLiGenericArgument_SelectedIndexChanged;
            cmbGeLiListType.SelectedIndexChanged -= CmbGeLiListType_SelectedIndexChanged;
        }

        private void SetDefaultValues()
        {
            RemoveEventHandlers();
            if (configureReturnTypeForm.GenericArguments.Any())
                cmbGeLiGenericArgument.SelectedValue = configureReturnTypeForm.GenericArguments.First();
            cmbGeLiListType.SelectedValue = ListType.GenericList;
            AddEventHandlers();
        }

        private void ValidateFields()
        {
            configureReturnTypeForm.ClearMessage();

            List<string> errors = new();
            if (cmbGeLiGenericArgument.SelectedIndex < 0)
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.returnTypeFieldNotSelectedFormat, lblGeLiGenericArgument.Text));
            if (cmbGeLiListType.SelectedIndex < 0)
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.returnTypeFieldNotSelectedFormat, lblGeLiListType.Text));
            if (errors.Count > 0)
                configureReturnTypeForm.SetErrorMessage(string.Join(Environment.NewLine, errors));

            ValidateOk();
        }

        private void ValidateOk()
        {
            configureReturnTypeForm.SetOkValidation(cmbGeLiGenericArgument.SelectedIndex > -1 && cmbGeLiListType.SelectedIndex > -1);
        }

        #region Event Handlers
        private void CmbGeLiListType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            ValidateFields();
        }

        private void CmbGeLiGenericArgument_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            ValidateFields();
        }
        #endregion Event Handlers
    }
}
