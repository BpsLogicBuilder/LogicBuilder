using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor
{
    internal partial class SelectConstructorListViewControl : UserControl, ISelectConstructorListViewControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly ISelectConstructorControl selectConstructorControl;

        public SelectConstructorListViewControl(
            IConfigurationService configurationService,
            ISelectConstructorControl selectConstructorControl)
        {
            InitializeComponent();
            _configurationService = configurationService;
            this.selectConstructorControl = selectConstructorControl;
            Initialize();
        }

        public event EventHandler? Changed;

        public string ConstructorName => radListControl1.SelectedItem.Text;

        public bool ItemSelected => radListControl1.Items.Count != 0 && radListControl1.SelectedIndex != -1;

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

            radListControl1.SelectedValue = constructor;
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
            IList<Constructor> constructors = _configurationService.ConstructorList.Constructors.Values.OrderBy(v => v.Name).ToArray();
            if (constructors.Count > 0)
            {
                radListControl1.Items.AddRange(constructors.Select(d => new RadListDataItem(d.Name, d)).ToArray());
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
            Constructor dataItem = (Constructor)args.VisualItem.Data.Value;
            args.VisualItem.ToolTipText = dataItem.ToString();
        }
        #endregion Event Handlers
    }
}
