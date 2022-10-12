using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories
{
    internal class ReturnTypeInfoFactory : IReturnTypeInfoFactory
    {
        private readonly Func<MethodInfo, GenericReturnTypeInfo> _getGenericReturnTypeInfo;
        private readonly Func<MethodInfo, ListOfGenericsReturnTypeInfo> _getListOfGenericsReturnTypeInfo;
        private readonly Func<MethodInfo, ListOfLiteralsReturnTypeInfo> _getListOfLiteralsReturnTypeInfo;
        private readonly Func<MethodInfo, ListOfObjectsReturnTypeInfo> _getListOfObjectsReturnTypeInfo;
        private readonly Func<MethodInfo, LiteralReturnTypeInfo> _getLiteralReturnTypeInfo;
        private readonly Func<MethodInfo, ObjectReturnTypeInfo> _getObjectReturnTypeInfo;

        public ReturnTypeInfoFactory(
            Func<MethodInfo, GenericReturnTypeInfo> getGenericReturnTypeInfo,
            Func<MethodInfo, ListOfGenericsReturnTypeInfo> getListOfGenericsReturnTypeInfo,
            Func<MethodInfo, ListOfLiteralsReturnTypeInfo> getListOfLiteralsReturnTypeInfo,
            Func<MethodInfo, ListOfObjectsReturnTypeInfo> getListOfObjectsReturnTypeInfo,
            Func<MethodInfo, LiteralReturnTypeInfo> getLiteralReturnTypeInfo,
            Func<MethodInfo, ObjectReturnTypeInfo> getObjectReturnTypeInfo)
        {
            _getGenericReturnTypeInfo = getGenericReturnTypeInfo;
            _getListOfGenericsReturnTypeInfo = getListOfGenericsReturnTypeInfo;
            _getListOfLiteralsReturnTypeInfo = getListOfLiteralsReturnTypeInfo;
            _getListOfObjectsReturnTypeInfo = getListOfObjectsReturnTypeInfo;
            _getLiteralReturnTypeInfo = getLiteralReturnTypeInfo;
            _getObjectReturnTypeInfo = getObjectReturnTypeInfo;
        }

        public GenericReturnTypeInfo GetGenericReturnTypeInfo(MethodInfo methodInfo)
            => _getGenericReturnTypeInfo(methodInfo);

        public ListOfGenericsReturnTypeInfo GetListOfGenericsReturnTypeInfo(MethodInfo methodInfo)
            => _getListOfGenericsReturnTypeInfo(methodInfo);

        public ListOfLiteralsReturnTypeInfo GetListOfLiteralsReturnTypeInfo(MethodInfo methodInfo)
            => _getListOfLiteralsReturnTypeInfo(methodInfo);

        public ListOfObjectsReturnTypeInfo GetListOfObjectsReturnTypeInfo(MethodInfo methodInfo)
            => _getListOfObjectsReturnTypeInfo(methodInfo);

        public LiteralReturnTypeInfo GetLiteralReturnTypeInfo(MethodInfo methodInfo)
            => _getLiteralReturnTypeInfo(methodInfo);

        public ObjectReturnTypeInfo GetObjectReturnTypeInfo(MethodInfo methodInfo)
            => _getObjectReturnTypeInfo(methodInfo);
    }
}
