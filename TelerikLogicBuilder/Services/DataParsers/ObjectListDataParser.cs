﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class ObjectListDataParser : IObjectListDataParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;

        public ObjectListDataParser(IContextProvider contextProvider)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _enumHelper = contextProvider.EnumHelper;
        }

        public ObjectListData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw _exceptionHelper.CriticalException("{A1B4ABA9-CFE6-4E10-A4A7-9C555284AECD}");

            return new ObjectListData
            (
                xmlElement.GetAttribute(XmlDataConstants.OBJECTTYPEATTRIBUTE),
                _enumHelper.ParseEnumText<ListType>(xmlElement.GetAttribute(XmlDataConstants.LISTTYPEATTRIBUTE)),
                xmlElement.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE),
                _xmlDocumentHelpers.GetChildElements(xmlElement),
                xmlElement
            );
        }
    }
}