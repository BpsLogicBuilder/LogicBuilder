﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IXmlDocumentHelpers
    {
        XmlWriter CreateFormattedXmlWriter(StringBuilder stringBuilder);
        XmlWriter CreateFormattedXmlWriterWithDeclaration(StringBuilder stringBuilder);
        XmlWriter CreateUnformattedXmlWriter(StringBuilder stringBuilder);
        List<XmlElement> GetChildElements(XmlNode xmlNode, Func<XmlElement, bool>? filter = null, Func<IEnumerable<XmlElement>, IEnumerable<XmlElement>>? enumerableFunc = null);
        XmlElement GetDocumentElement(XmlDocument xmlDocument);
        List<XmlElement> GetSiblingParameterElements(XmlElement parameterElement, XmlNode constructorOrFunctionNode);
        XmlElement GetSingleChildElement(XmlNode xmlNode, Func<XmlElement, bool>? filter = null);
        string GetUnformattedXmlString(XmlDocument xmlDocument);
        string GetXmlString(XmlDocument xmlDocument);
        XmlAttribute MakeAttribute(XmlDocument xmlDocument, string name, string attributeValue);
        XmlElement MakeElement(XmlDocument xmlDocument, string name, string? innerXml = null, IDictionary<string, string>? attributes = null);
        XmlDocumentFragment MakeFragment(XmlDocument xmlDocument, string? innerXml = null);
        XmlDocument ToXmlDocument(XmlNode xmlNode, bool preserveWhiteSpace = true);
        XmlDocument ToXmlDocument(string xmlString, bool preserveWhiteSpace = true);
    }
}
