using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class GetObjectRichTextBoxVisibleText : IGetObjectRichTextBoxVisibleText
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public GetObjectRichTextBoxVisibleText(
            IExceptionHelper exceptionHelper,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public string GetVisibleText(string xmlString, ApplicationTypeInfo application)
        {
            XmlElement documentElement = _xmlDocumentHelpers.ToXmlElement(xmlString);
            return GetVisibleText(documentElement.Name, documentElement.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE));

            string GetVisibleText(string elementName, string visibleText)
            {
                return elementName switch
                {
                    XmlDataConstants.VARIABLEELEMENT => string.Format(CultureInfo.CurrentCulture, Strings.popupVariableDescriptionFormat, visibleText),
                    XmlDataConstants.FUNCTIONELEMENT => string.Format(CultureInfo.CurrentCulture, Strings.popupFunctionDescriptionFormat, visibleText),
                    XmlDataConstants.CONSTRUCTORELEMENT => string.Format(CultureInfo.CurrentCulture, Strings.popupConstructorDescriptionFormat, visibleText),
                    XmlDataConstants.LITERALLISTELEMENT or XmlDataConstants.OBJECTLISTELEMENT => visibleText,
                    _ => throw _exceptionHelper.CriticalException("{33438A6C-017A-41A2-B115-4D2CB238D94D}"),
                };
            }
        }

        public string RefreshVisibleTexts(string xmlString, ApplicationTypeInfo application)
        {
            XmlDocument shapeXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlString);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshVariableVisibleTexts(shapeXmlDocument);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshFunctionVisibleTexts(shapeXmlDocument, application);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshConstructorVisibleTexts(shapeXmlDocument, application);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshLiteralListVisibleTexts(shapeXmlDocument, application);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshObjectListVisibleTexts(shapeXmlDocument, application);
            return shapeXmlDocument.OuterXml;
        }
    }
}
