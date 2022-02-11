using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters
{
    internal interface IMultipleChoiceParameterValidator
    {
        bool ValidateMultipleChoiceParameter(MethodInfo methodInfo);
    }
}
