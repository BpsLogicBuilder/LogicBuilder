using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class MainWindow : IMainWindow
    {
        private readonly IExceptionHelper _exceptionHelper;
        private RightToLeft? _rightToLeft;
        private IWin32Window? _mainWindow;

        public MainWindow(IExceptionHelper exceptionHelper)
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
            set
            {
                _rightToLeft = ((Form)value).RightToLeft;
                _mainWindow = value;
            }
        }

        public RightToLeft RightToLeft 
        { 
            get
            {
                if (!_rightToLeft.HasValue)
                    throw _exceptionHelper.CriticalException("{ACEE9906-C13F-43A0-AB1F-5982883EB593}");

                return _rightToLeft.Value;
            }
        }
    }
}
