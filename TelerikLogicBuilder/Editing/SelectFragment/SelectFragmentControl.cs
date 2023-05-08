using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment.Factories;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment
{
    internal partial class SelectFragmentControl : UserControl, ISelectFragmentControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IImageListService _imageListService;
        private readonly ISelectFragmentViewControlFactory _selectFragmentControlFactory;

        private readonly ISelectFragmentForm selectFragmentForm;
        private readonly CommandBarToggleButton[] toggleButtons;

        public event EventHandler? Changed;

        private enum ViewType : short
        {
            List,
            Tree,
            Dropdown
        }

        public SelectFragmentControl(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IImageListService imageListService,
            ISelectFragmentViewControlFactory selectFragmentControlFactory,
            ISelectFragmentForm selectFragmentFrom)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _imageListService = imageListService;
            _selectFragmentControlFactory = selectFragmentControlFactory;
            toggleButtons = new CommandBarToggleButton[]
            {
                commandBarToggleButtonDropDown,
                commandBarToggleButtonList,
                commandBarToggleButtonTreeView
            };
            this.selectFragmentForm = selectFragmentFrom;
            Initialize();
        }

        private ISelectFragmentViewControl CurrentViewControl
        {
            get
            {
                if (radPanelFragmentView.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{870A9DBB-2906-4E9A-8DE9-6A574771C3B1}");

                return (ISelectFragmentViewControl)radPanelFragmentView.Controls[0];
            }
        }

        public string? FragmentName => CurrentViewControl.FragmentName;

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public bool IsValid => ValidateSelectedFragment().Count == 0;

        public bool ItemSelected => CurrentViewControl.ItemSelected;

        public void ClearMessage() => selectFragmentForm.ClearMessage();

        public void SetFragment(string fragmentName)
        {
            CurrentViewControl.SelectFragment(fragmentName);
        }

        public void SetErrorMessage(string message) => selectFragmentForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => selectFragmentForm.SetMessage(message, title);

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
            string? currentFragment = CurrentViewControl.ItemSelected
                                            ? CurrentViewControl.FragmentName
                                            : null;
            Navigate(view);
            if (currentFragment != null)
            {
                CurrentViewControl.SelectFragment(currentFragment);
                DisplayFragmentDescription(currentFragment);
            }
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
            IList<string> errors = ValidateSelectedFragment();
            if (errors.Count > 0)
                SetErrorMessage(string.Join(Environment.NewLine, errors));
            else
                DisplayFragmentDescription(this.FragmentName!);

            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void DisplayFragmentDescription(string fragmentName)
        {
            if (!_configurationService.FragmentList.Fragments.TryGetValue(fragmentName, out Fragment? fragment))
                throw _exceptionHelper.CriticalException("{5B0F7919-2079-4AD2-AC3C-4CCCB9C8F2E1}");

            SetMessage(fragment.Description);
        }

        private void Initialize()
        {
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
            if (ItemSelected)
            {
                DisplayFragmentDescription(this.FragmentName!);
            }
        }

        private void Navigate(Control newControl)
        {
            NavigationUtility.Navigate(this.Handle, radPanelFragmentView, newControl);
        }

        private void Navigate(ViewType view)
        {
            ISelectFragmentViewControl selectFragmentViewControl = GetControl();
            selectFragmentViewControl.Changed += SelectFragmentViewControl_Changed;
            Navigate((Control)selectFragmentViewControl);

            ISelectFragmentViewControl GetControl()
            {
                return view switch
                {
                    ViewType.Dropdown => _selectFragmentControlFactory.GetSelectFragmentDropdownViewControl(this),
                    ViewType.List => _selectFragmentControlFactory.GetSelectFragmentListViewControl(this),
                    ViewType.Tree => _selectFragmentControlFactory.GetSelectFragmentTreeViewControl(this),
                    _ => throw _exceptionHelper.CriticalException("{202114F3-2EE1-4710-A3EB-ABF1D3B05997}"),
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

        private IList<string> ValidateSelectedFragment()
        {
            List<string> errors = new();
            if (!ItemSelected)
            {
                errors.Add(Strings.validFragmentMustBeSelected);
                return errors;
            }

            if (!_configurationService.FragmentList.Fragments.TryGetValue(this.FragmentName!, out Fragment? _))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.fragmentNotConfiguredFormat, this.FragmentName));
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
                throw _exceptionHelper.CriticalException("{17783DA3-946D-43E8-A0D3-F407451E6BDF}");
            if (sender is not CommandBarToggleButton commandBarToggleButton)
                throw _exceptionHelper.CriticalException("{C14A6B00-C2A1-4FF9-A047-56C6ABFBE7A8}");

            SetOtherToggleStatesOff(commandBarToggleButton);
            ChangeView((ViewType)commandBarToggleButton.Tag);
        }

        private void SelectFragmentViewControl_Changed(object? sender, EventArgs e) => CheckForValidSelection();
        #endregion Event Handlers
    }
}
