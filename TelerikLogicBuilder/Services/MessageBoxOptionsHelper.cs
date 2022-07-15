using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class MessageBoxOptionsHelper : IMessageBoxOptionsHelper
    {
        private readonly IExceptionHelper _exceptionHelper;
        private IWin32Window? _mainWindow;

        public MessageBoxOptionsHelper(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public IWin32Window Instance
        {
            get
            {
                if (_mainWindow == null)
                    throw _exceptionHelper.CriticalException("{AB3C4989-4FFF-4CC4-88AB-49DBA1BE35E3}");

                return _mainWindow;
            }
            set => _mainWindow = value;
        }

        public RightToLeft RightToLeft { get; set; }
    }
}
