using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable
{
    internal partial class SelectVariableDropDownViewControl : UserControl, ISelectVariableDropDownViewControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ISelectVariableControl selectVariableControl;

        public SelectVariableDropDownViewControl(
            IConfigurationService configurationService,
            IRadDropDownListHelper radDropDownListHelper,
            ISelectVariableControl selectVariableControl)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _radDropDownListHelper = radDropDownListHelper;
            this.selectVariableControl = selectVariableControl;
            Initialize();
        }

        public event EventHandler? Changed;

        public string VariableName => radDropDownList1.Text;

        public bool ItemSelected => _configurationService.VariableList.Variables.ContainsKey(radDropDownList1.Text);

        public void SelectVariable(string variableName)
        {
            if (!_configurationService.VariableList.Variables.TryGetValue(variableName, out VariableBase? variable))
            {
                selectVariableControl.SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.decisionNotConfiguredFormat2, variableName)
                );
                return;
            }

            radDropDownList1.Text = variable.Name;
        }

        private void Initialize()
        {
            radDropDownList1.TextChanged += RadDropDownList1_TextChanged;
            LoadDropdown();
        }

        private void LoadDropdown()
        {
            IList<string> variables = _configurationService.VariableList.Variables.Keys.OrderBy(k => k).ToArray();
            if (variables.Count> 0)
                _radDropDownListHelper.LoadTextItems(radDropDownList1, variables, RadDropDownStyle.DropDown);
        }

        #region Event Handlers
        private void RadDropDownList1_TextChanged(object? sender, EventArgs e)
        {
            Changed?.Invoke(this, e);
        }
        #endregion Event Handlers
    }
}
