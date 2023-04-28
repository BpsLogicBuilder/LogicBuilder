using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class DisplayIndexInformationCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;

        public DisplayIndexInformationCommand(
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow)
        {
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;

            if (mdiParent.EditControl is not IDrawingControl control)
                throw _exceptionHelper.CriticalException("{2A7805C1-237B-419B-841B-DB3F5C501C7B}");

            control.DisplayIndexInformation();
        }
    }
}
