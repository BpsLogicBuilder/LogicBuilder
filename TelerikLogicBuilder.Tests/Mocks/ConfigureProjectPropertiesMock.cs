using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;
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

        public DialogResult DialogResult => throw new System.NotImplementedException();

        public void ClearMessage()
        {
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void SetErrorMessage(string message)
        {
        }

        public void SetMessage(string message, string title = "")
        {
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            throw new System.NotImplementedException();
        }

        public void ValidateXmlDocument()
        {
            _treeViewXmlDocumentHelper.ValidateXmlDocument();
        }
    }
}
