using Microsoft.Office.Interop.Visio;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IShapeXmlHelper
    {
        void SetXmlString(Shape shape, string xml, string visibleText);
        string GetXmlString(Shape shape);
    }
}
