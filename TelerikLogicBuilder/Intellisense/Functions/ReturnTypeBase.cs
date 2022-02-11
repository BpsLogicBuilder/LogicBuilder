using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal abstract class ReturnTypeBase
    {
        #region Properties
        internal abstract ReturnTypeCategory ReturnTypeCategory { get; }
        internal abstract string ToXml { get; }
        internal abstract string Description { get; }
        #endregion Properties
    }
}
