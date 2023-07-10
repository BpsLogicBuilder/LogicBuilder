using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Services.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue
{
    internal partial class ConfigureLiteralListDefaultValueControl : UserControl, IListBoxHost<LiteralListDefaultValueItem>, IConfigureLiteralListDefaultValueControl
    {
        private readonly IConfigureLiteralListDefaultValueCommandFactory _configureLiteralListDefaultValueCommandFactory;
        private readonly IConfigureLiteralListDefaultValueForm _configureLiteralListDefaultValueForm;
        private readonly ILiteralListDefaultValueItemFactory _literalListDefaultValueItemFactory;
        private readonly IRadListBoxManager<LiteralListDefaultValueItem> radListBoxManager;
        private EventHandler btnAddClickHandler;
        private EventHandler btnUpdateClickHandler;

        public ConfigureLiteralListDefaultValueControl(
            IConfigureLiteralListDefaultValueCommandFactory configureLiteralListDefaultValueCommandFactory,
            IGetPromptForLiteralDomainUpdate getPromptForLiteralDomainUpdate,
            ILiteralListDefaultValueItemFactory literalListDefaultValueItemFactory,
            IConfigureLiteralListDefaultValueForm configureLiteralListDefaultValueForm)
        {
            InitializeComponent();
            _configureLiteralListDefaultValueCommandFactory = configureLiteralListDefaultValueCommandFactory;
            _configureLiteralListDefaultValueForm = configureLiteralListDefaultValueForm;
            _literalListDefaultValueItemFactory = literalListDefaultValueItemFactory;
            radListBoxManager = new RadListBoxManager<LiteralListDefaultValueItem>(this);
            radGroupBoxAddDefaultValueItem.Text = getPromptForLiteralDomainUpdate.Get(_configureLiteralListDefaultValueForm.Type);
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

        public IRadListBoxManager<LiteralListDefaultValueItem> RadListBoxManager => radListBoxManager;

        public RadTextBox TxtDefaultValueItem => txtDefaultValueItem;

        public Type Type => _configureLiteralListDefaultValueForm.Type;

        public void ClearInputControls()
            => txtDefaultValueItem.Text = string.Empty;

        public void ClearMessage()
            => _configureLiteralListDefaultValueForm.ClearMessage();

        public void DisableControlsDuringEdit(bool disable)
            => _configureLiteralListDefaultValueForm.DisableControlsDuringEdit(disable);

        public IList<string> GetDefaultValueItems()
            => ListBox.Items
                    .Select(i => ((LiteralListDefaultValueItem)i.Value).Item)
                    .ToArray();

        public void SetDefaultValueItems(IList<string> domainItems)
        {
            ListBox.Items.AddRange
            (
                domainItems
                    .Select(p => _literalListDefaultValueItemFactory.GetLiteralListDefaultValueItem(p.Trim(), _configureLiteralListDefaultValueForm.Type))
                    .Select(ap => new RadListDataItem(ap.ToString(), ap))
            );
        }

        public void SetErrorMessage(string message)
            => _configureLiteralListDefaultValueForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _configureLiteralListDefaultValueForm.SetMessage(message, title);

        public void UpdateInputControls(LiteralListDefaultValueItem item)
            => txtDefaultValueItem.Text = item.Item;

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnAdd.Click += btnAddClickHandler;
            btnUpdate.Click += btnUpdateClickHandler;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(btnAddClickHandler),
        nameof(btnUpdateClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant

        private void Initialize()
        {
            ControlsLayoutUtility.LayoutAddUpdateItemGroupBox(this, radGroupBoxAddDefaultValueItem);
            ControlsLayoutUtility.LayoutGroupBox(this, radGroupBoxDefaultValueItems);
            ControlsLayoutUtility.LayoutAddUpdateButtonPanel(radPanelAddButton, tableLayoutPanelAddUpdate);
            CollapsePanelBorder(radPanelTxtDefaultValueItem);
            CollapsePanelBorder(radPanelAddButton);

            Disposed += ConfigureLiteralListDefaultValueControl_Disposed;

            btnAddClickHandler = InitializeHButtonCommand
            (
                _configureLiteralListDefaultValueCommandFactory.GetAddLiteralListDefaultValueItemCommand(this)
            );
            btnUpdateClickHandler = InitializeHButtonCommand
            (
                _configureLiteralListDefaultValueCommandFactory.GetUpdateLiteralListDefaultValueItemCommand(this)
            );

            managedListBoxControl.CreateCommands(radListBoxManager);
            AddClickCommands();
        }

        private static EventHandler InitializeHButtonCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void RemoveClickCommands()
        {
            btnAdd.Click -= btnAddClickHandler;
            btnUpdate.Click -= btnUpdateClickHandler;
        }

        #region Event Handlers
        private void ConfigureLiteralListDefaultValueControl_Disposed(object? sender, EventArgs e)
        {
            RemoveClickCommands();
        }
        #endregion Event Handlers
    }
}
