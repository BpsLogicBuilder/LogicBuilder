using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class DocumentEditorFactory : IDocumentEditorFactory
    {
        private readonly Func<string, bool, TableControl> _getTableControl;
        private readonly Func<string, bool, VisioControl> _getVisioControl;

        public DocumentEditorFactory(
            Func<string, bool, TableControl> getTableControl,
            Func<string, bool, VisioControl> getVisioControl)
        {
            _getTableControl = getTableControl;
            _getVisioControl = getVisioControl;
        }

        public TableControl GetTableControl(string tableSourceFile, bool openedAsReadOnly)
            => _getTableControl(tableSourceFile, openedAsReadOnly);

        public VisioControl GetVisioControl(string visioSourceFile, bool openedAsReadOnly)
            => _getVisioControl(visioSourceFile, openedAsReadOnly);
    }
}
