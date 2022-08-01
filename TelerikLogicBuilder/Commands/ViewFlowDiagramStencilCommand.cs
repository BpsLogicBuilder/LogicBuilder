using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class ViewFlowDiagramStencilCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMDIParent mdiParent;

        public ViewFlowDiagramStencilCommand(IExceptionHelper exceptionHelper, IMDIParent mdiParent)
        {
            _exceptionHelper = exceptionHelper;
            this.mdiParent = mdiParent;
        }

        public override void Execute()
        {
            if (this.mdiParent.EditControl is not IDrawingControl drawingControl)
                throw _exceptionHelper.CriticalException("{9B24A505-B224-43A4-A556-0DCB1D0E4C27}");

            drawingControl.ShowFlowDiagramStencil();
        }
    }
}
