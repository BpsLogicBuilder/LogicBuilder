using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class FindReplaceVariableCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;

        public FindReplaceVariableCommand(
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow)
        {
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;

            if (mdiParent.EditControl == null)
                throw _exceptionHelper.CriticalException("{AE918B81-9425-4729-8E5F-BCCB6232CA52}");

            mdiParent.EditControl.ReplaceVariable();
        }
    }
}
