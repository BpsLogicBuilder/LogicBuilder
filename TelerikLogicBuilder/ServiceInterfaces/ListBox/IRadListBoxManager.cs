namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox
{
    internal interface IRadListBoxManager<T> : IRadListBoxManager
        where T : IListBoxManageable
    {
        bool Add(T item);
        bool Update(T item);
    }

    internal interface IRadListBoxManager
    {
        void Cancel();
        void Copy();
        void Edit();
        void MoveDown();
        void MoveUp();
        bool Remove();
        void ResetControls();
    }
}
