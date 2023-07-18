using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction
{
    internal partial class SelectFunctionControl : UserControl, ISelectFunctionControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IImageListService _imageListService;
        private readonly ISelectFunctionViewControlFactory _selectFunctionControlFactory;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        private readonly ISelectFunctionForm selectFunctionForm;
        private readonly CommandBarToggleButton[] toggleButtons;
        private readonly Type assignedTo;

        public event EventHandler? Changed;

        private enum ViewType : short
        {
            List,
            Tree,
            Dropdown
        }

        public SelectFunctionControl(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IImageListService imageListService,
            ISelectFunctionViewControlFactory selectFunctionControlFactory,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            ISelectFunctionForm selectFunctionForm,
            Type assignedTo)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _imageListService = imageListService;
            _selectFunctionControlFactory = selectFunctionControlFactory;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            toggleButtons = new CommandBarToggleButton[]
            {
                commandBarToggleButtonDropDown,
                commandBarToggleButtonList,
                commandBarToggleButtonTreeView
            };
            this.selectFunctionForm = selectFunctionForm;
            this.assignedTo = assignedTo;
            Initialize();
        }

        private ISelectFunctionViewControl CurrentViewControl
        {
            get
            {
                if (radPanelFunctionView.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{8F6BE813-97DF-4EEF-8203-0F7C52B3D04C}");

                return (ISelectFunctionViewControl)radPanelFunctionView.Controls[0];
            }
        }

        private ApplicationTypeInfo Application => selectFunctionForm.Application;

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public string? FunctionName => CurrentViewControl.FunctionName;

        public bool IsValid => ValidateSelectedFunction().Count == 0;

        public bool ItemSelected => CurrentViewControl.ItemSelected;

        public IList<TreeFolder> TreeFolders => selectFunctionForm.TreeFolders;

        public IDictionary<string, Function> FunctionDictionary => selectFunctionForm.FunctionDictionary;

        public void ClearMessage() => selectFunctionForm.ClearMessage();

        public void SetErrorMessage(string message) => selectFunctionForm.SetErrorMessage(message);

        public void SetFunction(string functionName)
        {
            CurrentViewControl.SelectFunction(functionName);
        }

        public void SetMessage(string message, string title = "") => selectFunctionForm.SetMessage(message, title);

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
            string? currentFunction = CurrentViewControl.ItemSelected
                                            ? CurrentViewControl.FunctionName
                                            : null;
            Navigate(view);
            if (currentFunction != null)
                CurrentViewControl.SelectFunction(currentFunction);
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
            IList<string> errors = ValidateSelectedFunction();
            if (errors.Count > 0)
                SetErrorMessage(string.Join(Environment.NewLine, errors));
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void Initialize()
        {
            Disposed += SelectFunctionControl_Disposed;
            commandBarStripElement1.Grip.Visibility = ElementVisibility.Collapsed;
            commandBarStripElement1.OverflowButton.Visibility = ElementVisibility.Collapsed;
            commandBarStripElement1.BorderWidth = 0;
            radCommandBar1.ImageList = _imageListService.ImageList;
            commandBarToggleButtonDropDown.ImageIndex = ImageIndexes.EDITIMAGEINDEX;
            commandBarToggleButtonList.ImageIndex = ImageIndexes.SORTIMAGEINDEX;
            commandBarToggleButtonTreeView.ImageIndex = ImageIndexes.TREEVIEWIMAGEINDEX;
            commandBarToggleButtonDropDown.Tag = ViewType.Dropdown;
            commandBarToggleButtonList.Tag = ViewType.List;
            commandBarToggleButtonTreeView.Tag = ViewType.Tree;

            SetToggleStateOn(commandBarToggleButtonList);
            Navigate(ViewType.List);
        }

        private void Navigate(Control newControl)
        {
            NavigationUtility.Navigate(this.Handle, radPanelFunctionView, newControl);
        }

        private void Navigate(ViewType view)
        {
            ISelectFunctionViewControl selectFunctionViewControl = GetControl();
            selectFunctionViewControl.Changed += SelectFunctionViewControl_Changed;
            Navigate((Control)selectFunctionViewControl);

            ISelectFunctionViewControl GetControl()
            {
                return view switch
                {
                    ViewType.Dropdown => _selectFunctionControlFactory.GetSelectFunctionDropdownViewControl(this),
                    ViewType.List => _selectFunctionControlFactory.GetSelectFunctionListViewControl(this),
                    ViewType.Tree => _selectFunctionControlFactory.GetSelectFunctionTreeViewControl(this),
                    _ => throw _exceptionHelper.CriticalException("{3D4C0E48-C437-4856-8616-A62C9C5C0B50}"),
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

        private IList<string> ValidateSelectedFunction()
        {
            List<string> errors = new();
            if (!ItemSelected)
            {
                errors.Add(Strings.validFunctionMustBeSelected);
                return errors;
            }

            if (!_configurationService.FunctionList.Functions.TryGetValue(this.FunctionName!, out Function? function))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, this.FunctionName));
                return errors;
            }

            if (function.ReturnType is GenericReturnType || function.ReturnType is ListOfGenericsReturnType)
                return errors;

            if (!_typeLoadHelper.TryGetSystemType(function.ReturnType, Array.Empty<GenericConfigBase>(), Application, out Type? functionType))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, function.TypeName));
                return errors;
            }

            if (!_typeHelper.AssignableFrom(assignedTo, functionType))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.typeNotAssignableFormat, functionType.ToString(), assignedTo.ToString()));
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
                throw _exceptionHelper.CriticalException("{8FE64CCF-6786-488F-AC16-269BA3AC6AD6}");
            if (sender is not CommandBarToggleButton commandBarToggleButton)
                throw _exceptionHelper.CriticalException("{A4D61C6F-031C-4855-8683-889CFCCD2D57}");

            SetOtherToggleStatesOff(commandBarToggleButton);
            ChangeView((ViewType)commandBarToggleButton.Tag);
        }

        private void SelectFunctionControl_Disposed(object? sender, EventArgs e)
        {
            radCommandBar1.ImageList = null;
            RemoveChangeEvents();
        }

        private void SelectFunctionViewControl_Changed(object? sender, EventArgs e) => CheckForValidSelection();
        #endregion Event Handlers
    }
}
