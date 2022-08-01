using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class ViewApplicationsStencilCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMDIParent mdiParent;

        public ViewApplicationsStencilCommand(IExceptionHelper exceptionHelper, IMDIParent mdiParent)
        {
            _exceptionHelper = exceptionHelper;
            this.mdiParent = mdiParent;
        }

        public override void Execute()
        {
            if (this.mdiParent.EditControl is not IDrawingControl drawingControl)
                throw _exceptionHelper.CriticalException("{9EA5D049-35F8-4A78-BA89-E1E9790B4C6F}");

            drawingControl.ShowApplicationsStencil();
        }
    }
}
