using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal class UpdateParameterControlValues : IUpdateParameterControlValues
    {
        private readonly IConstructorTypeHelper _constructorTypeHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public UpdateParameterControlValues(
            IConstructorTypeHelper constructorTypeHelper,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _constructorTypeHelper = constructorTypeHelper;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void PrepopulateRequiredFields(
            IDictionary<string, ParameterControlSet> editControlsSet,
            IDictionary<string, XmlElement> parameterElementsDictionary,
            IDictionary<string, ParameterBase> parametersDictionary,
            XmlDocument xmlDocument,
            string parametersXPath,
            ApplicationTypeInfo application)
        {
            foreach (KeyValuePair<string, ParameterBase> keyValuePair in parametersDictionary)
            {
                if(keyValuePair.Value.IsOptional
                    || parameterElementsDictionary.ContainsKey(keyValuePair.Key)) 
                    continue;

                switch (keyValuePair.Value) 
                {
                    case LiteralParameter literalParameter:
                        SetParameterDefaultElement
                        (
                            keyValuePair.Key,
                            _xmlDocumentHelpers.MakeElement
                            (
                                xmlDocument,
                                XmlDataConstants.LITERALPARAMETERELEMENT,
                                literalParameter.GetDefaultString(),
                                new Dictionary<string, string> { { XmlDataConstants.NAMEATTRIBUTE, keyValuePair.Value.Name } }
                            )
                        );
                        break;
                    case ObjectParameter objectParameter:
                        ClosedConstructor? closedConstructor = _constructorTypeHelper.GetConstructor(objectParameter.ObjectType, application);
                        if (closedConstructor == null)
                            break;

                        SetParameterDefaultElement
                        (
                            keyValuePair.Key,
                            _xmlDocumentHelpers.MakeElement
                            (
                                xmlDocument,
                                XmlDataConstants.OBJECTPARAMETERELEMENT,
                                _xmlDataHelper.BuildDefaultConstructorXml(closedConstructor),
                                new Dictionary<string, string> { { XmlDataConstants.NAMEATTRIBUTE, keyValuePair.Value.Name } }
                            )
                        );

                        break;
                    case ListOfLiteralsParameter listOfLiteralsParameter:
                        SetParameterDefaultElement
                        (
                            keyValuePair.Key,
                            _xmlDocumentHelpers.MakeElement
                            (
                                xmlDocument,
                                XmlDataConstants.LITERALLISTPARAMETERELEMENT,
                                _xmlDataHelper.BuildLiteralListXml
                                (
                                    _enumHelper.GetLiteralListElementType(listOfLiteralsParameter.LiteralType),
                                    listOfLiteralsParameter.ListType,
                                    string.Format(CultureInfo.CurrentCulture, Strings.listParameterCountFormat, listOfLiteralsParameter.Name, 0),
                                    string.Empty
                                ),
                                new Dictionary<string, string> { { XmlDataConstants.NAMEATTRIBUTE, keyValuePair.Value.Name } }
                            )
                        );
                        break;
                    case ListOfObjectsParameter listOfObjectsParameter:
                        SetParameterDefaultElement
                        (
                            keyValuePair.Key,
                            _xmlDocumentHelpers.MakeElement
                            (
                                xmlDocument,
                                XmlDataConstants.OBJECTLISTPARAMETERELEMENT,
                                _xmlDataHelper.BuildObjectListXml
                                (
                                    listOfObjectsParameter.ObjectType,
                                    listOfObjectsParameter.ListType,
                                    string.Format(CultureInfo.CurrentCulture, Strings.listParameterCountFormat, listOfObjectsParameter.Name, 0),
                                    string.Empty
                                ),
                                new Dictionary<string, string> { { XmlDataConstants.NAMEATTRIBUTE, keyValuePair.Value.Name } }
                            )
                        );
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{9F95CFE1-835F-443B-8C40-B39DAD571D27}");
                }
            }

            void SetParameterDefaultElement(string key, XmlElement element)
            {
                _xmlDocumentHelpers.SelectSingleElement(xmlDocument, parametersXPath).AppendChild(element);
                editControlsSet[key].ValueControl.Update(element);
            }
        }

        public void SetDefaultsForLiterals(IDictionary<string, ParameterControlSet> editControlsSet, IDictionary<string, ParameterBase> parametersDictionary)
        {
            foreach (KeyValuePair<string, ParameterBase> keyValuePair in parametersDictionary)
            {
                if (keyValuePair.Value is LiteralParameter literalParameter)
                    SetLiteralDefault(literalParameter, editControlsSet[keyValuePair.Key]);
                else if (keyValuePair.Value is ListOfLiteralsParameter listOfLiteralsParameter)
                    SetLiteralListDefault(listOfLiteralsParameter, editControlsSet[keyValuePair.Key]);
                else
                    continue;
            }

            void SetLiteralDefault(LiteralParameter parameter, ParameterControlSet parameterControlSet)
            {
                if (string.IsNullOrEmpty(parameter.DefaultValue))
                    return;

                parameterControlSet.ChkInclude.Checked = true;
                parameterControlSet.ValueControl.Update
                (
                    _xmlDocumentHelpers.ToXmlElement
                    (
                        _xmlDataHelper.BuildParameterXml(parameter, parameter.DefaultValue)
                    )
                );
            }

            void SetLiteralListDefault(ListOfLiteralsParameter parameter, ParameterControlSet parameterControlSet)
            {
                if (parameter.DefaultValues?.Any() != true)
                    return;

                parameterControlSet.ChkInclude.Checked = true;
                parameterControlSet.ValueControl.Update(_xmlDocumentHelpers.ToXmlElement(GetLiteralListXml()));

                string GetLiteralListXml()
                {
                    StringBuilder stringBuilder = new();
                    using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
                    {
                        xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALLISTPARAMETERELEMENT);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, parameter.Name);
                        xmlTextWriter.WriteRaw
                        (
                            _xmlDataHelper.BuildLiteralListXml
                            (
                                _enumHelper.GetLiteralListElementType(parameter.LiteralType),
                                parameter.ListType,
                                string.Format(CultureInfo.CurrentCulture, Strings.listParameterCountFormat, parameter.Name, parameter.DefaultValues.Count),
                                GetDefaultValuesXml()
                            )
                        );
                        xmlTextWriter.WriteEndElement();
                        xmlTextWriter.Flush();
                    }
                    return stringBuilder.ToString();
                }

                string GetDefaultValuesXml()
                {
                    StringBuilder stringBuilder = new();
                    using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
                    {
                        parameter.DefaultValues.ForEach
                        (
                            val => xmlTextWriter.WriteElementString(XmlDataConstants.LITERALELEMENT, val)
                        );
                        xmlTextWriter.Flush();
                    }
                    return stringBuilder.ToString();
                }
            }
        }

        public void UpdateExistingFields(IList<XmlElement> parameterElementsList, IDictionary<string, ParameterControlSet> editControlsSet, IDictionary<string, ParameterBase> parametersDictionary, string? selectedParameter = null)
        {
            foreach (XmlElement parameterElement in parameterElementsList)
            {
                string parameterName = parameterElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
                if (!editControlsSet.TryGetValue(parameterName, out ParameterControlSet? parameterControlSet))
                    continue;

                UpdateParameterField
                (
                    parameterElement,
                    parameterControlSet,
                    parametersDictionary[parameterName],
                    parameterName,
                    selectedParameter
                );
            }
        }


        private void UpdateParameterField(XmlElement parameterElement, ParameterControlSet parameterControlSet, ParameterBase parameter, string parameterName, string? selectedParameter)
        {
            if (parameterElement.Name != _xmlDataHelper.GetElementName(parameter.ParameterCategory))
                return;

            parameterControlSet.ChkInclude.Checked = true;
            parameterControlSet.ValueControl.Update(parameterElement);
            if (parameterName == selectedParameter)
                parameterControlSet.ValueControl.Focus();
        }
    }
}
