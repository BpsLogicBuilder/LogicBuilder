using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class RedoCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;

        public RedoCommand(
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow)
        {
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            if (_mainWindow.MDIParent.EditControl is not IDrawingControl control)
                throw _exceptionHelper.CriticalException("{5BDBEB12-7BE4-4F34-BFF8-5E5C585D434C}");

            control.Redo();
        }
    }
}
