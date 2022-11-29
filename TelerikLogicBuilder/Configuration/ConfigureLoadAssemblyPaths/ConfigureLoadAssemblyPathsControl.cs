using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Services.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
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

        public void DisableControlsDuringEdit(bool disable) { }

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

        private static void CollapsePanelBorder(RadPanel radPanel) 
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            CollapsePanelBorder(radPanelTxtPath);
            CollapsePanelBorder(radPanelAddButton);

            InitializeHelperButtonCommand
            (
                txtPath,
                new BrowseToAssemblyPathCommand(this)
            );
            InitializeHButtonCommand
            (
                BtnAdd,
                _loadAssemblyPathsCommandFactory.GetAddAssemblyPathListBoxItemCommand(this)
            );
            InitializeHButtonCommand
            (
                BtnUpdate,
                _loadAssemblyPathsCommandFactory.GetUpdateAssemblyPathListBoxItemCommand(this)
            );

            managedListBoxControl.CreateCommands(radListBoxManager);
        }

        private static void InitializeHButtonCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

        private static void InitializeHelperButtonCommand(HelperButtonTextBox helperButtonTextBox, IClickCommand command)
        {
            helperButtonTextBox.ButtonClick += (sender, args) => command.Execute();
        }
    }
}
