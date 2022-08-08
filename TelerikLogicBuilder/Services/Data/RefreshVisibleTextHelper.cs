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

        public XmlDocument RefreshSetValueToNullFunctionVisibleTexts(XmlDocument xmlDocument)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.RETRACTFUNCTIONELEMENT}")
                .ForEach
                (
                    element => _updateVisibleTextAttribute.UpdateRetractFunctionVisibleText
                    (
                        element
                    )
                );

            return xmlDocument;
        }

        public XmlDocument RefreshVariableVisibleTexts(XmlDocument xmlDocument)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.VARIABLEELEMENT}")
                .ForEach
                (
                    element => _updateVisibleTextAttribute.UpdateVariableVisibleText(element)
                );

            return xmlDocument;
        }
    }
}
