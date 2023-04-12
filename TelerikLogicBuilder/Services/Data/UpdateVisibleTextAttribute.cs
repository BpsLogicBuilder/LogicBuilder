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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Data
{
    internal class UpdateVisibleTextAttribute : IUpdateVisibleTextAttribute
    {
        private readonly IAssertFunctionDataParser _assertFunctionDataParser;
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IEditFormFieldSetHelper _editFormFieldSetHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IGetValidConfigurationFromData _getValidConfigurationFromData;
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IRetractFunctionDataParser _retractFunctionDataParser;
        private readonly IStringHelper _stringHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IVariableValueDataParser _variableValueDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public UpdateVisibleTextAttribute(
            IAssertFunctionDataParser assertFunctionDataParser,
            IConfigurationService configurationService,
            IConstructorDataParser constructorDataParser,
            IDecisionDataParser decisionDataParser,
            IEditFormFieldSetHelper editFormFieldSetHelper,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
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
            _editFormFieldSetHelper = editFormFieldSetHelper;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _getValidConfigurationFromData = getValidConfigurationFromData;
            _literalListDataParser = literalListDataParser;
            _objectListDataParser = objectListDataParser;
            _retractFunctionDataParser = retractFunctionDataParser;
            _stringHelper = stringHelper;
            _variableDataParser = variableDataParser;
            _variableValueDataParser = variableValueDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void UpdateAssertFunctionVisibleText(XmlElement functionElement, ApplicationTypeInfo application)
        {
            functionElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetAssertFunctionVisibleText(functionElement, application);
        }

        public void UpdateConstructorVisibleText(XmlElement constructorElement, ApplicationTypeInfo application)
        {
            constructorElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetConstructorVisibleText(constructorElement, application);
        }

        public void UpdateDecisionVisibleText(XmlElement decisionElement, ApplicationTypeInfo application)
        {
            decisionElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetDecisionVisibleText(decisionElement, application);
        }

        public void UpdateFunctionVisibleText(XmlElement functionElement, ApplicationTypeInfo application)
        {
            functionElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetFunctionVisibleText(functionElement, application);
        }

        public void UpdateLiteralListVisibleText(XmlElement literalListElement, ApplicationTypeInfo application, string? literalListParameterName)
        {
            literalListElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetLiteralListVisibleText(literalListElement, application, literalListParameterName);
        }

        public void UpdateObjectListVisibleText(XmlElement objectListElement, ApplicationTypeInfo application, string? objectListParameterName)
        {
            objectListElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetObjectListisibleText(objectListElement, application, objectListParameterName);
        }

        public void UpdateRetractFunctionVisibleText(XmlElement functionElement)
        {
            functionElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetRetractFunctionVisibleText(functionElement);
        }

        public void UpdateVariableVisibleText(XmlElement variableElement)
        {
            variableElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = GetVariableVisibleText(variableElement);
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
                GetParameterDescriptionForBinaryOperation(parametersList[0].Value, dataElement1, application),
                functionData.Name,
                GetParameterDescriptionForBinaryOperation(parametersList[1].Value, dataElement2, application)
            );
        }

        private string GetConstructorVisibleText(XmlElement constructorElement, ApplicationTypeInfo application)
        {
            if (constructorElement.Name != XmlDataConstants.CONSTRUCTORELEMENT)
                throw _exceptionHelper.CriticalException("{D7944A4F-0C28-4224-9526-AF57D5B1C728}");

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
                throw _exceptionHelper.CriticalException("{E54D5D7B-5524-4E01-A863-CF440FF42AF4}");

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
                throw _exceptionHelper.CriticalException("{E7C621FD-8098-464F-AD73-F55169269B36}");

            FunctionData functionData = _functionDataParser.Parse(functionElement);

            if (!application.AssemblyAvailable
                || !_getValidConfigurationFromData.TryGetFunction(functionData, application, out Function? function))
            {
                return functionData.Name;
            }

            if (_editFormFieldSetHelper.GetFieldSetForFunction(function) == EditFormFieldSet.BinaryFunction)
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

        private string GetLiteralListVisibleText(XmlElement literalListElement, ApplicationTypeInfo application, string? literalListParameterName)
        {
            if (literalListElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{EA0E4806-EAFD-445C-BA76-D2E662131734}");

            LiteralListData literalListData = _literalListDataParser.Parse(literalListElement);

            if (literalListParameterName == null)
                return GetObjectVisibleText(literalListData.LiteralListElement, application);

            return string.Format
            (
                CultureInfo.CurrentCulture,
                Strings.popupLiteralListDescriptionFormat,
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.listParameterCountFormat,
                    literalListParameterName,
                    literalListData.ChildElements.Count.ToString(CultureInfo.CurrentCulture)
                )
            );
        }

        private string GetLiteralParameterValueVisibleText(XmlElement element, ApplicationTypeInfo application)
        {
            if (element.Name != XmlDataConstants.LITERALPARAMETERELEMENT)
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
                                    sb.Append(Strings.functionVisibleTextBegin);
                                    sb.Append
                                    (
                                        GetFunctionVisibleText
                                        (
                                            xmlElement,
                                            application
                                        )
                                    );
                                    sb.Append(Strings.functionVisibleTextEnd);
                                    break;
                                case XmlDataConstants.CONSTRUCTORELEMENT:
                                    sb.Append(Strings.constructorVisibleTextBegin);
                                    //We don't use curly braces for a possible literal constructor to avoid possible problems parsing the hidden text.
                                    sb.Append
                                    (
                                        GetConstructorVisibleText
                                        (
                                            xmlElement,
                                            application
                                        )
                                    );
                                    sb.Append(Strings.constructorVisibleTextBegin);
                                    break;
                                default:
                                    throw _exceptionHelper.CriticalException("{896FEA72-7AD0-430D-808D-08AE2A6FF2E2}");
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

        private string GetLiteralVariableValueVisibleText(XmlElement element, ApplicationTypeInfo application)
        {
            if (element.Name != XmlDataConstants.LITERALVARIABLEELEMENT)
                return string.Empty;

            return GetLiteralValueVisibleText(element, application);
        }

        private string GetObjectListisibleText(XmlElement objectListElement, ApplicationTypeInfo application, string? objectListParameterName)
        {
            if (objectListElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw _exceptionHelper.CriticalException("{07EBA92B-C5A4-4A1F-A672-ED0E24EE6AA9}");

            ObjectListData objectlListData = _objectListDataParser.Parse(objectListElement);

            if (objectListParameterName == null)
                return GetObjectVisibleText(objectlListData.ObjectListElement, application);

            return string.Format
            (
                CultureInfo.CurrentCulture,
                Strings.popupObjectListDescriptionFormat,
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.listParameterCountFormat,
                    objectListParameterName,
                    objectlListData.ChildElements.Count.ToString(CultureInfo.CurrentCulture)
                )
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
                    return string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.popupFunctionDescriptionFormat,
                        GetFunctionVisibleText
                        (
                            objectElement,
                            application
                        )
                    );
                case XmlDataConstants.CONSTRUCTORELEMENT:
                    return string.Format
                    (
                        CultureInfo.CurrentCulture, 
                        //We can use curly braces for the object constructor
                        //becuase the control is a plain RichTextBox
                        Strings.popupConstructorDescriptionFormat, 
                        GetConstructorVisibleText
                        (
                            objectElement,
                            application
                        )
                    );
                case XmlDataConstants.LITERALLISTELEMENT:
                    LiteralListData literalListData = _literalListDataParser.Parse(objectElement);
                    return string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.popupLiteralListDescriptionFormat,
                        string.Format
                        (
                            CultureInfo.CurrentCulture,
                            Strings.listParameterCountFormat,
                            _enumHelper.GetTypeDescription
                            (
                                literalListData.ListType,
                                _enumHelper.GetVisibleEnumText(literalListData.LiteralType)
                            ),
                            literalListData.ChildElements.Count.ToString(CultureInfo.CurrentCulture)
                        )
                    );
                case XmlDataConstants.OBJECTLISTELEMENT:
                    ObjectListData objectListData = _objectListDataParser.Parse(objectElement);
                    return string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.popupObjectListDescriptionFormat,
                        string.Format
                        (
                            CultureInfo.CurrentCulture,
                            Strings.listParameterCountFormat,
                            _enumHelper.GetTypeDescription
                            (
                                objectListData.ListType,
                                this._stringHelper.ToShortName(objectListData.ObjectType)
                            ),
                            objectListData.ChildElements.Count.ToString(CultureInfo.CurrentCulture)
                        )
                    );
                default:
                    throw _exceptionHelper.CriticalException("{8FF9357A-E00C-4D9D-A4FB-54258F37C007}");
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
                _ => throw _exceptionHelper.CriticalException("{22EACCB9-D8E5-446F-9856-49BECFE9E346}"),
            };
        }

        private string GetParameterDescriptionForBinaryOperation(ParameterBase configuredParameter, XmlElement dataElement, ApplicationTypeInfo application)
        {
            if (configuredParameter.ParameterCategory == ParameterCategory.ObjectList) return string.Empty;
            if (configuredParameter.ParameterCategory == ParameterCategory.LiteralList) return string.Empty;

            return configuredParameter.ParameterCategory switch
            {
                ParameterCategory.Literal => GetLiteralParameterValueVisibleText(dataElement, application),
                ParameterCategory.Object => ((ObjectParameter)configuredParameter).ToString(),
                _ => throw _exceptionHelper.CriticalException("{1906390C-8694-4B5D-959F-FAF76B481B29}"),
            };
        }

        private string GetRetractFunctionVisibleText(XmlElement functionElement)
        {
            if (functionElement.Name != XmlDataConstants.RETRACTFUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{F0CC2CD8-B44D-49D9-86E1-58D4C3A97DCA}");

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

        private string GetVariableValueVisibleText(XmlElement objectElement, ApplicationTypeInfo application)
        {
            return objectElement.Name switch
            {
                XmlDataConstants.LITERALVARIABLEELEMENT => GetLiteralVariableValueVisibleText(objectElement, application),
                XmlDataConstants.OBJECTVARIABLEELEMENT or XmlDataConstants.LITERALLISTVARIABLEELEMENT or XmlDataConstants.OBJECTLISTVARIABLEELEMENT => GetObjectVariableVisibleText(objectElement, application),
                _ => throw _exceptionHelper.CriticalException("{3AA526B2-FA51-48A8-A77E-B1AC77B4E780}"),
            };
        }

        private string GetVariableVisibleText(XmlElement variableElement)
        {
            if (variableElement.Name != XmlDataConstants.VARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{EA2CFD1F-FEEF-45EB-B3B0-40FDAF9B38FB}");

            return variableElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
        }
    }
}
