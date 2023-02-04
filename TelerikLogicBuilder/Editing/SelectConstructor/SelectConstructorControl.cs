using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor
{
    internal partial class SelectConstructorControl : UserControl, ISelectConstructorControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IImageListService _imageListService;
        private readonly ISelectConstructorViewControlFactory _selectConstructorControlFactory;
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

        public SelectConstructorControl(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IImageListService imageListService,
            ISelectConstructorViewControlFactory selectConstructorControlFactory,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IEditingForm editingForm,
            Type assignedTo)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _imageListService = imageListService;
            _selectConstructorControlFactory = selectConstructorControlFactory;
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

        private ISelectConstructorViewControl CurrentViewControl
        {
            get
            {
                if (radPanelConstructorView.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{96A1BD22-3C26-4E02-A4BA-2A4317FDD0CE}");

                return (ISelectConstructorViewControl)radPanelConstructorView.Controls[0];
            }
        }

        private ApplicationTypeInfo Application => editingForm.Application;

        public string? ConstructorName => CurrentViewControl.ConstructorName;

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public bool IsValid => ValidateSelectedConstructor().Count == 0;

        public bool ItemSelected => CurrentViewControl.ItemSelected;

        public void ClearMessage() => editingForm.ClearMessage();

        public void RequestDocumentUpdate() => editingForm.RequestDocumentUpdate();

        public void SetErrorMessage(string message) => editingForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => editingForm.SetMessage(message, title);

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
            string? currentConstructor = CurrentViewControl.ItemSelected
                                            ? CurrentViewControl.ConstructorName
                                            : null;
            Navigate(view);
            if (currentConstructor != null)
                CurrentViewControl.SelectConstructor(currentConstructor);
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
            IList<string> errors = ValidateSelectedConstructor();
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
            ((ISupportInitialize)radPanelConstructorView).BeginInit();
            radPanelConstructorView.SuspendLayout();

            ClearFieldControls();
            newControl.Dock = DockStyle.Fill;
            newControl.Location = new Point(0, 0);
            radPanelConstructorView.Controls.Add(newControl);

            ((ISupportInitialize)radPanelConstructorView).EndInit();
            radPanelConstructorView.ResumeLayout(true);

            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);

            void ClearFieldControls()
            {
                foreach (Control control in radPanelConstructorView.Controls)
                    control.Visible = false;

                radPanelConstructorView.Controls.Clear();
            }
        }

        private void Navigate(ViewType view)
        {
            ISelectConstructorViewControl selectConstructorViewControl = GetControl();
            selectConstructorViewControl.Changed += SelectConstructorViewControl_Changed;
            Navigate((Control)selectConstructorViewControl);

            ISelectConstructorViewControl GetControl()
            {
                return view switch
                {
                    ViewType.Dropdown => _selectConstructorControlFactory.GetSelectConstructorDropdownViewControl(this),
                    ViewType.List => _selectConstructorControlFactory.GetSelectConstructorListViewControl(this),
                    ViewType.Tree => _selectConstructorControlFactory.GetSelectConstructorTreeViewControl(this),
                    _ => throw _exceptionHelper.CriticalException("{00109A96-18FD-437B-9E47-D7D65DC260EE}"),
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

        private IList<string> ValidateSelectedConstructor()
        {
            List<string> errors = new();
            if (!ItemSelected)
            {
                errors.Add(Strings.validConstructorMustBeSelected);
                return errors;
            }

            if (!_configurationService.ConstructorList.Constructors.TryGetValue(this.ConstructorName!, out Constructor? constructor))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.constructorNotConfiguredFormat, this.ConstructorName));
                return errors;
            }

            if (!_typeLoadHelper.TryGetSystemType(constructor.TypeName, Application, out Type? constructorType))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, constructor.TypeName));
                return errors;
            }

            if (!_typeHelper.AssignableFrom(assignedTo, constructorType))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.typeNotAssignableFormat, constructorType.ToString(), assignedTo.ToString()));
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
                throw _exceptionHelper.CriticalException("{EDD2D66A-A269-4961-AA64-25490F2E5296}");
            if (sender is not CommandBarToggleButton commandBarToggleButton)
                throw _exceptionHelper.CriticalException("{35F76E0A-ECE2-4DF0-910E-07B027426C4F}");

            SetOtherToggleStatesOff(commandBarToggleButton);
            ChangeView((ViewType)commandBarToggleButton.Tag);
        }

        private void SelectConstructorViewControl_Changed(object? sender, EventArgs e) => CheckForValidSelection();
        #endregion Event Handlers
    }
}
