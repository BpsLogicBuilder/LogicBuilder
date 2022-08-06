using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }
    }
}
