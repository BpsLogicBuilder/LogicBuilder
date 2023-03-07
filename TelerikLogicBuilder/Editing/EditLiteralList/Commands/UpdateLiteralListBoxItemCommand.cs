﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands
{
    internal class UpdateLiteralListBoxItemCommand : ClickCommandBase
    {
        private readonly ILiteralListBoxItemFactory _literalListBoxItemFactory;
        private readonly IEditLiteralListControl editLiteralListControl;

        public UpdateLiteralListBoxItemCommand(
            ILiteralListBoxItemFactory literalListBoxItemFactory,
            IEditLiteralListControl editLiteralListControl)
        {
            _literalListBoxItemFactory = literalListBoxItemFactory;
            this.editLiteralListControl = editLiteralListControl;
        }

        private IRadListBoxManager<ILiteralListBoxItem> radListBoxManager => editLiteralListControl.RadListBoxManager;

        public override void Execute()
        {
            radListBoxManager.Update
            (
                _literalListBoxItemFactory.GetParameterLiteralListBoxItem
                (
                    editLiteralListControl.ValueControl.VisibleText,
                    editLiteralListControl.ValueControl.MixedXml,
                    editLiteralListControl.LiteralType,
                    editLiteralListControl.ApplicationForm,
                    editLiteralListControl.ListControl
                )
            );
        }
    }
}
