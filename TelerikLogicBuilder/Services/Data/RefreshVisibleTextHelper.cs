using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Data
{
    internal class RefreshVisibleTextHelper : IRefreshVisibleTextHelper
    {
        private readonly IUpdateVisibleTextAttribute _updateVisibleTextAttribute;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public RefreshVisibleTextHelper(
            IUpdateVisibleTextAttribute updateVisibleTextAttribute,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _updateVisibleTextAttribute = updateVisibleTextAttribute;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public XmlElement RefreshAllVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application)
        {
            XmlDocument shapeXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlElement.OuterXml);
            shapeXmlDocument = RefreshVariableVisibleTexts(shapeXmlDocument);
            shapeXmlDocument = RefreshFunctionVisibleTexts(shapeXmlDocument, application);
            shapeXmlDocument = RefreshConstructorVisibleTexts(shapeXmlDocument, application);
            shapeXmlDocument = RefreshLiteralListVisibleTexts(shapeXmlDocument, application);
            shapeXmlDocument = RefreshObjectListVisibleTexts(shapeXmlDocument, application);
            shapeXmlDocument = RefreshDecisionVisibleTexts(shapeXmlDocument, application);
            shapeXmlDocument = RefreshSetValueFunctionVisibleTexts(shapeXmlDocument, application);
            shapeXmlDocument = RefreshSetValueToNullFunctionVisibleTexts(shapeXmlDocument);
            return _xmlDocumentHelpers.GetDocumentElement(shapeXmlDocument);
        }

        public XmlDocument RefreshConstructorVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.CONSTRUCTORELEMENT}")
                .ForEach
                (
                    element => _updateVisibleTextAttribute.UpdateConstructorVisibleText
                    (
                        element, 
                        application
                    )
                );

            return xmlDocument;
        }

        public XmlElement RefreshConstructorVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application)
        {
            XmlDocument shapeXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlElement.OuterXml);
            shapeXmlDocument = RefreshConstructorVisibleTexts(shapeXmlDocument, application);
            return _xmlDocumentHelpers.GetDocumentElement(shapeXmlDocument);
        }

        public XmlDocument RefreshDecisionVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.DECISIONELEMENT}")
                .ForEach
                (
                    element => _updateVisibleTextAttribute.UpdateDecisionVisibleText
                    (
                        element,
                        application
                    )
                );

            return xmlDocument;
        }

        public XmlElement RefreshDecisionVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application)
        {
            XmlDocument shapeXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlElement.OuterXml);
            shapeXmlDocument = RefreshDecisionVisibleTexts(shapeXmlDocument, application);
            return _xmlDocumentHelpers.GetDocumentElement(shapeXmlDocument);
        }

        public XmlDocument RefreshFunctionVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.FUNCTIONELEMENT}")
                .ForEach
                (
                    element => _updateVisibleTextAttribute.UpdateFunctionVisibleText
                    (
                        element,
                        application
                    )
                );

            return xmlDocument;
        }

        public XmlElement RefreshFunctionVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application)
        {
            XmlDocument shapeXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlElement.OuterXml);
            shapeXmlDocument = RefreshFunctionVisibleTexts(shapeXmlDocument, application);
            return _xmlDocumentHelpers.GetDocumentElement(shapeXmlDocument);
        }

        public XmlDocument RefreshLiteralListVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.LITERALLISTELEMENT}")
                .ForEach
                (
                    element => _updateVisibleTextAttribute.UpdateLiteralListVisibleText
                    (
                        element,
                        application,
                        element.ParentNode?.Name == XmlDataConstants.LITERALLISTPARAMETERELEMENT
                            ? ((XmlElement)element.ParentNode).GetAttribute(XmlDataConstants.NAMEATTRIBUTE)
                            : null
                    )
                );

            return xmlDocument;
        }

        public XmlElement RefreshLiteralListVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application)
        {
            XmlDocument shapeXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlElement.OuterXml);
            shapeXmlDocument = RefreshLiteralListVisibleTexts(shapeXmlDocument, application);
            return _xmlDocumentHelpers.GetDocumentElement(shapeXmlDocument);
        }

        public XmlDocument RefreshObjectListVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.OBJECTLISTELEMENT}")
                .ForEach
                (
                    element => _updateVisibleTextAttribute.UpdateObjectListVisibleText
                    (
                        element,
                        application,
                        element.ParentNode?.Name == XmlDataConstants.OBJECTLISTPARAMETERELEMENT
                            ? ((XmlElement)element.ParentNode).GetAttribute(XmlDataConstants.NAMEATTRIBUTE)
                            : null
                    )
                );

            return xmlDocument;
        }

        public XmlElement RefreshObjectListVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application)
        {
            XmlDocument shapeXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlElement.OuterXml);
            shapeXmlDocument = RefreshObjectListVisibleTexts(shapeXmlDocument, application);
            return _xmlDocumentHelpers.GetDocumentElement(shapeXmlDocument);
        }

        public XmlDocument RefreshSetValueFunctionVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.ASSERTFUNCTIONELEMENT}")
                .ForEach
                (
                    element => _updateVisibleTextAttribute.UpdateAssertFunctionVisibleText
                    (
                        element,
                        application
                    )
                );

            return xmlDocument;
        }

        public XmlElement RefreshSetValueFunctionVisibleTexts(XmlElement xmlElement, ApplicationTypeInfo application)
        {
            XmlDocument shapeXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlElement.OuterXml);
            shapeXmlDocument = RefreshSetValueFunctionVisibleTexts(shapeXmlDocument, application);
            return _xmlDocumentHelpers.GetDocumentElement(shapeXmlDocument);
        }

        public XmlDocument RefreshSetValueToNullFunctionVisibleTexts(XmlDocument xmlDocument)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.RETRACTFUNCTIONELEMENT}")
                .ForEach
                (
                    _updateVisibleTextAttribute.UpdateRetractFunctionVisibleText
                );

            return xmlDocument;
        }

        public XmlElement RefreshSetValueToNullFunctionVisibleTexts(XmlElement xmlElement)
        {
            XmlDocument shapeXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlElement.OuterXml);
            shapeXmlDocument = RefreshSetValueToNullFunctionVisibleTexts(shapeXmlDocument);
            return _xmlDocumentHelpers.GetDocumentElement(shapeXmlDocument);
        }

        public XmlDocument RefreshVariableVisibleTexts(XmlDocument xmlDocument)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.VARIABLEELEMENT}")
                .ForEach
                (
                    _updateVisibleTextAttribute.UpdateVariableVisibleText
                );

            return xmlDocument;
        }

        public XmlElement RefreshVariableVisibleTexts(XmlElement xmlElement)
        {
            XmlDocument shapeXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlElement.OuterXml);
            shapeXmlDocument = RefreshVariableVisibleTexts(shapeXmlDocument);
            return _xmlDocumentHelpers.GetDocumentElement(shapeXmlDocument);
        }
    }
}
