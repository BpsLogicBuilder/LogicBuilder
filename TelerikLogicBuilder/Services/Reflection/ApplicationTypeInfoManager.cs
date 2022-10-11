using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Reflection
{
    internal class ApplicationTypeInfoManager : IApplicationTypeInfoManager
    {
        private readonly IApplicationTypeInfoHelper _applicationTypeInfoHelper;

        private IDictionary<string, ApplicationTypeInfo>? _applicationInfos = new Dictionary<string, ApplicationTypeInfo>();

        public ApplicationTypeInfoManager(IApplicationTypeInfoHelper applicationTypeInfoHelper)
        {
            _applicationTypeInfoHelper = applicationTypeInfoHelper;
        }

        public bool HasApplications => _applicationInfos?.All(application => application.Value.AssemblyAvailable) == true;

        public void ClearApplications()
        {
            _applicationInfos?.Clear();
            _applicationInfos = null;
        }

        public ApplicationTypeInfo GetApplicationTypeInfo(string applicationName)
        {
            applicationName = applicationName.ToLowerInvariant();
            if (_applicationInfos == null)
                _applicationInfos = new Dictionary<string, ApplicationTypeInfo>();

            if (_applicationInfos.TryGetValue(applicationName, out ApplicationTypeInfo? applicationTypeInfo))
            {
                if (applicationTypeInfo.AssemblyAvailable)
                    return applicationTypeInfo;

                return GetApplicationTypeInfo();
            }
            {
                return GetApplicationTypeInfo();
            }

            ApplicationTypeInfo GetApplicationTypeInfo()
            {
                _applicationInfos[applicationName] = this.CreateApplicationTypeInfo();
                return _applicationInfos[applicationName];
            }
        }

        private ApplicationTypeInfo CreateApplicationTypeInfo() 
            => _applicationTypeInfoHelper.CreateApplicationTypeInfo();
    }
}
