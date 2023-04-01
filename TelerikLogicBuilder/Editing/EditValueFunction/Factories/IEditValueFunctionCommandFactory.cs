using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Factories
{
    internal interface IEditValueFunctionCommandFactory
    {
        EditValueFunctionFormXmlCommand GetEditValueFunctionFormXmlCommand(IEditValueFunctionForm editFunctionForm);
        SelectValueFunctionCommand GetSelectValueFunctionCommand(IEditValueFunctionForm editFunctionForm);
    }
}
