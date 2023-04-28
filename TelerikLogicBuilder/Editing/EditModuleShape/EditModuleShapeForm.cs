using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditModuleShape
{
    internal partial class EditModuleShapeForm : Telerik.WinControls.UI.RadForm, IEditModuleShapeForm
    {
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEditModuleShapeTreeViewBuilder _editModuleNameTreeViewBuilder;
        private readonly IFormInitializer _formInitializer;
        private readonly IModuleDataParser _moduleDataParser;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public EditModuleShapeForm(
            IDialogFormMessageControl dialogFormMessageControl,
            IEditModuleShapeTreeViewBuilder editModuleNameTreeViewBuilder,
            IFormInitializer formInitializer,
            IModuleDataParser moduleDataParser,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            XmlDocument? moduleXmlDocument)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;
            _editModuleNameTreeViewBuilder = editModuleNameTreeViewBuilder;
            _formInitializer = formInitializer;
            _moduleDataParser = moduleDataParser;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            Initialize();
            UpdateModuleName(moduleXmlDocument);
        }

        public string ShapeXml
        {
            get
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
                {
                    xmlTextWriter.WriteStartElement(XmlDataConstants.SHAPEDATAELEMENT);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, UniversalMasterName.MODULE);
                        xmlTextWriter.WriteElementString(XmlDataConstants.VALUEELEMENT, ShapeVisibleText);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.Flush();
                }

                return stringBuilder.ToString();
            }
        }

        public string ShapeVisibleText => radTreeView.SelectedNode.Text;

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            ControlsLayoutUtility.LayoutGroupBox(radPanelTop, radGroupBoxTop);

            radTreeView.NodeExpandedChanged += RadTreeView_NodeExpandedChanged;
            radTreeView.SelectedNodeChanged += RadTreeView_SelectedNodeChanged;
            FormClosing += EditModuleNameForm_FormClosing;

            _formInitializer.SetFormDefaults(this, 648);
            _editModuleNameTreeViewBuilder.Build(radTreeView);

            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelMessages);
            CollapsePanelBorder(radPanelTop);
            ValidateOk();
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void UpdateModuleName(XmlDocument? moduleXmlDocument)
        {
            if (moduleXmlDocument == null)
                return;

            string moduleName = _moduleDataParser.Parse
            (
                _xmlDocumentHelpers.GetDocumentElement(moduleXmlDocument)
            );

            if (string.IsNullOrEmpty(moduleName))
                return;

            _treeViewService.SelectTreeNode
            (
                radTreeView, 
                n => n.Text == moduleName && _treeViewService.IsFileNode(n)
            );
        }

        private void ValidateOk()
        {
            btnOk.Enabled = radTreeView.SelectedNode != null && _treeViewService.IsFileNode(radTreeView.SelectedNode);
        }

        #region Event Handlers
        private void EditModuleNameForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _dialogFormMessageControl.ClearMessage();
            if 
            (
                DialogResult == DialogResult.OK 
                    &&
                (radTreeView.SelectedNode == null || !_treeViewService.IsFileNode(radTreeView.SelectedNode))
            )
            {
                _dialogFormMessageControl.SetErrorMessage(Strings.moduleMustBeSelected);
                e.Cancel = true;
            }
        }

        private void RadTreeView_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            if (_treeViewService.IsRootNode(e.Node)
                || _treeViewService.IsFileNode(e.Node))/*NodeExpandedChanged runs for file nodes on double click*/
                return;

            e.Node.ImageIndex = e.Node.Expanded
                ? ImageIndexes.OPENEDFOLDERIMAGEINDEX
                : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
        }

        private void RadTreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            ValidateOk();
        }
        #endregion Event Handlers
    }
}
