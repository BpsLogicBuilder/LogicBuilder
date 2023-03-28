using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal class XmlDataHelper : IXmlDataHelper
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public XmlDataHelper(
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public string BuildConstructorXml(string name, string visibleText, string genericArgumentsXml, string parametersXml)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.CONSTRUCTORELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, name);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.VISIBLETEXTATTRIBUTE, visibleText);
                    xmlTextWriter.WriteRaw(genericArgumentsXml);
                    xmlTextWriter.WriteRaw(parametersXml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }

        public string BuildDefaultConstructorXml(ClosedConstructor closedConstructor)
        {
            IDictionary<string, string> parameterDefaults = closedConstructor.Constructor.Parameters.OfType<LiteralParameter>()
                .Where(p => !string.IsNullOrEmpty(p.DefaultValue))
                .ToDictionary(p => p.Name, p => p.DefaultValue);

            return BuildConstructorXml
            (
                closedConstructor.Constructor.Name,
                closedConstructor.Constructor.Name,
                BuildGenericArgumentsXml(closedConstructor.GenericArguments),
                BuildDefaultParametersXml(closedConstructor.Constructor.Parameters, parameterDefaults)
            );
        }

        public string BuildEmptyConstructorXml(string name, string visibleText)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.CONSTRUCTORELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, name);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.VISIBLETEXTATTRIBUTE, visibleText);
                    xmlTextWriter.WriteElementString(XmlDataConstants.GENERICARGUMENTSELEMENT, string.Empty);
                    xmlTextWriter.WriteElementString(XmlDataConstants.PARAMETERSELEMENT, string.Empty);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }

        public string BuildFunctionXml(string name, string visibleText, string genericArgumentsXml, string parametersXml)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.FUNCTIONELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, name);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.VISIBLETEXTATTRIBUTE, visibleText);
                    xmlTextWriter.WriteRaw(genericArgumentsXml);
                    xmlTextWriter.WriteRaw(parametersXml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }

        public string BuildGenericArgumentsXml(IList<GenericConfigBase> genericArgs)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.GENERICARGUMENTSELEMENT);
                    foreach (GenericConfigBase genericArg in genericArgs)
                        xmlTextWriter.WriteRaw(genericArg.ToXml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }

        public string BuildLiteralListXml(LiteralListElementType literalType, ListType listType, string visibleText, string innerXml)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALLISTELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.LITERALTYPEATTRIBUTE, Enum.GetName(typeof(LiteralListElementType), literalType));
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.LISTTYPEATTRIBUTE, Enum.GetName(typeof(ListType), listType));
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.VISIBLETEXTATTRIBUTE, visibleText);
                    xmlTextWriter.WriteRaw(innerXml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }

        public string BuildLiteralXml(string innerXml)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALELEMENT);
                    xmlTextWriter.WriteRaw(innerXml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }

        public string BuildNotXml(string innerXml)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.NOTELEMENT);
                    xmlTextWriter.WriteRaw(innerXml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }

        public string BuildObjectListXml(string objectType, ListType listType, string visibleText, string innerXml)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTLISTELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.OBJECTTYPEATTRIBUTE, objectType);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.LISTTYPEATTRIBUTE, Enum.GetName(typeof(ListType), listType));
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.VISIBLETEXTATTRIBUTE, visibleText);
                    xmlTextWriter.WriteRaw(innerXml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }

        public string BuildObjectXml(string innerXml)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTELEMENT);
                    xmlTextWriter.WriteRaw(innerXml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }

        public string BuildParameterXml(ParameterBase parameter, string innerXml)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(GetElementName(parameter.ParameterCategory));
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, parameter.Name);
                    xmlTextWriter.WriteRaw(innerXml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }

        public string BuildVariableXml(string name)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.VARIABLEELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, name);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.VISIBLETEXTATTRIBUTE, name);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }

        public string GetElementName(ParameterCategory category)
        {
            return category switch
            {
                ParameterCategory.Literal => XmlDataConstants.LITERALPARAMETERELEMENT,
                ParameterCategory.Object => XmlDataConstants.OBJECTPARAMETERELEMENT,
                ParameterCategory.Generic => XmlDataConstants.GENERICPARAMETERELEMENT,
                ParameterCategory.LiteralList => XmlDataConstants.LITERALLISTPARAMETERELEMENT,
                ParameterCategory.ObjectList => XmlDataConstants.OBJECTLISTPARAMETERELEMENT,
                ParameterCategory.GenericList => XmlDataConstants.GENERICLISTPARAMETERELEMENT,
                _ => throw _exceptionHelper.CriticalException("{4FEA1202-E684-4BDB-96A4-1ED9EED477EB}"),
            };
        }

        private string BuildDefaultParametersXml(IList<ParameterBase> parameters, IDictionary<string, string> parameterDefaults)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.PARAMETERSELEMENT);
                foreach (LiteralParameter parameter in parameters.OfType<LiteralParameter>())
                {
                    //if (!defaults.TryGetValue(parameter.Name, out string innerXml))
                    // innerXml = ((LiteralParameter)parameter).GetDefaultString();
                    //Do not add element when there is no default value
                    //to prevent parameter checkboxes being checked for optional
                    //items with no default value
                    if (parameterDefaults.TryGetValue(parameter.Name, out string? innerXml))
                        xmlTextWriter.WriteRaw(BuildParameterXml(parameter, innerXml));
                }
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }
    }
}
