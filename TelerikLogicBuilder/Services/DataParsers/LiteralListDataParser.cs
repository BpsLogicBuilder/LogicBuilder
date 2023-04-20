using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class LiteralListDataParser : ILiteralListDataParser
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ILiteralListBoxItemFactory _literalListBoxItemFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public LiteralListDataParser(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            ILiteralListBoxItemFactory literalListBoxItemFactory,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _literalListBoxItemFactory = literalListBoxItemFactory;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public LiteralListData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{BD446244-51A5-4EDC-A756-1B0ED0F01023}");

            return new LiteralListData
            (
                _enumHelper.ParseEnumText<LiteralListElementType>
                (
                    xmlElement.Attributes[XmlDataConstants.LITERALTYPEATTRIBUTE]!.Value
                ),
                _enumHelper.ParseEnumText<ListType>
                (
                    xmlElement.Attributes[XmlDataConstants.LISTTYPEATTRIBUTE]!.Value
                ),
                xmlElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value,
                _xmlDocumentHelpers.GetChildElements(xmlElement),
                xmlElement
            );
        }

        public LiteralListData Parse(XmlElement xmlElement, LiteralListParameterElementInfo listInfo, IApplicationControl applicationControl)
        {
            if (xmlElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{ADBBEEE2-6E59-47CC-9DC5-D78F349D57C7}");
            
            LiteralParameterType literalType = _enumHelper.ParseEnumText<LiteralParameterType>(xmlElement.Attributes[XmlDataConstants.LITERALTYPEATTRIBUTE]!.Value);
            ListType listType = _enumHelper.ParseEnumText<ListType>(xmlElement.Attributes[XmlDataConstants.LISTTYPEATTRIBUTE]!.Value);
            List<XmlElement> chileElements = GetUniqueChildElements
            (
                _enumHelper.GetSystemType(literalType),
                _xmlDocumentHelpers.GetChildElements(xmlElement)
            );

            return new LiteralListData
            (
                _enumHelper.ParseEnumText<LiteralListElementType>
                (
                    xmlElement.Attributes[XmlDataConstants.LITERALTYPEATTRIBUTE]!.Value
                ),
                listType,
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.listParameterCountFormat,
                    listInfo.HasParameter
                        ? listInfo.Parameter.Name
                        : _enumHelper.GetTypeDescription(listType, _enumHelper.GetVisibleEnumText(literalType)),
                    chileElements.Count
                ),
                chileElements,
                xmlElement
            );

            List<XmlElement> GetUniqueChildElements(Type literalType, List<XmlElement> allChileElements)
            {
                HashSet<ILiteralListBoxItem> listBoxItems = new();
                List<XmlElement> elements = new();
                foreach (XmlElement element in allChileElements)
                {
                    ILiteralListBoxItem literalListBoxItem = _literalListBoxItemFactory.GetParameterLiteralListBoxItem
                    (
                        _xmlDocumentHelpers.GetVisibleText(element),
                        element.InnerXml,
                        literalType,
                        applicationControl,
                        listInfo.ListControl
                    );

                    if (listBoxItems.Contains(literalListBoxItem))
                        continue;

                    elements.Add(element);
                }

                return elements;
            }
        }

        public LiteralListData Parse(XmlElement xmlElement, LiteralListVariableElementInfo listInfo, IApplicationControl applicationForm)
        {
            if (xmlElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{DDD61368-2FCD-4C0F-A847-FFEB97BFD8BE}");

            LiteralVariableType literalType = _enumHelper.ParseEnumText<LiteralVariableType>(xmlElement.Attributes[XmlDataConstants.LITERALTYPEATTRIBUTE]!.Value);
            ListType listType = _enumHelper.ParseEnumText<ListType>(xmlElement.Attributes[XmlDataConstants.LISTTYPEATTRIBUTE]!.Value);
            List<XmlElement> chileElements = GetUniqueChildElements
            (
                _enumHelper.GetSystemType(literalType),
                _xmlDocumentHelpers.GetChildElements(xmlElement)
            );

            return new LiteralListData
            (
                _enumHelper.ParseEnumText<LiteralListElementType>
                (
                    xmlElement.Attributes[XmlDataConstants.LITERALTYPEATTRIBUTE]!.Value
                ),
                listType,
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.listParameterCountFormat,
                    listInfo.HasVariable
                        ? listInfo.Variable.Name
                        : _enumHelper.GetTypeDescription(listType, _enumHelper.GetVisibleEnumText(literalType)),
                    chileElements.Count
                ),
                chileElements,
                xmlElement
            );

            List<XmlElement> GetUniqueChildElements(Type literalType, List<XmlElement> allChileElements)
            {
                HashSet<ILiteralListBoxItem> listBoxItems = new();
                List<XmlElement> elements = new();
                foreach (XmlElement element in allChileElements)
                {
                    ILiteralListBoxItem literalListBoxItem = _literalListBoxItemFactory.GetVariableLiteralListBoxItem
                    (
                        _xmlDocumentHelpers.GetVisibleText(element),
                        element.InnerXml,
                        literalType,
                        applicationForm,
                        listInfo.ListControl
                    );

                    if (listBoxItems.Contains(literalListBoxItem))
                        continue;

                    elements.Add(element);
                }

                return elements;
            }
        }
    }
}
