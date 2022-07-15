namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IDrawingControl
    {
        void DisplayIndexInformation();
        void FindShape();
        void PageSetup();
        void Redo();
        void ShowFlowDiagramStencil();
        void ShowApplicationsStencil();
        void ShowPanAndZoom();
        void Undo();
    }
}
