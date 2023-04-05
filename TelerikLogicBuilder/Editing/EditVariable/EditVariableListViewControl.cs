using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable
{
    internal partial class EditVariableListViewControl : UserControl, IEditVariableListViewControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEditVariableControl editVariableControl;

        public EditVariableListViewControl(
            IConfigurationService configurationService,
            IEditVariableControl editVariableControl)
        {
            InitializeComponent();
            _configurationService = configurationService;
            this.editVariableControl = editVariableControl;
            Initialize();
        }

        public event EventHandler? Changed;

        public new event EventHandler? Validated;

        public string VariableName => radListControl1.SelectedItem.Text;

        public bool ItemSelected => radListControl1.Items.Count != 0 && radListControl1.SelectedIndex != -1;

        public void SelectVariable(string variableName)
        {
            if (variableName.Trim().Length == 0)
                return;

            if (!_configurationService.VariableList.Variables.TryGetValue(variableName, out VariableBase? variable))
            {
                editVariableControl.SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.decisionNotConfiguredFormat2, variableName)
                );
                return;
            }

            radListControl1.SelectedValue = variable;
            radListControl1.ScrollToItem(radListControl1.SelectedItem);
        }

        private void Initialize()
        {
            radListControl1.SelectedIndexChanged += RadListControl1_SelectedIndexChanged;
            radListControl1.Validated += RadListControl1_Validated;
            radListControl1.VisualItemFormatting += RadListControl1_VisualItemFormatting;
            radListControl1.SelectionMode = SelectionMode.One;
            LoadListBox();
        }

        private void LoadListBox()
        {
            IList<VariableBase> variables = _configurationService.VariableList.Variables.Values.OrderBy(v => v.Name).ToArray();
            if (variables.Count > 0)
            {
                radListControl1.Items.AddRange(variables.Select(d => new RadListDataItem(d.Name, d)).ToArray());
                radListControl1.SelectedIndex = 0;
            }
        }

        #region Event Handlers
        private void RadListControl1_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        private void RadListControl1_VisualItemFormatting(object sender, VisualItemFormattingEventArgs args)
        {
            VariableBase dataItem = (VariableBase)args.VisualItem.Data.Value;
            args.VisualItem.ToolTipText = dataItem.MemberName;
        }

        private void RadListControl1_Validated(object? sender, EventArgs e)
        {
            Validated?.Invoke(this, e);
        }
        #endregion Event Handlers
    }
}
