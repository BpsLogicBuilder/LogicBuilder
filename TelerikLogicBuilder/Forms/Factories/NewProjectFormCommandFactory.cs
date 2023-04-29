using ABIS.LogicBuilder.FlowBuilder.Forms.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Forms.Factories
{
    internal class NewProjectFormCommandFactory : INewProjectFormCommandFactory
    {
        private readonly Func<INewProjectForm, SelectFolderCommand> _getSelectFolderCommand;

        public NewProjectFormCommandFactory(Func<INewProjectForm, SelectFolderCommand> getSelectFolderCommand)
        {
            _getSelectFolderCommand = getSelectFolderCommand;
        }

        public SelectFolderCommand GetSelectFolderCommand(INewProjectForm newProjectForm)
            => _getSelectFolderCommand(newProjectForm);
    }
}
