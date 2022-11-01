using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using System;
using System.Xml;
using Telerik.WinControls.UI;

namespace TelerikLogicBuilder.Tests.Mocks
{
    internal class ConfigureProjectPropertiesMock : IConfigureProjectProperties
    {
        public ConfigureProjectPropertiesMock(Action onInvalidXmlRestoreDocumentAndThrow,
            RadTreeView treeView,
            XmlDocument xmlDocument)
        {
            OnInvalidXmlRestoreDocumentAndThrow = onInvalidXmlRestoreDocumentAndThrow;
            TreeView = treeView;
            XmlDocument = xmlDocument;
        }

        public Action OnInvalidXmlRestoreDocumentAndThrow { get; }

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
    }
}
