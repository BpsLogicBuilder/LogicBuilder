using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Diagnostics.CodeAnalysis;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    /// <summary>
    /// The parent element of objectList is not always objectListVariable. ObjectListVariableElementInfo captures
    /// the relevant details for the ObjectListControl irrespective of the context.
    /// </summary>
    internal class ObjectListVariableElementInfo
    {
        public ObjectListVariableElementInfo(
        string name,
        ListType listType,
        string objectType,
        ListVariableInputStyle listControl,
        string comments,
        ListOfObjectsVariable? parameter = null)
        {
            Name = name;
            ListType = listType;
            ObjectType = objectType;
            ListControl = listControl;
            Comments = comments;
            Variable = parameter;
        }

        public string Name { get; }
        public ListType ListType { get; }
        public string ObjectType { get; }
        public ListVariableInputStyle ListControl { get; }
        public string Comments { get; }
        public ListOfObjectsVariable? Variable { get; }
        [MemberNotNullWhen(true, nameof(Variable))]
        public bool HasVariable => Variable != null;
    }
}
