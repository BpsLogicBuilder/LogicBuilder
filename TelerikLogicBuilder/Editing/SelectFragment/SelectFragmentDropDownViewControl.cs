using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment
{
    internal partial class SelectFragmentDropDownViewControl : UserControl, ISelectFragmentDropDownViewControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ISelectFragmentControl selectFragmentControl;

        public SelectFragmentDropDownViewControl(
            IConfigurationService configurationService,
            IRadDropDownListHelper radDropDownListHelper,
            ISelectFragmentControl selectFragmentControl)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _radDropDownListHelper = radDropDownListHelper;
            this.selectFragmentControl = selectFragmentControl;
            Initialize();
        }

        public event EventHandler? Changed;

        public string FragmentName => radDropDownList1.Text;

        public bool ItemSelected => _configurationService.FragmentList.Fragments.ContainsKey(radDropDownList1.Text);

        public void SelectFragment(string fragmentName)
        {
            if (!_configurationService.FragmentList.Fragments.TryGetValue(fragmentName, out Fragment? fragment))
            {
                selectFragmentControl.SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.fragmentNotConfiguredFormat, fragmentName)
                );
                return;
            }

            radDropDownList1.Text = fragment.Name;
        }

        private void Initialize()
        {
            radDropDownList1.TextChanged += RadDropDownList1_TextChanged;
            Disposed += SelectFragmentDropDownViewControl_Disposed;
            LoadDropdown();
        }

        private void LoadDropdown()
        {
            IList<string> fragments = _configurationService.FragmentList.Fragments.Keys.OrderBy(k => k).ToArray();
            if (fragments.Count > 0)
                _radDropDownListHelper.LoadTextItems(radDropDownList1, fragments, RadDropDownStyle.DropDown);
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

        private void SelectFragmentDropDownViewControl_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
        }
        #endregion Event Handlers
    }
}
