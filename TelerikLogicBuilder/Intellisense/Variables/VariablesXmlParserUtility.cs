using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    internal class VariablesXmlParserUtility
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IContextProvider _contextProvider;

        public VariablesXmlParserUtility(XmlElement xmlElement, IContextProvider contextProvider)
        {
            this.xmlElement = xmlElement;
            _enumHelper = contextProvider.EnumHelper;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _contextProvider = contextProvider;
            variableTypeCategory = _enumHelper.GetVariableTypeCategory(this.xmlElement.Name);
        }

        #region Fields
        private readonly XmlElement xmlElement;
        private readonly VariableTypeCategory variableTypeCategory;
        #endregion Fields

        #region Properties
        internal VariableBase Variable 
            => this.variableTypeCategory switch
            {
                VariableTypeCategory.Literal => BuildLiteralVariable(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value, xmlElement.ChildNodes.OfType<XmlElement>().ToDictionary(e => e.Name)),
                VariableTypeCategory.Object => BuildObjectVariable(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value, xmlElement.ChildNodes.OfType<XmlElement>().ToDictionary(e => e.Name)),
                VariableTypeCategory.LiteralList => BuildLiteralListVariable(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value, xmlElement.ChildNodes.OfType<XmlElement>().ToDictionary(e => e.Name)),
                VariableTypeCategory.ObjectList => BuildObjectListVariable(xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value, xmlElement.ChildNodes.OfType<XmlElement>().ToDictionary(e => e.Name)),
                _ => throw _exceptionHelper.CriticalException("{1F294C7F-AC82-4020-8C3A-5007DD24E0E9}"),
            };
        #endregion Properties

        #region Methods
        private VariableBase BuildLiteralVariable(string nameAttribute, IDictionary<string, XmlElement> elements) 
            => new LiteralVariable
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
                elements[XmlDataConstants.DOMAINELEMENT].ChildNodes.OfType<XmlElement>().Select(e => e.InnerText).ToList(),
                _contextProvider
            );

        private VariableBase BuildObjectVariable(string nameAttribute, IDictionary<string, XmlElement> elements) 
            => new ObjectVariable
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
                _contextProvider
            );

        private VariableBase BuildLiteralListVariable(string nameAttribute, IDictionary<string, XmlElement> elements) 
            => new ListOfLiteralsVariable
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
                elements[XmlDataConstants.DOMAINELEMENT].ChildNodes.OfType<XmlElement>().Select(e => e.InnerText).ToList(),
                _contextProvider
            );

        private VariableBase BuildObjectListVariable(string nameAttribute, IDictionary<string, XmlElement> elements) 
            => new ListOfObjectsVariable
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
                (ListVariableInputStyle)Enum.Parse(typeof(ListVariableInputStyle), elements[XmlDataConstants.CONTROLELEMENT].InnerText),
                _contextProvider
            );
        #endregion Methods
    }
}
