using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class TreeViewXmlDocumentHelper : ITreeViewXmlDocumentHelper
    {
        private readonly IXmlValidator _xmlValidator;

        public TreeViewXmlDocumentHelper(
            IXmlValidatorFactory xmlValidatorFactory,
            SchemaName schema)
        {
            _xmlValidator = xmlValidatorFactory.GetXmlValidator(schema);
        }

        public XmlDocument BackupXmlTreeDocument { get; } = new XmlDocument();

        public XmlDocument XmlTreeDocument { get; } = new XmlDocument();

        public void LoadXmlDocument(string xmlString)
        {
            try
            {
                XmlTreeDocument.LoadXml(xmlString);
                BackupXmlTreeDocument.LoadXml(xmlString);
                ValidateXmlDocument();
            }
            catch (XmlException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (LogicBuilderException)
            {
                throw;
            }
        }

        public void ValidateXmlDocument()
        {
            XmlValidationResponse validationResponse = _xmlValidator.Validate(XmlTreeDocument.OuterXml);
            if (validationResponse.Success)
            {
                BackupXmlTreeDocument.LoadXml(XmlTreeDocument.OuterXml);
            }
            else
            {
                XmlTreeDocument.LoadXml(BackupXmlTreeDocument.OuterXml);
                throw new LogicBuilderException(string.Join(Environment.NewLine, validationResponse.Errors));
            }
        }
    }
}
