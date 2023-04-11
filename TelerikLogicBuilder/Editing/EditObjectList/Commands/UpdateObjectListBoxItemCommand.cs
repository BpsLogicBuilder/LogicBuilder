﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands
{
    internal class UpdateObjectListBoxItemCommand : ClickCommandBase
    {
        private readonly IObjectListBoxItemFactory _objectListBoxItemFactory;
        private readonly IEditParameterObjectListControl editObjectListControl;

        public UpdateObjectListBoxItemCommand(
            IObjectListBoxItemFactory objectListBoxItemFactory,
            IEditParameterObjectListControl editObjectListControl)
        {
            _objectListBoxItemFactory = objectListBoxItemFactory;
            this.editObjectListControl = editObjectListControl;
        }

        private IRadListBoxManager<IObjectListBoxItem> RadListBoxManager => editObjectListControl.RadListBoxManager;

        public override void Execute()
        {
            if (this.editObjectListControl.ValueControl.IsEmpty)
                return;

            RadListBoxManager.Update
            (
                _objectListBoxItemFactory.GetParameterObjectListBoxItem
                (
                    editObjectListControl.ValueControl.VisibleText,
                    editObjectListControl.ValueControl.MixedXml,
                    editObjectListControl.ObjectType,
                    editObjectListControl.ApplicationControl,
                    editObjectListControl.ListControl
                )
            );
        }
    }
}
