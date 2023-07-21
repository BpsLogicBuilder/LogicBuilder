namespace ABIS.LogicBuilder.FlowBuilder.Components.Factories
{
    internal interface IComponentFactory
    {
        IFileSystemTreeView GetFileSystemTreeView();
        ObjectRichTextBox GetObjectRichTextBox();
        RichInputBox GetRichInputBox();
    }
}
