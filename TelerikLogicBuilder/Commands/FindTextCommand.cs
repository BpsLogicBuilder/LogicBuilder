using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class FindTextCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMDIParent mdiParent;

        public FindTextCommand(IExceptionHelper exceptionHelper, IMDIParent mdiParent)
        {
            _exceptionHelper = exceptionHelper;
            this.mdiParent = mdiParent;
        }

        public override void Execute()
        {
            if (mdiParent.EditControl == null)
                throw _exceptionHelper.CriticalException("{824F3D98-C41F-4E07-9263-6433186EE858}");

            mdiParent.EditControl.FindText();
        }
    }
}
