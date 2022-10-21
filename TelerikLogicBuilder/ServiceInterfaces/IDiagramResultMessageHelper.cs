using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IDiagramResultMessageHelper
    {
        void AddValidationMessage(string message);
        ResultMessage GetResultMessage(string message);
    }
}
