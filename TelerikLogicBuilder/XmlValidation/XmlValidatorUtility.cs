using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            Dictionary<SchemaName, XmlSchema> schemas = new()
            {
                [SchemaName.ProjectPropertiesSchema] = Schemas.ProjectPropertiesSchema,
                [SchemaName.ShapeDataSchema] = Schemas.ShapeDataSchema,
                [SchemaName.ConnectorDataSchema] = Schemas.ConnectorDataSchema,
                [SchemaName.DecisionsDataSchema] = Schemas.DecisionsDataSchema,
                [SchemaName.ConditionsDataSchema] = Schemas.ConditionsDataSchema,
                [SchemaName.FunctionsDataSchema] = Schemas.FunctionsDataSchema,
                [SchemaName.TableSchema] = Schemas.TableSchema,
                [SchemaName.ParametersDataSchema] = Schemas.ParametersDataSchema,
                [SchemaName.FragmentsSchema] = Schemas.FragmentsSchema
            };

            if (!schemas.TryGetValue(xmlSchema, out XmlSchema schema))
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{1B68AB17-53C8-4B5B-9D9D-99482235437D}"));

            return new XmlValidatorUtility(schema, xmlString);
        }

        protected internal virtual XmlValidationResponse ValidateXmlDocument()
        {
            DoXmlSchemaValidation();
            IList<string> validationErrors = LoadXmlDocumentValidationErrors();

            return new XmlValidationResponse
            {
                Success = validationErrors.Count == 0,
                Errors = validationErrors.ToList()
            };
        }

        protected void DoXmlSchemaValidation()
        {
            if (xmlSchema == null)
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{F384E2F2-E91C-41FC-B4CA-2EFDBBCD167F}"));

            xmlDocument.Schemas.Add(xmlSchema);
            xmlDocument.Validate(ValidateXmlDocumentEventHandler);
        }

        protected IList<string> LoadXmlDocumentValidationErrors()
        {
            List<string> errorMessages = new(xmlDocumentErrors);
            xmlDocumentErrors.Clear();
            return errorMessages;
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
