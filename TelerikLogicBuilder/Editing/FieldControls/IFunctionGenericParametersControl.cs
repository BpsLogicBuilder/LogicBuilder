using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal interface IFunctionGenericParametersControl
    {
        ApplicationTypeInfo Application { get; }
        Function Function { get; }
        XmlDocument XmlDocument { get; }

        void ResetControls();
        void UpdateValidState();
    }
}
