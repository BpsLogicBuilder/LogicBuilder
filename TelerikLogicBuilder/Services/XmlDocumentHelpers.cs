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
            Func<XmlElement, bool> filter = null, 
            Func<IEnumerable<XmlElement>, IEnumerable<XmlElement>> enumerableFunc = null)
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

            return GetElements(parameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value);

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

        public XmlElement GetSingleChildElement(XmlNode xmlNode, Func<XmlElement, bool> filter = null) 
            => filter != null
                ? xmlNode.ChildNodes.OfType<XmlElement>().SingleOrDefault(filter)
                : xmlNode.ChildNodes.OfType<XmlElement>().SingleOrDefault();

        public XmlAttribute MakeAttribute(XmlDocument xmlDocument, string name, string attributeValue)
        {
            XmlAttribute attribute = xmlDocument.CreateAttribute(name);
            attribute.Value = attributeValue;

            return attribute;
        }

        public XmlElement MakeElement(XmlDocument xmlDocument, string name, string innerXml = null, IDictionary<string, string> attributes = null)
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

        public XmlDocumentFragment MakeFragment(XmlDocument xmlDocument, string innerXml = null)
        {
            XmlDocumentFragment xmlDocumentFragment = xmlDocument.CreateDocumentFragment();
            if (innerXml != null)
                xmlDocumentFragment.InnerXml = innerXml;

            return xmlDocumentFragment;
        }

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
