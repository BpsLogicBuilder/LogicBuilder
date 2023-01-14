using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructorsFolder
{
    internal partial class ConfigureConstructorsFolderControl : UserControl, IConfigureConstructorsFolderControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsFolderControl(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            InitializeComponent();
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        private RadTreeView TreeView => configureConstructorsForm.TreeView;
        private XmlDocument XmlDocument => configureConstructorsForm.XmlDocument;

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsFolderNode(treeNode))
                throw _exceptionHelper.CriticalException("{B9422494-D9E1-48B4-9708-B6AF1D285E93}");

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
                throw _exceptionHelper.CriticalException("{AB51E719-78CC-4E1A-A0F7-F23C71CD0750}");

            ValidateFolderName();

            string folderName = txtFolderName.Text.Trim();
            XmlElement folderElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            folderElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = folderName;

            configureConstructorsForm.ValidateXmlDocument();

            if (treeNode.Expanded && configureConstructorsForm.ExpandedNodes.ContainsKey(treeNode.Name))
                configureConstructorsForm.ExpandedNodes.Remove(treeNode.Name);

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.FOLDERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{folderName}\"]";
            treeNode.Text = folderName;

            if (treeNode.Expanded)
                configureConstructorsForm.ExpandedNodes.Add(treeNode.Name, folderName);

            configureConstructorsForm.RenameChildNodes(treeNode);
        }

        private void ValidateFolderName()
        {
            configureConstructorsForm.ClearMessage();
            if (!XmlAttributeRegex().IsMatch(txtFolderName.Text))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidAttributeFormat, lblFolderName.Text));
        }

        [GeneratedRegex(RegularExpressions.XMLATTRIBUTE)]
        private static partial Regex XmlAttributeRegex();
    }
}
