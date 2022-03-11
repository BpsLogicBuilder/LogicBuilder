﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.XmlValidation.Configuration
{
    internal class ConnectorDataXmlValidatorUtility : XmlValidatorUtility
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal ConnectorDataXmlValidatorUtility(string xmlString, IContextProvider contextProvider) : base(Schemas.ConnectorDataSchema, xmlString)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
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
            XmlElement documentElement = _xmlDocumentHelpers.GetDocumentElement(xDoc);
            ValidateElements
            (
                documentElement.GetAttribute(XmlDataConstants.CONNECTORCATEGORYATTRIBUTE),
                _xmlDocumentHelpers
                    .GetChildElements(documentElement)
                    .ToDictionary(e => e.Name)
            );
        }

        private void ValidateElements(string categoryString, Dictionary<string, XmlElement> elements)
        {
            ConnectorCategory category = (ConnectorCategory)short.Parse(categoryString, CultureInfo.InvariantCulture);
            if (!ValidateCategory(categoryString, category))
                return;

            ValidateMetaObjectElement(category, elements);
        }

        private void ValidateMetaObjectElement(ConnectorCategory category, Dictionary<string, XmlElement> elements)
        {
            elements.TryGetValue(XmlDataConstants.METAOBJECTELEMENT, out XmlElement? metaObjectElement);
            if (category == ConnectorCategory.Dialog && metaObjectElement == null)
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.metaObjectRequiredForDialogConnectorsFormat, XmlDataConstants.METAOBJECTELEMENT));
            }
        }

        private bool ValidateCategory(string categoryString, ConnectorCategory category)
        {
            if (!Enum.IsDefined(typeof(ConnectorCategory), category))
            {
                XmlDocumentErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.connectorCategoryUndefinedFormat, categoryString));
                return false;
            }

            return true;
        }
    }
}