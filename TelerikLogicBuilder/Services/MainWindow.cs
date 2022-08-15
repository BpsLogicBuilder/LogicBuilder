using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class MainWindow : IMainWindow
    {
        private readonly IExceptionHelper _exceptionHelper;
        private RightToLeft? _rightToLeft;
        private Form? _instance;

        public MainWindow(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public IConfigurationExplorer ConfigurationExplorer => ProjectExplorer.ConfigurationExplorer;

        public IDocumentsExplorer DocumentsExplorer => ProjectExplorer.DocumentsExplorer;

        public Form Instance
        {
            get
            {
                if (_instance == null)
                    throw _exceptionHelper.CriticalException("{AB3C4989-4FFF-4CC4-88AB-49DBA1BE35E3}");

                return _instance;
            }
            set
            {
                _rightToLeft = value.RightToLeft;
                _instance = value;
            }
        }

        public IMDIParent MDIParent => (IMDIParent)Instance;

        public IMessages Messages => MDIParent.Messages;

        public IProjectExplorer ProjectExplorer => MDIParent.ProjectExplorer;

        public RightToLeft RightToLeft 
        { 
            get
            {
                if (!_rightToLeft.HasValue)
                    throw _exceptionHelper.CriticalException("{ACEE9906-C13F-43A0-AB1F-5982883EB593}");

                return _rightToLeft.Value;
            }
        }

        public IRulesExplorer RulesExplorer => ProjectExplorer.RulesExplorer;
    }
}
