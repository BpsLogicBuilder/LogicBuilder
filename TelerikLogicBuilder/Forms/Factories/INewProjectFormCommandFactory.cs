using ABIS.LogicBuilder.FlowBuilder.Forms.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Forms.Factories
{
    internal interface INewProjectFormCommandFactory
    {
        SelectFolderCommand GetSelectFolderCommand(INewProjectForm newProjectForm);
    }
}
