using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureVariablesFolder
{
    internal partial class ConfigureVariablesFolderControl : UserControl, IConfigureVariablesFolderControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesFolderControl(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureVariablesForm configureVariablesForm)
        {
            InitializeComponent();
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureVariablesForm = configureVariablesForm;
        }

        private RadTreeView TreeView => configureVariablesForm.TreeView;
        private XmlDocument XmlDocument => configureVariablesForm.XmlDocument;

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsFolderNode(treeNode))
                throw _exceptionHelper.CriticalException("{915BB041-435A-4C73-84A1-64B8D923E2A5}");

            XmlElement folderElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);

            txtFolderName.Text = folderElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtFolderName.Select();
            txtFolderName.SelectAll();
        }

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (TreeView.SelectedNode == null)
                return;

            if (!_treeViewService.IsFolderNode(treeNode))
                throw _exceptionHelper.CriticalException("{B767DBE7-9608-4400-9404-E69F9F2B7976}");

            ValidateFolderName();

            string folderName = txtFolderName.Text.Trim();
            XmlElement folderElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            folderElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = folderName;

            configureVariablesForm.ValidateXmlDocument();

            if (treeNode.Expanded && configureVariablesForm.ExpandedNodes.ContainsKey(treeNode.Name))
                configureVariablesForm.ExpandedNodes.Remove(treeNode.Name);

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.FOLDERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{folderName}\"]";
            treeNode.Text = folderName;

            if (treeNode.Expanded)
                configureVariablesForm.ExpandedNodes.Add(treeNode.Name, folderName);

            configureVariablesForm.RenameChildNodes(treeNode);
        }

        private void ValidateFolderName()
        {
            configureVariablesForm.ClearMessage();
            if (!Regex.IsMatch(txtFolderName.Text, RegularExpressions.XMLATTRIBUTE))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidTxtNameTextFormat, lblFolderName.Text));
        }
    }
}
