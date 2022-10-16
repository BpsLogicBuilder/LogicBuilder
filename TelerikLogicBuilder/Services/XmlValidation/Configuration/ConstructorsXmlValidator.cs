using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.Configuration
{
    internal class ConstructorsXmlValidator : XmlValidator, IConstructorsXmlValidator
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConstructorsXmlValidator(
            IXmlDocumentHelpers xmlDocumentHelpers) : base(Schemas.ConstructorSchema)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public override XmlValidationResponse Validate(string xmlString)
        {
            XmlDocument.LoadXml(xmlString);
            XmlDocument xDoc = XmlDocument;
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
            _xmlDocumentHelpers.SelectElements(xDoc, $"//{XmlDataConstants.CONSTRUCTORELEMENT}")
                .ForEach
                (
                    constructorNode =>
                    {
                        ValidateElements
                        (
                            constructorNode,
                            constructorNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                            _xmlDocumentHelpers
                                .GetChildElements(constructorNode)
                                .ToDictionary(e => e.Name)
                        );
                    }
                );
        }

        private void ValidateElements(XmlElement constructorNode, string constructorName, Dictionary<string, XmlElement> elements)
        {
            ValidateParameterSourcedProperty
            (
                constructorNode,
                constructorName,
                elements[XmlDataConstants.PARAMETERSELEMENT],
                XmlDataConstants.LITERALPARAMETERELEMENT,
                XmlDataConstants.CONTROLELEMENT
            );

            ValidateParameterSourcedProperty
            (
                constructorNode,
                constructorName,
                elements[XmlDataConstants.PARAMETERSELEMENT],
                XmlDataConstants.LITERALLISTPARAMETERELEMENT,
                XmlDataConstants.ELEMENTCONTROLELEMENT
            );

            ValidateGenericArguments
            (
                constructorName,
                elements[XmlDataConstants.PARAMETERSELEMENT],
                elements[XmlDataConstants.GENERICARGUMENTSELEMENT]
            );

            ValidateParameterOrder(constructorName, elements[XmlDataConstants.PARAMETERSELEMENT]);

            ValidateTypeName(constructorName, elements[XmlDataConstants.TYPENAMEELEMENT].InnerText.Trim());
        }

        private void ValidateParameterSourcedProperty(XmlElement constructorNode, string constructorName, XmlElement paremtersElement, string parameterElementName, string controlElementName)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                paremtersElement,
                p => p.Name == parameterElementName
            )
            .ForEach
            (
                literalParameterNode =>
                {
                    if
                    (
                        GetParameterInputStyle
                        (
                            _xmlDocumentHelpers.GetSingleChildElement
                            (
                                literalParameterNode,
                                e => e.Name == controlElementName
                            )
                        ) != LiteralParameterInputStyle.ParameterSourcedPropertyInput
                    )
                    {
                        return;
                    }

                    ValidateParameterSourcedProperty
                    (
                        constructorName,
                        literalParameterNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                        _xmlDocumentHelpers.GetSingleChildElement
                        (
                            literalParameterNode,
                            e => e.Name == XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT
                        )
                        .InnerText.Trim(),
                        _xmlDocumentHelpers.GetSiblingParameterElements
                        (
                            literalParameterNode,
                            constructorNode
                        )
                        .Where(e => e.Name == XmlDataConstants.LITERALPARAMETERELEMENT)
                        .Select(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
                        .ToHashSet()
                    );
                }
            );
        }

        private void ValidateParameterSourcedProperty(string constructorName, string parameterName, string siblingParameter, HashSet<string> siblingNames)
        {
            if (!siblingNames.Contains(siblingParameter))
            {
                XmlDocumentErrors.Add
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.cannotLoadPropertySourceParameterFormat,
                        siblingParameter,
                        parameterName,
                        constructorName,
                        string.Join(", ", siblingNames),
                        Strings.enumDescriptionParameterSourcedPropertyInput
                    )
                );
            }
        }

        private void ValidateGenericArguments(string constructorName, XmlElement parametersElement, XmlElement genericArgumentsElement)
        {
            HashSet<string> genericArguments = _xmlDocumentHelpers.GetChildElements
            (
                genericArgumentsElement
            ).Select(e => e.InnerText).ToHashSet();

            if (!ValidateGenericArgumentNames())
                return;

            _xmlDocumentHelpers.GetChildElements
            (
                parametersElement,
                p => p.Name == XmlDataConstants.GENERICPARAMETERELEMENT
                        || p.Name == XmlDataConstants.GENERICLISTPARAMETERELEMENT
            )
            .ForEach
            (
                genericParameterNode =>
                {
                    string parameterName = genericParameterNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
                    string genericArgName = _xmlDocumentHelpers.GetSingleChildElement
                    (
                        genericParameterNode, e => e.Name == XmlDataConstants.GENERICARGUMENTNAMEELEMENT
                    ).InnerText.Trim();

                    if (!genericArguments.Contains(genericArgName))
                    {
                        XmlDocumentErrors.Add
                        (
                            string.Format
                            (
                                CultureInfo.CurrentCulture,
                                Strings.genericParameterArgNameNotFoundFormat,
                                genericArgName,
                                parameterName,
                                constructorName,
                                string.Join(", ", genericArguments)
                            )
                        );
                    }
                }
            );

            bool ValidateGenericArgumentNames()
            {
                return genericArguments.Aggregate(true, (allValid, next) =>
                {
                    bool valid = Regex.IsMatch(next, RegularExpressions.GENERICARGUMENTNAME);

                    if (!valid)
                    {
                        XmlDocumentErrors.Add
                        (
                            string.Format
                            (
                                CultureInfo.CurrentCulture,
                                Strings.constrGenericArgNameInvalidFormat,
                                next,
                                constructorName
                            )
                        );
                    }

                    return allValid && valid;
                });
            }
        }

        private void ValidateParameterOrder(string constructorName, XmlElement paremtersElement)
        {
            bool optionalExists = false;
            _xmlDocumentHelpers
                .GetChildElements(paremtersElement)
                .ForEach
                (
                    parameterNode =>
                    {
                        bool optional = bool.Parse
                        (
                            _xmlDocumentHelpers.GetSingleChildElement
                            (
                                parameterNode,
                                e => e.Name == XmlDataConstants.OPTIONALELEMENT
                            ).InnerText.Trim()
                        );

                        if (optional)
                            optionalExists = true;

                        if (!optional && optionalExists)//optional is true for a previous parameter
                            XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidConsParameterOrder, constructorName));
                    }
                );
        }

        private bool ValidateTypeName(string constructorName, string typeName)
        {
            if (!Regex.IsMatch(typeName, RegularExpressions.FULLYQUALIFIEDCLASSNAME))
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.typeReferenceNameIsInvalidConstructorFormat, constructorName));
                return false;
            }

            return true;
        }

        private static LiteralParameterInputStyle GetParameterInputStyle(XmlNode controlElement)
            => (LiteralParameterInputStyle)Enum.Parse
            (
                typeof(LiteralParameterInputStyle),
                controlElement.InnerText.Trim()
            );
    }
}
