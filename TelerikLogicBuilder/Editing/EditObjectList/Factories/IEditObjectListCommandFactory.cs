using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal interface IEditObjectListCommandFactory
    {
        AddParameterObjectListBoxItemCommand GetAddParameterObjectListBoxItemCommand(IEditParameterObjectListControl editParameterObjectListControl);
        AddVariableObjectListBoxItemCommand GetAddVariableObjectListBoxItemCommand(IEditVariableObjectListControl editVariableObjectListControl);
        EditParameterObjectListFormXmlCommand GetEditParameterObjectListFormXmlCommand(IEditParameterObjectListForm editParameterObjectListForm);
        EditVariableObjectListFormXmlCommand GetEditVariableObjectListFormXmlCommand(IEditVariableObjectListForm editVariableObjectListForm);
        UpdateParameterObjectListBoxItemCommand GetUpdateParameterObjectListBoxItemCommand(IEditParameterObjectListControl editParameterObjectListControl);
        UpdateVariableObjectListBoxItemCommand GetUpdateVariableObjectListBoxItemCommand(IEditVariableObjectListControl editVariableObjectListControl);
    }
}
