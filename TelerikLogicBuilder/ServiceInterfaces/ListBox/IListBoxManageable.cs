using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox
{
    internal interface IListBoxManageable
    {
        IList<string> Errors { get; }
    }
}
