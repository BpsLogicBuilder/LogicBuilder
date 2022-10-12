using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories
{
    internal class ReturnTypeFactory : IReturnTypeFactory
    {
        private readonly Func<string, GenericReturnType> _getGenericReturnType;
        private readonly Func<string, ListType, ListOfGenericsReturnType> _getListOfGenericsReturnType;
        private readonly Func<LiteralFunctionReturnType, ListType, ListOfLiteralsReturnType> _getListOfLiteralsReturnType;
        private readonly Func<string, ListType, ListOfObjectsReturnType> _getListOfObjectsReturnType;
        private readonly Func<LiteralFunctionReturnType, LiteralReturnType> _getLiteralReturnType;
        private readonly Func<string, ObjectReturnType> _getObjectReturnType;

        public ReturnTypeFactory(
            Func<string, GenericReturnType> getGenericReturnType,
            Func<string, ListType, ListOfGenericsReturnType> getListOfGenericsReturnType,
            Func<LiteralFunctionReturnType, ListType, ListOfLiteralsReturnType> getListOfLiteralsReturnType,
            Func<string, ListType, ListOfObjectsReturnType> getListOfObjectsReturnType,
            Func<LiteralFunctionReturnType, LiteralReturnType> getLiteralReturnType,
            Func<string, ObjectReturnType> getObjectReturnType)
        {
            _getGenericReturnType = getGenericReturnType;
            _getListOfGenericsReturnType = getListOfGenericsReturnType;
            _getListOfLiteralsReturnType = getListOfLiteralsReturnType;
            _getListOfObjectsReturnType = getListOfObjectsReturnType;
            _getLiteralReturnType = getLiteralReturnType;
            _getObjectReturnType = getObjectReturnType;
        }

        public GenericReturnType GetGenericReturnType(string genericArgumentName)
             => _getGenericReturnType(genericArgumentName);

        public ListOfGenericsReturnType GetListOfGenericsReturnType(string genericArgumentName, ListType listType)
             => _getListOfGenericsReturnType(genericArgumentName, listType);

        public ListOfLiteralsReturnType GetListOfLiteralsReturnType(LiteralFunctionReturnType underlyingLiteralType, ListType listType)
             => _getListOfLiteralsReturnType(underlyingLiteralType, listType);

        public ListOfObjectsReturnType GetListOfObjectsReturnType(string objectType, ListType listType)
             => _getListOfObjectsReturnType(objectType, listType);

        public LiteralReturnType GetLiteralReturnType(LiteralFunctionReturnType literalType)
            => _getLiteralReturnType(literalType);

        public ObjectReturnType GetObjectReturnType(string objectType)
             => _getObjectReturnType(objectType);
    }
}
