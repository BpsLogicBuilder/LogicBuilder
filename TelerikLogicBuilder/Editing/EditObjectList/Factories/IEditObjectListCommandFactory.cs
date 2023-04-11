using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal interface IEditObjectListCommandFactory
    {
        AddObjectListBoxItemCommand GetAddObjectListBoxItemCommand(IEditParameterObjectListControl editObjectListControl);
        EditObjectListFormXmlCommand GetEditObjectListFormXmlCommand(IEditParameterObjectListForm editObjectListForm);
        UpdateObjectListBoxItemCommand GetUpdateObjectListBoxItemCommand(IEditParameterObjectListControl editObjectListControl);
    }
}
