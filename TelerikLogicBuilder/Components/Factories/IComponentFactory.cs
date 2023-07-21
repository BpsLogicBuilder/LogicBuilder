namespace ABIS.LogicBuilder.FlowBuilder.Components.Factories
{
    internal interface IComponentFactory
    {
        FileSystemTreeView GetFileSystemTreeView();
        ObjectRichTextBox GetObjectRichTextBox();
        RichInputBox GetRichInputBox();
    }
}
