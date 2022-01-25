using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal interface IParameterAttributeReader
    {
        Dictionary<string, string> GetNameValueTable(ParameterInfo parameter);
        LiteralParameterInputStyle GetLiteralInputStyle(ParameterInfo parameter);
        ObjectParameterInputStyle GetObjectInputStyle(ParameterInfo parameter);
        ListParameterInputStyle GetListInputStyle(ParameterInfo parameter);
        string GetComments(ParameterInfo parameter);
        List<string> GetDomain(ParameterInfo parameter);
    }
}
