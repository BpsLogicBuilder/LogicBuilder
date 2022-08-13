using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class FindConstructorCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;

        public FindConstructorCommand(IExceptionHelper exceptionHelper, IMainWindow mainWindow)
        {
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;

            if (mdiParent.EditControl == null)
                throw _exceptionHelper.CriticalException("{9649BEE2-2431-49DF-A765-C702B74D05BD}");

            mdiParent.EditControl.FindConstructor();
        }
    }
}
