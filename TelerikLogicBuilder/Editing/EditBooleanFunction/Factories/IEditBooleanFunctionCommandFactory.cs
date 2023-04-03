using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Factories
{
    internal interface IEditBooleanFunctionCommandFactory
    {
        EditBooleanFunctionFormXmlCommand GetEditBooleanFunctionFormXmlCommand(IEditBooleanFunctionForm editFunctionForm);
        SelectBooleanFunctionCommand GetSelectBooleanFunctionCommand(IEditBooleanFunctionForm editFunctionForm);
    }
}
