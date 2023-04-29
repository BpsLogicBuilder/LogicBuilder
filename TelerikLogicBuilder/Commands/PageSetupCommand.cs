using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class PageSetupCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;

        public PageSetupCommand(
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow)
        {
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            if (_mainWindow.MDIParent.EditControl is not IDrawingControl control)
                throw _exceptionHelper.CriticalException("{7F37B31F-981D-4707-9EAD-9CC5DA7AD702}");

            control.PageSetup();
        }
    }
}
