namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal interface IDataGraphEditingHostEventsHelper
    {
        void RequestDocumentUpdate(IEditingControl editingControl);
        void Setup();
    }
}
