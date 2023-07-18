using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties
{
    internal partial class ConfigureProjectPropertiesForm : RadForm, IConfigureProjectPropertiesForm
    {
        private readonly IConfigureProjectPropertiesControlFactory _configurationControlFactory;
        private readonly IConfigurationService _configurationService;
        private readonly IConfigureProjectPropertiesContextMenuCommandFactory _configureProjectPropertiesContextMenuCommandFactory;
        private readonly IConfigureProjectPropertiesTreeViewBuilder _configureProjectPropertiesTreeviewBuilder;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IImageListService _imageListService;
        private readonly ILoadProjectProperties _loadProjectProperties;
        private readonly IProjectPropertiesXmlParser _projectPropertiesXmlParser;
        private readonly ITreeViewService _treeViewService;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        private readonly IUpdateProjectProperties _updateProjectProperties;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;

        private readonly RadMenuItem mnuItemAdd = new(Strings.mnuItemAddText);
        private readonly RadMenuItem mnuItemDelete = new(Strings.mnuItemDeleteText) { ImageIndex = ImageIndexes.DELETEIMAGEINDEX };
        private readonly bool openedAsReadOnly;
        private EventHandler mnuItemAddClickHandler;
        private EventHandler mnuItemDeleteClickHandler;

        public ConfigureProjectPropertiesForm(
            IConfigureProjectPropertiesControlFactory configurationControlFactory,
            IConfigurationService configurationService,
            IConfigureProjectPropertiesContextMenuCommandFactory configureProjectPropertiesContextMenuCommandFactory,
            IConfigureProjectPropertiesTreeViewBuilder configureProjectPropertiesTreeviewBuilder,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IImageListService imageListService,
            ILoadProjectProperties loadProjectProperties,
            IProjectPropertiesXmlParser projectPropertiesXmlParser,
            IServiceFactory serviceFactory,
            ITreeViewService treeViewService,
            IUpdateProjectProperties updateProjectProperties,
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            bool openedAsReadOnly)
        {

            InitializeComponent();
            _configurationControlFactory = configurationControlFactory;
            _configurationService = configurationService;
            _configureProjectPropertiesContextMenuCommandFactory = configureProjectPropertiesContextMenuCommandFactory;
            _configureProjectPropertiesTreeviewBuilder = configureProjectPropertiesTreeviewBuilder;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _imageListService = imageListService;
            _loadProjectProperties = loadProjectProperties;
            _projectPropertiesXmlParser = projectPropertiesXmlParser;
            _treeViewService = treeViewService;
            _treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ProjectPropertiesSchema
            );
            _updateProjectProperties = updateProjectProperties;

            this.openedAsReadOnly = openedAsReadOnly;
            this._dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();

            Initialize();
        }

        #region Properties
        public RadTreeView TreeView => radTreeView1;
        public XmlDocument XmlDocument => _treeViewXmlDocumentHelper.XmlTreeDocument;
        #endregion Properties

        #region Methods
        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void SetErrorMessage(string message)
            => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _dialogFormMessageControl.SetMessage(message, title);

        public void ValidateXmlDocument()
            => _treeViewXmlDocumentHelper.ValidateXmlDocument();

        private void AddClickCommands()
        {
            RemoveClickCommands();
            mnuItemAdd.Click += mnuItemAddClickHandler;
            mnuItemDelete.Click += mnuItemDeleteClickHandler;
        }

        private void BuildTreeView()
        {
            if (_treeViewXmlDocumentHelper.XmlTreeDocument.DocumentElement == null)
                throw _exceptionHelper.CriticalException("{5AA7735F-819E-44A1-9329-6694050DF46E}");

            try
            {
                _treeViewXmlDocumentHelper.ValidateXmlDocument();
            }
            catch (LogicBuilderException ex)
            {
                throw new CriticalLogicBuilderException(ex.Message, ex);
            }

            _configureProjectPropertiesTreeviewBuilder.Build(radTreeView1, _treeViewXmlDocumentHelper.XmlTreeDocument);
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemAddClickHandler),
        nameof(mnuItemDeleteClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void CreateContextMenus()
        {
            mnuItemAddClickHandler = InitializeContextMenuClickCommand
            (
                _configureProjectPropertiesContextMenuCommandFactory.GetAddApplicationCommand(this)
            );

            mnuItemDeleteClickHandler = InitializeContextMenuClickCommand
            (
                _configureProjectPropertiesContextMenuCommandFactory.GetDeleteApplicationCommand(this)
            );

            radTreeView1.RadContextMenu = new()
            {
                ImageList = _imageListService.ImageList,
                Items =
                {
                    new RadMenuSeparatorItem(),
                    mnuItemAdd,
                    new RadMenuSeparatorItem(),
                    mnuItemDelete,
                    new RadMenuSeparatorItem()
                }
            };
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemAddClickHandler),
        nameof(mnuItemDeleteClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            InitializeDialogFormMessageControl();

            radTreeView1.SelectedNodeChanged += RadTreeView1_SelectedNodeChanged;
            radTreeView1.SelectedNodeChanging += RadTreeView1_SelectedNodeChanging;
            radTreeView1.NodeMouseClick += RadTreeView1_NodeMouseClick;
            radTreeView1.MouseDown += RadTreeView1_MouseDown;
            Disposed += ConfigureProjectPropertiesForm_Disposed;
            FormClosing += ConfigureProjectProperties_FormClosing;

            _formInitializer.SetFormDefaults(this, 685);
            _formInitializer.SetToConfigSize(this);

            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            LoadXmlDocument();
            BuildTreeView();
            CreateContextMenus();

            radTreeView1.SelectedNode ??= radTreeView1.Nodes[0];
            AddClickCommands();
        }

        private static EventHandler InitializeContextMenuClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void LoadXmlDocument()
        {
            ProjectProperties projectProperties = _loadProjectProperties.Load(_configurationService.ProjectProperties.ProjectFileFullName);
            _treeViewXmlDocumentHelper.LoadXmlDocument(projectProperties.ToXml);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void Navigate(Control newEditingControl)
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            ((ISupportInitialize)this.radSplitContainerTop).BeginInit();
            this.radSplitContainerTop.SuspendLayout();
            ((ISupportInitialize)this.splitPanelRight).BeginInit();
            this.splitPanelRight.SuspendLayout();
            ((ISupportInitialize)this).BeginInit();
            this.SuspendLayout();

            ClearSplitPanelControls();

            newEditingControl.Dock = DockStyle.Fill;
            newEditingControl.Location = new System.Drawing.Point(0, 0);
            splitPanelRight.Controls.Add(newEditingControl);

            ((ISupportInitialize)this.radSplitContainerTop).EndInit();
            this.radSplitContainerTop.ResumeLayout(false);
            ((ISupportInitialize)this.splitPanelRight).EndInit();
            this.splitPanelRight.ResumeLayout(true);//Both ApplicationControl and RootNodeControl created larger than the parent without this.splitPanelRight.ResumeLayout(true)
            ((ISupportInitialize)this).EndInit();
            this.ResumeLayout(false);

            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);

            void ClearSplitPanelControls()
            {
                foreach (Control control in splitPanelRight.Controls)
                {
                    control.Visible = false;
                    if (!control.IsDisposed)
                        control.Dispose();
                }

                splitPanelRight.Controls.Clear();
            }
        }

        private void Navigate(RadTreeNode treeNode)
        {
            if (_treeViewService.IsProjectRootNode(treeNode))
                Navigate(new ProjectPropertiesRootNodeControl());
            else if (_treeViewService.IsApplicationNode(treeNode))
                Navigate((Control)_configurationControlFactory.GetApplicationControl(this));
            else
                throw _exceptionHelper.CriticalException("{6F6940D1-13D7-4FBF-BD07-4C799E2E6E07}");
        }

        private void RemoveClickCommands()
        {
            mnuItemAdd.Click -= mnuItemAddClickHandler;
            mnuItemDelete.Click -= mnuItemDeleteClickHandler;
        }

        private void RemoveEventHandlers()
        {
            radTreeView1.SelectedNodeChanged -= RadTreeView1_SelectedNodeChanged;
            radTreeView1.SelectedNodeChanging -= RadTreeView1_SelectedNodeChanging;
            radTreeView1.NodeMouseClick -= RadTreeView1_NodeMouseClick;
            radTreeView1.MouseDown -= RadTreeView1_MouseDown;
            FormClosing -= ConfigureProjectProperties_FormClosing;
        }

        private void SetContextMenuState(RadTreeNode selectedNode)
        {
            mnuItemDelete.Enabled = selectedNode != radTreeView1.Nodes[0]
                                    && radTreeView1.Nodes[0].Nodes.Count > 1;
        }

        private void SetControlValues(RadTreeNode treeNode)
        {
            if (treeNode == null)
                return;

            Navigate(treeNode);
            IConfigurationXmlElementControl xmlElementControl = (IConfigurationXmlElementControl)splitPanelRight.Controls[0];
            xmlElementControl.SetControlValues(treeNode);
        }

        private void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (splitPanelRight.Controls.Count != 1)
                throw _exceptionHelper.CriticalException("{DFE20147-6B93-4C49-97E0-F73EA3616086}");

            if (splitPanelRight.Controls[0] is not IConfigurationXmlElementControl xmlElementControl)
                throw _exceptionHelper.CriticalException("{82D82DCC-B0DA-4590-A4F3-24F1EED844D8}");

            xmlElementControl.UpdateXmlDocument(treeNode);
        }
        #endregion Methods

        #region Event Handlers
        private void ConfigureProjectPropertiesForm_Disposed(object? sender, EventArgs e)
        {
            RemoveClickCommands();
            RemoveEventHandlers();
            _treeViewService.ClearImageLists(TreeView);
        }

        private void RadTreeView1_MouseDown(object? sender, MouseEventArgs e)
        {//handles case in which clicked area doesn't have a node
            RadTreeNode treeNode = this.radTreeView1.GetNodeAt(e.Location);
            if (treeNode == null && this.radTreeView1.Nodes.Count > 0)
            {
                //this.radTreeView1.SelectedNode = this.radTreeView1.Nodes[0];
                //SetContextMenuState(this.radTreeView1.SelectedNode);
            }
        }

        private void RadTreeView1_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            SetContextMenuState(this.radTreeView1.SelectedNode);
        }

        private void RadTreeView1_SelectedNodeChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            try
            {
                if (TreeView.SelectedNode == null
                    || e.Node == null)//Don't update if e.Node is null because
                    return;             //1) The selected node may have been deleted
                                        //2) There is no navigation (i.e. e.Node == null)

                _dialogFormMessageControl.ClearMessage();
                UpdateXmlDocument(TreeView.SelectedNode);
            }
            catch (XmlException ex)
            {
                e.Cancel = true;
                _dialogFormMessageControl.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                e.Cancel = true;
                _dialogFormMessageControl.SetErrorMessage(ex.Message);
            }
        }

        private void RadTreeView1_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            SetControlValues(e.Node);
        }

        private void ConfigureProjectProperties_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                if (!this.openedAsReadOnly && this.DialogResult == DialogResult.OK)
                {
                    if (radTreeView1.SelectedNode != null)
                    {
                        UpdateXmlDocument(radTreeView1.SelectedNode);
                    }

                    ProjectProperties projectProperties = _projectPropertiesXmlParser.GeProjectProperties
                    (
                        _treeViewXmlDocumentHelper.XmlTreeDocument.DocumentElement!,
                        _configurationService.ProjectProperties.ProjectName,
                        _configurationService.ProjectProperties.ProjectPath
                    );

                    _updateProjectProperties.Update
                    (
                        projectProperties.ProjectFileFullName,
                        projectProperties.ApplicationList,
                        projectProperties.ConnectorObjectTypes
                    );
                }
            }
            catch (LogicBuilderException ex)
            {
                e.Cancel = true;
                _dialogFormMessageControl.SetErrorMessage(ex.Message);
            }
        }
        #endregion Event Handlers
    }
}
