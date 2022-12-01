using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Commands
{
    internal class AddLiteralDomainListBoxItemCommand : ClickCommandBase
    {
        private readonly ILiteralDomainItemFactory _literalDomainItemFactory;
        private readonly IRadListBoxManager<LiteralDomainItem> radListBoxManager;
        private readonly RadTextBox txtDomainItem;
        private readonly Type type;

        public AddLiteralDomainListBoxItemCommand(
            ILiteralDomainItemFactory literalDomainItemFactory,
            IConfigureLiteralDomainControl configureLiteralDomainControl)
        {
            _literalDomainItemFactory = literalDomainItemFactory;
            radListBoxManager = configureLiteralDomainControl.RadListBoxManager;
            txtDomainItem = configureLiteralDomainControl.TxtDomainItem;
            type = configureLiteralDomainControl.Type;
        }

        public override void Execute()
        {
            radListBoxManager.Add
            (
                _literalDomainItemFactory.GetLiteralDomainItem(txtDomainItem.Text.Trim(), type)
            );
        }
    }
}
