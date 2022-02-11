using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables
{
    internal interface IVariablesManager
    {
        VariableBase GetVariable(string name,
                                string memberName,
                                VariableCategory variableCategory,
                                string castVariableAs,
                                string typeName,
                                string referenceName,
                                string referenceDefinition,
                                string castReferenceAs,
                                ReferenceCategories referenceCategory,
                                MemberInfo mInfo,
                                Type memberType);
    }
}
