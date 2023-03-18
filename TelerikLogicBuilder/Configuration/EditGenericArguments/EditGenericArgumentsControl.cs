using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Services.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments
{
    internal partial class EditGenericArgumentsControl : UserControl, IListBoxHost<GenericArgumentName>, IEditGenericArgumentsControl
    {
        private readonly IEditGenericArgumentsCommandFactory _editGenericArgumentsCommandFactory;

        private readonly IEditGenericArgumentsForm editGenericArgumentsForm;
        private readonly IRadListBoxManager<GenericArgumentName> radListBoxManager;

        public EditGenericArgumentsControl(
            IEditGenericArgumentsCommandFactory editGenericArgumentsCommandFactory,
            IEditGenericArgumentsForm editGenericArgumentsForm)
        {
            InitializeComponent();
            _editGenericArgumentsCommandFactory = editGenericArgumentsCommandFactory;
            this.editGenericArgumentsForm = editGenericArgumentsForm;
            radListBoxManager = new RadListBoxManager<GenericArgumentName>(this);
            Initialize();
        }

        public RadButton BtnAdd => btnAdd;

        public RadButton BtnUpdate => btnUpdate;

        public RadButton BtnCancel => managedListBoxControl.BtnCancel;

        public RadButton BtnCopy => managedListBoxControl.BtnCopy;

        public RadButton BtnEdit => managedListBoxControl.BtnEdit;

        public RadButton BtnRemove => managedListBoxControl.BtnRemove;

        public RadButton BtnUp => managedListBoxControl.BtnUp;

        public RadButton BtnDown => managedListBoxControl.BtnDown;

        public RadListControl ListBox => managedListBoxControl.ListBox;

        public IRadListBoxManager<GenericArgumentName> RadListBoxManager => radListBoxManager;

        public RadTextBox TxtArgument => txtArgument;

        public void ClearInputControls()
            => txtArgument.Text = string.Empty;

        public void ClearMessage() => editGenericArgumentsForm.ClearMessage();

        public void DisableControlsDuringEdit(bool disable) => editGenericArgumentsForm.DisableControlsDuringEdit(disable);

        public IList<string> GetArguments()
            => ListBox.Items
                    .Select(i => ((GenericArgumentName)i.Value).Item)
                    .ToArray();

        public void SetArguments(IList<string> arguments)
        {
            ListBox.Items.AddRange
            (
                arguments
                    .Select(p => new GenericArgumentName(p.Trim()))
                    .Select(arg => new RadListDataItem(arg.ToString(), arg))
            );
        }

        public void SetErrorMessage(string message) => editGenericArgumentsForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => editGenericArgumentsForm.SetMessage(message, title);

        public void UpdateInputControls(GenericArgumentName item) => txtArgument.Text = item.Item;

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            ControlsLayoutUtility.LayoutAddUpdateItemGroupBox(this, radGroupBoxEditArgument);
            ControlsLayoutUtility.LayoutGroupBox(this, radGroupBoxArguments);
            ControlsLayoutUtility.LayoutAddUpdateButtonPanel(radPanelAddButton, tableLayoutPanelAddUpdate);
            CollapsePanelBorder(radPanelTxtArgument);
            CollapsePanelBorder(radPanelAddButton);

            InitializeHButtonCommand
            (
                BtnAdd,
                _editGenericArgumentsCommandFactory.GetAddGenericArgumentCommand(this)
            );
            InitializeHButtonCommand
            (
                BtnUpdate,
                _editGenericArgumentsCommandFactory.GetUpdateGenericArgumentCommand(this)
            );

            managedListBoxControl.CreateCommands(radListBoxManager);
        }

        private static void InitializeHButtonCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }
    }
}
