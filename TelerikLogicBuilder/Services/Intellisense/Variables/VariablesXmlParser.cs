using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables
{
    internal class VariablesXmlParser : IVariablesXmlParser
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IVariableFactory _variableFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public VariablesXmlParser(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IVariableFactory variableFactory,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _variableFactory = variableFactory;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public IDictionary<string, VariableBase> GetVariablesDictionary(XmlDocument xmlDocument)
            => _xmlDocumentHelpers.SelectElements
            (
                xmlDocument,
                string.Format
                (
                    CultureInfo.InvariantCulture, 
                    "//{0}|//{1}|//{2}|//{3}", 
                    XmlDataConstants.LITERALVARIABLEELEMENT, 
                    XmlDataConstants.OBJECTVARIABLEELEMENT, 
                    XmlDataConstants.LITERALLISTVARIABLEELEMENT, 
                    XmlDataConstants.OBJECTLISTVARIABLEELEMENT
                )
            )
            .ToDictionary
            (
                e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                e => Parse(e)
            );

        public VariableBase Parse(XmlElement xmlElement)
        {
            VariableTypeCategory variableTypeCategory = _enumHelper.GetVariableTypeCategory(xmlElement.Name);
            return variableTypeCategory switch
            {
                VariableTypeCategory.Literal => BuildLiteralVariable(xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), xmlElement.ChildNodes.OfType<XmlElement>().ToDictionary(e => e.Name)),
                VariableTypeCategory.Object => BuildObjectVariable(xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), xmlElement.ChildNodes.OfType<XmlElement>().ToDictionary(e => e.Name)),
                VariableTypeCategory.LiteralList => BuildLiteralListVariable(xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), xmlElement.ChildNodes.OfType<XmlElement>().ToDictionary(e => e.Name)),
                VariableTypeCategory.ObjectList => BuildObjectListVariable(xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), xmlElement.ChildNodes.OfType<XmlElement>().ToDictionary(e => e.Name)),
                _ => throw _exceptionHelper.CriticalException("{1F294C7F-AC82-4020-8C3A-5007DD24E0E9}"),
            };
        }

        #region Methods
        private VariableBase BuildLiteralVariable(string nameAttribute, IDictionary<string, XmlElement> elements)
            => _variableFactory.GetLiteralVariable
            (
                nameAttribute,
                elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText,
                (VariableCategory)Enum.Parse(typeof(VariableCategory), elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText),
                elements[XmlDataConstants.CASTVARIABLEASELEMENT].InnerText,
                elements[XmlDataConstants.TYPENAMEELEMENT].InnerText,
                elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText,
                elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText,
                elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText,
                (ReferenceCategories)Enum.Parse(typeof(ReferenceCategories), elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                (LiteralVariableType)Enum.Parse(typeof(LiteralVariableType), elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText),
                (LiteralVariableInputStyle)Enum.Parse(typeof(LiteralVariableInputStyle), elements[XmlDataConstants.CONTROLELEMENT].InnerText),
                elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText,
                elements[XmlDataConstants.DEFAULTVALUEELEMENT].InnerText,
                elements[XmlDataConstants.DOMAINELEMENT].ChildNodes.OfType<XmlElement>().Select(e => e.InnerText).ToList()
            );

        private VariableBase BuildObjectVariable(string nameAttribute, IDictionary<string, XmlElement> elements)
            => _variableFactory.GetObjectVariable
            (
                nameAttribute,
                elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText,
                (VariableCategory)Enum.Parse(typeof(VariableCategory), elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText),
                elements[XmlDataConstants.CASTVARIABLEASELEMENT].InnerText,
                elements[XmlDataConstants.TYPENAMEELEMENT].InnerText,
                elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText,
                elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText,
                elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText,
                (ReferenceCategories)Enum.Parse(typeof(ReferenceCategories), elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText
            );

        private VariableBase BuildLiteralListVariable(string nameAttribute, IDictionary<string, XmlElement> elements)
            => _variableFactory.GetListOfLiteralsVariable
            (
                nameAttribute,
                elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText,
                (VariableCategory)Enum.Parse(typeof(VariableCategory), elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText),
                elements[XmlDataConstants.CASTVARIABLEASELEMENT].InnerText,
                elements[XmlDataConstants.TYPENAMEELEMENT].InnerText,
                elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText,
                elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText,
                elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText,
                (ReferenceCategories)Enum.Parse(typeof(ReferenceCategories), elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                (LiteralVariableType)Enum.Parse(typeof(LiteralVariableType), elements[XmlDataConstants.LITERALTYPEELEMENT].InnerText),
                (ListType)Enum.Parse(typeof(ListType), elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                (ListVariableInputStyle)Enum.Parse(typeof(ListVariableInputStyle), elements[XmlDataConstants.CONTROLELEMENT].InnerText),
                (LiteralVariableInputStyle)Enum.Parse(typeof(LiteralVariableInputStyle), elements[XmlDataConstants.ELEMENTCONTROLELEMENT].InnerText),
                elements[XmlDataConstants.PROPERTYSOURCEELEMENT].InnerText,
                elements[XmlDataConstants.DEFAULTVALUEELEMENT].ChildNodes.OfType<XmlElement>().Select(e => e.InnerText).ToList(),
                elements[XmlDataConstants.DOMAINELEMENT].ChildNodes.OfType<XmlElement>().Select(e => e.InnerText).ToList()
            );

        private VariableBase BuildObjectListVariable(string nameAttribute, IDictionary<string, XmlElement> elements)
            => _variableFactory.GetListOfObjectsVariable
            (
                nameAttribute,
                elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText,
                (VariableCategory)Enum.Parse(typeof(VariableCategory), elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText),
                elements[XmlDataConstants.CASTVARIABLEASELEMENT].InnerText,
                elements[XmlDataConstants.TYPENAMEELEMENT].InnerText,
                elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText,
                elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText,
                elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText,
                (ReferenceCategories)Enum.Parse(typeof(ReferenceCategories), elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText),
                elements[XmlDataConstants.COMMENTSELEMENT].InnerText,
                elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText,
                (ListType)Enum.Parse(typeof(ListType), elements[XmlDataConstants.LISTTYPEELEMENT].InnerText),
                (ListVariableInputStyle)Enum.Parse(typeof(ListVariableInputStyle), elements[XmlDataConstants.CONTROLELEMENT].InnerText)
            );
        #endregion Methods
    }
}
