namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IDrawingControl
    {
        void DisplayIndexInformation();
        void FindShape();
        void FindShape(int pageIndex, int shapeIndex, int pageId, int shapeId);
        void PageSetup();
        void Redo();
        void ShowFlowDiagramStencil();
        void ShowApplicationsStencil();
        void ShowPanAndZoom();
        void Undo();
    }
}
