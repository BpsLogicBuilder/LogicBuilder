using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class ConfigurationService : IConfigurationService
    {
        private readonly IExceptionHelper _exceptionHelper;
        private ProjectProperties _projectProperties;

        public ConfigurationService(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public ProjectProperties ProjectProperties
        {
            get 
            {
                if (_projectProperties == null)
                    _exceptionHelper.CriticalException("{2F01EE09-8764-432E-846B-22D86A3860A9}");

                return _projectProperties; 
            }

            set => _projectProperties = value;
        }

        public ConstructorList ConstructorList { get; set; }
        public FragmentList FragmentList { get; set; }
        public FunctionList FunctionList { get; set; }
        public VariableList VariableList { get; set; }

        public Application GetSelectedApplication()
            => ProjectProperties.ApplicationList.First().Value;

        public string GetSelectedApplicationKey()
            => ProjectProperties.ApplicationList.Keys.First();
    }
}
