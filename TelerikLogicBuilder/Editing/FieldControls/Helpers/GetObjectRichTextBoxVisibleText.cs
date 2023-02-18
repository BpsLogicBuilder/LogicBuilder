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
            XmlDocument shapeXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlString);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshVariableVisibleTexts(shapeXmlDocument);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshFunctionVisibleTexts(shapeXmlDocument, application);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshConstructorVisibleTexts(shapeXmlDocument, application);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshLiteralListVisibleTexts(shapeXmlDocument, application);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshObjectListVisibleTexts(shapeXmlDocument, application);

            XmlElement documentElement = _xmlDocumentHelpers.GetDocumentElement(shapeXmlDocument);
            return GetVisibleText(documentElement.Name, documentElement.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE));

            string GetVisibleText(string elementName, string visibleText)
            {
                switch (elementName)
                {
                    case XmlDataConstants.VARIABLEELEMENT:
                        return string.Format(CultureInfo.CurrentCulture, Strings.popupVariableDescriptionFormat, visibleText);
                    case XmlDataConstants.FUNCTIONELEMENT:
                        return string.Format(CultureInfo.CurrentCulture, Strings.popupFunctionDescriptionFormat, visibleText);
                    case XmlDataConstants.CONSTRUCTORELEMENT:
                        return string.Format(CultureInfo.CurrentCulture, Strings.popupConstructorDescriptionFormat, visibleText);
                    case XmlDataConstants.LITERALLISTELEMENT:
                    case XmlDataConstants.OBJECTLISTELEMENT:
                        return visibleText;
                    default:
                        throw _exceptionHelper.CriticalException("{33438A6C-017A-41A2-B115-4D2CB238D94D}");
                }
            }
        }
    }
}
