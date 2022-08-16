using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class FindReplaceConstructorCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;

        public FindReplaceConstructorCommand(
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
                throw _exceptionHelper.CriticalException("{9BFBB6D8-8F52-487E-92AF-1E2B07BA446D}");

            mdiParent.EditControl.ReplaceConstructor();
        }
    }
}
