using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
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
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Helpers
{
    internal class ObjectHashSetListBoxItemComparer : IObjectHashSetListBoxItemComparer
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IGetValidConfigurationFromData _getValidConfigurationFromData;
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ObjectHashSetListBoxItemComparer(
            IConfigurationService configurationService,
            IConstructorDataParser constructorDataParser,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IGetValidConfigurationFromData getValidConfigurationFromData,
            ILiteralListDataParser literalListDataParser,
            IObjectListDataParser objectListDataParser,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _configurationService = configurationService;
            _constructorDataParser = constructorDataParser;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _getValidConfigurationFromData = getValidConfigurationFromData;
            _literalListDataParser = literalListDataParser;
            _objectListDataParser = objectListDataParser;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public bool Compare(XmlElement? first, XmlElement? second, ApplicationTypeInfo application)
        {
            if (first == null ^ second == null) return false;
            if (first == null && second == null) return true;
            if (first!.Name != second!.Name) return false;

            return first.Name switch
            {
                XmlDataConstants.VARIABLEELEMENT => CompareVariableElements(first, second),
                XmlDataConstants.FUNCTIONELEMENT => CompareFunctionElements(first, second, application),
                XmlDataConstants.CONSTRUCTORELEMENT => CompareConstructorElements(first, second, application),
                XmlDataConstants.LITERALLISTELEMENT => CompareLiteralListElements(first, second),
                XmlDataConstants.OBJECTLISTELEMENT => CompareObjectListElements(first, second),
                _ => throw _exceptionHelper.CriticalException("{52BA0E18-FB6C-4909-8662-802600A35233}"),
            };
        }

        private bool CompareConstructorElements(XmlElement first, XmlElement second, ApplicationTypeInfo application)
        {
            ConstructorData firstConstructorData = _constructorDataParser.Parse(first);
            ConstructorData secondConstructorData = _constructorDataParser.Parse(second);

            if (firstConstructorData.Name != firstConstructorData.Name)
                return false;

            if (!_configurationService.ConstructorList.Constructors.TryGetValue(firstConstructorData.Name, out Constructor? constructor))
                return false;

            if (constructor.HasGenericArguments)
            {
                if (!firstConstructorData.GenericArguments.Order().SequenceEqual(secondConstructorData.GenericArguments.Order()))
                    return false;

                if (!_getValidConfigurationFromData.TryGetConstructor(firstConstructorData, application, out constructor))
                    return false;//generic configs are the same so just the first data item will do.
            }

            return CompareParameters
            (
                constructor.Parameters.ToDictionary(p => p.Name),
                firstConstructorData.ParameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)),
                secondConstructorData.ParameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)),
                application
            );
        }

        private bool CompareFunctionElements(XmlElement first, XmlElement second, ApplicationTypeInfo application)
        {
            FunctionData firstFunctionData = _functionDataParser.Parse(first);
            FunctionData secondFunctionData = _functionDataParser.Parse(second);

            if (firstFunctionData.Name != secondFunctionData.Name)
                return false;

            if (!_configurationService.FunctionList.Functions.TryGetValue(firstFunctionData.Name, out Function? function))
                return false;

            if (function.HasGenericArguments)
            {
                if (!firstFunctionData.GenericArguments.Order().SequenceEqual(secondFunctionData.GenericArguments.Order()))
                    return false;

                if (!_getValidConfigurationFromData.TryGetFunction(firstFunctionData, application, out function))
                    return false;//generic configs are the same so just the first data item will do.
            }

            return CompareParameters
            (
                function.Parameters.ToDictionary(p => p.Name),
                firstFunctionData.ParameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)),
                secondFunctionData.ParameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)),
                application
            );
        }

        private bool CompareLiteralListElements(XmlElement first, XmlElement second)
        {
            return DoComparison
            (
                _literalListDataParser.Parse(first),
                _literalListDataParser.Parse(second)
            );

            static bool DoComparison(LiteralListData firstLiteralListData, LiteralListData secondLiteralListData)
            {
                if (firstLiteralListData.ListType != secondLiteralListData.ListType
                            || firstLiteralListData.LiteralType != secondLiteralListData.LiteralType)
                    return false;

                if (firstLiteralListData.ChildElements.Count != secondLiteralListData.ChildElements.Count)
                    return false;

                for (int i = 0; i < firstLiteralListData.ChildElements.Count; i++)
                {
                    XmlElement firstElement = firstLiteralListData.ChildElements[i];
                    XmlElement secondElement = secondLiteralListData.ChildElements[i];
                    if (firstElement.OuterXml != secondElement.OuterXml) return false;
                }

                return true;
            }
        }

        private bool CompareObjectListElements(XmlElement first, XmlElement second)
        {
            return DoComparison
            (
                _objectListDataParser.Parse(first),
                _objectListDataParser.Parse(second)
            );

            static bool DoComparison(ObjectListData firstObjectListData, ObjectListData secondObjectListData)
            {
                if (firstObjectListData.ListType != secondObjectListData.ListType
                            || firstObjectListData.ObjectType != secondObjectListData.ObjectType)
                    return false;

                if (firstObjectListData.ChildElements.Count != secondObjectListData.ChildElements.Count)
                    return false;

                for (int i = 0; i < firstObjectListData.ChildElements.Count; i++)
                {
                    XmlElement firstElement = firstObjectListData.ChildElements[i];
                    XmlElement secondElement = secondObjectListData.ChildElements[i];
                    if (firstElement.OuterXml != secondElement.OuterXml) return false;
                }

                return true;
            }
        }

        private bool CompareParameters(IDictionary<string, ParameterBase> parametersDictionary,
                                            IDictionary<string, XmlElement> firstParameters,
                                            IDictionary<string, XmlElement> secondParameters,
                                            ApplicationTypeInfo application)
        {
            if (firstParameters.Count != secondParameters.Count)
                return false;

            if (firstParameters.Keys.Except(secondParameters.Keys).Any())
                return false;

            if (secondParameters.Keys.Except(firstParameters.Keys).Any())
                return false;

            foreach (var pair in parametersDictionary)
            {
                //exclude parameters where UseForEquality == false
                //Only literals and objects can be IComparableParameter
                if (pair.Value is IComparableParameter comparableParameter && !comparableParameter.UseForEquality) 
                    continue;

                if (!firstParameters.TryGetValue(pair.Key, out XmlElement? firstElement))
                    continue;

                XmlElement secondElement = secondParameters[pair.Key];

                if (firstElement.Name != secondElement.Name) 
                    return false;

                if (firstElement.Name != _xmlDataHelper.GetElementName(pair.Value.ParameterCategory))
                    return false;//configuration must have changed.

                if (pair.Value.ParameterCategory == ParameterCategory.Literal 
                    && firstElement.InnerXml != secondElement.InnerXml)
                    return false;

                if 
                (
                    (
                        pair.Value.ParameterCategory == ParameterCategory.Object
                        || pair.Value.ParameterCategory == ParameterCategory.ObjectList
                        || pair.Value.ParameterCategory == ParameterCategory.LiteralList
                    )
                    && 
                    !Compare
                    (
                        _xmlDocumentHelpers.GetSingleOrDefaultChildElement(firstElement), 
                        _xmlDocumentHelpers.GetSingleOrDefaultChildElement(firstElement), 
                        application
                    )
                )
                {
                    return false;
                }
            }

            //Not concerned about Generic and GenericList categories. the function/constructor will be converted before creating data items.
            return true;
        }

        private static bool CompareVariableElements(XmlElement first, XmlElement second) 
            => first.GetAttribute(XmlDataConstants.NAMEATTRIBUTE) == second.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
    }
}
