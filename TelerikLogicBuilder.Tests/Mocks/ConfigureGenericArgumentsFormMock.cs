using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace TelerikLogicBuilder.Tests.Mocks
{
    internal class ConfigureGenericArgumentsFormMock : IConfigureGenericArgumentsForm
    {
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        public ConfigureGenericArgumentsFormMock(ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper,
            RadTreeView treeView,
            XmlDocument xmlDocument)
        {
            _treeViewXmlDocumentHelper = treeViewXmlDocumentHelper;
            TreeView = treeView;
            XmlDocument = xmlDocument;
        }

        public ApplicationTypeInfo Application => throw new NotImplementedException();

        public RadTreeView TreeView { get; }

        public XmlDocument XmlDocument { get; }

        public IList<string> ConfiguredGenericArgumentNames => throw new NotImplementedException();

        public IList<ParameterBase> MemberParameters => throw new NotImplementedException();

        public DialogResult DialogResult => throw new NotImplementedException();

        public IList<GenericConfigBase> GenericArguments => throw new NotImplementedException();

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SetErrorMessage(string message)
        {
            ApplicationChanged?.Invoke(this, new ApplicationChangedEventArgs(null!));
            throw new NotImplementedException();
        }

        public void SetMessage(string message, string title = "")
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            throw new NotImplementedException();
        }

        public void ValidateXmlDocument()
        {
            _treeViewXmlDocumentHelper.ValidateXmlDocument();
        }
    }
}
