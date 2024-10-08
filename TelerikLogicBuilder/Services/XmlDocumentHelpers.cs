﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
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

        public string GetFunctionTreeNodeDescription(XmlElement element)
        {
            Dictionary<string, string> descriptionTable = new()
            {
                [XmlDataConstants.LITERALELEMENT] = Strings.literalFuncNodeDescription,
                [XmlDataConstants.OBJECTELEMENT] = Strings.objectFuncNodeDescription,
                [XmlDataConstants.GENERICELEMENT] = Strings.genericFuncNodeDescription,
                [XmlDataConstants.LITERALLISTELEMENT] = Strings.listOfLiteralsFuncNodeDescription,
                [XmlDataConstants.OBJECTLISTELEMENT] = Strings.listOfObjectsFuncNodeDescription,
                [XmlDataConstants.GENERICLISTELEMENT] = Strings.listOfGenericsFuncNodeDescription,
            };

            if (!descriptionTable.TryGetValue(element.Name, out string? description))
                throw _exceptionHelper.CriticalException("{D93963B9-4358-410B-A5AC-ED4239DF3716}");

            return description;
        }

        public string[] GetGenericArguments(XmlDocument xmlDocument, string genericArgumentsXPath) 
            => GetChildElements
            (
                SelectSingleElement(xmlDocument, genericArgumentsXPath)
            )
            .Select(e => e.InnerText)
            .ToArray();

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
                [XmlDataConstants.GENERICPARAMETERELEMENT] = ImageIndexes.GENERICPARAMETERIMAGEINDEX,
                [XmlDataConstants.LITERALLISTPARAMETERELEMENT] = ImageIndexes.LITERALLISTPARAMETERIMAGEINDEX,
                [XmlDataConstants.LITERALLISTVARIABLEELEMENT] = ImageIndexes.LITERALLISTPARAMETERIMAGEINDEX,
                [XmlDataConstants.OBJECTLISTPARAMETERELEMENT] = ImageIndexes.OBJECTLISTPARAMETERIMAGEINDEX,
                [XmlDataConstants.OBJECTLISTVARIABLEELEMENT] = ImageIndexes.OBJECTLISTPARAMETERIMAGEINDEX,
                [XmlDataConstants.GENERICLISTPARAMETERELEMENT] = ImageIndexes.GENERICLISTPARAMETERIMAGEINDEX,
            };

            if (!indexTable.TryGetValue(element.Name, out int imageIndex))
                throw _exceptionHelper.CriticalException("{658C52A4-5380-4D8C-BFA9-3133968249C0}");

            return imageIndex;
        }

        public IList<XmlElement> GetParameterElements(XmlElement constructorOrFunctionElement)
        {
            if (constructorOrFunctionElement.Name != XmlDataConstants.CONSTRUCTORELEMENT
                && constructorOrFunctionElement.Name != XmlDataConstants.FUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{AAAF9071-4DCE-4BD2-B70A-E307C31769FC}");

            return GetElements();

            IList<XmlElement> GetElements()
                => GetChildElements
                (
                    GetSingleChildElement
                    (
                        constructorOrFunctionElement,
                        e => e.Name == XmlDataConstants.PARAMETERSELEMENT
                    )
                )
                .ToArray();
        }

        public string GetParameterTreeNodeDescription(XmlElement element)
        {
            Dictionary<string, string> descriptionTable = new()
            {
                [XmlDataConstants.LITERALPARAMETERELEMENT] = Strings.literalParamNodeDescription,
                [XmlDataConstants.OBJECTPARAMETERELEMENT] = Strings.objectParamNodeDescription,
                [XmlDataConstants.GENERICPARAMETERELEMENT] = Strings.genericParamNodeDescription,
                [XmlDataConstants.LITERALLISTPARAMETERELEMENT] = Strings.listOfLiteralsParamNodeDescription,
                [XmlDataConstants.OBJECTLISTPARAMETERELEMENT] = Strings.listOfObjectsParamNodeDescription,
                [XmlDataConstants.GENERICLISTPARAMETERELEMENT] = Strings.listOfGenericsParamNodeDescription,
            };

            if (!descriptionTable.TryGetValue(element.Name, out string? description))
                throw _exceptionHelper.CriticalException("{D6D3D0CC-BDBC-43A2-A985-A43707945DD4}");

            return description;
        }

        public List<XmlElement> GetSiblingParameterElements(XmlElement parameterElement)
        {
            switch (parameterElement.Name)
            {
                case XmlDataConstants.LITERALPARAMETERELEMENT:
                case XmlDataConstants.LITERALLISTPARAMETERELEMENT:
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{B52855E1-4F4B-4F7A-9667-773F9BCBAF26}");
            }

            return GetElements(parameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value);

            List<XmlElement> GetElements(string parameterName) 
                => GetChildElements
                (
                    parameterElement.ParentNode ?? throw _exceptionHelper.CriticalException("{5FE9F9BD-DEF5-475E-B6E8-D46088A4C631}")
                )
                .Where(e => e.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value != parameterName)
                .ToList();
        }

        public XmlElement GetSingleChildElement(XmlNode xmlNode, Func<XmlElement, bool>? filter = null) 
            => filter != null
                ? xmlNode.ChildNodes.OfType<XmlElement>().Single(filter)
                : xmlNode.ChildNodes.OfType<XmlElement>().Single();

        public XmlElement? GetSingleOrDefaultChildElement(XmlNode xmlNode, Func<XmlElement, bool>? filter = null)
            => filter != null
                ? xmlNode.ChildNodes.OfType<XmlElement>().SingleOrDefault(filter)
                : xmlNode.ChildNodes.OfType<XmlElement>().SingleOrDefault();

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

        public string GetXmlString(string xmlDocumentString) 
            => GetXmlString(ToXmlDocument(xmlDocumentString, false));//formatting with initial partial whitespace result in a not fully formatted doc.

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

        public TResult Query<TResult>(XmlNode xmlNode, Func<XmlElement, bool> filter, Func<IEnumerable<XmlElement>, TResult> enumerableFunc)
        {
            return enumerableFunc(Filter());

            IEnumerable<XmlElement> Filter()
                => xmlNode.ChildNodes.OfType<XmlElement>().Where(filter);
        }

        public XmlElement ReplaceElement(XmlElement currentElement, XmlElement newElement)
        {
            if (currentElement.ParentNode == null)
                throw _exceptionHelper.CriticalException("{DDFF7E3B-4A81-4E53-ADFC-0615338B5FDC}");

            newElement = AddElementToDoc(currentElement.OwnerDocument, newElement);

            return (XmlElement)currentElement.ParentNode.ReplaceChild(newElement, currentElement);
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
