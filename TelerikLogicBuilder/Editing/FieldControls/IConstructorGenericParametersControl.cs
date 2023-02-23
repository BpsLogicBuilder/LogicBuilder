using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal interface IConstructorGenericParametersControl
    {
        ApplicationTypeInfo Application { get; }
        Constructor Constructor { get; }
        XmlDocument XmlDocument { get; }

        void ResetControls();
        void UpdateValidState();
    }
}
