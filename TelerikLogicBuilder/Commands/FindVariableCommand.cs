using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class FindVariableCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMDIParent mdiParent;

        public FindVariableCommand(IExceptionHelper exceptionHelper, IMDIParent mdiParent)
        {
            _exceptionHelper = exceptionHelper;
            this.mdiParent = mdiParent;
        }

        public override void Execute()
        {
            if (mdiParent.EditControl == null)
                throw _exceptionHelper.CriticalException("{75F6F9EF-560E-4623-A61D-B74976B21F9C}");

            mdiParent.EditControl.FindVariable();
        }
    }
}
