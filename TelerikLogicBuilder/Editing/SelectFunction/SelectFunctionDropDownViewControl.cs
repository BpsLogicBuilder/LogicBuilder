using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction
{
    internal partial class SelectFunctionDropDownViewControl : UserControl, ISelectFunctionDropDownViewControl
    {
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ISelectFunctionControl selectFunctionControl;

        public SelectFunctionDropDownViewControl(
            IRadDropDownListHelper radDropDownListHelper,
            ISelectFunctionControl selectFunctionControl)
        {
            InitializeComponent();
            _radDropDownListHelper = radDropDownListHelper;
            this.selectFunctionControl = selectFunctionControl;
            Initialize();
        }

        public string FunctionName => radDropDownList1.Text;

        public bool ItemSelected => selectFunctionControl.FunctionDictionary.ContainsKey(radDropDownList1.Text);

        public event EventHandler? Changed;

        public void SelectFunction(string functionName)
        {
            if (functionName.Trim().Length == 0)
                return;

            if (!selectFunctionControl.FunctionDictionary.TryGetValue(functionName, out Function? function))
            {
                selectFunctionControl.SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, functionName)
                );
                return;
            }

            radDropDownList1.Text = function.Name;
        }

        private void Initialize()
        {
            radDropDownList1.TextChanged += RadDropDownList1_TextChanged;
            LoadDropdown();
        }

        private void LoadDropdown()
        {
            IList<string> functions = selectFunctionControl.FunctionDictionary.Keys.OrderBy(k => k).ToArray();
            if (functions.Count > 0)
                _radDropDownListHelper.LoadTextItems(radDropDownList1, functions, RadDropDownStyle.DropDown);
        }

        #region Event Handlers
        private void RadDropDownList1_TextChanged(object? sender, EventArgs e)
        {
            Changed?.Invoke(this, e);
        }
        #endregion Event Handlers
    }
}
