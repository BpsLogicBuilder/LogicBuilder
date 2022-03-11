﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class ConnectorDataParser : IConnectorDataParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IExceptionHelper _exceptionHelper;

        public ConnectorDataParser(IContextProvider contextProvider)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
        }

        public ConnectorData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.CONNECTORELEMENT)
                throw _exceptionHelper.CriticalException("{C24202A1-B01A-44E4-8E61-59141CE6FCA2}");

            Dictionary<string, XmlElement> childElements = _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name);
            childElements.TryGetValue(XmlDataConstants.METAOBJECTELEMENT, out XmlElement? metaObjectElement);

            return new ConnectorData
            (
                xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                childElements[XmlDataConstants.TEXTELEMENT],
                metaObjectElement,
                (ConnectorCategory)short.Parse(xmlElement.GetAttribute(XmlDataConstants.CONNECTORCATEGORYATTRIBUTE), CultureInfo.InvariantCulture),
                 xmlElement
            );
        }
    }
}