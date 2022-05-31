using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions
{
    internal interface IFunctionManager
    {
        Function? GetFunction(string name, string memberName, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory, ParametersLayout parametersLayout, MethodInfo methodInfo);
    }
}
