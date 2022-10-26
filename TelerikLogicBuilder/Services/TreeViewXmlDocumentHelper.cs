using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Globalization;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class TreeViewXmlDocumentHelper : ITreeViewXmlDocumentHelper
    {
        private readonly IEncryption _encryption;
        private readonly IXmlValidatorFactory _xmlValidatorFactory;

        public TreeViewXmlDocumentHelper(
            IEncryption encryption,
            IXmlValidatorFactory xmlValidatorFactory,
            RadTreeView treeView,
            SchemaName schema)
        {
            _encryption = encryption;
            _xmlValidatorFactory = xmlValidatorFactory;
            TreeView = treeView;
            Schema = schema;
        }

        public XmlDocument BackupXmlTreeDocument { get; } = new XmlDocument();

        public RadTreeView TreeView { get; }

        public SchemaName Schema { get; }

        public XmlDocument XmlTreeDocument { get; } = new XmlDocument();

        public void LoadXmlDocument(string xmlFileFullName)
        {
            try
            {
                string xmlString = _encryption.DecryptFromFile(xmlFileFullName);
                XmlTreeDocument.LoadXml(xmlString);
                BackupXmlTreeDocument.LoadXml(xmlString);
                ValidateXmlDocument();
            }
            catch (XmlException ex)
            {
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidConfigurationDocumentFormat, xmlFileFullName), ex);
            }
            catch (LogicBuilderException)
            {
                throw;
            }
        }

        public void ValidateXmlDocument()
        {
            XmlValidationResponse validationResponse = _xmlValidatorFactory.GetXmlValidator(Schema).Validate(XmlTreeDocument.OuterXml);
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
