using ABIS.LogicBuilder.FlowBuilder.Editing.EditShape.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Office.Interop.Visio;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditShape
{
    internal class DiagramEditor : IDiagramEditor
    {
        private readonly IMainWindow _mainWindow;
        private readonly IShapeEditorFactory _shapeEditorFactory;

        public DiagramEditor(
            IMainWindow mainWindow,
            IShapeEditorFactory shapeEditorFactory)
        {
            _mainWindow = mainWindow;
            _shapeEditorFactory = shapeEditorFactory;
        }

        public async void EditShape(Shape shape)
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            await mdiParent.RunLoadContextAsync(Edit);

            Task Edit(CancellationTokenSource cancellationTokenSource)
            {
                mdiParent.ChangeCursor(Cursors.WaitCursor);
                _shapeEditorFactory
                    .GetShapeEditor(shape.Master.NameU)
                    .Edit(shape);
                mdiParent.ChangeCursor(Cursors.Default);

                return Task.CompletedTask;
            }
        }
    }
}
