using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Services.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths
{
    internal partial class ConfigureLoadAssemblyPathsControl : UserControl, IListBoxHost<AssemblyPath>, IConfigureLoadAssemblyPathsControl
    {
        private readonly ILoadAssemblyPathsItemFactory _loadAssemblyPathsItemFactory;
        private readonly IConfigureLoadAssemblyPathsCommandFactory _loadAssemblyPathsCommandFactory;
        private readonly IConfigureLoadAssemblyPathsForm _configureLoadAssemblyPaths;
        private readonly IRadListBoxManager<AssemblyPath> radListBoxManager;
        private EventHandler<EventArgs> txtPathButtonClickHandler;
        private EventHandler btnAddClickHandler;
        private EventHandler btnUpdateClickHandler;

        public RadButton BtnAdd => btnAdd;

        public RadButton BtnUpdate => btnUpdate;

        public RadButton BtnCancel => managedListBoxControl.BtnCancel;

        public RadButton BtnCopy => managedListBoxControl.BtnCopy;

        public RadButton BtnEdit => managedListBoxControl.BtnEdit;

        public RadButton BtnRemove => managedListBoxControl.BtnRemove;

        public RadButton BtnUp => managedListBoxControl.BtnUp;

        public RadButton BtnDown => managedListBoxControl.BtnDown;

        public RadListControl ListBox => managedListBoxControl.ListBox;

        public IRadListBoxManager<AssemblyPath> RadListBoxManager => radListBoxManager;

        public HelperButtonTextBox TxtPath => txtPath;

        public ConfigureLoadAssemblyPathsControl(
            IConfigureLoadAssemblyPathsCommandFactory loadAssemblyPathsCommandFactory,
            ILoadAssemblyPathsItemFactory loadAssemblyPathsItemFactory,
            IConfigureLoadAssemblyPathsForm configureLoadAssemblyPaths)
        {
            InitializeComponent();
            _loadAssemblyPathsItemFactory = loadAssemblyPathsItemFactory;
            _configureLoadAssemblyPaths = configureLoadAssemblyPaths;
            _loadAssemblyPathsCommandFactory = loadAssemblyPathsCommandFactory;
            radListBoxManager = new RadListBoxManager<AssemblyPath>(this);
            Initialize();
        }

        public void ClearInputControls()
            => txtPath.Text = string.Empty;

        public void ClearMessage()
            => _configureLoadAssemblyPaths.ClearMessage();

        public void DisableControlsDuringEdit(bool disable)
            => _configureLoadAssemblyPaths.DisableControlsDuringEdit(disable);

        public IList<string> GetPaths()
            => ListBox.Items
                    .Select(i => ((AssemblyPath)i.Value).Path)
                    .ToArray();

        public void SetErrorMessage(string message)
            => _configureLoadAssemblyPaths.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => _configureLoadAssemblyPaths.SetMessage(message, title);

        public void SetPaths(IList<string> paths)
        {
            ListBox.Items.AddRange
            (
                paths
                    .Select(p => _loadAssemblyPathsItemFactory.GetAssemblyPath(p.Trim()))
                    .Select(ap => new RadListDataItem(ap.ToString(), ap))
            );
        }

        public void UpdateInputControls(AssemblyPath item)
            => txtPath.Text = item.Path;

        private void AddClickCommands()
        {
            RemoveClickCommands();
            txtPath.ButtonClick += txtPathButtonClickHandler;
            btnAdd.Click += btnAddClickHandler;
            btnUpdate.Click += btnUpdateClickHandler;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(txtPathButtonClickHandler),
        nameof(btnAddClickHandler),
        nameof(btnUpdateClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            ControlsLayoutUtility.LayoutAddUpdateItemGroupBox(this, radGroupBoxAddPath);
            ControlsLayoutUtility.LayoutGroupBox(this, radGroupBoxPaths);
            ControlsLayoutUtility.LayoutAddUpdateButtonPanel(radPanelAddButton, tableLayoutPanelAddUpdate);
            CollapsePanelBorder(radPanelTxtPath);
            CollapsePanelBorder(radPanelAddButton);

            Disposed += ConfigureLoadAssemblyPathsControl_Disposed;

            txtPathButtonClickHandler = InitializeHelperButtonCommand
            (
                new BrowseToAssemblyPathCommand(this)
            );
            btnAddClickHandler = InitializeHButtonCommand
            (
                _loadAssemblyPathsCommandFactory.GetAddAssemblyPathListBoxItemCommand(this)
            );
            btnUpdateClickHandler = InitializeHButtonCommand
            (
                _loadAssemblyPathsCommandFactory.GetUpdateAssemblyPathListBoxItemCommand(this)
            );

            managedListBoxControl.CreateCommands(radListBoxManager);
            AddClickCommands();
        }

        private static EventHandler InitializeHButtonCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private static EventHandler<EventArgs> InitializeHelperButtonCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void RemoveClickCommands()
        {
            txtPath.ButtonClick -= txtPathButtonClickHandler;
            btnAdd.Click -= btnAddClickHandler;
            btnUpdate.Click -= btnUpdateClickHandler;
        }

        #region Event Handlers
        private void ConfigureLoadAssemblyPathsControl_Disposed(object? sender, EventArgs e)
        {
            RemoveClickCommands();
        }
        #endregion Event Handlers
    }
}
