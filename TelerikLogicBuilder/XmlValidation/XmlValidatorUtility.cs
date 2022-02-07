using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace ABIS.LogicBuilder.FlowBuilder.XmlValidation
{
    internal class XmlValidatorUtility
    {
        internal XmlValidatorUtility(XmlSchema xmlSchema, string xmlString)
        {
            this.xmlSchema = xmlSchema;
            xmlDocument.LoadXml(xmlString);
        }

        #region Constants
        #endregion Constants

        #region Variables
        private readonly XmlDocument xmlDocument = new();
        private XmlSchema xmlSchema;
        private readonly List<string> xmlDocumentErrors = new();
        #endregion Variables

        #region Properties
        protected IXPathNavigable XmlDocument
        {
            get { return xmlDocument; }
        }

        protected List<string> XmlDocumentErrors
        {
            get { return xmlDocumentErrors; }
        }

        protected virtual XmlSchema Schema
        {
            get { return xmlSchema; }
            set { xmlSchema = value; }
        }
        #endregion Properties

        #region Methods
        internal static XmlValidatorUtility GetXmlValidator(SchemaName xmlSchema, string xmlString)
        {
            return xmlSchema switch
            {
                //SchemaName.VariablesSchema => new VariablesXmlValidator(Schemas.VariablesSchema, xmlString),
                //SchemaName.FunctionsSchema => new FunctionsXmlValidatorNew(Schemas.FunctionsSchema, xmlString),
                //SchemaName.ConstructorSchema => new ConstructorsXmlValidator(Schemas.ConstructorSchema, xmlString),
                SchemaName.ProjectPropertiesSchema => new XmlValidatorUtility(Schemas.ProjectPropertiesSchema, xmlString),
                SchemaName.ShapeDataSchema => new XmlValidatorUtility(Schemas.ShapeDataSchema, xmlString),
                SchemaName.ConnectorDataSchema => new XmlValidatorUtility(Schemas.ConnectorDataSchema, xmlString),
                SchemaName.DecisionsDataSchema => new XmlValidatorUtility(Schemas.DecisionsDataSchema, xmlString),
                SchemaName.ConditionsDataSchema => new XmlValidatorUtility(Schemas.ConditionsDataSchema, xmlString),
                SchemaName.FunctionsDataSchema => new XmlValidatorUtility(Schemas.FunctionsDataSchema, xmlString),
                SchemaName.TableSchema => new XmlValidatorUtility(Schemas.TableSchema, xmlString),
                SchemaName.ParametersDataSchema => new XmlValidatorUtility(Schemas.ParametersDataSchema, xmlString),
                SchemaName.FragmentsSchema => new XmlValidatorUtility(Schemas.FragmentsSchema, xmlString),
                _ => throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{1B68AB17-53C8-4B5B-9D9D-99482235437D}")),
            };
        }

        protected internal virtual void ValidateXmlDocument()
        {
            DoXmlSchemaValidation();
            string validationErrors = LoadXmlDocumentValidationErrors();

            if (validationErrors.Length != 0)
            {
                throw new XmlValidationException(validationErrors);
            }
        }

        protected void DoXmlSchemaValidation()
        {
            if (xmlSchema == null)
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{F384E2F2-E91C-41FC-B4CA-2EFDBBCD167F}"));

            xmlDocument.Schemas.Add(xmlSchema);
            xmlDocument.Validate(ValidateXmlDocumentEventHandler);
        }

        protected string LoadXmlDocumentValidationErrors()
        {
            StringBuilder stringBuilder = new();
            foreach (string error in xmlDocumentErrors)
            {
                stringBuilder.Append(error);
                stringBuilder.Append(Environment.NewLine);
            }
            xmlDocumentErrors.Clear();
            return stringBuilder.ToString();
        }
        #endregion Methods

        #region EventHandlers
        private void ValidateXmlDocumentEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
            {
                xmlDocumentErrors.Add(e.Message);
            }
            else
            {
                //Debug.Print(e.Message);
            }
        }
        #endregion EventHandlers
    }
}
