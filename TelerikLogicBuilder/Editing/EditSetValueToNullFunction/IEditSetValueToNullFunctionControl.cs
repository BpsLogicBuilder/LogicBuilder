using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueToNullFunction
{
    internal interface IEditSetValueToNullFunctionControl : IDataGraphEditingControl
    {
        Function Function { get; }
        XmlDocument XmlDocument { get; }

        void ResetControls();
    }
}
