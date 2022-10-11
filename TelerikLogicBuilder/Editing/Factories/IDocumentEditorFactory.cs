namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IDocumentEditorFactory
    {
        TableControl GetTableControl(string tableSourceFile, bool openedAsReadOnly);
        VisioControl GetVisioControl(string visioSourceFile, bool openedAsReadOnly);
    }
}
