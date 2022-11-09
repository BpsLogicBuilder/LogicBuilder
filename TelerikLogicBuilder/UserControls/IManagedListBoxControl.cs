using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IManagedListBoxControl
    {
        RadButton BtnUpdate { get; }
        RadButton BtnCancel { get; }
        RadButton BtnCopy { get; }
        RadButton BtnEdit { get; }
        RadButton BtnRemove { get; }
        RadButton BtnUp { get; }
        RadButton BtnDown { get; }
        RadListControl ListBox { get; }
    }
}
