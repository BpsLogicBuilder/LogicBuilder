using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureObjectReturnType
{
    internal partial class ConfigureObjectReturnTypeControl : UserControl, IConfigureObjectReturnTypeControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IReturnTypeFactory _returnTypeFactory;
        private readonly IConfigureReturnTypeForm configureReturnTypeForm;

        public ConfigureObjectReturnTypeControl(
            IExceptionHelper exceptionHelper,
            IReturnTypeFactory returnTypeFactory,
            IConfigureReturnTypeForm configureReturnTypeForm)
        {
            InitializeComponent();
            _exceptionHelper = exceptionHelper;
            _returnTypeFactory = returnTypeFactory;
            this.configureReturnTypeForm = configureReturnTypeForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public ReturnTypeBase ReturnType => _returnTypeFactory.GetObjectReturnType(cmbCoObjectType.Text);

        public void SetValues(ReturnTypeBase returnType)
        {
            if (returnType is not ObjectReturnType objectReturnType)
                throw _exceptionHelper.CriticalException("{743FFE2F-B3B6-46AF-B6BF-FB3715D72B7C}");

            RemoveEventHandlers();
            cmbCoObjectType.Text = objectReturnType.ObjectType;
            AddEventHandlers();
            ValidateOk();
        }

        private void AddEventHandlers()
        {
            cmbCoObjectType.TextChanged += CmbCoObjectType_TextChanged;
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
            helpProvider.SetHelpString(cmbCoObjectType, Strings.retTypeObjectHelp);
            toolTip.SetToolTip(lblCoObjectType, Strings.retTypeObjectHelp);
        }

        private static void LoadDropDowns()
        {
        }

        private void RemoveEventHandlers()
        {
            cmbCoObjectType.TextChanged -= CmbCoObjectType_TextChanged;
        }

        private void SetDefaultValues()
        {
            RemoveEventHandlers();
            cmbCoObjectType.Text = MiscellaneousConstants.DEFAULT_OBJECT_TYPE;
            AddEventHandlers();
        }

        private void ValidateFields()
        {
            configureReturnTypeForm.ClearMessage();

            List<string> errors = new();
            if (!FullyQualifiedClassNameRegex().IsMatch(cmbCoObjectType.Text))
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidClassNameFormat, lblCoObjectType.Text));
            if (errors.Count > 0)
                configureReturnTypeForm.SetErrorMessage(string.Join(Environment.NewLine, errors));

            ValidateOk();
        }

        private void ValidateOk()
        {
            configureReturnTypeForm.SetOkValidation
            (
                FullyQualifiedClassNameRegex().IsMatch(cmbCoObjectType.Text)
            );
        }

        #region Event Handlers
        private void CmbCoObjectType_TextChanged(object? sender, EventArgs e)
        {
            ValidateFields();
        }
        #endregion Event Handlers

        [GeneratedRegex(RegularExpressions.FULLYQUALIFIEDCLASSNAME)]
        private static partial Regex FullyQualifiedClassNameRegex();
    }
}
