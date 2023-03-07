using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    /// <summary>
    /// The parent element of literalList is not always literalListParameter. LiteralListParameterElementInfo captures
    /// the relevant details for the LiteralListControl irrespective of the context.
    /// </summary>
    internal class LiteralListParameterElementInfo
    {
        public LiteralListParameterElementInfo(
            string name,
            ListType listType,
            LiteralParameterType literalType,
            ListParameterInputStyle listControl,
            LiteralParameterInputStyle elementControl,
            List<string> domain,
            List<string> defaultValues,
            string comments,
            ListOfLiteralsParameter? parameter = null,
            string parameterSourceClassName = "")
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
            ParameterSourceClassName = parameterSourceClassName;
        }

        public string Name { get; }
        public ListType ListType { get; }
        public LiteralParameterType LiteralType { get; }
        public ListParameterInputStyle ListControl { get; }
        public LiteralParameterInputStyle ElementControl { get; }
        public List<string> Domain { get; }
        public List<string> DefaultValues { get;}
        public string Comments { get; private set; }
        public ListOfLiteralsParameter? Parameter { get; }
        [MemberNotNullWhen(true, nameof(Parameter))]
        public bool HasParameter => Parameter != null;
        public string ParameterSourceClassName { get; }
    }
}
