using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class PriorityDataParser : IPriorityDataParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public PriorityDataParser(
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public int? Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.SHAPEDATAELEMENT)
                throw _exceptionHelper.CriticalException("{33F56766-7ACF-4CA6-A85F-C30B3E3B1072}");

            if (xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value != TableColumnName.PRIORITYCOLUMN)
                return null;

            return int.TryParse
            (
                _xmlDocumentHelpers.GetSingleChildElement(xmlElement).InnerText,
                out int priority
            ) ? priority : null;
        }
    }
}
