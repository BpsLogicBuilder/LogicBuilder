﻿using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper
{
    internal partial class ConfigureConstructorsHelperForm : Telerik.WinControls.UI.RadForm, IConfigureConstructorsHelperForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IChildConstructorFinderFactory _childConstructorFinderFactory;
        private readonly IConstructorManager _constructorManager;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IExistingConstructorFinder _existingConstructorFinder;
        private readonly IFormInitializer _formInitializer;
        private readonly IIntellisenseConstructorsFormManager _intellisenseConstructorsFormManager;
        private readonly IStringHelper _stringHelper;
        private readonly ITypeHelper _typeHelper;

        private ApplicationTypeInfo _application;
        private readonly IDictionary<string, Constructor> originalConstructors;
        private readonly string? constructorToUpdate;

        private Constructor? selectedConstructor;

        public ConfigureConstructorsHelperForm(
            IChildConstructorFinderFactory childConstructorFinderFactory,
            IConstructorManager constructorManager,
            IDialogFormMessageControl dialogFormMessageControl,
            IExceptionHelper exceptionHelper,
            IExistingConstructorFinder existingConstructorFinder,
            IFormInitializer formInitializer,
            IIntellisenseFactory intellisenseFactory,
            IServiceFactory serviceFactory,
            IStringHelper stringHelper,
            ITypeHelper typeHelper,
            IDictionary<string, Constructor> existingConstructors,
            ConstructorHelperStatus? helperStatus,
            string? constructorToUpdate = null)
        {
            InitializeComponent();
            this.originalConstructors = existingConstructors;
            this.constructorToUpdate = constructorToUpdate;

            _constructorManager = constructorManager;
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _childConstructorFinderFactory = childConstructorFinderFactory;
            _exceptionHelper = exceptionHelper;
            _existingConstructorFinder = existingConstructorFinder;
            _formInitializer = formInitializer;
            _intellisenseConstructorsFormManager = intellisenseFactory.GetIntellisenseConstructorsFormManager(this);
            _stringHelper = stringHelper;
            _typeHelper = typeHelper;

            Initialize();

            _intellisenseConstructorsFormManager.UpdateSelection(helperStatus);
        }

        public ApplicationDropDownList CmbApplication => (ApplicationDropDownList)_applicationDropDownList;

        public AutoCompleteRadDropDownList CmbClass => cmbClass;

        public Constructor Constructor => selectedConstructor ?? throw _exceptionHelper.CriticalException("{A147FC9E-B259-403C-A9E3-752E2A43B5EE}");

        public ICollection<Constructor> ChildConstructors => listNewConstructors.Items.Select(i => (Constructor)i.Value).ToArray();

        public RadTreeView TreeView => radTreeView1;

        public ApplicationTypeInfo Application => _application ?? throw _exceptionHelper.CriticalException("{2DBEF057-E065-4B00-834F-DE6D24770B88}");

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
            ClearMessage();
            listNewConstructors.Items.Clear();
            if (TreeView.SelectedNode == null
                || TreeView.SelectedNode is not ConstructorTreeNode)
            {
                btnOk.Enabled = false;
                return;
            }

            IDictionary<string, Constructor> constructors = new Dictionary<string, Constructor>(originalConstructors);
            ConstructorTreeNode treeNode = (ConstructorTreeNode)TreeView.SelectedNode;
            Constructor? constructor = _existingConstructorFinder.FindExisting(treeNode.CInfo, constructors);
            if (constructor != null && this.constructorToUpdate != constructor.Name)
            {
                SetErrorMessage
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture, 
                        Strings.constructorExistsFormat,
                        constructor.Name,
                        constructor.TypeName,
                        string.Join(Strings.itemsCommaSeparator, constructor.Parameters.Select(parameter => parameter.ToString()))
                    )
                );
                btnOk.Enabled = false;
                return;
            }

            if (treeNode.CInfo.DeclaringType == null)
            {
                btnOk.Enabled = false;
                return;
            }

            string constructorName = this.constructorToUpdate ?? _stringHelper.EnsureUniqueName
            (
                _typeHelper.GetTypeDescription(treeNode.CInfo.DeclaringType),
                constructors.Keys.ToHashSet()
            );
            IChildConstructorFinder childConstructorFinder = _childConstructorFinderFactory.GetChildConstructorFinder(constructors);
            childConstructorFinder.AddChildConstructors(treeNode.CInfo.GetParameters());
            SortedDictionary<string, Constructor> sortedDictionary = new
            (
                constructors
                    .Where(c => !this.originalConstructors.ContainsKey(c.Key))
                    .ToDictionary(k => k.Key, v => v.Value)
            );
            listNewConstructors.Items.AddRange(sortedDictionary.Values.Select(i => new RadListDataItem(i.ToString(), i)));
            this.selectedConstructor = _constructorManager.CreateConstructor(constructorName, treeNode.CInfo);
            btnOk.Enabled = true;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            CmbClass.TextChanged += CmbClass_TextChanged;
            TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;

            _intellisenseConstructorsFormManager.Initialize();

            _formInitializer.SetFormDefaults(this, 717);

            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            CollapsePanelBorder(radPanelApplication);
            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelMessages);
            CollapsePanelBorder(radPanelNewConstructors);
            CollapsePanelBorder(radPanelSource);
            CollapsePanelBorder(radPanelTableParent);
        }

        private void InitializeApplicationDropDownList()
        {
            ((ISupportInitialize)radPanelApplication).BeginInit();
            radPanelApplication.SuspendLayout();

            _applicationDropDownList.Dock = DockStyle.Fill;
            _applicationDropDownList.Location = new Point(0, 0);
            radPanelApplication.Controls.Add((Control)_applicationDropDownList);

            ((ISupportInitialize)radPanelApplication).EndInit();
            radPanelApplication.ResumeLayout(true);
        }

        private void InitializeDialogFormMessageControl()
        {
            ((ISupportInitialize)this.radPanelMessages).BeginInit();
            this.radPanelMessages.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new Point(0, 0);
            this.radPanelMessages.Controls.Add((Control)_dialogFormMessageControl);

            ((ISupportInitialize)this.radPanelMessages).EndInit();
            this.radPanelMessages.ResumeLayout(true);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);

            _intellisenseConstructorsFormManager.ApplicationChanged();
        }

        private void CmbClass_TextChanged(object? sender, EventArgs e)
        {
            _intellisenseConstructorsFormManager.CmbClassTextChanged();
        }

        private void TreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            ValidateOk();
        }
        #endregion Event Handlers
    }
}