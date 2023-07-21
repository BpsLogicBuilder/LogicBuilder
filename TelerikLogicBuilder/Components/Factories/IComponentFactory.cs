namespace ABIS.LogicBuilder.FlowBuilder.Components.Factories
{
    internal interface IComponentFactory
    {
        IFileSystemTreeView GetFileSystemTreeView();
        IObjectRichTextBox GetObjectRichTextBox();
        RichInputBox GetRichInputBox();
    }
}
