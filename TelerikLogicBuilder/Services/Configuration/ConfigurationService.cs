using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class ConfigurationService : IConfigurationService
    {
        private readonly IExceptionHelper _exceptionHelper;
        private ProjectProperties? _projectProperties;
        private ConstructorList? _constructorList;
        private FragmentList? _fragmentList;
        private FunctionList? _functionList;
        private VariableList? _variableList;

        public ConfigurationService(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public ProjectProperties ProjectProperties
        {
            get
            {
                if (_projectProperties == null)
                    throw _exceptionHelper.CriticalException("{2F01EE09-8764-432E-846B-22D86A3860A9}");

                return _projectProperties;
            }

            set => _projectProperties = value;
        }

        public ConstructorList ConstructorList
        {
            get 
            {
                if (_constructorList == null)
                    throw _exceptionHelper.CriticalException("{F6D7B17D-11BE-4A30-A81D-90AF959CD58D}");

                return _constructorList; 
            }
            set => _constructorList = value;
        }

        public FragmentList FragmentList
        {
            get
            {
                if (_fragmentList == null)
                    throw _exceptionHelper.CriticalException("{26389294-6669-4969-B825-3464C85A7BBC}");

                return _fragmentList;
            }

            set => _fragmentList = value;
        }

        public FunctionList FunctionList
        {
            get
            {
                if (_functionList == null)
                    throw _exceptionHelper.CriticalException("{11085490-1C7B-47DF-88E5-97DCC64602EE}");

                return _functionList;
            }

            set => _functionList = value;
        }

        public VariableList VariableList
        {
            get
            {
                if (_variableList == null)
                    throw _exceptionHelper.CriticalException("{7B8C0DAE-E0DC-4FA2-8338-C0AB684E669D}");

                return _variableList;
            }

            set => _variableList = value;
        }

        public Application GetSelectedApplication()
            => ProjectProperties.ApplicationList.First().Value;

        public string GetSelectedApplicationKey()
            => ProjectProperties.ApplicationList.Keys.First();
    }
}
