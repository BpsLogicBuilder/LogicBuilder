﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class ObjectListVariableDataParser : IObjectListVariableDataParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;

        public ObjectListVariableDataParser(IContextProvider contextProvider)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _enumHelper = contextProvider.EnumHelper;
        }

        public ObjectListVariableData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.OBJECTLISTVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{3F9B041C-9E47-4244-B379-900169E94DD4}");

            return new ObjectListVariableData
            (
                _xmlDocumentHelpers.GetSingleChildElement(xmlElement),
                xmlElement,
                _enumHelper
            );
        }
    }
}