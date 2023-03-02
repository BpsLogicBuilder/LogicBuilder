using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class LiteralListElementInfo
    {
        public LiteralListElementInfo(
            string name,
            ListType listType,
            LiteralParameterType literalType,
            ListParameterInputStyle listControl,
            LiteralParameterInputStyle elementControl,
            List<string> domain,
            List<string> defaultValues,
            string comments,
            ListOfLiteralsParameter? parameter = null)
        {
            Name = name;
            ListType = listType;
            LiteralType = literalType;
            ListControl = listControl;
            ElementControl = elementControl;
            Domain = domain;
            DefaultValues = defaultValues;
            Comments = comments;
            Parameter = parameter;
        }

        public string Name { get; private set; }
        public ListType ListType { get; private set; }
        public LiteralParameterType LiteralType { get; private set; }
        public ListParameterInputStyle ListControl { get; private set; }
        public LiteralParameterInputStyle ElementControl { get; private set; }
        public List<string> Domain { get; private set; }
        public List<string> DefaultValues { get; private set; }
        public string Comments { get; private set; }
        public ListOfLiteralsParameter? Parameter { get; }
        [MemberNotNullWhen(true, nameof(Parameter))]
        public bool HasParameter => Parameter != null;
    }
}
