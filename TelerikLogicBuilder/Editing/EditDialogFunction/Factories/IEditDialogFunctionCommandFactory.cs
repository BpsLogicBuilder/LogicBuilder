using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Factories
{
    internal interface IEditDialogFunctionCommandFactory
    {
        EditDialogFunctionFormXmlCommand GetEditDialogFunctionFormXmlCommand(IEditDialogFunctionForm editDialogFunctionForm);
        SelectDialogFunctionCommand GetSelectDialogFunctionCommand(IEditDialogFunctionForm editDialogFunctionForm);
    }
}
