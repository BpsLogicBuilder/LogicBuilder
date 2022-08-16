using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class FindReplaceFunctionCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;

        public FindReplaceFunctionCommand(
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
                throw _exceptionHelper.CriticalException("{D1618CCB-74D1-4356-8B90-C275853359C8}");

            mdiParent.EditControl.ReplaceFunction();
        }
    }
}
