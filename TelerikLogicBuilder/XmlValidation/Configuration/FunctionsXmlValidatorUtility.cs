using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
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
        internal FunctionsXmlValidatorUtility(string xmlString, IXmlDocumentHelpers xmlDocumentHelpers) : base(Schemas.FragmentsSchema, xmlString)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
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
            throw new NotImplementedException();
        }

        private bool ValidateTypeName(string functionName,
            ReferenceCategories referenceCategory,
            XmlNode typeNameNode)
        {
            if ((referenceCategory == ReferenceCategories.Type || referenceCategory == ReferenceCategories.StaticReference)
                    && !Regex.IsMatch(typeNameNode.InnerText, RegularExpressions.FULLYQUALIFIEDCLASSNAME))
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.typeReferenceNameIsInvalidFormat, functionName));
                return false;
            }

            return true;
        }

        private bool ValidateTypeNameMustBeEmpty(string functionName,
            ReferenceCategories referenceCategory,
            XmlNode typeNameNode)
        {
            if ((referenceCategory != ReferenceCategories.Type && referenceCategory != ReferenceCategories.StaticReference)
                && typeNameNode.InnerText.Trim().Length != 0)
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
                    parameterNode.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value,
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
                    parameterNode.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value,
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

        private void ValidateGenericArguments(string functionName, ReferenceCategories referenceCategory, XmlElement returnTypeElement, List<XmlElement> parameterNodeList, List<XmlElement> genericArgumentsList)
        {
            if (referenceCategory != ReferenceCategories.Type && genericArgumentsList.Count > 0)
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

            ValidateReturnType
            (
                _xmlDocumentHelpers.GetSingleChildElement(returnTypeElement),
                genericArgumentsList.Select(e => e.InnerText).ToList()
            );

            parameterNodeList.ForEach(parameterNode =>
            {
                if (parameterNode.Name != XmlDataConstants.GENERICPARAMETERELEMENT
                            && parameterNode.Name != XmlDataConstants.GENERICLISTPARAMETERELEMENT)
                    return;

                ValidateGenericArguments
                (
                    genericArgumentsList.Select(e => e.InnerText).ToList(),
                    parameterNode.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value,
                    _xmlDocumentHelpers.GetSingleChildElement
                    (
                        parameterNode, 
                        e => e.Name == XmlDataConstants.GENERICARGUMENTNAMEELEMENT
                    ).InnerText.Trim()
                );
            });

            void ValidateGenericArguments(List<string> genericArguments, string parameterName, string genericArgName)
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

                if (!Regex.IsMatch(genericArgName, RegularExpressions.GENERICARGUMENTNAME))
                {
                    XmlDocumentErrors.Add
                    (
                        string.Format
                        (
                            CultureInfo.CurrentCulture, 
                            Strings.funcGenericArgNameInvalidFormat, 
                            genericArgName, 
                            parameterName, 
                            functionName
                        )
                    );
                }
            }

            void ValidateReturnType(XmlElement returnTypeChildElement, List<string> genericArguments)
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
        }
    }
}
