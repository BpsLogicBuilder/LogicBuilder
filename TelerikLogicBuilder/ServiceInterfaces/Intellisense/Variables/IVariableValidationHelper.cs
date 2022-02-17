using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables
{
    internal interface IVariableValidationHelper
    {
        void ValidateMemberName(VariableCategory variableCategory, string memberName, string decisionName, ICollection<string> errors, IDictionary<string, VariableBase> variables);
        void ValidateVariableIndirectReferenceName(ValidIndirectReference referenceDefinition, string referenceName, string variableName, ICollection<string> errors, IDictionary<string, VariableBase> variables);
    }
}
