using ABIS.LogicBuilder.FlowBuilder.Components;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IUpdateRichInputBoxXml
    {
        void Update(XmlElement xmlElement, IRichInputBox richInputBox);
    }
}
