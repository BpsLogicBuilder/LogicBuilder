using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class FindConstructorCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMDIParent mdiParent;

        public FindConstructorCommand(IExceptionHelper exceptionHelper, IMDIParent mdiParent)
        {
            _exceptionHelper = exceptionHelper;
            this.mdiParent = mdiParent;
        }

        public override void Execute()
        {
            if (mdiParent.EditControl == null)
                throw _exceptionHelper.CriticalException("{9649BEE2-2431-49DF-A765-C702B74D05BD}");

            mdiParent.EditControl.FindConstructor();
        }
    }
}
