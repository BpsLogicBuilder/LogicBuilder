using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal interface IMemberAttributeReader
    {
        Dictionary<string, string> GetNameValueTable(MemberInfo member);
        LiteralVariableInputStyle GetLiteralInputStyle(MemberInfo member);
        ObjectVariableInputStyle GetObjectInputStyle(MemberInfo member);
        ListVariableInputStyle GetListInputStyle(MemberInfo member);
        string GetFunctionSummary(MemberInfo member);
        string GetVariableComments(MemberInfo member);
        string GetAlsoKnownAs(MemberInfo member);
        FunctionCategories GetFunctionCategory(MemberInfo member);
        List<string> GetDomain(MemberInfo member);
        bool IsVariableConfigurableFromClassHelper(MemberInfo member);
        bool IsFunctionConfigurableFromClassHelper(MemberInfo member);
    }
}
