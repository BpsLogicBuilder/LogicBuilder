using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.XmlValidation.Configuration
{
    internal class FunctionsXmlValidatorUtility : XmlValidatorUtility
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEnumHelper _enumHelper;
        private readonly IStringHelper _stringHelper;
        private readonly IFunctionValidationHelper _functionValidationHelper;
        internal FunctionsXmlValidatorUtility(string xmlString, IFunctionValidationHelper functionValidationHelper, IContextProvider contextProvider) : base(Schemas.FunctionsSchema, xmlString)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _enumHelper = contextProvider.EnumHelper;
            _stringHelper = contextProvider.StringHelper;
            _functionValidationHelper = functionValidationHelper;
        }

        protected internal override XmlValidationResponse ValidateXmlDocument()
        {
            XmlDocument xDoc = (XmlDocument)XmlDocument;
            List<string> validationErrors = new();
            DoXmlSchemaValidation();
            validationErrors.AddRange(LoadXmlDocumentValidationErrors());

            //must return schema validation errors here to prevent unexpected null exceptions in further validation
            if (validationErrors.Count != 0)
            {
                return new XmlValidationResponse
                {
                    Success = false,
                    Errors = validationErrors
                };
            }

            NonSchemaValidation(xDoc);
            validationErrors.AddRange(LoadXmlDocumentValidationErrors());

            return new XmlValidationResponse
            {
                Success = validationErrors.Count == 0,
                Errors = validationErrors
            };
        }

        private void NonSchemaValidation(XmlDocument xDoc)
        {
            xDoc.SelectNodes($"//{XmlDataConstants.FUNCTIONELEMENT}")!/*Never null when SelectNodes is called on an XmlDocument.*/
                .OfType<XmlElement>()
                .ToList()
                .ForEach(functionNode =>
                {
                    Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(functionNode)
                        .ToDictionary(e => e.Name);

                    ValidateElement
                    (
                        functionNode,
                        elements,
                        functionNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                        (FunctionCategories)Enum.Parse(typeof(FunctionCategories), elements[XmlDataConstants.FUNCTIONCATEGORYELEMENT].InnerText.Trim()),
                        (ParametersLayout)Enum.Parse(typeof(ParametersLayout), elements[XmlDataConstants.PARAMETERSLAYOUTELEMENT].InnerText.Trim()),
                        (ReferenceCategories)Enum.Parse(typeof(ReferenceCategories), elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText.Trim()),
                        _stringHelper.SplitWithQuoteQualifier(elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText.Trim(), MiscellaneousConstants.PERIODSTRING),
                        _stringHelper.SplitWithQuoteQualifier(elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText.Trim(), MiscellaneousConstants.PERIODSTRING),
                        _stringHelper.SplitWithQuoteQualifier(elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText.Trim(), MiscellaneousConstants.PERIODSTRING)
                    );
                });
        }

        private void ValidateElement(XmlElement functionNode, Dictionary<string, XmlElement> elements, string functionName, FunctionCategories functionCategory, ParametersLayout parametersLayout, ReferenceCategories referenceCategory, string[] referenceDefinitionArray, string[] referenceNameArray, string[] castReferenceAsArray)
        {
            if (!ValidateReferenceDefinitionList(referenceDefinitionArray, functionName))
                return;

            ValidateParameterOrder(functionName, _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.PARAMETERSELEMENT]));

            ValidateMemberName(elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText, functionName);

            ValidateBinaryOperator(elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText, functionName, functionCategory);

            if (!ValidateReferenceNameMustBeEmpty(functionName, referenceCategory, elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText.Trim()))
                return;

            if (!ValidateReferenceMustBePopulated(functionName, referenceCategory, elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText.Trim(), elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText.Trim()))
                return;

            if (!ValidateReferenceDefinitionLengthMatchesReferenceNameLength(functionName, referenceDefinitionArray, referenceNameArray))
                return;

            if (!ValidateReferenceNameAndCastReferenceAsLengthsMatch(functionName, referenceNameArray, castReferenceAsArray))
                return;

            if (!ValidateTypeName(functionName, referenceCategory, elements[XmlDataConstants.TYPENAMEELEMENT].InnerText.Trim()))
                return;

            if (!ValidateTypeNameMustBeEmpty(functionName, referenceCategory, elements[XmlDataConstants.TYPENAMEELEMENT].InnerText.Trim()))
                return;

            ValidateIndirectReferenceName
            (
                _enumHelper.ConvertToEnumList<ValidIndirectReference>(referenceDefinitionArray),
                referenceNameArray,
                functionName
            );

            ValidateParametersLayoutMustBeBinaryForBinaryOperator(functionName, functionCategory, parametersLayout);

            ValidateReferenceCategoryMustBeNoneForBinaryOperator(functionName, functionCategory, referenceCategory);

            ValidateParameterCountMustBeTwoForBinaryParamerLayout(functionName, parametersLayout, _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.PARAMETERSELEMENT]));

            ValidateReferenceCategoryCannotBeNone(functionName, functionCategory, referenceCategory);

            ValidateLiteralParameterSourcedProperty(functionNode, functionName, _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.PARAMETERSELEMENT]));

            ValidateLiteralListParameterSourcedProperty(functionNode, functionName, _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.PARAMETERSELEMENT]));

            ValidateGenericArguments
            (
                functionName, 
                referenceCategory, 
                elements[XmlDataConstants.RETURNTYPEELEMENT],  
                _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.PARAMETERSELEMENT]), 
                _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.GENERICARGUMENTSELEMENT])
                    .Select(e => e.InnerText)
                    .ToList()
            );
        }

        private bool ValidateReferenceDefinitionList(string[] referenceDefinitionArray, string functionName) 
            => referenceDefinitionArray.Aggregate
            (
                true,
                (valid, definition) =>
                {
                    if (!Enum.IsDefined(typeof(ValidIndirectReference), definition))
                    {
                        valid = false;
                        XmlDocumentErrors.Add
                        (
                            string.Format
                            (
                                CultureInfo.CurrentCulture,
                                Strings.functionInvalidIndirectDefinitionFormat,
                                functionName,
                                definition,
                                Environment.NewLine,
                                _enumHelper.GetValidIndirectReferencesList()
                            )
                        );
                    }

                    return valid;
                }
            );

        private void ValidateParameterOrder(string functionName, List<XmlElement> parameterNodeList)
        {
            bool optionalExists = false;
            parameterNodeList.ForEach(parameterNode =>
            {
                bool optional = bool.Parse(_xmlDocumentHelpers.GetSingleChildElement(parameterNode, e => e.Name == XmlDataConstants.OPTIONALELEMENT).InnerText.Trim());
                if (optional)
                    optionalExists = true;

                if (!optional && optionalExists)//optional is true for a previous parameter
                    XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidParameterOrder, functionName));

            });
        }

        private void ValidateMemberName(string memberName, string functionName)
        {
            if (!Regex.IsMatch(memberName, RegularExpressions.VARIABLEORFUNCTIONNAME))
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.memberNameIsInvalidFormat, functionName));
        }

        private void ValidateBinaryOperator(string memberName, string functionName, FunctionCategories functionCategory)
        {
            if (functionCategory == FunctionCategories.BinaryOperator && !_enumHelper.IsValidCodeBinaryOperator(memberName))
            {
                XmlDocumentErrors.Add
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.binaryOperatorCodeNameInvalidFormat,
                        memberName,
                        functionName,
                        string.Join
                        (
                            Strings.itemsCommaSeparator, 
                            _enumHelper.ConvertEnumListToStringList
                            (
                                new CodeBinaryOperatorType[] { CodeBinaryOperatorType.Assign }
                            )
                        )
                    )
                );
            }
        }

        private bool ValidateReferenceNameMustBeEmpty(string functionName,
            ReferenceCategories referenceCategory,
            string referenceName)
        {
            if ((referenceCategory == ReferenceCategories.This || referenceCategory == ReferenceCategories.None || referenceCategory == ReferenceCategories.Type) && referenceName.Length != 0)
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionReferenceMustBeEmptyFormat, functionName));
                return false;
            }

            return true;
        }

        private bool ValidateReferenceMustBePopulated(string functionName,
            ReferenceCategories referenceCategory,
            string referenceDefinition,
            string referenceName)
        {
            if ((referenceCategory == ReferenceCategories.InstanceReference || referenceCategory == ReferenceCategories.StaticReference)
                    && (referenceDefinition.Length == 0 || referenceName.Length == 0))
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionReferenceInfoMustBePopulatedFormat, functionName));
                return false;
            }

            return true;
        }

        private bool ValidateReferenceDefinitionLengthMatchesReferenceNameLength(string functionName,
            string[] referenceDefinitionArray,
            string[] referenceNameArray)
        {
            if (referenceDefinitionArray.Length != referenceNameArray.Length)
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionNameAndDefinitionFormat, functionName));
                return false;
            }

            return true;
        }

        private bool ValidateReferenceNameAndCastReferenceAsLengthsMatch(string functionName,
            string[] referenceNameArray,
            string[] castReferenceAsArray)
        {
            if (castReferenceAsArray.Length != 0 && castReferenceAsArray.Length != referenceNameArray.Length)
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionNameAndCastAsFormat, functionName));
                return false;
            }

            return true;
        }

        private void ValidateIndirectReferenceName(IList<ValidIndirectReference> referenceDefinition,
            IList<string> referenceNameArray,
            string functionName)
        {
            for (int i = 0; i < referenceDefinition.Count; i++)
            {
                _functionValidationHelper.ValidateFunctionIndirectReferenceName
                (
                    referenceDefinition[i], 
                    referenceNameArray[i], 
                    functionName, 
                    XmlDocumentErrors
                );
            }
        }

        private bool ValidateTypeName(string functionName,
            ReferenceCategories referenceCategory,
            string typeName)
        {
            if ((referenceCategory == ReferenceCategories.Type || referenceCategory == ReferenceCategories.StaticReference)
                    && !Regex.IsMatch(typeName, RegularExpressions.FULLYQUALIFIEDCLASSNAME))
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.typeReferenceNameIsInvalidFormat, functionName));
                return false;
            }

            return true;
        }

        private bool ValidateTypeNameMustBeEmpty(string functionName,
            ReferenceCategories referenceCategory,
            string typeName)
        {
            if ((referenceCategory != ReferenceCategories.Type && referenceCategory != ReferenceCategories.StaticReference)
                && typeName.Length != 0)
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionTypeNameMustBeEmptyFormat, functionName));
                return false;
            }

            return true;
        }

        private void ValidateParametersLayoutMustBeBinaryForBinaryOperator(string functionName, FunctionCategories functionCategory, ParametersLayout parametersLayout)
        {
            if (functionCategory == FunctionCategories.BinaryOperator && parametersLayout != ParametersLayout.Binary)
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.parametersLayoutMustBeBinaryFormat, functionName));
        }

        private void ValidateReferenceCategoryMustBeNoneForBinaryOperator(string functionName, FunctionCategories functionCategory, ReferenceCategories referenceCategory)
        {
            if (functionCategory == FunctionCategories.BinaryOperator && referenceCategory != ReferenceCategories.None)
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.referenceCategoryMustBeNoneFormat, functionName));
        }

        private void ValidateParameterCountMustBeTwoForBinaryParamerLayout(string functionName, ParametersLayout parametersLayout, List<XmlElement> parameterNodeList)
        {
            if (parametersLayout == ParametersLayout.Binary && parameterNodeList.Count != 2)
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.parameterCountMustBeTwoFormat, functionName));
        }

        private void ValidateReferenceCategoryCannotBeNone(string functionName, FunctionCategories functionCategory, ReferenceCategories referenceCategory)
        {
            switch (functionCategory)
            {
                case FunctionCategories.Standard:
                case FunctionCategories.DialogForm:
                    if (referenceCategory == ReferenceCategories.None)
                        XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.referenceCategoryCannotBeNoneFormat, functionName));
                    break;
                default:
                    break;
            }
        }

        private void ValidateLiteralParameterSourcedProperty(XmlElement functionNode, string functionName, List<XmlElement> parameterNodeList)
        {
            parameterNodeList.ForEach(parameterNode =>
            {
                if (parameterNode.Name != XmlDataConstants.LITERALPARAMETERELEMENT)
                    return;

                ValidateParameterSourcedProperty
                (
                    functionName,
                    parameterNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                    (LiteralParameterInputStyle)Enum.Parse
                    (
                        typeof(LiteralParameterInputStyle),
                        _xmlDocumentHelpers.GetSingleChildElement
                        (
                            parameterNode, 
                            e => e.Name == XmlDataConstants.CONTROLELEMENT
                        ).InnerText.Trim()
                    ),
                    _xmlDocumentHelpers.GetSingleChildElement
                    (
                        parameterNode, 
                        e => e.Name == XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT
                    ).InnerText.Trim(),
                    new HashSet<string>
                    (
                        _xmlDocumentHelpers
                            .GetSiblingParameterElements(parameterNode, functionNode)
                            .Where(e => e.Name == XmlDataConstants.LITERALPARAMETERELEMENT)
                            .Select(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
                    )
                );
            });
        }

        private void ValidateLiteralListParameterSourcedProperty(XmlElement functionNode, string functionName, List<XmlElement> parameterNodeList)
        {
            parameterNodeList.ForEach(parameterNode =>
            {
                if (parameterNode.Name != XmlDataConstants.LITERALLISTPARAMETERELEMENT)
                    return;

                ValidateParameterSourcedProperty
                (
                    functionName,
                    parameterNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                    (LiteralParameterInputStyle)Enum.Parse
                    (
                        typeof(LiteralParameterInputStyle), 
                        _xmlDocumentHelpers.GetSingleChildElement
                        (
                            parameterNode, 
                            e => e.Name == XmlDataConstants.ELEMENTCONTROLELEMENT
                        ).InnerText.Trim()
                    ),
                    _xmlDocumentHelpers.GetSingleChildElement
                    (
                        parameterNode, 
                        e => e.Name == XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT
                    ).InnerText.Trim(),
                    new HashSet<string>
                    (
                        _xmlDocumentHelpers
                            .GetSiblingParameterElements(parameterNode, functionNode)
                            .Where(e => e.Name == XmlDataConstants.LITERALPARAMETERELEMENT)
                            .Select(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
                    )
                );
            });
        }

        private void ValidateParameterSourcedProperty(string functionName, string parameterName, LiteralParameterInputStyle control, string siblingParameter, HashSet<string> siblingNames)
        {
            if (control != LiteralParameterInputStyle.ParameterSourcedPropertyInput)
                return;

            if (!siblingNames.Contains(siblingParameter))
            {
                XmlDocumentErrors.Add
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture, 
                        Strings.funcCannotLoadPropertySourceParameterFormat,
                        siblingParameter,
                        parameterName,
                        functionName,
                        string.Join(", ", siblingNames),
                        Strings.enumDescriptionParameterSourcedPropertyInput
                    )
                );
            }
        }

        private void ValidateGenericArguments(string functionName, ReferenceCategories referenceCategory, XmlElement returnTypeElement, List<XmlElement> parameterNodeList, List<string> genericArguments)
        {
            if (referenceCategory != ReferenceCategories.Type && genericArguments.Count > 0)
            {
                XmlDocumentErrors.Add
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture, 
                        Strings.funcGenericArgNotValidForAllReferenceCatFormat, 
                        functionName
                    )
                );

                return;
            }

            if(!ValidateGenericArgumentNames())
                return;

            ValidateReturnType
            (
                _xmlDocumentHelpers.GetSingleChildElement(returnTypeElement)
            );

            parameterNodeList.ForEach(parameterNode =>
            {
                if (parameterNode.Name != XmlDataConstants.GENERICPARAMETERELEMENT
                            && parameterNode.Name != XmlDataConstants.GENERICLISTPARAMETERELEMENT)
                    return;

                ValidateGenericArguments
                (
                    parameterNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                    _xmlDocumentHelpers.GetSingleChildElement
                    (
                        parameterNode, 
                        e => e.Name == XmlDataConstants.GENERICARGUMENTNAMEELEMENT
                    ).InnerText.Trim()
                );
            });

            #region Local Methods
            bool ValidateGenericArgumentNames()
            {
                return genericArguments.Aggregate(true, (allValid, next) =>
                {
                    bool valid = Regex.IsMatch(next, RegularExpressions.GENERICARGUMENTNAME);

                    if(!valid)
                    {
                        XmlDocumentErrors.Add
                        (
                            string.Format
                            (
                                CultureInfo.CurrentCulture,
                                Strings.funcGenericArgNameInvalidFormat,
                                next,
                                functionName
                            )
                        );
                    }

                    return allValid && valid;
                });
            }

            void ValidateGenericArguments(string parameterName, string genericArgName)
            {
                if (!new HashSet<string>(genericArguments).Contains(genericArgName))
                {
                    XmlDocumentErrors.Add
                    (
                        string.Format
                        (
                            CultureInfo.CurrentCulture,
                            Strings.funcGenericParameterArgNameNotFoundFormat,
                            genericArgName,
                            parameterName,
                            functionName,
                            string.Join(", ", genericArguments)
                        )
                    );
                }
            }

            void ValidateReturnType(XmlElement returnTypeChildElement)
            {
                if (!new HashSet<string>(new string[] { XmlDataConstants.GENERICELEMENT, XmlDataConstants.GENERICLISTELEMENT }).Contains(returnTypeChildElement.Name))
                    return;

                DoValidateReturnType
                (
                    _xmlDocumentHelpers.GetSingleChildElement
                    (
                        returnTypeChildElement,
                        e => e.Name == XmlDataConstants.GENERICARGUMENTNAMEELEMENT
                    ).InnerText
                );

                void DoValidateReturnType(string genericArgumentName)
                {
                    if (!new HashSet<string>(genericArguments).Contains(genericArgumentName))
                    {
                        XmlDocumentErrors.Add
                        (
                            string.Format
                            (
                                CultureInfo.CurrentCulture,
                                Strings.funcGenericReturnTypeArgNameNotFoundFormat,
                                genericArgumentName,
                                functionName,
                                string.Join(", ", genericArguments)
                            )
                        );
                    }
                }
            }
            #endregion Local Methods
        }
    }
}
