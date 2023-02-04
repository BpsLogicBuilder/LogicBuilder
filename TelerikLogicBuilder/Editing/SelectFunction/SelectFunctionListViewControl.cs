using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction
{
    internal partial class SelectFunctionListViewControl : UserControl, ISelectFunctionListViewControl
    {
        private readonly ISelectFunctionControl selectFunctionControl;

        public SelectFunctionListViewControl(
            ISelectFunctionControl selectFunctionControl)
        {
            InitializeComponent();
            this.selectFunctionControl = selectFunctionControl;
            Initialize();
        }

        public event EventHandler? Changed;

        public string FunctionName => radListControl1.SelectedItem.Text;

        public bool ItemSelected => radListControl1.Items.Count != 0 && radListControl1.SelectedIndex != -1;

        public void SelectFunction(string functionName)
        {
            if (!selectFunctionControl.FunctionDictionary.TryGetValue(functionName, out Function? function))
            {
                selectFunctionControl.SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, functionName)
                );
                return;
            }

            radListControl1.SelectedValue = function;
            radListControl1.ScrollToItem(radListControl1.SelectedItem);
        }

        private void Initialize()
        {
            radListControl1.SelectedIndexChanged += RadListControl1_SelectedIndexChanged;
            radListControl1.VisualItemFormatting += RadListControl1_VisualItemFormatting;
            radListControl1.SelectionMode = SelectionMode.One;
            LoadListBox();
        }

        private void LoadListBox()
        {
            IList<Function> functions = selectFunctionControl.FunctionDictionary.Values.OrderBy(v => v.Name).ToArray();
            if (functions.Count > 0)
            {
                radListControl1.Items.AddRange(functions.Select(d => new RadListDataItem(d.Name, d)).ToArray());
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
            Function dataItem = (Function)args.VisualItem.Data.Value;
            args.VisualItem.ToolTipText = dataItem.ToString();
        }
        #endregion Event Handlers
    }
}
