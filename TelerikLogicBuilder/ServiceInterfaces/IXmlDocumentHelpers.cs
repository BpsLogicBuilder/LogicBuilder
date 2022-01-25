using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IXmlDocumentHelpers
    {
        XmlWriter CreateUnformattedXmlWriter(StringBuilder stringBuilder);
    }
}
