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
            return GetVisibleText(documentElement.Name, documentElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value);

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
            XmlDocument xmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlString);
            xmlDocument = _refreshVisibleTextHelper.RefreshVariableVisibleTexts(xmlDocument);
            xmlDocument = _refreshVisibleTextHelper.RefreshFunctionVisibleTexts(xmlDocument, application);
            xmlDocument = _refreshVisibleTextHelper.RefreshConstructorVisibleTexts(xmlDocument, application);
            xmlDocument = _refreshVisibleTextHelper.RefreshLiteralListVisibleTexts(xmlDocument, application);
            xmlDocument = _refreshVisibleTextHelper.RefreshObjectListVisibleTexts(xmlDocument, application);
            return xmlDocument.OuterXml;
        }
    }
}
