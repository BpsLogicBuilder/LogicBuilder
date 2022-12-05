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

        private static XmlWriterSettings FragmentSettings
        {
            get
            {
                return new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    ConformanceLevel = ConformanceLevel.Fragment
                };
            }
        }
        #endregion Properties

        #region Methods
        public XmlElement AddElementToDoc(XmlDocument xmlDocument, XmlElement element) 
            => MakeElement
            (
                xmlDocument,
                element.Name,
                element.InnerXml,
                element.Attributes.Cast<XmlAttribute>().ToDictionary(e => e.Name, e => e.Value)
            );

        public XmlWriter CreateFormattedXmlWriter(string fullPath)
            => XmlWriter.Create(fullPath, FormattedSettings);

        public XmlWriter CreateFormattedXmlWriter(string fullPath, Encoding encoding)
        {
            XmlWriterSettings xmlWriterSettings = FormattedSettings.Clone();
            xmlWriterSettings.Encoding = encoding;
            return XmlWriter.Create(fullPath, xmlWriterSettings);
        }

        public XmlWriter CreateFormattedXmlWriter(StringBuilder stringBuilder) 
            => XmlWriter.Create(new StringWriter(stringBuilder, CultureInfo.InvariantCulture), FormattedSettings);

        public XmlWriter CreateFormattedXmlWriterWithDeclaration(StringBuilder stringBuilder) 
            => XmlWriter.Create(new StringWriter(stringBuilder, CultureInfo.InvariantCulture), FormattedSettingsWithDeclaration);

        public XmlWriter CreateFragmentXmlWriter(StringBuilder stringBuilder)
            => XmlWriter.Create(new StringWriter(stringBuilder, CultureInfo.InvariantCulture), FragmentSettings);

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

        public XmlElement GetDocumentElement(XmlDocument xmlDocument)
        {
            if (xmlDocument.DocumentElement == null)
                throw _exceptionHelper.CriticalException("{1FF53CC7-2D56-4E59-8AC4-AAC49D7BF296}");

            return xmlDocument.DocumentElement;
        }

        public string GetGenericArgumentTreeNodeDescription(XmlElement element)
        {
            Dictionary<string, string> descriptionTable = new()
            {
                [XmlDataConstants.LITERALPARAMETERELEMENT] = Strings.literalGenericArgDescription,
                [XmlDataConstants.OBJECTPARAMETERELEMENT] = Strings.objectGenericArgDescription,
                [XmlDataConstants.LITERALLISTPARAMETERELEMENT] = Strings.listOfLiteralsGenericArgDescription,
                [XmlDataConstants.OBJECTLISTPARAMETERELEMENT] = Strings.listOfObjectsGenericArgDescription,
            };

            if (!descriptionTable.TryGetValue(element.Name, out string? description))
                throw _exceptionHelper.CriticalException("{0E683F61-C0D3-484E-A183-C211D0F45C3D}");

            return description;
        }

        public int GetImageIndex(XmlElement element)
        {
            Dictionary<string, int> indexTable = new()
            {
                [XmlDataConstants.LITERALPARAMETERELEMENT]= ImageIndexes.LITERALPARAMETERIMAGEINDEX,
                [XmlDataConstants.LITERALVARIABLEELEMENT] = ImageIndexes.LITERALPARAMETERIMAGEINDEX,
                [XmlDataConstants.OBJECTPARAMETERELEMENT] = ImageIndexes.OBJECTPARAMETERIMAGEINDEX,
                [XmlDataConstants.OBJECTVARIABLEELEMENT] = ImageIndexes.OBJECTPARAMETERIMAGEINDEX,
                [XmlDataConstants.LITERALLISTPARAMETERELEMENT] = ImageIndexes.LITERALLISTPARAMETERIMAGEINDEX,
                [XmlDataConstants.LITERALLISTVARIABLEELEMENT] = ImageIndexes.LITERALLISTPARAMETERIMAGEINDEX,
                [XmlDataConstants.OBJECTLISTPARAMETERELEMENT] = ImageIndexes.OBJECTLISTPARAMETERIMAGEINDEX,
                [XmlDataConstants.OBJECTLISTVARIABLEELEMENT] = ImageIndexes.OBJECTLISTPARAMETERIMAGEINDEX,
            };

            if (!indexTable.TryGetValue(element.Name, out int imageIndex))
                throw _exceptionHelper.CriticalException("{658C52A4-5380-4D8C-BFA9-3133968249C0}");

            return imageIndex;
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

            return GetElements(parameterElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE));

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

        public string GetVariableTreeNodeDescription(XmlElement element)
        {
            Dictionary<string, string> descriptionTable = new()
            {
                [XmlDataConstants.LITERALVARIABLEELEMENT] = Strings.literalVarNodeDescription,
                [XmlDataConstants.OBJECTVARIABLEELEMENT] = Strings.objectVarNodeDescription,
                [XmlDataConstants.LITERALLISTVARIABLEELEMENT] = Strings.listOfLiteralsVarNodeDescription,
                [XmlDataConstants.OBJECTLISTVARIABLEELEMENT] = Strings.listOfObjectsVarNodeDescription,
            };

            if (!descriptionTable.TryGetValue(element.Name, out string? description))
                throw _exceptionHelper.CriticalException("{B194FB8C-8B0B-48D6-A4A2-5F17F45F3E16}");

            return description;
        }

        public string GetVisibleText(XmlElement element)
            => element.ChildNodes
                .OfType<XmlNode>()
                .Aggregate
                (
                    new StringBuilder(), (sb, next) =>
                    {
                        switch (next.NodeType)
                        {
                            case XmlNodeType.Element:
                                XmlElement xmlElement = (XmlElement)next;
                                switch (xmlElement.Name)
                                {
                                    case XmlDataConstants.VARIABLEELEMENT:
                                        sb.Append(string.Format(CultureInfo.CurrentCulture, Strings.popupVariableDescriptionFormat, xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value));/*name attribute not null*/
                                        break;
                                    case XmlDataConstants.FUNCTIONELEMENT:
                                        sb.Append(xmlElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value);/*visibleText attribute not null*/
                                        break;
                                    case XmlDataConstants.CONSTRUCTORELEMENT:
                                        sb.Append(xmlElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value);/*visibleText attribute not null*/
                                        break;
                                    default:
                                        throw _exceptionHelper.CriticalException("{51C90B4E-2ABE-4381-817E-87EA1D72F684}");
                                }
                                break;
                            case XmlNodeType.Text:
                                XmlText xmlText = (XmlText)next;
                                sb.Append(xmlText.Value);
                                break;
                            case XmlNodeType.Whitespace:
                                XmlWhitespace xmlWhitespace = (XmlWhitespace)next;
                                sb.Append(xmlWhitespace.Value);
                                break;
                            default:
                                break;
                        }
                        return sb;
                    }
                ).ToString();

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
                foreach (XmlElement element in SelectElements(xmlDocument, "//literalParameter|//literal|//literalVariable|//text"))
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

        public List<XmlElement> SelectElements(XmlDocument xmlDocument, string xPath) 
            => xmlDocument
                .SelectNodes(xPath)!/*SelectNodes is never null when XmlNode is XmlDocument*/
                .OfType<XmlElement>()
                .ToList();

        public XmlElement SelectSingleElement(XmlDocument xmlDocument, string xPath)
        {
            if (xmlDocument.SelectSingleNode(xPath) is not XmlElement xmlElement)
                throw _exceptionHelper.CriticalException("{AD1845E9-CF50-45B5-B7BC-183B644F05C5}");

            return xmlElement;
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

        public XmlElement ToXmlElement(string xmlString, bool preserveWhiteSpace = true)
        {
            if (string.IsNullOrEmpty(xmlString))
                throw _exceptionHelper.CriticalException("{F125DCE9-91EB-4E14-8E25-7ABF51DA0DA2}");

            return ToXmlDocument(xmlString, preserveWhiteSpace).DocumentElement!;
        }
        #endregion Methods
    }
}
