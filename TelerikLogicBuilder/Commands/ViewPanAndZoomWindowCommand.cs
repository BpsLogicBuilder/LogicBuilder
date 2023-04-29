using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class ViewPanAndZoomWindowCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;

        public ViewPanAndZoomWindowCommand(
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow)
        {
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            if (_mainWindow.MDIParent.EditControl is not IDrawingControl drawingControl)
                throw _exceptionHelper.CriticalException("{8D8EFADA-40F1-4F6C-864F-18D7DB2E2121}");

            drawingControl.ShowPanAndZoom();
        }
    }
}
