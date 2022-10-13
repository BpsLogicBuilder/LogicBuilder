using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories
{
    internal class ParameterNodeInfoFactory : IParameterNodeInfoFactory
    {
        private readonly Func<ParameterInfo, GenericParameterNodeInfo> _getGenericParameterNodeInfo;
        private readonly Func<ParameterInfo, ListOfGenericsParameterNodeInfo> _getListOfGenericsParameterNodeInfo;
        private readonly Func<ParameterInfo, ListOfLiteralsParameterNodeInfo> _getListOfLiteralsParameterNodeInfo;
        private readonly Func<ParameterInfo, ListOfObjectsParameterNodeInfo> _getListOfObjectsParameterNodeInfo;
        private readonly Func<ParameterInfo, LiteralParameterNodeInfo> _getLiteralParameterNodeInfo;
        private readonly Func<ParameterInfo, ObjectParameterNodeInfo> _getObjectParameterNodeInfo;

        public ParameterNodeInfoFactory(
            Func<ParameterInfo, GenericParameterNodeInfo> getGenericParameterNodeInfo,
            Func<ParameterInfo, ListOfGenericsParameterNodeInfo> getListOfGenericsParameterNodeInfo,
            Func<ParameterInfo, ListOfLiteralsParameterNodeInfo> getListOfLiteralsParameterNodeInfo,
            Func<ParameterInfo, ListOfObjectsParameterNodeInfo> getListOfObjectsParameterNodeInfo,
            Func<ParameterInfo, LiteralParameterNodeInfo> getLiteralParameterNodeInfo,
            Func<ParameterInfo, ObjectParameterNodeInfo> getObjectParameterNodeInfo)
        {
            _getGenericParameterNodeInfo = getGenericParameterNodeInfo;
            _getListOfGenericsParameterNodeInfo = getListOfGenericsParameterNodeInfo;
            _getListOfLiteralsParameterNodeInfo = getListOfLiteralsParameterNodeInfo;
            _getListOfObjectsParameterNodeInfo = getListOfObjectsParameterNodeInfo;
            _getLiteralParameterNodeInfo = getLiteralParameterNodeInfo;
            _getObjectParameterNodeInfo = getObjectParameterNodeInfo;
        }

        public GenericParameterNodeInfo GetGenericParameterNodeInfo(ParameterInfo pInfo)
             => _getGenericParameterNodeInfo(pInfo);

        public ListOfGenericsParameterNodeInfo GetListOfGenericsParameterNodeInfo(ParameterInfo pInfo)
             => _getListOfGenericsParameterNodeInfo(pInfo);

        public ListOfLiteralsParameterNodeInfo GetListOfLiteralsParameterNodeInfo(ParameterInfo pInfo)
             => _getListOfLiteralsParameterNodeInfo(pInfo);

        public ListOfObjectsParameterNodeInfo GetListOfObjectsParameterNodeInfo(ParameterInfo pInfo)
             => _getListOfObjectsParameterNodeInfo(pInfo);

        public LiteralParameterNodeInfo GetLiteralParameterNodeInfo(ParameterInfo pInfo)
             => _getLiteralParameterNodeInfo(pInfo);

        public ObjectParameterNodeInfo GetObjectParameterNodeInfo(ParameterInfo pInfo)
             => _getObjectParameterNodeInfo(pInfo);
    }
}
