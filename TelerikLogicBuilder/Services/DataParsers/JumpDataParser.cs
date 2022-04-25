using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class JumpDataParser : IJumpDataParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IExceptionHelper _exceptionHelper;

        public JumpDataParser(IXmlDocumentHelpers xmlDocumentHelpers, IExceptionHelper exceptionHelper)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _exceptionHelper = exceptionHelper;
        }

        public string Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.SHAPEDATAELEMENT)
                throw _exceptionHelper.CriticalException("{61BBC8E3-D218-4775-87BA-E235797B994A}");

            if (xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE) != UniversalMasterName.JUMPOBJECT)
                return string.Empty;
                
            return _xmlDocumentHelpers.GetSingleChildElement(xmlElement).InnerText;
        }
    }
}
