using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class ObjectListElementInfo
    {
        public ObjectListElementInfo(
            string name,
            ListType listType,
            string objectType,
            ListParameterInputStyle listControl,
            string comments,
            ListOfObjectsParameter? parameter = null)
        {
            Name = name;
            ListType = listType;
            ObjectType = objectType;
            ListControl = listControl;
            Comments = comments;
            Parameter = parameter;
        }

        public string Name { get; }
        public ListType ListType { get; }
        public string ObjectType { get; }
        public ListParameterInputStyle ListControl { get; }
        public string Comments { get; }
        public ListOfObjectsParameter? Parameter { get; }
        public bool HasParameter => Parameter != null;
    }
}
