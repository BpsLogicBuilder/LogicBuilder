using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.Configuration
{
    internal class VariablesXmlValidator : XmlValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IStringHelper _stringHelper;
        private readonly IVariablesXmlParser _variablesXmlParser;
        private readonly IVariableValidationHelper _variableValidationHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public VariablesXmlValidator(IEnumHelper enumHelper,
            IStringHelper stringHelper,
            IVariablesXmlParser variablesXmlParser,
            IVariableValidationHelper variableValidationHelper,
            IXmlDocumentHelpers xmlDocumentHelpers) : base(Schemas.VariablesSchema)
        {
            _enumHelper = enumHelper;
            _stringHelper = stringHelper;
            _variablesXmlParser = variablesXmlParser;
            _variableValidationHelper = variableValidationHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Variables
        private IDictionary<string, VariableBase>? variables;
        #endregion Variables

        #region Methods
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

            //Get variables dictionary after Xml Schema Validation
            this.variables = _variablesXmlParser.GetVariablesDictionary(xDoc);
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
            _xmlDocumentHelpers.SelectElements
            (
                xDoc,
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
            .ForEach(variableNode =>
            {
                Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(variableNode)
                    .ToDictionary(e => e.Name);

                ValidateElement
                (
                    elements,
                    variableNode.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                    (VariableCategory)Enum.Parse(typeof(VariableCategory), elements[XmlDataConstants.VARIABLECATEGORYELEMENT].InnerText.Trim()),
                    (ReferenceCategories)Enum.Parse(typeof(ReferenceCategories), elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText.Trim()),
                    _stringHelper.SplitWithQuoteQualifier(elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText.Trim(), MiscellaneousConstants.PERIODSTRING),
                    _stringHelper.SplitWithQuoteQualifier(elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText.Trim(), MiscellaneousConstants.PERIODSTRING),
                    _stringHelper.SplitWithQuoteQualifier(elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText.Trim(), MiscellaneousConstants.PERIODSTRING)
                );

            });
        }

        private void ValidateElement(Dictionary<string, XmlElement> elements, string variableName, VariableCategory variableCategory, ReferenceCategories referenceCategory, string[] referenceDefinitionArray, string[] referenceNameArray, string[] castReferenceAsArray)
        {
            if (!ValidateReferenceDefinitionList(referenceDefinitionArray, variableName))
                return;

            ValidateMemberName(variableCategory, elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText.Trim(), variableName);

            if (!ValidateReferenceNameMustBeEmpty(variableName, referenceCategory, elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText.Trim()))
                return;

            if (!ValidateReferenceDefinitionMustBeEmpty(variableName, referenceCategory, elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText.Trim()))
                return;

            if (!ValidateCastReferenceAsMustBeEmpty(variableName, referenceCategory, elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText.Trim()))
                return;

            if (!ValidateReferenceMustBePopulated(variableName, referenceCategory, elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText.Trim(), elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText.Trim()))
                return;

            if (!ValidateReferenceDefinitionLengthMatchesReferenceNameLength(variableName, referenceDefinitionArray, referenceNameArray))
                return;

            if (!ValidateReferenceNameAndCastReferenceAsLengthsMatch(variableName, referenceNameArray, castReferenceAsArray))
                return;

            ValidateIndirectReferenceName
            (
                _enumHelper.ConvertToEnumList<ValidIndirectReference>(referenceDefinitionArray),
                referenceNameArray,
                variableName
            );

            ValidateTypeName(variableName, referenceCategory, elements[XmlDataConstants.TYPENAMEELEMENT].InnerText.Trim());
        }

        private bool ValidateReferenceDefinitionList(string[] referenceDefinitionArray,
            string variableName)
        {
            return referenceDefinitionArray.Aggregate
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
                                Strings.variableInvalidIndirectDefinitionFormat,
                                variableName,
                                definition,
                                Environment.NewLine,
                                _enumHelper.GetValidIndirectReferencesList()
                            )
                        );
                    }

                    return valid;
                }
            );
        }

        private void ValidateMemberName(VariableCategory variableCategory,
            string memberName,
            string variableName)
        {
            _variableValidationHelper.ValidateMemberName(variableCategory, memberName, variableName, XmlDocumentErrors, variables!);//NonSchemaValidation called after setting variables
        }

        private bool ValidateReferenceNameMustBeEmpty(string variableName,
            ReferenceCategories referenceCategory,
            string referenceName)
        {
            if ((referenceCategory == ReferenceCategories.This || referenceCategory == ReferenceCategories.Type)
                && referenceName.Length != 0)
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableReferenceMustBeEmptyFormat, variableName));
                return false;
            }

            return true;
        }

        private bool ValidateReferenceDefinitionMustBeEmpty(string variableName,
            ReferenceCategories referenceCategory,
            string referenceDefinition)
        {
            if ((referenceCategory == ReferenceCategories.This || referenceCategory == ReferenceCategories.Type)
                && referenceDefinition.Length != 0)
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableReferenceDefinitionMustBeEmptyFormat, variableName));
                return false;
            }

            return true;
        }

        private bool ValidateCastReferenceAsMustBeEmpty(string variableName,
            ReferenceCategories referenceCategory,
            string castReferenceAs)
        {
            if ((referenceCategory == ReferenceCategories.This || referenceCategory == ReferenceCategories.Type)
                && castReferenceAs.Length != 0)
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.castReferenceAsMustBeEmptyFormat, variableName));
                return false;
            }

            return true;
        }

        private bool ValidateReferenceMustBePopulated(string variableName,
            ReferenceCategories referenceCategory,
            string referenceDefinition,
            string referenceName)
        {
            if ((referenceCategory == ReferenceCategories.InstanceReference || referenceCategory == ReferenceCategories.StaticReference)
                && (referenceDefinition.Length == 0 || referenceName.Length == 0))
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableReferenceInfoMustBePopulatedFormat, variableName));
                return false;
            }

            return true;
        }

        private bool ValidateReferenceDefinitionLengthMatchesReferenceNameLength(string variableName,
            string[] referenceDefinitionArray,
            string[] referenceNameArray)
        {
            if (referenceDefinitionArray.Length != referenceNameArray.Length)
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableNameAndDefinitionFormat, variableName));
                return false;
            }

            return true;
        }

        private bool ValidateReferenceNameAndCastReferenceAsLengthsMatch(string variableName,
            string[] referenceNameArray,
            string[] castReferenceAsArray)
        {
            if (castReferenceAsArray.Length != 0 && castReferenceAsArray.Length != referenceNameArray.Length)
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.referenceNameAndCastReferenceAFormat, variableName));
                return false;
            }

            return true;
        }

        private void ValidateIndirectReferenceName(IList<ValidIndirectReference> referenceDefinition,
            string[] referenceNameArray,
            string variableName)
        {
            for (int i = 0; i < referenceDefinition.Count; i++)
                _variableValidationHelper.ValidateVariableIndirectReferenceName(referenceDefinition[i], referenceNameArray[i], variableName, XmlDocumentErrors, variables!);//NonSchemaValidation called after setting variables
        }

        private void ValidateTypeName(string variableName,
            ReferenceCategories referenceCategory,
            string typeName)
        {
            if ((referenceCategory == ReferenceCategories.Type || referenceCategory == ReferenceCategories.StaticReference)
                && !Regex.IsMatch(typeName, RegularExpressions.FULLYQUALIFIEDCLASSNAME))
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.typeReferenceNameIsInvalidFormatVariable, variableName));

            if ((referenceCategory != ReferenceCategories.Type && referenceCategory != ReferenceCategories.StaticReference)
                && typeName.Length != 0)
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableTypeNameMustBeEmptyFormat, variableName));
        }
        #endregion Methods
    }
}
