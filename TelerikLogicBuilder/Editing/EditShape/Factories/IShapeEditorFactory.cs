namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditShape.Factories
{
    internal interface IShapeEditorFactory
    {
        IShapeEditor GetShapeEditor(string universalMasterName);
    }
}
