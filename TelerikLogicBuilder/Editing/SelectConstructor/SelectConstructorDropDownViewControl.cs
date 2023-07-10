using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor
{
    internal partial class SelectConstructorDropDownViewControl : UserControl, ISelectConstructorDropDownViewControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ISelectConstructorControl selectConstructorControl;

        public SelectConstructorDropDownViewControl(
            IConfigurationService configurationService,
            IRadDropDownListHelper radDropDownListHelper,
            ISelectConstructorControl selectConstructorControl)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _radDropDownListHelper = radDropDownListHelper;
            this.selectConstructorControl = selectConstructorControl;
            Initialize();
        }

        public event EventHandler? Changed;

        public string ConstructorName => radDropDownList1.Text;

        public bool ItemSelected => _configurationService.ConstructorList.Constructors.ContainsKey(radDropDownList1.Text);

        public void SelectConstructor(string constructorName)
        {
            if (constructorName.Trim().Length == 0)
                return;

            if (!_configurationService.ConstructorList.Constructors.TryGetValue(constructorName, out Constructor? constructor))
            {
                selectConstructorControl.SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.constructorNotConfiguredFormat, constructorName)
                );
                return;
            }

            radDropDownList1.Text = constructor.Name;
        }

        private void Initialize()
        {
            radDropDownList1.TextChanged += RadDropDownList1_TextChanged;
            Disposed += SelectConstructorDropDownViewControl_Disposed;
            LoadDropdown();
        }

        private void LoadDropdown()
        {
            IList<string> constructors = _configurationService.ConstructorList.Constructors.Keys.OrderBy(k => k).ToArray();
            if (constructors.Count > 0)
                _radDropDownListHelper.LoadTextItems(radDropDownList1, constructors, RadDropDownStyle.DropDown);
        }

        private void RemoveEventHandlers()
        {
            radDropDownList1.TextChanged -= RadDropDownList1_TextChanged;
        }

        #region Event Handlers
        private void RadDropDownList1_TextChanged(object? sender, EventArgs e)
        {
            if (radDropDownList1.Disposing
                || radDropDownList1.IsDisposed)
                return;

            Changed?.Invoke(this, e);
        }

        private void SelectConstructorDropDownViewControl_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
        }
        #endregion Event Handlers
    }
}
