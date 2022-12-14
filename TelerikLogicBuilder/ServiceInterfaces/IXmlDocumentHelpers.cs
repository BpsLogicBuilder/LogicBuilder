using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IXmlDocumentHelpers
    {
        XmlElement AddElementToDoc(XmlDocument xmlDocument, XmlElement element);
        XmlWriter CreateFormattedXmlWriter(string fullPath);
        XmlWriter CreateFormattedXmlWriter(string fullPath, Encoding encoding);
        XmlWriter CreateFormattedXmlWriter(StringBuilder stringBuilder);
        XmlWriter CreateFormattedXmlWriterWithDeclaration(StringBuilder stringBuilder);
        XmlWriter CreateFragmentXmlWriter(StringBuilder stringBuilder);
        XmlWriter CreateUnformattedXmlWriter(StringBuilder stringBuilder);
        List<XmlElement> GetChildElements(XmlNode xmlNode, Func<XmlElement, bool>? filter = null, Func<IEnumerable<XmlElement>, IEnumerable<XmlElement>>? enumerableFunc = null);
        XmlElement GetDocumentElement(XmlDocument xmlDocument);
        string GetFunctionTreeNodeDescription(XmlElement element);
        string GetGenericArgumentTreeNodeDescription(XmlElement element);
        string GetParameterTreeNodeDescription(XmlElement element);
        string GetVariableTreeNodeDescription(XmlElement element);
        int GetImageIndex(XmlElement element);
        List<XmlElement> GetSiblingParameterElements(XmlElement parameterElement, XmlNode constructorOrFunctionNode);
        XmlElement GetSingleChildElement(XmlNode xmlNode, Func<XmlElement, bool>? filter = null);
        string GetUnformattedXmlString(XmlDocument xmlDocument);
        /// <summary>
        /// Returns the visible text represention of the elements mixed XML child nodes
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        string GetVisibleText(XmlElement element);
        string GetXmlString(XmlDocument xmlDocument);
        XmlAttribute MakeAttribute(XmlDocument xmlDocument, string name, string attributeValue);
        XmlElement MakeElement(XmlDocument xmlDocument, string name, string? innerXml = null, IDictionary<string, string>? attributes = null);
        XmlDocumentFragment MakeFragment(XmlDocument xmlDocument, string? innerXml = null);
        TResult Query<TResult>(XmlNode xmlNode, Func<XmlElement, bool> filter, Func<IEnumerable<XmlElement>, TResult> enumerableFunc);
        List<XmlElement> SelectElements(XmlDocument xmlDocument, string xPath);
        XmlElement SelectSingleElement(XmlDocument xmlDocument, string xPath);
        XmlDocument ToXmlDocument(XmlNode xmlNode, bool preserveWhiteSpace = true);
        XmlDocument ToXmlDocument(string xmlString, bool preserveWhiteSpace = true);
        XmlElement ToXmlElement(string xmlString, bool preserveWhiteSpace = true);
    }
}
