using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands.LoadAssemblyPaths;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ListBox.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Services.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls
{
    internal partial class LoadAssemblyPathsControl : UserControl, IListBoxHost<AssemblyPath>, ILoadAssemblyPathsControl
    {
        private readonly ILoadAssemblyPathsCommandFactory _loadAssemblyPathsCommandFactory;
        private readonly IConfigureLoadAssemblyPaths _configureLoadAssemblyPaths;
        private readonly IRadListBoxManager<AssemblyPath> radListBoxManager;

        public RadButton BtnAdd => btnAdd;

        public RadButton BtnUpdate => managedListBoxControl.BtnUpdate;

        public RadButton BtnCancel => managedListBoxControl.BtnCancel;

        public RadButton BtnCopy => managedListBoxControl.BtnCopy;

        public RadButton BtnEdit => managedListBoxControl.BtnEdit;

        public RadButton BtnRemove => managedListBoxControl.BtnRemove;

        public RadButton BtnUp => managedListBoxControl.BtnUp;

        public RadButton BtnDown => managedListBoxControl.BtnDown;

        public RadListControl ListBox => managedListBoxControl.ListBox;

        public IRadListBoxManager<AssemblyPath> RadListBoxManager => radListBoxManager;

        public HelperButtonTextBox TxtPath => txtPath;

        public LoadAssemblyPathsControl(
            ILoadAssemblyPathsCommandFactory loadAssemblyPathsCommandFactory,
            IConfigureLoadAssemblyPaths configureLoadAssemblyPaths)
        {
            InitializeComponent();
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
            => ListBox.Enabled = !disable;

        public void SetErrorMessage(string message) 
            => _configureLoadAssemblyPaths.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") 
            => _configureLoadAssemblyPaths.SetMessage(message, title);

        public void UpdateInputControls(AssemblyPath item) 
            => txtPath.Text = item.Path;

        private static void CollapsePanelBorder(RadPanel radPanel) 
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            txtPath.Anchor = AnchorConstants.AnchorsLeftTopRight;
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
            InitializeHButtonCommand(BtnCancel, new ListBoxManagerCancelCommand(radListBoxManager));
            InitializeHButtonCommand(BtnCopy, new ListBoxManagerCopyCommand(radListBoxManager));
            InitializeHButtonCommand(BtnEdit, new ListBoxManagerEditCommand(radListBoxManager));
            InitializeHButtonCommand(BtnRemove, new ListBoxManagerRemoveCommand(radListBoxManager));
            InitializeHButtonCommand(BtnUp, new ListBoxManagerMoveUpCommand(radListBoxManager));
            InitializeHButtonCommand(BtnDown, new ListBoxManagerMoveDownCommand(radListBoxManager));
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
