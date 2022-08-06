using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Data
{
    internal class RefreshVisibleTextHelper : IRefreshVisibleTextHelper
    {
        private readonly IAssertFunctionDataParser _assertFunctionDataParser;
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionsFormFieldSetHelper _functionsFormFieldSetHelper;
        private readonly IGetValidConfigurationFromData _getValidConfigurationFromData;
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IRetractFunctionDataParser _retractFunctionDataParser;
        private readonly IStringHelper _stringHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IVariableValueDataParser _variableValueDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public RefreshVisibleTextHelper(
            IAssertFunctionDataParser assertFunctionDataParser,
            IConfigurationService configurationService,
            IConstructorDataParser constructorDataParser,
            IDecisionDataParser decisionDataParser,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IFunctionsFormFieldSetHelper functionsFormFieldSetHelper,
            IGetValidConfigurationFromData getValidConfigurationFromData,
            ILiteralListDataParser literalListDataParser,
            IObjectListDataParser objectListDataParser,
            IRetractFunctionDataParser retractFunctionDataParser,
            IStringHelper stringHelper,
            IVariableDataParser variableDataParser,
            IVariableValueDataParser variableValueDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _assertFunctionDataParser = assertFunctionDataParser;
            _configurationService = configurationService;
            _constructorDataParser = constructorDataParser;
            _decisionDataParser = decisionDataParser;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _functionsFormFieldSetHelper = functionsFormFieldSetHelper;
            _getValidConfigurationFromData = getValidConfigurationFromData;
            _literalListDataParser = literalListDataParser;
            _objectListDataParser = objectListDataParser;
            _retractFunctionDataParser = retractFunctionDataParser;
            _stringHelper = stringHelper;
            _variableDataParser = variableDataParser;
            _variableValueDataParser = variableValueDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public XmlDocument RefreshConstructorVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.CONSTRUCTORELEMENT}")
                .ForEach
                (
                    element => element.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetConstructorVisibleText
                    (
                        element, 
                        application
                    )
                );

            return xmlDocument;
        }

        public XmlDocument RefreshDecisionVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.DECISIONELEMENT}")
                .ForEach
                (
                    element => element.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetDecisionVisibleText
                    (
                        element,
                        application
                    )
                );

            return xmlDocument;
        }

        public XmlDocument RefreshFunctionVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.FUNCTIONELEMENT}")
                .ForEach
                (
                    element => element.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetFunctionVisibleText
                    (
                        element,
                        application
                    )
                );

            return xmlDocument;
        }

        public XmlDocument RefreshLiteralListVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.LITERALLISTELEMENT}")
                .ForEach
                (
                    element => element.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetLiteralListisibleText
                    (
                        element,
                        application,
                        element.ParentNode?.Name == XmlDataConstants.LITERALLISTPARAMETERELEMENT
                            ? ((XmlElement)element.ParentNode).GetAttribute(XmlDataConstants.NAMEATTRIBUTE)
                            : null
                    )
                );

            return xmlDocument;
        }

        public XmlDocument RefreshObjectListVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.OBJECTLISTELEMENT}")
                .ForEach
                (
                    element => element.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetObjectListisibleText
                    (
                        element,
                        application,
                        element.ParentNode?.Name == XmlDataConstants.OBJECTLISTPARAMETERELEMENT
                            ? ((XmlElement)element.ParentNode).GetAttribute(XmlDataConstants.NAMEATTRIBUTE)
                            : null
                    )
                );

            return xmlDocument;
        }

        public XmlDocument RefreshSetValueFunctionVisibleTexts(XmlDocument xmlDocument, ApplicationTypeInfo application)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.ASSERTFUNCTIONELEMENT}")
                .ForEach
                (
                    element => element.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetAssertFunctionVisibleText
                    (
                        element,
                        application
                    )
                );

            return xmlDocument;
        }

        public XmlDocument RefreshSetValueToNullFunctionVisibleTexts(XmlDocument xmlDocument)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.RETRACTFUNCTIONELEMENT}")
                .ForEach
                (
                    element => element.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetRetractFunctionVisibleText
                    (
                        element
                    )
                );

            return xmlDocument;
        }

        public XmlDocument RefreshVariableVisibleTexts(XmlDocument xmlDocument)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, $"//{XmlDataConstants.VARIABLEELEMENT}")
                .ForEach
                (
                    element => element.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetVariableElementVisibleText(element)
                );

            return xmlDocument;
        }

        private string GetAssertFunctionVisibleText(XmlElement functionElement, ApplicationTypeInfo application)
        {
            if (functionElement.Name != XmlDataConstants.ASSERTFUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{7767B8A6-1769-438E-98BD-17DBF639114F}");

            AssertFunctionData functionData = _assertFunctionDataParser.Parse(functionElement);

            if (!_configurationService.FunctionList.Functions.TryGetValue(functionData.Name, out _))
                return functionData.Name;

            VariableData variableData = _variableDataParser.Parse(functionData.VariableElement);
            VariableValueData variableValueData = _variableValueDataParser.Parse(functionData.VariableValueElement);

            string variableValueVisibleText = string.Format
            (
                Strings.binaryFunctionParametersFormat,
                string.Format(CultureInfo.CurrentCulture, Strings.popupVariableDescriptionFormat, variableData.Name),
                Strings.builtInFunctionNameEquals,
                //variableValueData.ChildElement is literalVariable, objectVariable, literalListVariable, objectListVariable
                GetVariableValueVisibleText(variableValueData.ChildElement, application)
            );

            return string.Format
            (
                Strings.setValueFunctionVisibleTextFormat, 
                functionData.Name, 
                variableValueVisibleText
            );
        }

        private string GetBinaryFunctionVisibleText(FunctionData functionData, Dictionary<string, ParameterBase> parametersDictionary, Dictionary<string, XmlElement> dataParameters, ApplicationTypeInfo application)
        {
            List<KeyValuePair<string, ParameterBase>> parametersList = parametersDictionary.ToList();

            if (!dataParameters.TryGetValue(parametersList[0].Key, out XmlElement? dataElement1))
                return string.Empty;
            if (!dataParameters.TryGetValue(parametersList[1].Key, out XmlElement? dataElement2))
                return string.Empty;

            return string.Format
            (
                CultureInfo.CurrentCulture,
                Strings.binaryFunctionParametersFormat,
                GetParameterDescription(parametersList[0].Value, dataElement1, application),
                functionData.Name,
                GetParameterDescription(parametersList[1].Value, dataElement2, application)
            );
        }

        private string GetConstructorVisibleText(XmlElement constructorElement, ApplicationTypeInfo application)
        {
            if (constructorElement.Name != XmlDataConstants.CONSTRUCTORELEMENT)
                throw _exceptionHelper.CriticalException("{CE1C2DAB-8A1C-4B94-9A18-BBEF0C9864CF}");

            ConstructorData constructorData = _constructorDataParser.Parse(constructorElement);
            if (!application.AssemblyAvailable 
                || !_getValidConfigurationFromData.TryGetConstructor(constructorData, application, out Constructor? constructor))
            {
                return constructorData.Name;
            }

            return string.Format
            (
                CultureInfo.CurrentCulture,
                Strings.memberParametersFormat,
                constructorData.Name,
                GetParametersDescription
                (
                    constructor.Parameters.ToDictionary(p => p.Name),
                    constructorData.ParameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)),
                    application
                )
            );
        }

        private string GetDecisionVisibleText(XmlElement decisionElement, ApplicationTypeInfo application)
        {
            if (decisionElement.Name != XmlDataConstants.DECISIONELEMENT)
                throw _exceptionHelper.CriticalException("{1194287B-808D-4D3F-9FA8-480E6966DA48}");

            DecisionData decisionData = _decisionDataParser.Parse(decisionElement);
            return UpdateForNotDecision
            (
                string.Join
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.binaryFunctionSeparatorFormat,
                        decisionData.FirstChildElementName == XmlDataConstants.ORELEMENT ? Strings.builtInFunctionNameOr : Strings.builtInFunctionNameAnd
                    ),
                    decisionData.FunctionElements.Select
                    (
                        e => GetFunctionVisibleText(e, application)
                    )
                )
            );

            string UpdateForNotDecision(string predicates)
                => decisionData.IsNotDecision 
                    ? string.Format
                    (
                        Strings.notFromDecisionStringFormat, 
                        Strings.builtInFunctionNameNot, 
                        Strings.notFromDecisionSeparator, 
                        predicates
                    ) 
                    : predicates;
        }

        private string GetFunctionVisibleText(XmlElement functionElement, ApplicationTypeInfo application)
        {
            if (functionElement.Name != XmlDataConstants.FUNCTIONELEMENT
                && functionElement.Name != XmlDataConstants.NOTELEMENT)
                throw _exceptionHelper.CriticalException("{7CC94856-49CE-4F20-8851-50B3100AFFA0}");

            FunctionData functionData = _functionDataParser.Parse(functionElement);

            if (!application.AssemblyAvailable 
                || !_getValidConfigurationFromData.TryGetFunction(functionData, application, out Function? function))
            {
                return functionData.Name;
            }

            if (_functionsFormFieldSetHelper.GetFunctionsFormFieldSet(function) == FunctionsFormFieldSet.BinaryLayout)
            {
                return GetBinaryFunctionVisibleText
                (
                    functionData,
                    function.Parameters.ToDictionary(p => p.Name),
                    functionData.ParameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)),
                    application
                );
            }

            return UpdateForNotFunction
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.memberParametersFormat, 
                    functionData.Name,
                    GetParametersDescription
                    (
                        function.Parameters.ToDictionary(p => p.Name),
                        functionData.ParameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)),
                        application
                    )
                )
            );

            string UpdateForNotFunction(string visibleText) 
                => functionData.IsNotFunction
                    ? string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.notFromDecisionStringFormat,
                        Strings.builtInFunctionNameNot,
                        Strings.notFromDecisionSeparator,
                        visibleText
                    )
                    : visibleText;
        }

        private string GetLiteralParameterValueVisibleText(XmlElement element, ApplicationTypeInfo application)
        {
            if (element.Name != XmlDataConstants.LITERALPARAMETERELEMENT)
                return string.Empty;

            return GetLiteralValueVisibleText(element, application);
        }

        private string GetLiteralListisibleText(XmlElement literalListElement, ApplicationTypeInfo application, string? literalListParameterName)
        {
            if (literalListElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{CDA0A31D-8773-4420-AB5D-FA30B02CD5C5}");

            LiteralListData literalListData = _literalListDataParser.Parse(literalListElement);

            if (literalListParameterName == null)
                return GetObjectVisibleText(literalListData.LiteralListElement, application);

            return string.Format
            (
                CultureInfo.CurrentCulture,
                Strings.listParameterCountFormat,
                literalListParameterName,
                literalListData.ChildElements.Count.ToString(CultureInfo.CurrentCulture)
            );
        }

        private string GetLiteralVariableValueVisibleText(XmlElement element, ApplicationTypeInfo application)
        {
            if (element.Name != XmlDataConstants.LITERALVARIABLEELEMENT)
                return string.Empty;

            return GetLiteralValueVisibleText(element, application);
        }

        private string GetLiteralValueVisibleText(XmlElement element, ApplicationTypeInfo application)
        {
            return element.ChildNodes.OfType<XmlNode>().Aggregate
            (
                new StringBuilder(), (sb, next) =>
                {
                    switch (next.NodeType)
                    {
                        case XmlNodeType.Element:
                            XmlElement xmlElement = (XmlElement)next;
                            switch (xmlElement.Name)
                            {
                                case XmlDataConstants.VARIABLEELEMENT:
                                    sb.Append(string.Format(CultureInfo.CurrentCulture, Strings.popupVariableDescriptionFormat, xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value));/*name attribute not null*/
                                    break;
                                case XmlDataConstants.FUNCTIONELEMENT:
                                    sb.Append
                                    (
                                        GetFunctionVisibleText
                                        (
                                            xmlElement,
                                            application
                                        )
                                    );
                                    break;
                                case XmlDataConstants.CONSTRUCTORELEMENT:
                                    sb.Append
                                    (
                                        GetConstructorVisibleText
                                        (
                                            xmlElement,
                                            application
                                        )
                                    );
                                    break;
                                default:
                                    throw _exceptionHelper.CriticalException("{51C90B4E-2ABE-4381-817E-87EA1D72F684}");
                            }
                            break;
                        case XmlNodeType.Text:
                            XmlText xmlText = (XmlText)next;
                            sb.Append(xmlText.Value);
                            break;
                        case XmlNodeType.Whitespace:
                            XmlWhitespace xmlWhitespace = (XmlWhitespace)next;
                            sb.Append(xmlWhitespace.Value);
                            break;
                        default:
                            break;
                    }
                    return sb;
                }
            ).ToString();
        }

        private string GetObjectListisibleText(XmlElement objectListElement, ApplicationTypeInfo application, string? objectListParameterName)
        {
            if (objectListElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw _exceptionHelper.CriticalException("{199AD3CD-949E-4497-9130-884F690801F5}");

            ObjectListData objectlListData = _objectListDataParser.Parse(objectListElement);

            if (objectListParameterName == null)
                return GetObjectVisibleText(objectlListData.ObjectListElement, application);

            return string.Format
            (
                CultureInfo.CurrentCulture,
                Strings.listParameterCountFormat,
                objectListParameterName,
                objectlListData.ChildElements.Count.ToString(CultureInfo.CurrentCulture)
            );
        }

        private string GetObjectVariableVisibleText(XmlElement element, ApplicationTypeInfo application)
        {
            if (element.Name != XmlDataConstants.OBJECTVARIABLEELEMENT
                && element.Name != XmlDataConstants.LITERALLISTVARIABLEELEMENT
                && element.Name != XmlDataConstants.OBJECTLISTVARIABLEELEMENT)
                return string.Empty;

            return GetObjectVisibleText(_xmlDocumentHelpers.GetSingleChildElement(element), application);
        }

        private string GetObjectVisibleText(XmlElement objectElement, ApplicationTypeInfo application)
        {
            switch (objectElement.Name)
            {
                case XmlDataConstants.VARIABLEELEMENT:
                    return string.Format(CultureInfo.CurrentCulture, Strings.popupVariableDescriptionFormat, objectElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value);
                case XmlDataConstants.FUNCTIONELEMENT:
                    return GetFunctionVisibleText
                    (
                        objectElement,
                        application
                    );
                case XmlDataConstants.CONSTRUCTORELEMENT:
                    return GetConstructorVisibleText
                    (
                        objectElement,
                        application
                    );
                case XmlDataConstants.LITERALLISTELEMENT:
                    LiteralListData literalListData = _literalListDataParser.Parse(objectElement);
                    return string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.listParameterCountFormat,
                        _enumHelper.GetTypeDescription
                        (
                            literalListData.ListType,
                            _enumHelper.GetVisibleEnumText(literalListData.LiteralType)
                        ),
                        literalListData.ChildElements.Count.ToString(CultureInfo.CurrentCulture)
                    );
                case XmlDataConstants.OBJECTLISTELEMENT:
                    ObjectListData objectListData = _objectListDataParser.Parse(objectElement);
                    return string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.listParameterCountFormat,
                        _enumHelper.GetTypeDescription
                        (
                            objectListData.ListType,
                            this._stringHelper.ToShortName(objectListData.ObjectType)
                        ),
                        objectListData.ChildElements.Count.ToString(CultureInfo.CurrentCulture)
                    );
                default:
                    throw _exceptionHelper.CriticalException("{A603875D-939D-4268-B18F-9B0B07978887}");
            }
        }

        private string GetParametersDescription(Dictionary<string, ParameterBase> parametersDictionary, Dictionary<string, XmlElement> dataParameters, ApplicationTypeInfo application)
        {
            return string.Join
            (
                Strings.parametersDelimiter,
                parametersDictionary.Aggregate(new List<string>(), (list, next) =>
                {
                    if (!dataParameters.TryGetValue(next.Key, out XmlElement? dataElement))
                        return list;

                    string description = GetParameterDescription(next.Value, dataElement, application);
                    if (!string.IsNullOrEmpty(description))
                        list.Add(description);

                    return list;
                })
            );
        }

        private string GetParameterDescription(ParameterBase configuredParameter, XmlElement dataElement, ApplicationTypeInfo application)
        {
            return configuredParameter.ParameterCategory switch
            {
                ParameterCategory.Literal => string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.literalParameterDescriptionFormat,
                    configuredParameter.Name,
                    GetLiteralParameterValueVisibleText(dataElement, application)
                ),
                ParameterCategory.Object => ((ObjectParameter)configuredParameter).ToString(),
                ParameterCategory.LiteralList => ((ListOfLiteralsParameter)configuredParameter).ToString(),
                ParameterCategory.ObjectList => ((ListOfObjectsParameter)configuredParameter).ToString(),
                _ => throw _exceptionHelper.CriticalException("{D0BB0342-A7B7-4861-88FB-26DB827333D7}"),
            };
        }

        private string GetRetractFunctionVisibleText(XmlElement functionElement)
        {
            if (functionElement.Name != XmlDataConstants.RETRACTFUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{635860C2-81FC-46E6-949A-F4A1E0E4C5DB}");

            RetractFunctionData functionData = _retractFunctionDataParser.Parse(functionElement);

            if (!_configurationService.FunctionList.Functions.TryGetValue(functionData.Name, out _))
                return functionData.Name;

            VariableData variableData = _variableDataParser.Parse(functionData.VariableElement);

            return string.Format
            (
                Strings.setValueFunctionVisibleTextFormat,
                functionData.Name,
                string.Format(CultureInfo.CurrentCulture, Strings.popupVariableDescriptionFormat, variableData.Name)
            );
        }

        private string GetVariableElementVisibleText(XmlElement variableElement)
        {
            if (variableElement.Name != XmlDataConstants.VARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{78FFC909-BECA-4759-90DF-CD3E68E394AD}");

            return variableElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
        }

        private string GetVariableValueVisibleText(XmlElement objectElement, ApplicationTypeInfo application)
        {
            switch (objectElement.Name)
            {
                case XmlDataConstants.LITERALVARIABLEELEMENT:
                    return GetLiteralVariableValueVisibleText(objectElement, application);
                case XmlDataConstants.OBJECTVARIABLEELEMENT:
                case XmlDataConstants.LITERALLISTVARIABLEELEMENT:
                case XmlDataConstants.OBJECTLISTVARIABLEELEMENT:
                    return GetObjectVariableVisibleText(objectElement, application);
                default:
                    throw _exceptionHelper.CriticalException("{28B64CFE-66E2-4DAE-B604-FB80A622D063}");
            }
        }
    }
}
