using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Factories
{
    internal interface IEditConstructorCommandFactory
    {
        SelectConstructorCommand GetSelectConstructorCommand(IEditConstructorForm editConstructorForm);
    }
}
