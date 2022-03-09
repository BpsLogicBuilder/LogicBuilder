using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments
{
    internal abstract class GenericConfigBase
    {
        internal GenericConfigBase(string genericArgumentName)
        {
            this.GenericArgumentName = genericArgumentName;
        }

        internal string GenericArgumentName { get; private set; }
        internal abstract GenericConfigCategory GenericConfigCategory { get; }
        internal abstract string ToXml { get; }
    }
}
