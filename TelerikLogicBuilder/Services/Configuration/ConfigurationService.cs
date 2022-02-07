using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class ConfigurationService : IConfigurationService
    {
        public ProjectProperties ProjectProperties { get; set; }

        public Application GetSelectedApplication()
            => ProjectProperties.ApplicationList.First().Value;

        public string GetSelectedApplicationKey() 
            => ProjectProperties.ApplicationList.Keys.First();
    }
}
