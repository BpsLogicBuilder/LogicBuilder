using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
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
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        private RadTreeView TreeView => configureConstructorsForm.TreeView;
        private XmlDocument XmlDocument => configureConstructorsForm.XmlDocument;

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsFolderNode(treeNode))
                throw _exceptionHelper.CriticalException("{B9422494-D9E1-48B4-9708-B6AF1D285E93}");

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

        private void AddEventHandlers()
        {
            txtFolderName.TextChanged += TxtFolderName_TextChanged;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            radPanelFolder.VerticalScrollBarState = ScrollState.AlwaysShow;
            InitializeTableLayoutPanel();
            InitializeFolderControls();
            CollapsePanelBorder(radPanelFolder);
            CollapsePanelBorder(radPanelTableParent);
        }

        private void InitializeFolderControls()
        {
            helpProvider.SetHelpString(txtFolderName, Strings.constrConfigFolderNameHelp);
            toolTip.SetToolTip(lblFolderName, Strings.constrConfigFolderNameHelp);
        }

        private void InitializeTableLayoutPanel()
        {
            float size_20 = 20F / 76 * 100;
            float size_30 = 30F / 76 * 100;
            float size_6 = 6F / 76 * 100;

            ((ISupportInitialize)this.radPanelTableParent).BeginInit();
            this.radPanelTableParent.SuspendLayout();

            this.tableLayoutPanel.RowStyles[0] = new RowStyle(SizeType.Percent, size_20);
            this.tableLayoutPanel.RowStyles[1] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[2] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[3] = new RowStyle(SizeType.Percent, size_20);

            ((ISupportInitialize)this.radPanelTableParent).EndInit();
            this.radPanelTableParent.ResumeLayout(true);
        }

        private void RemoveEventHandlers()
        {
            txtFolderName.TextChanged -= TxtFolderName_TextChanged;
        }

        private void ValidateFolderName()
        {
            configureConstructorsForm.ClearMessage();
            if (!XmlAttributeRegex().IsMatch(txtFolderName.Text))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidAttributeFormat, lblFolderName.Text));
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
                configureConstructorsForm.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                configureConstructorsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}
