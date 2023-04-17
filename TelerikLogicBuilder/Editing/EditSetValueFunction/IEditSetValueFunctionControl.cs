using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction
{
    internal interface IEditSetValueFunctionControl : IDataGraphEditingControl
    {
        Function Function { get; }
        XmlDocument XmlDocument { get; }

        void ResetControls();
    }
}
