using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IXmlDocumentHelpers
    {
        XmlWriter CreateUnformattedXmlWriter(StringBuilder stringBuilder);
        List<XmlElement> GetChildElements(XmlNode xmlNode, Func<XmlElement, bool> filter = null, Func<IEnumerable<XmlElement>, IEnumerable<XmlElement>> enumerableFunc = null);
    }
}
