using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IManagedListBoxControl
    {
        RadButton BtnCancel { get; }
        RadButton BtnCopy { get; }
        RadButton BtnEdit { get; }
        RadButton BtnRemove { get; }
        RadButton BtnUp { get; }
        RadButton BtnDown { get; }
        RadListControl ListBox { get; }

        void CreateCommands(IRadListBoxManager radListBoxManager);
        void DisableControls();
        void EnableControls();
    }
}
