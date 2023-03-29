using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Factories
{
    internal interface IEditConstructorCommandFactory
    {
        EditConstructorFormXmlCommand GetEditFormXmlCommand(IEditConstructorForm editConstructorForm);
        SelectConstructorCommand GetSelectConstructorCommand(IEditConstructorForm editConstructorForm);
    }
}
