﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Commands
{
    internal class UpdateLiteralListDefaultValueItemCommand : ClickCommandBase
    {
        private readonly ILiteralListDefaultValueItemFactory _literalListDefaultValueItemFactory;
        private readonly IRadListBoxManager<LiteralListDefaultValueItem> radListBoxManager;
        private readonly RadTextBox txtDefaultValueItem;
        private readonly Type type;

        public UpdateLiteralListDefaultValueItemCommand(
            ILiteralListDefaultValueItemFactory literalListDefaultValueItemFactory,
            IConfigureLiteralListDefaultValueControl configureLiteralListDefaultValueControl)
        {
            _literalListDefaultValueItemFactory = literalListDefaultValueItemFactory;
            radListBoxManager = configureLiteralListDefaultValueControl.RadListBoxManager;
            txtDefaultValueItem = configureLiteralListDefaultValueControl.TxtDefaultValueItem;
            type = configureLiteralListDefaultValueControl.Type;
        }

        public override void Execute()
        {
            radListBoxManager.Update
            (
                _literalListDefaultValueItemFactory.GetLiteralListDefaultValueItem(txtDefaultValueItem.Text.Trim(), type)
            );
        }
    }
}