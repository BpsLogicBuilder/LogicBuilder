using Microsoft.Office.Interop.Visio;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IShapeDataCellManager
    {
        void AddPropertyCell(Shape shape, string cellName, string labelName);
        bool CellExists(Shape shape, string cellName);
        string GetPropertyString(Shape shape, string cellName);
        string GetRulesDataString(Shape shape);
        void SetPropertyString(Shape shape, string cellName, string stringValue);
        void SetRulesDataString(Shape shape, string stringValue);
    }
}
