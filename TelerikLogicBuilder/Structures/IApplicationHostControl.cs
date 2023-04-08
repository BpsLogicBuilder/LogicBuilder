using System;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal interface IApplicationHostControl : IApplicationControl, ISetDialogMessages
    {
        event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;
    }
}
