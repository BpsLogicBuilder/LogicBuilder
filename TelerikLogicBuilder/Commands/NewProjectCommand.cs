using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class NewProjectCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;

        public NewProjectCommand(
            IMainWindow mainWindow,
            IPathHelper pathHelper)
        {
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
        }

        private IMDIParent MDIParent => _mainWindow.MDIParent;

        public override void Execute()
        {
            using IScopedDisposableManager<INewProjectForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<INewProjectForm>>();
            INewProjectForm newProjectForm = disposableManager.ScopedService;
            if (newProjectForm.ShowDialog(_mainWindow.Instance) == System.Windows.Forms.DialogResult.OK)
            {
                MDIParent.CreateNewProject
                (
                    $"{_pathHelper.CombinePaths(newProjectForm.ProjectPath, newProjectForm.ProjectName, newProjectForm.ProjectName)}{FileExtensions.PROJECTFILEEXTENSION}"
                );
            }
        }
    }
}
