using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;
using Telerik.WinControls.UI;

namespace TelerikLogicBuilder.Tests.Mocks
{
    internal class ConfigureProjectPropertiesMock : IConfigureProjectPropertiesForm
    {
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;

        public ConfigureProjectPropertiesMock(ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper,
            RadTreeView treeView,
            XmlDocument xmlDocument)
        {
            _treeViewXmlDocumentHelper = treeViewXmlDocumentHelper;
            TreeView = treeView;
            XmlDocument = xmlDocument;
        }

        public RadTreeView TreeView { get; }

        public XmlDocument XmlDocument { get; }

        public void ClearMessage()
        {
        }

        public void SetErrorMessage(string message)
        {
        }

        public void SetMessage(string message, string title = "")
        {
        }

        public void ValidateXmlDocument()
        {
            _treeViewXmlDocumentHelper.ValidateXmlDocument();
        }
    }
}
