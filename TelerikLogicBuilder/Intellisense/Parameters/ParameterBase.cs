using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal abstract class ParameterBase
    {
        internal ParameterBase(string Name, bool IsOptional, string Comments)
        {
            this.Name = Name;
            this.IsOptional = IsOptional;
            this.Comments = Comments;
        }
        internal string Name { get; private set; }
        internal bool IsOptional { get; private set; }
        internal string Comments { get; private set; }
        internal abstract ParameterCategory ParameterCategory { get; }
        internal abstract string ToXml { get; }
        internal abstract string Description { get; }
    }
}
