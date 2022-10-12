using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories
{
    internal interface IReturnTypeInfoFactory
    {
        GenericReturnTypeInfo GetGenericReturnTypeInfo(MethodInfo methodInfo);
        ListOfGenericsReturnTypeInfo GetListOfGenericsReturnTypeInfo(MethodInfo methodInfo);
        ListOfLiteralsReturnTypeInfo GetListOfLiteralsReturnTypeInfo(MethodInfo methodInfo);
        ListOfObjectsReturnTypeInfo GetListOfObjectsReturnTypeInfo(MethodInfo methodInfo);
        LiteralReturnTypeInfo GetLiteralReturnTypeInfo(MethodInfo methodInfo);
        ObjectReturnTypeInfo GetObjectReturnTypeInfo(MethodInfo methodInfo);
    }
}
