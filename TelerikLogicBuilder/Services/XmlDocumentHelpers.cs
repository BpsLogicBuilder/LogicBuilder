using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class XmlDocumentHelpers : IXmlDocumentHelpers
    {
        private readonly IExceptionHelper _exceptionHelper;

        public XmlDocumentHelpers(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        #region Properties
        private static XmlWriterSettings FormattedSettings
        {
            get
            {
                return new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    OmitXmlDeclaration = true
                };
            }
        }

        private static XmlWriterSettings FormattedSettingsWithDeclaration
        {
            get
            {
                return new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t"
                };
            }
        }

        private static XmlWriterSettings UnformattedSettings
        {
            get
            {
                return new XmlWriterSettings
                {
                    OmitXmlDeclaration = true
                };
            }
        }

        //private static XmlWriterSettings FragmentSettings
        //{
        //    get
        //    {
        //        return new XmlWriterSettings
        //        {
        //            OmitXmlDeclaration = true,
        //            ConformanceLevel = ConformanceLevel.Fragment
        //        };
        //    }
        //}
        #endregion Properties

        #region Methods
        public XmlWriter CreateFormattedXmlWriter(StringBuilder stringBuilder) 
            => XmlWriter.Create(new StringWriter(stringBuilder, CultureInfo.InvariantCulture), FormattedSettings);

        public XmlWriter CreateFormattedXmlWriterWithDeclaration(StringBuilder stringBuilder) 
            => XmlWriter.Create(new StringWriter(stringBuilder, CultureInfo.InvariantCulture), FormattedSettingsWithDeclaration);

        public XmlWriter CreateUnformattedXmlWriter(StringBuilder stringBuilder) 
            => XmlWriter.Create(new StringWriter(stringBuilder, CultureInfo.InvariantCulture), UnformattedSettings);

        public List<XmlElement> GetChildElements(XmlNode xmlNode, 
            Func<XmlElement, bool>? filter = null, 
            Func<IEnumerable<XmlElement>, IEnumerable<XmlElement>>? enumerableFunc = null)
        {
            IEnumerable<XmlElement> getChildElements()
            {
                return xmlNode.ChildNodes.OfType<XmlElement>().Where(filter ?? (x => true));
            }

            return enumerableFunc != null
                ? enumerableFunc(getChildElements()).ToList()
                : getChildElements().ToList();
        }

        public List<XmlElement> GetSiblingParameterElements(XmlElement parameterElement, XmlNode constructorOrFunctionNode)
        {
            switch (parameterElement.Name)
            {
                case XmlDataConstants.LITERALPARAMETERELEMENT:
                case XmlDataConstants.LITERALLISTPARAMETERELEMENT:
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{B52855E1-4F4B-4F7A-9667-773F9BCBAF26}");
            }

            if (constructorOrFunctionNode.Name != XmlDataConstants.CONSTRUCTORELEMENT
                && constructorOrFunctionNode.Name != XmlDataConstants.FUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{D3537A44-DA97-4393-A4F5-6683C0BA9981}");

            return GetElements(parameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value);/*Attribute is required by schema definition.*/

            List<XmlElement> GetElements(string parameterName) 
                => GetChildElements
                (
                    GetSingleChildElement
                    (
                        constructorOrFunctionNode,
                        e => e.Name == XmlDataConstants.PARAMETERSELEMENT
                    )
                )
                .Where(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE) != parameterName)
                .ToList();
        }

        public XmlElement GetSingleChildElement(XmlNode xmlNode, Func<XmlElement, bool>? filter = null) 
            => filter != null
                ? xmlNode.ChildNodes.OfType<XmlElement>().Single(filter)
                : xmlNode.ChildNodes.OfType<XmlElement>().Single();

        public string GetUnformattedXmlString(XmlDocument xmlDocument)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlDataWriter = CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlDocument.Save(xmlDataWriter);
                xmlDataWriter.Flush();
            }

            return stringBuilder.ToString();
        }

        public string GetXmlString(XmlDocument xmlDocument)
        {
            StringBuilder stringBuilder = new();
            PreserveWhitespace();

            using (XmlWriter xmlDataWriter = CreateFormattedXmlWriter(stringBuilder))
            {
                xmlDocument.Save(xmlDataWriter);
                xmlDataWriter.Flush();
            }

            return stringBuilder.ToString();

            void PreserveWhitespace()
            {//The formatted writer inserts newline and tabs in from of the first
                //child element for mixed XML whne the first child is an elemnt.
                //The empty string significant whitespace is a workaround to avoid the new lines.
                foreach (XmlNode element in xmlDocument.SelectNodes("//literalParameter|//literal|//literalVariable|//text")!)/*Never null when SelectNodes is called on an XmlDocument.*/
                {
                    if (
                            element.ChildNodes.Count > 1
                            && element.ChildNodes.OfType<XmlNode>().Any
                            (
                                n => n.NodeType == XmlNodeType.Text
                                    || n.NodeType == XmlNodeType.Whitespace
                            )
                       )
                    {
                        element.InsertBefore
                        (
                            //element.OwnerDocument!.CreateSignificantWhitespace(""),
                            xmlDocument.CreateSignificantWhitespace(""),
                            element.FirstChild
                        );
                    }
                }
            }
        }

        public XmlAttribute MakeAttribute(XmlDocument xmlDocument, string name, string attributeValue)
        {
            XmlAttribute attribute = xmlDocument.CreateAttribute(name);
            attribute.Value = attributeValue;

            return attribute;
        }

        public XmlElement MakeElement(XmlDocument xmlDocument, string name, string? innerXml = null, IDictionary<string, string>? attributes = null)
        {
            XmlElement element = xmlDocument.CreateElement(name);
            if (innerXml != null)
                element.InnerXml = innerXml;

            if (attributes != null)
            {
                foreach (KeyValuePair<string, string> entry in attributes)
                    element.Attributes.Append(MakeAttribute(xmlDocument, entry.Key, entry.Value));
            }

            return element;
        }

        public XmlDocumentFragment MakeFragment(XmlDocument xmlDocument, string? innerXml = null)
        {
            XmlDocumentFragment xmlDocumentFragment = xmlDocument.CreateDocumentFragment();
            if (innerXml != null)
                xmlDocumentFragment.InnerXml = innerXml;

            return xmlDocumentFragment;
        }

        public XmlDocument ToXmlDocument(XmlNode xmlNode, bool preserveWhiteSpace = true) 
            => ToXmlDocument(xmlNode.OuterXml, preserveWhiteSpace);

        public XmlDocument ToXmlDocument(string xmlString, bool preserveWhiteSpace = true)
        {
            return LoadXml(new XmlDocument { PreserveWhitespace = preserveWhiteSpace });
            XmlDocument LoadXml(XmlDocument xmlDocument)
            {
                xmlDocument.LoadXml(xmlString);
                return xmlDocument;
            }
        }
        #endregion Methods
    }
}
