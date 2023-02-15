﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable
{
    internal partial class SelectVariableControl : UserControl, ISelectVariableControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IImageListService _imageListService;
        private readonly ISelectVariableViewControlFactory _selectVariableControlFactory;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        private readonly IEditingForm editingForm;
        private readonly CommandBarToggleButton[] toggleButtons;
        private readonly Type assignedTo;

        public event EventHandler? Changed;

        private enum ViewType : short
        {
            List,
            Tree,
            Dropdown
        }

        public SelectVariableControl(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper, 
            IImageListService imageListService,
            ISelectVariableViewControlFactory selectVariableControlFactory,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IEditingForm editingForm,
            Type assignedTo)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _imageListService = imageListService;
            _selectVariableControlFactory = selectVariableControlFactory;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            toggleButtons = new CommandBarToggleButton[]
            {
                commandBarToggleButtonDropDown,
                commandBarToggleButtonList,
                commandBarToggleButtonTreeView
            };
            this.editingForm = editingForm;
            this.assignedTo = assignedTo;
            Initialize();
        }

        private ISelectVariableViewControl CurrentViewControl
        {
            get
            {
                if (radPanelVariableView.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{1C0FDCBF-6D2A-4ADD-A394-7F5578D2144D}");

                return (ISelectVariableViewControl)radPanelVariableView.Controls[0];
            }
        }

        public ApplicationTypeInfo Application => editingForm.Application;

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public bool ItemSelected => CurrentViewControl.ItemSelected;

        public string? VariableName => CurrentViewControl.VariableName;

        public bool IsValid => ValidateSelectedVariable().Count == 0;

        public IEditingForm EditingForm => editingForm;

        public void ClearMessage() => editingForm.ClearMessage();

        public void RequestDocumentUpdate() => editingForm.RequestDocumentUpdate();

        public void SetErrorMessage(string message) => editingForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => editingForm.SetMessage(message, title);

        public void SetVariable(string variableName)
        {
            CurrentViewControl.SelectVariable(variableName);
        }

        private void AddChangeEvents()
        {
            foreach (CommandBarToggleButton toggle in toggleButtons)
            {
                toggle.ToggleStateChanged += CommandBarToggleButton_ToggleStateChanged;
                toggle.ToggleStateChanging += CommandBarToggleButton_ToggleStateChanging;
            }
        }

        private void ChangeView(ViewType view)
        {
            string? currentVariable = CurrentViewControl.ItemSelected
                                            ? CurrentViewControl.VariableName
                                            : null;
            Navigate(view);
            if (currentVariable != null)
                CurrentViewControl.SelectVariable(currentVariable);
            else
            {
                CheckForValidSelection();
                /*The value may be set to e.g. selectedIndex == 0 (in the case of a list control) before the changed event was attached.
                 * This call means the parent form has an opportunity to e.g. disable the Ok button.
                    Best not to use a method or property on IEditingForm because of multiple possible implementations.*/
            }
        }

        private void CheckForValidSelection()
        {
            ClearMessage();
            IList<string> errors = ValidateSelectedVariable();
            if (errors.Count > 0)
                SetErrorMessage(string.Join(Environment.NewLine, errors));
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Initialize()
        {
            commandBarStripElement1.Grip.Visibility = ElementVisibility.Collapsed;
            commandBarStripElement1.OverflowButton.Visibility = ElementVisibility.Collapsed;
            commandBarStripElement1.BorderWidth = 0;
            radCommandBar1.ImageList = _imageListService.ImageList;
            commandBarToggleButtonDropDown.ImageIndex = ImageIndexes.EDITIMAGEINDEX;
            commandBarToggleButtonList.ImageIndex = ImageIndexes.SORTIMAGEINDEX;
            commandBarToggleButtonTreeView.ImageIndex = ImageIndexes.TREEVIEWMAGEINDEX;
            commandBarToggleButtonDropDown.Tag = ViewType.Dropdown;
            commandBarToggleButtonList.Tag = ViewType.List;
            commandBarToggleButtonTreeView.Tag = ViewType.Tree;

            SetToggleStateOn(commandBarToggleButtonList);
            Navigate(ViewType.List);
        }

        private void Navigate(Control newControl)
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            ((ISupportInitialize)radPanelVariableView).BeginInit();
            radPanelVariableView.SuspendLayout();

            ClearFieldControls();
            newControl.Dock = DockStyle.Fill;
            newControl.Location = new Point(0, 0);
            radPanelVariableView.Controls.Add(newControl);

            ((ISupportInitialize)radPanelVariableView).EndInit();
            radPanelVariableView.ResumeLayout(true);

            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);

            void ClearFieldControls()
            {
                foreach (Control control in radPanelVariableView.Controls)
                    control.Visible = false;

                radPanelVariableView.Controls.Clear();
            }
        }

        private void Navigate(ViewType view)
        {
            ISelectVariableViewControl selectVariableViewControl = GetControl();
            selectVariableViewControl.Changed += SelectVariableViewControl_Changed;
            Navigate((Control)selectVariableViewControl);

            ISelectVariableViewControl GetControl()
            {
                return view switch
                {
                    ViewType.Dropdown => _selectVariableControlFactory.GetSelectVariableDropdownViewControl(this),
                    ViewType.List => _selectVariableControlFactory.GetSelectVariableListViewControl(this),
                    ViewType.Tree => _selectVariableControlFactory.GetSelectVariableTreeViewControl(this),
                    _ => throw _exceptionHelper.CriticalException("{C5B0C5E4-B033-4A8F-88B5-9C621FDD4C27}"),
                };
            }
        }

        private void RemoveChangeEvents()
        {
            foreach (CommandBarToggleButton toggle in toggleButtons)
            {
                toggle.ToggleStateChanged -= CommandBarToggleButton_ToggleStateChanged;
                toggle.ToggleStateChanging -= CommandBarToggleButton_ToggleStateChanging;
            }
        }

        private void SetOtherToggleStatesOff(CommandBarToggleButton toggleButton)
        {
            RemoveChangeEvents();
            foreach (CommandBarToggleButton toggle in toggleButtons)
            {
                if (!object.ReferenceEquals(toggleButton, toggle))
                    toggle.ToggleState = ToggleState.Off;
            }
            AddChangeEvents();
        }

        private void SetToggleStateOn(CommandBarToggleButton toggleButton)
        {
            RemoveChangeEvents();
            toggleButton.ToggleState = ToggleState.On;
            foreach (CommandBarToggleButton toggle in toggleButtons)
            {
                if (!object.ReferenceEquals(toggleButton, toggle))
                    toggle.ToggleState = ToggleState.Off;
            }
            AddChangeEvents();
        }

        private IList<string> ValidateSelectedVariable()
        {
            List<string> errors = new();
            if (!ItemSelected)
            {
                errors.Add(Strings.validVariableMustBeSelected);
                return errors;
            }

            if (!_configurationService.VariableList.Variables.TryGetValue(this.VariableName!, out VariableBase? variable))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.decisionNotConfiguredFormat2, this.VariableName));
                return errors;
            }

            if (!_typeLoadHelper.TryGetSystemType(variable, Application, out Type? variableType))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, variable.ObjectTypeString));
                return errors;
            }

            if (!_typeHelper.AssignableFrom(assignedTo, variableType))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.typeNotAssignableFormat, variableType.ToString(), assignedTo.ToString()));
                return errors;
            }

            return errors;
        }

        #region Event Handlers
        private void CommandBarToggleButton_ToggleStateChanging(object sender, StateChangingEventArgs args)
        {
            if (args.NewValue == ToggleState.Off)
                args.Cancel = true;
        }

        private void CommandBarToggleButton_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (args.ToggleState == ToggleState.Off)
                throw _exceptionHelper.CriticalException("{7001D968-43CE-4561-BF3C-D9099EA8CA86}");
            if (sender is not CommandBarToggleButton commandBarToggleButton)
                throw _exceptionHelper.CriticalException("{07C4DEBC-5629-4222-B371-A2F026548C77}");

            SetOtherToggleStatesOff(commandBarToggleButton);
            ChangeView((ViewType)commandBarToggleButton.Tag);
        }

        private void SelectVariableViewControl_Changed(object? sender, EventArgs e) => CheckForValidSelection();
        #endregion Event Handlers
    }
}