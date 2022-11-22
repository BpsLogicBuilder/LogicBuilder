using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Services.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects
{
    internal partial class ConfigureConnectorObjectsControl : UserControl, IListBoxHost<ConnectorObjectListBoxItem>, IConfigureConnectorObjectsControl
    {
        private readonly IConfigureConnectorObjectsCommandFactory _configureConnectorObjectsCommandFactory;
        private readonly IConnectorObjectsItemFactory _connectorObjectsItemFactory;
        private readonly ITypeAutoCompleteManager _typeAutoCompleteManager;

        private readonly IConfigureConnectorObjectsForm configureConnectorObjectsForm;
        private readonly IRadListBoxManager<ConnectorObjectListBoxItem> radListBoxManager;

        public ConfigureConnectorObjectsControl(
            IConfigureConnectorObjectsCommandFactory configureConnectorObjectsCommandFactory,
            IConnectorObjectsItemFactory connectorObjectsItemFactory,
            IServiceFactory serviceFactory,
            IConfigureConnectorObjectsForm configureConnectorObjectsForm)
        {
            InitializeComponent();
            _connectorObjectsItemFactory = connectorObjectsItemFactory;
            _configureConnectorObjectsCommandFactory = configureConnectorObjectsCommandFactory;
            _typeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configureConnectorObjectsForm,
                txtType
            );

            this.configureConnectorObjectsForm = configureConnectorObjectsForm;
            radListBoxManager = new RadListBoxManager<ConnectorObjectListBoxItem>(this);
            Initialize();
        }

        public RadButton BtnAdd => btnAdd;

        public RadButton BtnUpdate => btnUpdate;

        public RadButton BtnCancel => managedListBoxControl.BtnCancel;

        public RadButton BtnCopy => managedListBoxControl.BtnCopy;

        public RadButton BtnEdit => managedListBoxControl.BtnEdit;

        public RadButton BtnRemove => managedListBoxControl.BtnRemove;

        public RadButton BtnUp => managedListBoxControl.BtnUp;

        public RadButton BtnDown => managedListBoxControl.BtnDown;

        public RadListControl ListBox => managedListBoxControl.ListBox;

        public IRadListBoxManager<ConnectorObjectListBoxItem> RadListBoxManager => radListBoxManager;

        public AutoCompleteRadDropDownList TxtType => txtType;

        public void ClearInputControls()
            => txtType.Text = string.Empty;

        public void ClearMessage()
            => configureConnectorObjectsForm.ClearMessage();

        public void DisableControlsDuringEdit(bool disable) { }

        public IList<string> GetObjectTypes() 
            => ListBox.Items.Select(i => ((ConnectorObjectListBoxItem)i.Value).Text).ToArray();

        public void SetObjectTypes(IList<string> typeNames)
        {
            ListBox.Items.AddRange
            (
                typeNames
                    .Select(p => _connectorObjectsItemFactory.GetConnectorObjectListBoxItem(p.Trim()))
                    .Select(cl => new RadListDataItem(cl.ToString(), cl))
            );
        }

        public void SetErrorMessage(string message)
            => configureConnectorObjectsForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => configureConnectorObjectsForm.SetMessage(message, title);

        public void UpdateInputControls(ConnectorObjectListBoxItem item)
            => txtType.Text = item.Text;

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            txtType.Anchor = AnchorConstants.AnchorsLeftTopRight;
            CollapsePanelBorder(radPanelTxtType);
            CollapsePanelBorder(radPanelAddButton);

            InitializeHButtonCommand
            (
                BtnAdd,
                _configureConnectorObjectsCommandFactory.GetAddConnectorObjectListBoxItemCommand(this)
            );
            InitializeHButtonCommand
            (
                BtnUpdate,
                _configureConnectorObjectsCommandFactory.GetUpdateConnectorObjectListBoxItemCommand(this)
            );

            _typeAutoCompleteManager.Setup();

            managedListBoxControl.CreateCommands(radListBoxManager);
        }

        private static void InitializeHButtonCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }
    }
}
