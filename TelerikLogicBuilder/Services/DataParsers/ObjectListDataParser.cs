using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class ObjectListDataParser : IObjectListDataParser
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IObjectListBoxItemFactory _objectListBoxItemFactory;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ObjectListDataParser(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IObjectListBoxItemFactory objectListBoxItemFactory,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _objectListBoxItemFactory = objectListBoxItemFactory;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
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

        public ObjectListData Parse(XmlElement xmlElement, ObjectListParameterElementInfo listInfo, IApplicationControl applicationControl)
        {
            if (xmlElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw _exceptionHelper.CriticalException("{4B56236E-3BFC-41D6-A4E5-95F027CA3D1B}");

            string objectTypeString = xmlElement.GetAttribute(XmlDataConstants.OBJECTTYPEATTRIBUTE);
            ListType listType = _enumHelper.ParseEnumText<ListType>(xmlElement.GetAttribute(XmlDataConstants.LISTTYPEATTRIBUTE));
            List<XmlElement> chileElements = GetUniqueChildElements(_xmlDocumentHelpers.GetChildElements(xmlElement));
            return new ObjectListData
            (
                objectTypeString,
                listType,
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.listParameterCountFormat,
                    listInfo.HasParameter
                        ? listInfo.Parameter.Name
                        : _enumHelper.GetTypeDescription(listType, objectTypeString),
                    chileElements.Count
                ),
                chileElements,
                xmlElement
            );

            List<XmlElement> GetUniqueChildElements(List<XmlElement> allChileElements)
            {
                if (!_typeLoadHelper.TryGetSystemType(objectTypeString, applicationControl.Application, out Type? objectType))
                    return allChileElements;

                HashSet<IObjectListBoxItem> listBoxItems = new();
                List<XmlElement> elements = new();
                foreach (XmlElement element in allChileElements)
                {
                    IObjectListBoxItem objectListBoxItem = _objectListBoxItemFactory.GetParameterObjectListBoxItem
                    (
                        _xmlDocumentHelpers.GetSingleChildElement
                        (
                            element
                        ).GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE),
                        element.InnerXml,
                        objectType,
                        applicationControl,
                        listInfo.ListControl
                    );

                    if (listBoxItems.Contains(objectListBoxItem))
                        continue;

                    elements.Add(element);
                }

                return elements;
            }
        }
    }
}
