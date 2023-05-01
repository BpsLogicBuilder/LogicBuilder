using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Services.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision
{
    internal partial class EditDecisionForm : Telerik.WinControls.UI.RadForm, IEditDecisionForm, IListBoxHost<IDecisionFunctionListBoxItem>
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConfigurationService _configurationService;
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IDecisionFunctionListBoxItemFactory _decisionFunctionListBoxItemFactory;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEditDecisionFormCommandFactory _editDecisionFormCommandFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IEditConditionFunctionControl editConditionFunctionControl;
        private ApplicationTypeInfo _application;
        private readonly ObjectRichTextBox _objectRichTextBox;
        private readonly IRadListBoxManager<IDecisionFunctionListBoxItem> radListBoxManager;

        public EditDecisionForm(
            IConfigurationService configurationService,
            IDialogFormMessageControl dialogFormMessageControl,
            IDecisionDataParser decisionDataParser,
            IDecisionFunctionListBoxItemFactory decisionFunctionListBoxItemFactory,
            IEditDecisionFormCommandFactory editDecisionFormCommandFactory,
            IEditingControlFactory editingControlFactory,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IFunctionDataParser functionDataParser,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            ObjectRichTextBox objectRichTextBox,
            XmlDocument? decisionXmlDocument)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _decisionDataParser = decisionDataParser;
            _decisionFunctionListBoxItemFactory = decisionFunctionListBoxItemFactory;
            _editDecisionFormCommandFactory = editDecisionFormCommandFactory;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _functionDataParser = functionDataParser;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _objectRichTextBox = objectRichTextBox;

            UpdateDecisionFunctionsList(decisionXmlDocument?.DocumentElement?.OuterXml);

            radListBoxManager = new RadListBoxManager<IDecisionFunctionListBoxItem>(this);
            this.editConditionFunctionControl = editingControlFactory.GetEditConditionFunctionControl(this);
            Initialize();
        }

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public RadButton BtnAdd => btnAdd;
        public RadButton BtnUpdate => btnUpdate;
        public RadButton BtnCancel => managedListBoxControl.BtnCancel;
        public RadButton BtnCopy => managedListBoxControl.BtnCopy;
        public RadButton BtnEdit => managedListBoxControl.BtnEdit;
        public RadButton BtnRemove => managedListBoxControl.BtnRemove;
        public RadButton BtnUp => managedListBoxControl.BtnUp;
        public RadButton BtnDown => managedListBoxControl.BtnDown;
        public RadListControl ListBox => managedListBoxControl.ListBox;

        public IEditingControl? CurrentEditingControl => editConditionFunctionControl.CurrentEditingControl;

        public IEditConditionFunctionControl EditConditionFunctionControl => editConditionFunctionControl;

        public IRadListBoxManager<IDecisionFunctionListBoxItem> RadListBoxManager => radListBoxManager;

        public string DecisionXml
        {
            get
            {
                if (ListBox.Items.Count == 0)
                    return string.Empty;

                return _refreshVisibleTextHelper.RefreshAllVisibleTexts//include <function /> and <decision />
                (
                    _xmlDocumentHelpers.ToXmlElement(GetDecisionXml()),
                    Application
                ).OuterXml;

                string GetDecisionXml()
                {
                    if (ListBox.Items.Count < 2)
                    {
                        return chkNot.Checked
                            ? _xmlDataHelper.BuildNotXml(_xmlDataHelper.BuildDecisionXml(string.Empty, string.Empty, GetItemsFragment()))
                            : _xmlDataHelper.BuildDecisionXml(GetVariableName(), string.Empty, GetItemsFragment());
                    }

                    string xml = rdoOr.IsChecked
                            ? _xmlDataHelper.BuildDecisionXml(GetVariableName(), string.Empty, _xmlDataHelper.BuildOrXml(GetItemsFragment()))
                            : _xmlDataHelper.BuildDecisionXml(GetVariableName(), string.Empty, _xmlDataHelper.BuildAndXml(GetItemsFragment()));

                    return chkNot.Checked ? _xmlDataHelper.BuildNotXml(xml) : xml;
                }

                string GetVariableName()//variableName is no longer used for decisions.  Using the first variable name to allow validate and build using earlier versions. 
                    => _configurationService.VariableList.Variables.Any() 
                        ? _configurationService.VariableList.Variables.First().Key 
                        : string.Empty;

                string GetItemsFragment()
                {
                    StringBuilder stringBuilder = new();
                    using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
                    {
                        foreach (string innerXml in ListBox.Items.Select(i => ((IDecisionFunctionListBoxItem)i.Value).HiddenText))
                        {
                            xmlTextWriter.WriteRaw(innerXml);
                        }
                        xmlTextWriter.Flush();
                    }

                    return stringBuilder.ToString();
                }
            }
        }

        public string DecisionVisibleText 
            => _decisionDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(DecisionXml)).VisibleText;

        public ApplicationTypeInfo Application => _application;

        public void ClearInputControls()
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            _objectRichTextBox.Text = string.Empty;
            editConditionFunctionControl.ClearInputControls();
            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);
        }

        public void ClearMessage() => _dialogFormMessageControl.ClearMessage();

        public void DisableControlsDuringEdit(bool disable)
        {
            btnOk.Enabled = !disable;
            btnPasteXml.Enabled = !disable;
        }

        public void SetErrorMessage(string message) => _dialogFormMessageControl.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => _dialogFormMessageControl.SetMessage(message, title);

        public void UpdateDecisionFunctionsList(string? xmlString)
        {
            rdoAnd.IsChecked = true;
            ListBox.Items.Clear();

            if (xmlString == null)
                return;

            DecisionData decisionData = _decisionDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(xmlString));
            if (decisionData.FunctionElements.Count == 0)
                throw _exceptionHelper.CriticalException("{E4308ECB-6BF6-47F4-A61B-F1D821C2F2D7}");

            IList<IDecisionFunctionListBoxItem> listBoxItems = GetListBoxItems();
            IList<string> errors = GetErrors();
            if (errors.Count > 0)
                SetErrorMessage(string.Join(Environment.NewLine, errors));

            //Allow the items with errors to be edited where possible so do not filter them out.
            ListBox.Items.AddRange
            (
                listBoxItems.Select(item => new RadListDataItem(item.VisibleText, item))
            );

            if (decisionData.FirstChildElementName == XmlDataConstants.ORELEMENT)
                rdoOr.IsChecked = true;
            chkNot.Checked = decisionData.IsNotDecision;

            IList<IDecisionFunctionListBoxItem> GetListBoxItems()
                => decisionData.FunctionElements.Select
                (
                    e => _decisionFunctionListBoxItemFactory.GetDecisionFunctionListBoxItem
                    (
                        _functionDataParser.Parse(e).VisibleText,
                        e.OuterXml,
                        this
                    )
                ).ToArray();

            IList<string> GetErrors()
            {
                List<string> errors = new();
                foreach (var item in listBoxItems)
                    errors.AddRange(item.Errors);
                return errors;
            }
        }

        public void UpdateInputControls(IDecisionFunctionListBoxItem item)
        {
            editConditionFunctionControl.UpdateInputControls(item.HiddenText);

            if (CurrentEditingControl?.IsValid == true)
            {
                _objectRichTextBox.Text = editConditionFunctionControl.VisibleText;
            }
        }

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            ControlsLayoutUtility.CollapsePanelBorder(radScrollablePanelList);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelEdit);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelAddButton);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelFill);

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            editConditionFunctionControl.Changed += EditConditionFunctionControl_Changed;
            radListBoxManager.ListChanged += RadListBoxManager_ListChanged;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            if (ListBox.Items.Count == 0)
                rdoAnd.IsChecked = true;

            _formInitializer.SetToEditSize(this);

            LayoutGroupBoxList();
            ControlsLayoutUtility.LayoutAddUpdateButtonPanel(radPanelAddButton, tableLayoutPanelAddUpdate);

            InitializeEditConditionFunctionControl();
            InitializeEditControl();

            AddButtonClickCommand(btnCopyXml, _editDecisionFormCommandFactory.GetEditDecisionFormCopyXmlCommand(this));
            AddButtonClickCommand(btnPasteXml, _editDecisionFormCommandFactory.GetEditDecisionFormEditXmlCommand(this));

            AddButtonClickCommand(btnAdd, _editDecisionFormCommandFactory.GetAddDecisionFunctionListBoxItemCommand(this));
            AddButtonClickCommand(btnUpdate, _editDecisionFormCommandFactory.GetUpdateDecisionFunctionListBoxItemCommand(this));
            managedListBoxControl.CreateCommands(radListBoxManager);

            ValidateOk();
        }

        private void InitializeApplicationDropDownList()
        {
            ControlsLayoutUtility.LayoutApplicationGroupBox(this, radPanelApplication, radGroupBoxApplication, _applicationDropDownList);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void InitializeEditConditionFunctionControl()
        {
            ((ISupportInitialize)radPanelFill).BeginInit();
            radPanelFill.SuspendLayout();
            ((ISupportInitialize)this).BeginInit();
            this.SuspendLayout();

            Control control = (Control)editConditionFunctionControl;
            control.Dock = DockStyle.Fill;
            radPanelFill.Controls.Add(control);

            ((ISupportInitialize)radPanelFill).EndInit();
            radPanelFill.ResumeLayout(false);
            ((ISupportInitialize)this).EndInit();
            this.ResumeLayout(true);
        }

        private void InitializeEditControl()
        {
            _objectRichTextBox.Name = "objectRichTextBox";
            _objectRichTextBox.Dock = DockStyle.Fill;
            _objectRichTextBox.BorderStyle = BorderStyle.None;
            _objectRichTextBox.Margin = new Padding(0);
            _objectRichTextBox.Location = new Point(0, 0);
            _objectRichTextBox.DetectUrls = false;
            _objectRichTextBox.HideSelection = false;
            _objectRichTextBox.Multiline = false;
            _objectRichTextBox.ReadOnly = true;

            ControlsLayoutUtility.LayoutListItemGroupBox
            (
                this,
                radGroupBoxEdit,
                radPanelEdit,
                radPanelEditControl,
                _objectRichTextBox,
                multiLine: false
            );
        }

        private void LayoutGroupBoxList()
        {
            ((ISupportInitialize)radGroupBoxList).BeginInit();
            radGroupBoxList.SuspendLayout();
            ((ISupportInitialize)radScrollablePanelList).BeginInit();
            radScrollablePanelList.PanelContainer.SuspendLayout();
            radScrollablePanelList.SuspendLayout();
            ((ISupportInitialize)radPanelRadioButtons).BeginInit();
            radPanelRadioButtons.SuspendLayout();

            this.SuspendLayout();

            managedListBoxControl.Margin = new Padding(0);
            radScrollablePanelList.Margin = new Padding(0);
            radPanelRadioButtons.Size = new Size(PerFontSizeConstants.ConditionsRadioButtonPanelWidth, radPanelRadioButtons.Height);
            ControlsLayoutUtility.SetLabelMargin(rdoAnd);
            ControlsLayoutUtility.SetLabelMargin(rdoOr);
            ControlsLayoutUtility.SetLabelMargin(chkNot);
            radGroupBoxList.Margin = new Padding(0);
            radGroupBoxList.Padding = PerFontSizeConstants.GroupBoxPadding;
            radGroupBoxList.Size = new Size
            (
                radGroupBoxList.Width,
                (int)PerFontSizeConstants.BottomPanelHeight
                    + radGroupBoxList.Padding.Top
                    + radGroupBoxList.Padding.Bottom
            );

            ((ISupportInitialize)radPanelRadioButtons).EndInit();
            radPanelRadioButtons.ResumeLayout(false);
            ((ISupportInitialize)radGroupBoxList).EndInit();
            radGroupBoxList.ResumeLayout(false);
            radScrollablePanelList.PanelContainer.ResumeLayout(false);
            ((ISupportInitialize)radScrollablePanelList).EndInit();
            radScrollablePanelList.ResumeLayout(false);
            this.ResumeLayout(true);
        }

        private void ValidateOk()
        {
            bool enabled = ListBox.Items.Count > 0;
            btnOk.Enabled = enabled;
            btnCopyXml.Enabled = enabled;
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void EditConditionFunctionControl_Changed(object? sender, EventArgs e)
        {
            if (CurrentEditingControl == null)
                return;

            _objectRichTextBox.Text = CurrentEditingControl.VisibleText;
        }

        private void RadListBoxManager_ListChanged(object? sender, EventArgs e) => ValidateOk();
        #endregion Event Handlers
    }
}
