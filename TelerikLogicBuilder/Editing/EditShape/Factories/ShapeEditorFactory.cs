using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditShape.Factories
{
    internal class ShapeEditorFactory : IShapeEditorFactory
    {
        private readonly Func<string, IShapeEditor> _getShapeEditor;

        public ShapeEditorFactory(Func<string, IShapeEditor> getShapeEditor)
        {
            _getShapeEditor = getShapeEditor;
        }

        public IShapeEditor GetShapeEditor(string universalMasterName)
            => _getShapeEditor(universalMasterName);
    }
}
