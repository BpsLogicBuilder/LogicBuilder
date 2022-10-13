using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories
{
    internal interface IParameterNodeInfoFactory
    {
        GenericParameterNodeInfo GetGenericParameterNodeInfo(ParameterInfo pInfo);

        ListOfGenericsParameterNodeInfo GetListOfGenericsParameterNodeInfo(ParameterInfo pInfo);

        ListOfLiteralsParameterNodeInfo GetListOfLiteralsParameterNodeInfo(ParameterInfo pInfo);

        ListOfObjectsParameterNodeInfo GetListOfObjectsParameterNodeInfo(ParameterInfo pInfo);

        LiteralParameterNodeInfo GetLiteralParameterNodeInfo(ParameterInfo pInfo);

        ObjectParameterNodeInfo GetObjectParameterNodeInfo(ParameterInfo pInfo);
    }
}
