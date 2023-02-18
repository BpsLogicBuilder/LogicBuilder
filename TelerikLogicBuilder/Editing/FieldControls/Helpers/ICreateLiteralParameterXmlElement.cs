using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface ICreateLiteralParameterXmlElement
    {
        XmlElement Create(LiteralParameter literalParameter, string innerXml);
    }
}
