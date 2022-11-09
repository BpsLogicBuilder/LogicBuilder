using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox
{
    internal interface IListBoxHost<T> where T : IListBoxManageable
    {
        RadButton BtnAdd { get; }
        RadButton BtnUpdate { get; }
        RadButton BtnCancel { get; }
        RadButton BtnCopy { get; }
        RadButton BtnEdit { get; }
        RadButton BtnRemove { get; }
        RadButton BtnUp { get; }
        RadButton BtnDown { get; }
        RadListControl ListBox { get; }
        void ClearInputControls();
        void ClearMessage();
        void DisableControlsDuringEdit(bool disable);
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
        void UpdateInputControls(T item);
    }
}
