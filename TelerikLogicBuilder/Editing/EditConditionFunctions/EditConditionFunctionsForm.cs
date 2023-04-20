using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions
{
    internal partial class EditConditionFunctionsForm : Telerik.WinControls.UI.RadForm, IEditConditionFunctionsForm, IListBoxHost<IConditionFunctionListBoxItem>
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IConditionFunctionListBoxItemFactory _conditionFunctionListBoxItemFactory;
        private readonly IConditionsDataParser _conditionsDataParser;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEditConditionFunctionsFormCommandFactory _editConditionFunctionsFormCommandFactory;
        private readonly IFormInitializer _formInitializer;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IEditConditionFunctionControl editConditionFunctionControl;
        private ApplicationTypeInfo _application;
        private readonly ObjectRichTextBox _objectRichTextBox;
        private readonly IRadListBoxManager<IConditionFunctionListBoxItem> radListBoxManager;

        public EditConditionFunctionsForm(
            IDialogFormMessageControl dialogFormMessageControl,
            IConditionFunctionListBoxItemFactory conditionFunctionListBoxItemFactory,
            IConditionsDataParser conditionsDataParser,
            IEditConditionFunctionsFormCommandFactory editConditionFunctionsFormCommandFactory,
            IEditConditionFunctionsControlFactory editConditionFunctionsControlFactory,
            IFormInitializer formInitializer,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            ObjectRichTextBox objectRichTextBox,
            XmlDocument? conditionsXmlDocument)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _conditionsDataParser = conditionsDataParser;
            _editConditionFunctionsFormCommandFactory = editConditionFunctionsFormCommandFactory;
            _formInitializer = formInitializer;
            _conditionFunctionListBoxItemFactory = conditionFunctionListBoxItemFactory;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _objectRichTextBox = objectRichTextBox;

            UpdateConditionsList(conditionsXmlDocument?.DocumentElement?.OuterXml);

            radListBoxManager = new RadListBoxManager<IConditionFunctionListBoxItem>(this);
            this.editConditionFunctionControl = editConditionFunctionsControlFactory.GetEditConditionFunctionControl(this);
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

        public IRadListBoxManager<IConditionFunctionListBoxItem> RadListBoxManager => radListBoxManager;

        public string ShapeXml
        {
            get
            {
                return _refreshVisibleTextHelper.RefreshFunctionVisibleTexts
                (
                    _xmlDocumentHelpers.ToXmlElement(GetConditionsXml()),
                    Application
                ).OuterXml;

                string GetConditionsXml()
                {
                    if (ListBox.Items.Count < 2)
                        return _xmlDataHelper.BuildConditionsXml(GetItemsFragment());

                    return rdoOr.IsChecked
                        ? _xmlDataHelper.BuildConditionsXml(_xmlDataHelper.BuildOrXml(GetItemsFragment()))
                        : _xmlDataHelper.BuildConditionsXml(_xmlDataHelper.BuildAndXml(GetItemsFragment()));
                }

                string GetItemsFragment()
                {
                    if (ListBox.Items.Count == 0)
                        return string.Empty;

                    StringBuilder stringBuilder = new();
                    using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
                    {
                        foreach (string innerXml in ListBox.Items.Select(i => ((IConditionFunctionListBoxItem)i.Value).HiddenText))
                        {
                            xmlTextWriter.WriteRaw(innerXml);
                        }
                        xmlTextWriter.Flush();
                    }

                    return stringBuilder.ToString();
                }
            }
        }

        public string ShapeVisibleText
        {
            get
            {
                string logic = rdoOr.IsChecked ? rdoOr.Text : rdoAnd.Text;
                return string.Join
                (
                    $"{Environment.NewLine}{logic}{Environment.NewLine}",
                    _conditionsDataParser
                        .Parse(_xmlDocumentHelpers.ToXmlElement(ShapeXml))
                        .FunctionElements
                        .Select(e => e.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value)
                );
            }
        }

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

        public void UpdateConditionsList(string? xmlString)
        {
            rdoAnd.IsChecked = true;
            ListBox.Items.Clear();

            if (xmlString == null)
                return;

            if (!Application.AssemblyAvailable)
            {
                SetErrorMessage(Application.UnavailableMessage);
                return;
            }

            ConditionsData conditionsData = _conditionsDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(xmlString));
            if (conditionsData.FunctionElements.Count == 0)
                return;

            IList<IConditionFunctionListBoxItem> listBoxItems = GetListBoxItems();
            IList<string> errors = GetErrors();
            if (errors.Count > 0)
                SetErrorMessage(string.Join(Environment.NewLine, errors));

            //Allow the items with errors to be edited where possible so do not filter them out.
            ListBox.Items.AddRange
            (
                listBoxItems.Select(item => new RadListDataItem(item.VisibleText, item))
            );

            if (conditionsData.FirstChildElementName == XmlDataConstants.ORELEMENT)
                rdoOr.IsChecked = true;

            ValidateOk();

            IList<IConditionFunctionListBoxItem> GetListBoxItems()
                => conditionsData.FunctionElements.Select
                (
                    e => _conditionFunctionListBoxItemFactory.GetConditionFunctionListBoxItem
                    (
                        e.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value,
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

        public void UpdateInputControls(IConditionFunctionListBoxItem item)
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

            AddButtonClickCommand(btnCopyXml, _editConditionFunctionsFormCommandFactory.GetEditConditionFunctionsFormCopyXmlCommand(this));
            AddButtonClickCommand(btnPasteXml, _editConditionFunctionsFormCommandFactory.GetEditConditionFunctionsFormEditXmlCommand(this));

            AddButtonClickCommand(btnAdd, _editConditionFunctionsFormCommandFactory.GetAddConditionFunctionListBoxItemCommand(this));
            AddButtonClickCommand(btnUpdate, _editConditionFunctionsFormCommandFactory.GetUpdateConditionFunctionListBoxItemCommand(this));
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
