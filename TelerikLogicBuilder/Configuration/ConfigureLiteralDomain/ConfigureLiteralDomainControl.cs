using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Services.ListBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain
{
    internal partial class ConfigureLiteralDomainControl : UserControl, IListBoxHost<LiteralDomainItem>, IConfigureLiteralDomainControl
    {
        private readonly IConfigureLiteralDomainCommandFactory _configureLiteralDomainCommandFactory;
        private readonly IConfigureLiteralDomainForm _configureLiteralDomainForm;
        private readonly ILiteralDomainItemFactory _literalDomainItemFactory;
        private readonly IRadListBoxManager<LiteralDomainItem> radListBoxManager;

        public ConfigureLiteralDomainControl(
            IConfigureLiteralDomainCommandFactory configureLiteralDomainCommandFactory,
            IGetPromptForLiteralDomainUpdate getPromptForLiteralDomainUpdate,
            ILiteralDomainItemFactory literalDomainItemFactory,
            IConfigureLiteralDomainForm configureLiteralDomainForm)
        {
            InitializeComponent();
            _literalDomainItemFactory = literalDomainItemFactory;
            _configureLiteralDomainCommandFactory = configureLiteralDomainCommandFactory;
            _configureLiteralDomainForm = configureLiteralDomainForm;
            radListBoxManager = new RadListBoxManager<LiteralDomainItem>(this);
            radGroupBoxAddDomainItem.Text = getPromptForLiteralDomainUpdate.Get(_configureLiteralDomainForm.Type);
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

        public IRadListBoxManager<LiteralDomainItem> RadListBoxManager => radListBoxManager;

        public RadTextBox TxtDomainItem => txtDomainItem;

        public Type Type => _configureLiteralDomainForm.Type;

        public void ClearInputControls()
            => txtDomainItem.Text = string.Empty;

        public void ClearMessage()
            => _configureLiteralDomainForm.ClearMessage();

        public void DisableControlsDuringEdit(bool disable) { }

        public IList<string> GetDomainItems()
            => ListBox.Items
                    .Select(i => ((LiteralDomainItem)i.Value).Item)
                    .ToArray();

        public void SetDomainItems(IList<string> domainItems)
        {
            ListBox.Items.AddRange
            (
                domainItems
                    .Select(p => _literalDomainItemFactory.GetLiteralDomainItem(p.Trim(), _configureLiteralDomainForm.Type))
                    .Select(ap => new RadListDataItem(ap.ToString(), ap))
            );
        }

        public void SetErrorMessage(string message)
            => _configureLiteralDomainForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _configureLiteralDomainForm.SetMessage(message, title);

        public void UpdateInputControls(LiteralDomainItem item)
            => txtDomainItem.Text = item.Item;

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            CollapsePanelBorder(radPanelTxtDomainItem);
            CollapsePanelBorder(radPanelAddButton);

            InitializeHButtonCommand
            (
                BtnAdd,
                _configureLiteralDomainCommandFactory.GetAddLiteralDomainListBoxItemCommand(this)
            );
            InitializeHButtonCommand
            (
                BtnUpdate,
                _configureLiteralDomainCommandFactory.GetUpdateLiteralDomainListBoxItemCommand(this)
            );

            managedListBoxControl.CreateCommands(radListBoxManager);
        }

        private static void InitializeHButtonCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }
    }
}
