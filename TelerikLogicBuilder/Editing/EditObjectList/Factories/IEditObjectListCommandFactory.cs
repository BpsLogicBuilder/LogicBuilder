using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal interface IEditObjectListCommandFactory
    {
        AddParameterObjectListBoxItemCommand GetAddParameterObjectListBoxItemCommand(IEditParameterObjectListControl editParameterObjectListControl);
        EditParameterObjectListFormXmlCommand GetEditParameterObjectListFormXmlCommand(IEditParameterObjectListForm editParameterObjectListForm);
        UpdateParameterObjectListBoxItemCommand GetUpdateParameterObjectListBoxItemCommand(IEditParameterObjectListControl editParameterObjectListControl);
    }
}
