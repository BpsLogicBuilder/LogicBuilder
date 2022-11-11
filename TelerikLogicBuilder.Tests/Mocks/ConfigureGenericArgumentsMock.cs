using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using System;
using System.Xml;
using Telerik.WinControls.UI;

namespace TelerikLogicBuilder.Tests.Mocks
{
    internal class ConfigureGenericArgumentsMock : IConfigureGenericArguments
    {
        public ConfigureGenericArgumentsMock(Action onInvalidXmlRestoreDocumentAndThrow,
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
    }
}
