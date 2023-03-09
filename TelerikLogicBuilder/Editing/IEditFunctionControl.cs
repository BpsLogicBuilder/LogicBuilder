using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IEditFunctionControl : IDataGraphEditingControl
    {
        Function Function { get; }
        string? SelectedParameter { get; }
        XmlDocument XmlDocument { get; }

        void ResetControls();
    }
}
