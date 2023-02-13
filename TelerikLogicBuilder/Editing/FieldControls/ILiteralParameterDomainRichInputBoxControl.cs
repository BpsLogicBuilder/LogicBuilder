using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal interface ILiteralParameterDomainRichInputBoxControl : IRichInputBoxValueControl
    {
        string Comments { get; }
        IList<string> Domain { get; }
    }
}
