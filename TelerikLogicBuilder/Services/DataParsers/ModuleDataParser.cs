﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class ModuleDataParser : IModuleDataParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ModuleDataParser(
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public string Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.SHAPEDATAELEMENT)
                throw _exceptionHelper.CriticalException("{F3FF3F75-45F3-4816-8333-DE0EA2C9C9CF}");

            if (xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value != UniversalMasterName.MODULE)
                return string.Empty;
            
            return _xmlDocumentHelpers.GetSingleChildElement(xmlElement).InnerText;
        }
    }
}
