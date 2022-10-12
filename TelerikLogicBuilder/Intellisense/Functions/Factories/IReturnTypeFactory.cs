using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories
{
    internal interface IReturnTypeFactory
    {
        GenericReturnType GetGenericReturnType(string genericArgumentName);
        ListOfGenericsReturnType GetListOfGenericsReturnType(string genericArgumentName, ListType listType);
        ListOfLiteralsReturnType GetListOfLiteralsReturnType(LiteralFunctionReturnType underlyingLiteralType, ListType listType);
        ListOfObjectsReturnType GetListOfObjectsReturnType(string objectType, ListType listType);
        LiteralReturnType GetLiteralReturnType(LiteralFunctionReturnType literalType);
        ObjectReturnType GetObjectReturnType(string objectType);
    }
}
