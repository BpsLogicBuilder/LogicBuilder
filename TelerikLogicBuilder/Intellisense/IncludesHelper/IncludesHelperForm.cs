using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.IncludesHelper
{
    internal partial class IncludesHelperForm : Telerik.WinControls.UI.RadForm, IIncludesHelperForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IIntellisenseIncludesFormManager _intellisenseIncludesFormManager;
        private readonly IStringHelper _stringHelper;

        private ApplicationTypeInfo _application;
        private readonly string className;

        public IncludesHelperForm(
            IDialogFormMessageControl dialogFormMessageControl,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IIntellisenseFactory intellisenseFactory,
            IServiceFactory serviceFactory,
            IStringHelper stringHelper,
            string className)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _enumHelper = enumHelper;
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _stringHelper = stringHelper;
            this.className = className;
            _intellisenseIncludesFormManager = intellisenseFactory.GetIntellisenseIncludesFormManager(this);
            Initialize();
        }

        public AutoCompleteRadDropDownList CmbClass => cmbClass;

        public string Includes
        {
            get
            {
                IDictionary<string, ValidIndirectReference> definitionsDictionary = _enumHelper.ToValidIndirectReferenceDictionary();
                string[] referenceNameArray = _stringHelper.SplitWithQuoteQualifier(_intellisenseIncludesFormManager.ReferenceName.Trim(), MiscellaneousConstants.PERIODSTRING);
                string[] refereneDefinitionArray = _stringHelper.SplitWithQuoteQualifier(_intellisenseIncludesFormManager.ReferenceDefinition.Trim(), MiscellaneousConstants.PERIODSTRING);
                int count = -1;

                List<string> includesList = referenceNameArray.Aggregate(new List<string>(), (list, next) =>
                {
                    count++;
                    ValidIndirectReference referenceType = definitionsDictionary[refereneDefinitionArray[count].ToLowerInvariant()];
                    if (referenceType == ValidIndirectReference.Field || referenceType == ValidIndirectReference.Property)
                        list.Add(next);
                    return list;
                });

                return string.Join(MiscellaneousConstants.PERIODSTRING, referenceNameArray);
            }
        }

        public VariableTreeNode ReferenceTreeNode => (VariableTreeNode)TreeView.SelectedNode;

        public RadTreeView TreeView => radTreeView1;

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{F0D7AE01-FFF6-481D-8DA6-8D112BAC9E5B}");

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void ClearTreeView()
        {
            TreeView.Nodes.Clear();
            ValidateOk();
        }

        public void SetErrorMessage(string message)
           => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _dialogFormMessageControl.SetMessage(message, title);

        public void ValidateOk()
        {
            if (this.Disposing || this.IsDisposed) return;
            //ValidateOk() can be called on changed for components deing disposed.

            btnOk.Enabled = radTreeView1.SelectedNode != null;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeTableLayoutPanel();
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            CmbClass.TextChanged += CmbClass_TextChanged;
            TreeView.NodeExpandedChanging += TreeView_NodeExpandedChanging;
            TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;

            _intellisenseIncludesFormManager.Initialize();

            _formInitializer.SetFormDefaults(this, 717);

            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            CollapsePanelBorder(radPanelApplication);
            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelFields);
            CollapsePanelBorder(radPanelMessages);
            CollapsePanelBorder(radPanelSource);
            CollapsePanelBorder(radPanelTableParent);
            cmbClass.Text = className;
        }

        private void InitializeApplicationDropDownList()
        {
            ControlsLayoutUtility.LayoutApplicationPanel(radPanelApplication, _applicationDropDownList);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void InitializeTableLayoutPanel()
        {
            this.SuspendLayout();
            ControlsLayoutUtility.LayoutControls
            (
                radGroupBoxSource,
                radPanelSource,
                radPanelTableParent,
                tableLayoutPanel,
                2,
                false
            );

            //must adjust height because radGroupBoxSource.Dock is not Fill.
            ControlsLayoutUtility.LayoutTwoRowGroupBox(this, radGroupBoxSource, false);
            this.ResumeLayout(true);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);

            _intellisenseIncludesFormManager.ApplicationChanged();
        }

        private void CmbClass_TextChanged(object? sender, EventArgs e)
            => _intellisenseIncludesFormManager.CmbClassTextChanged();

        private void TreeView_NodeExpandedChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (e.Node.Expanded)
            {
                _intellisenseIncludesFormManager.BeforeCollapse((BaseTreeNode)e.Node);
            }
            else
            {
                _intellisenseIncludesFormManager.BeforeExpand((BaseTreeNode)e.Node);
            }
            this.Cursor = Cursors.Default;
        }

        private void TreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e) => ValidateOk();
        #endregion Event Handlers
    }
}
