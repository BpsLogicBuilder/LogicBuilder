using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Services.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        private EventHandler btnAddClickHandler;
        private EventHandler btnUpdateClickHandler;

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

        public void DisableControlsDuringEdit(bool disable)
            => configureConnectorObjectsForm.DisableControlsDuringEdit(disable);

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

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnAdd.Click += btnAddClickHandler;
            btnUpdate.Click += btnUpdateClickHandler;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(btnAddClickHandler), nameof(btnUpdateClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            ControlsLayoutUtility.LayoutAddUpdateItemGroupBox(this, radGroupBoxAddType);
            ControlsLayoutUtility.LayoutGroupBox(this, radGroupBoxTypes);
            ControlsLayoutUtility.LayoutAddUpdateButtonPanel(radPanelAddButton, tableLayoutPanelAddUpdate);
            CollapsePanelBorder(radPanelTxtType);
            CollapsePanelBorder(radPanelAddButton);

            this.Disposed += ConfigureConnectorObjectsControl_Disposed;

            btnAddClickHandler = InitializeHButtonCommand
            (
                _configureConnectorObjectsCommandFactory.GetAddConnectorObjectListBoxItemCommand(this)
            );
            btnUpdateClickHandler = InitializeHButtonCommand
            (
                _configureConnectorObjectsCommandFactory.GetUpdateConnectorObjectListBoxItemCommand(this)
            );

            AddClickCommands();

            _typeAutoCompleteManager.Setup();

            managedListBoxControl.CreateCommands(radListBoxManager);
        }

        private void RemoveClickCommands()
        {
            btnAdd.Click -= btnAddClickHandler;
            btnUpdate.Click -= btnUpdateClickHandler;
        }

        private static EventHandler InitializeHButtonCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        #region Event Handlers
        private void ConfigureConnectorObjectsControl_Disposed(object? sender, EventArgs e)
        {
            RemoveClickCommands();
        }
        #endregion Event Handlers
    }
}
