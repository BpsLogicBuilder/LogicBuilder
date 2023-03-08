using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal interface IEditObjectListCommandFactory
    {
        AddObjectListBoxItemCommand GetAddObjectListBoxItemCommand(IEditObjectListControl editObjectListControl);
        UpdateObjectListBoxItemCommand GetUpdateObjectListBoxItemCommand(IEditObjectListControl editObjectListControl);
    }
}
