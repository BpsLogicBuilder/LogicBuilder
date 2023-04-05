using Microsoft.Office.Interop.Visio;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditShape
{
    internal interface IDiagramEditor
    {
        void EditShape(Shape shape);
    }
}
