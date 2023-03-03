using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal interface IFunctionParameterControlSetValidator
    {
        void Validate(IDictionary<string, ParameterControlSet> editControlsSet, Function function, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
