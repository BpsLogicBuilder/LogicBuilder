using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal interface IXmlDocumentHelpers
    {
        XmlWriter CreateUnformattedXmlWriter(StringBuilder stringBuilder);
    }
}
