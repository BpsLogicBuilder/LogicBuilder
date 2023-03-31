using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment
{
    internal partial class SelectFragmentListViewControl : UserControl, ISelectFragmentListViewControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly ISelectFragmentControl selectFragmentControl;

        public SelectFragmentListViewControl(
            IConfigurationService configurationService,
            ISelectFragmentControl selectFragmentControl)
        {
            InitializeComponent();
            _configurationService = configurationService;
            this.selectFragmentControl = selectFragmentControl;
            Initialize();
        }

        public event EventHandler? Changed;

        public string FragmentName => radListControl1.SelectedItem.Text;

        public bool ItemSelected => radListControl1.Items.Count != 0 && radListControl1.SelectedIndex != -1;

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

            radListControl1.SelectedValue = fragment;
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
            IList<Fragment> fragments = _configurationService.FragmentList.Fragments.Values.OrderBy(v => v.Name).ToArray();
            if (fragments.Count > 0)
            {
                radListControl1.Items.AddRange(fragments.Select(d => new RadListDataItem(d.Name, d)).ToArray());
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
            Fragment dataItem = (Fragment)args.VisualItem.Data.Value;
            args.VisualItem.ToolTipText = dataItem.ToString();
        }
        #endregion Event Handlers
    }
}
