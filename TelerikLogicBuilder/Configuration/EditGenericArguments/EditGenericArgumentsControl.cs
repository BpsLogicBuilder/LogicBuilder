using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Services.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
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
        private EventHandler btnAddClickHandler;
        private EventHandler btnUpdateClickHandler;

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

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnAdd.Click += btnAddClickHandler;
            btnUpdate.Click += btnUpdateClickHandler;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(btnAddClickHandler),
        nameof(btnUpdateClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            ControlsLayoutUtility.LayoutAddUpdateItemGroupBox(this, radGroupBoxEditArgument);
            ControlsLayoutUtility.LayoutGroupBox(this, radGroupBoxArguments);
            ControlsLayoutUtility.LayoutAddUpdateButtonPanel(radPanelAddButton, tableLayoutPanelAddUpdate);
            CollapsePanelBorder(radPanelTxtArgument);
            CollapsePanelBorder(radPanelAddButton);

            Disposed += EditGenericArgumentsControl_Disposed;

            btnAddClickHandler = InitializeHButtonCommand
            (
                _editGenericArgumentsCommandFactory.GetAddGenericArgumentCommand(this)
            );
            btnUpdateClickHandler = InitializeHButtonCommand
            (
                _editGenericArgumentsCommandFactory.GetUpdateGenericArgumentCommand(this)
            );

            managedListBoxControl.CreateCommands(radListBoxManager);
            AddClickCommands();
        }

        private static EventHandler InitializeHButtonCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void RemoveClickCommands()
        {
            btnAdd.Click -= btnAddClickHandler;
            btnUpdate.Click -= btnUpdateClickHandler;
        }

        #region Event Handlers
        private void EditGenericArgumentsControl_Disposed(object? sender, EventArgs e)
        {
            RemoveClickCommands();
        }
        #endregion Event Handlers
    }
}
