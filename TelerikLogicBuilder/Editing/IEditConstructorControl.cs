using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IEditConstructorControl : IEditingControl
    {
        Constructor Constructor { get; }
        XmlDocument XmlDocument { get; }

        void ResetControls();
    }
}
