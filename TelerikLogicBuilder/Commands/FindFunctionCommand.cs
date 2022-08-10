using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class FindFunctionCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMDIParent mdiParent;

        public FindFunctionCommand(IExceptionHelper exceptionHelper, IMDIParent mdiParent)
        {
            _exceptionHelper = exceptionHelper;
            this.mdiParent = mdiParent;
        }

        public override void Execute()
        {
            if (mdiParent.EditControl == null)
                throw _exceptionHelper.CriticalException("{4B65CC5B-7CD2-45F6-BA2E-CA05344BA732}");

            mdiParent.EditControl.FindFunction();
        }
    }
}
