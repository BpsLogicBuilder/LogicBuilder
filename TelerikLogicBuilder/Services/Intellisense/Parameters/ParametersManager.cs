using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Parameters
{
    internal class ParametersManager : IParametersManager
    {
        private readonly IParameterNodeInfoFactory _parameterNodeInfoFactory;
        private readonly ITypeHelper _typeHelper;

        public ParametersManager(IParameterNodeInfoFactory parameterNodeInfoFactory, ITypeHelper typeHelper)
        {
            _parameterNodeInfoFactory = parameterNodeInfoFactory;
            _typeHelper = typeHelper;
        }

        public ICollection<ParameterNodeInfoBase> GetParameterNodeInfos(IEnumerable<ParameterInfo> parameters) 
            => parameters.Select(p => Create(p)).ToList();

        private ParameterNodeInfoBase Create(ParameterInfo pInfo)
        {
            if (_typeHelper.IsLiteralType(pInfo.ParameterType))
                return _parameterNodeInfoFactory.GetLiteralParameterNodeInfo(pInfo);
            else if (pInfo.ParameterType.IsGenericParameter)
                return _parameterNodeInfoFactory.GetGenericParameterNodeInfo(pInfo);
            else if (_typeHelper.IsValidList(pInfo.ParameterType))
            {
                Type underlyingType = _typeHelper.GetUndelyingTypeForValidList(pInfo.ParameterType);
                if (_typeHelper.IsLiteralType(underlyingType))
                    return _parameterNodeInfoFactory.GetListOfLiteralsParameterNodeInfo(pInfo);
                else if (underlyingType.IsGenericParameter)
                    return _parameterNodeInfoFactory.GetListOfGenericsParameterNodeInfo(pInfo);
                else
                    return _parameterNodeInfoFactory.GetListOfObjectsParameterNodeInfo(pInfo);
            }
            else if (pInfo.ParameterType.IsAbstract || pInfo.ParameterType.IsInterface || pInfo.ParameterType.IsEnum)
            {//keeping these separate form the regular concrete type below - may need further work
                return _parameterNodeInfoFactory.GetObjectParameterNodeInfo(pInfo);
            }
            else
            {
                return _parameterNodeInfoFactory.GetObjectParameterNodeInfo(pInfo);
            }
        }
    }
}
