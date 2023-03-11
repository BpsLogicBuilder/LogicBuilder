using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    /// <summary>
    /// The parent element of literalList is not always literalListVariable. LiteralListVariableElementInfo captures
    /// the relevant details for the VariableLiteralListControl irrespective of the context.
    /// </summary>
    internal class LiteralListVariableElementInfo
    {
        public LiteralListVariableElementInfo(
            string name,
            ListType listType,
            LiteralVariableType literalType,
            ListVariableInputStyle listControl,
            LiteralVariableInputStyle elementControl,
            List<string> domain,
            List<string> defaultValues,
            string comments,
            ListOfLiteralsVariable? variable = null)
        {
            Name = name;
            ListType = listType;
            LiteralType = literalType;
            ListControl = listControl;
            ElementControl = elementControl;
            Domain = domain;
            DefaultValues = defaultValues;
            Comments = comments;
            Variable = variable;
        }

        public string Name { get; }
        public ListType ListType { get; }
        public LiteralVariableType LiteralType { get; }
        public ListVariableInputStyle ListControl { get; }
        public LiteralVariableInputStyle ElementControl { get; }
        public List<string> Domain { get; }
        public List<string> DefaultValues { get; }
        public string Comments { get; private set; }
        public ListOfLiteralsVariable? Variable { get; }
        [MemberNotNullWhen(true, nameof(Variable))]
        public bool HasVariable => Variable != null;
    }
}
