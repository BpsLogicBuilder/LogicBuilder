using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.ConfigureFragmentsFolder
{
    internal partial class ConfigureFragmentsFolderControl : UserControl, IConfigureFragmentsFolderControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFragmentsForm configureFragmentsForm;

        public ConfigureFragmentsFolderControl(IExceptionHelper exceptionHelper, ITreeViewService treeViewService, IXmlDocumentHelpers xmlDocumentHelpers, IConfigureFragmentsForm configureFragmentsForm)
        {
            InitializeComponent();
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFragmentsForm = configureFragmentsForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        private RadTreeView TreeView => configureFragmentsForm.TreeView;
        private XmlDocument XmlDocument => configureFragmentsForm.XmlDocument;

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsFolderNode(treeNode))
                throw _exceptionHelper.CriticalException("{B416A9FF-FCFE-438B-B1EA-C22B647CE069}");

            XmlElement folderElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            RemoveEventHandlers();
            txtFolderName.Text = folderElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtFolderName.Select();
            txtFolderName.SelectAll();
            AddEventHandlers();
        }

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (TreeView.SelectedNode == null)
                return;

            if (!_treeViewService.IsFolderNode(treeNode))
                throw _exceptionHelper.CriticalException("{3A77218C-ECDE-4440-AB15-378ABE2DB07F}");

            ValidateFolderName();

            string folderName = txtFolderName.Text.Trim();
            XmlElement folderElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            folderElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = folderName;

            configureFragmentsForm.ValidateXmlDocument();

            if (treeNode.Expanded && configureFragmentsForm.ExpandedNodes.ContainsKey(treeNode.Name))
                configureFragmentsForm.ExpandedNodes.Remove(treeNode.Name);

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.FOLDERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{folderName}\"]";
            treeNode.Text = folderName;

            if (treeNode.Expanded)
                configureFragmentsForm.ExpandedNodes.Add(treeNode.Name, folderName);

            configureFragmentsForm.RenameChildNodes(treeNode);
        }

        private void AddEventHandlers()
        {
            txtFolderName.TextChanged += TxtFolderName_TextChanged;
        }

        private void Initialize()
        {
            ResetGroupBoxes();
            InitializeFolderControls();
        }

        private void InitializeFolderControls()
        {
            helpProvider.SetHelpString(txtFolderName, Strings.fragmentConfigFolderNameHelp);
            toolTip.SetToolTip(groupBoxFolder, Strings.fragmentConfigFolderNameHelp);
        }

        private void ResetGroupBoxes()
        {
            ((ISupportInitialize)radGroupBoxName).BeginInit();
            radGroupBoxName.SuspendLayout();
            ((ISupportInitialize)radPanelContent).BeginInit();
            radPanelContent.SuspendLayout();
            ((ISupportInitialize)radPanelName).BeginInit();
            radPanelName.SuspendLayout();
            ((ISupportInitialize)groupBoxFolder).BeginInit();
            groupBoxFolder.SuspendLayout();
            SuspendLayout();

            radPanelName.Size = new Size(radPanelName.Width, PerFontSizeConstants.SingleRowGroupBoxHeight);
            radGroupBoxName.Padding = PerFontSizeConstants.SingleRowGroupBoxPadding;
            groupBoxFolder.Padding = PerFontSizeConstants.GroupBoxPadding;

            ((ISupportInitialize)radGroupBoxName).EndInit();
            radGroupBoxName.ResumeLayout(false);
            ((ISupportInitialize)radPanelContent).EndInit();
            radPanelContent.ResumeLayout(false);
            ((ISupportInitialize)radPanelName).EndInit();
            radPanelName.ResumeLayout(false);
            ((ISupportInitialize)groupBoxFolder).EndInit();
            groupBoxFolder.ResumeLayout(false);
            ResumeLayout(true);
        }

        private void RemoveEventHandlers()
        {
            txtFolderName.TextChanged -= TxtFolderName_TextChanged;
        }

        private void ValidateFolderName()
        {
            configureFragmentsForm.ClearMessage();
            if (!XmlAttributeRegex().IsMatch(txtFolderName.Text))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidAttributeFormat, radGroupBoxName.Text));
        }

        [GeneratedRegex(RegularExpressions.XMLATTRIBUTE)]
        private static partial Regex XmlAttributeRegex();

        private void TxtFolderName_TextChanged(object? sender, System.EventArgs e)
        {
            try
            {
                UpdateXmlDocument(TreeView.SelectedNode);
            }
            catch (XmlException ex)
            {
                configureFragmentsForm.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                configureFragmentsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}
