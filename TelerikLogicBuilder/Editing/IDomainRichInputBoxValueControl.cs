using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IDomainRichInputBoxValueControl : IRichInputBoxValueControl
    {
        string Comments { get; }
        IList<string> Domain { get; }
    }
}
