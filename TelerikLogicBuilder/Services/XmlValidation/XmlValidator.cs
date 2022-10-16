using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation
{
    internal class XmlValidator : IXmlValidator
    {
        internal XmlValidator(XmlSchema xmlSchema)
        {
            this.xmlSchema = xmlSchema;
        }

        #region Variables
        private readonly XmlDocument xmlDocument = new();
        private XmlSchema xmlSchema;
        private readonly List<string> xmlDocumentErrors = new();
        private static readonly object lockValidation = new();
        #endregion Variables

        #region Properties
        protected XmlDocument XmlDocument
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
        public virtual XmlValidationResponse Validate(string xmlString)
        {
            xmlDocument.LoadXml(xmlString);
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

            lock (lockValidation)
            {
                xmlDocument.Schemas.Add(xmlSchema);
                xmlDocument.Validate(ValidateXmlDocumentEventHandler);
            }
        }

        protected IList<string> LoadXmlDocumentValidationErrors()
        {
            List<string> errorMessages = new(xmlDocumentErrors);
            xmlDocumentErrors.Clear();
            return errorMessages;
        }
        #endregion Methods

        #region EventHandlers
        private void ValidateXmlDocumentEventHandler(object? sender, ValidationEventArgs e)
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
