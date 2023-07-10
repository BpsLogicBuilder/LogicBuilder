using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
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
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions
{
    internal partial class EditFunctionsForm : Telerik.WinControls.UI.RadForm, IEditFunctionsForm, IListBoxHost<IFunctionListBoxItem>
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEditFunctionsCommandFactory _editFunctionsCommandFactory;
        private readonly IFormInitializer _formInitializer;
        private readonly IFunctionListBoxItemFactory _functionListBoxItemFactory;
        private readonly IFunctionsDataParser _functionsDataParser;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IEditVoidFunctionControl editVoidFunctionControl;
        private ApplicationTypeInfo _application;
        private EventHandler btnCopyXmlClickHandler;
        private EventHandler btnPasteXmlClickHandler;
        private EventHandler btnAddClickHandler;
        private EventHandler btnUpdateClickHandler;
        private readonly ObjectRichTextBox _objectRichTextBox;
        private readonly IRadListBoxManager<IFunctionListBoxItem> radListBoxManager;

        public EditFunctionsForm(
            IDialogFormMessageControl dialogFormMessageControl,
            IEditFunctionsCommandFactory editFunctionsCommandFactory,
            IEditFunctionsControlFactory editFunctionsControlFactory,
            IFormInitializer formInitializer,
            IFunctionListBoxItemFactory functionListBoxItemFactory,
            IFunctionsDataParser functionsDataParser,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            ObjectRichTextBox objectRichTextBox,
            IDictionary<string, Function> functionDictionary,
            IList<TreeFolder> treeFolders,
            XmlDocument? functionsXmlDocument)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _editFunctionsCommandFactory = editFunctionsCommandFactory;
            _formInitializer = formInitializer;
            _functionListBoxItemFactory = functionListBoxItemFactory;
            _functionsDataParser = functionsDataParser;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _objectRichTextBox = objectRichTextBox;
            FunctionDictionary = functionDictionary;
            TreeFolders = treeFolders;

            if (functionsXmlDocument?.DocumentElement != null)
                UpdateFunctionsList(functionsXmlDocument.DocumentElement.OuterXml);

            radListBoxManager = new RadListBoxManager<IFunctionListBoxItem>(this);
            this.editVoidFunctionControl = editFunctionsControlFactory.GetEditVoidFunctionControl(this);
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

        public IEditingControl? CurrentEditingControl => editVoidFunctionControl.CurrentEditingControl;

        public IEditVoidFunctionControl EditVoidFunctionControl => editVoidFunctionControl;

        public IRadListBoxManager<IFunctionListBoxItem> RadListBoxManager => radListBoxManager;

        public string ShapeXml
        {
            get
            {
                return _refreshVisibleTextHelper.RefreshFunctionVisibleTexts
                (
                    _xmlDocumentHelpers.ToXmlElement(_xmlDataHelper.BuildFunctionsXml(GetInnerXml())),
                    Application
                ).OuterXml;

                string GetInnerXml()
                {
                    if (ListBox.Items.Count == 0)
                        return string.Empty;

                    StringBuilder stringBuilder = new();
                    using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
                    {
                        foreach (string innerXml in ListBox.Items.Select(i => ((IFunctionListBoxItem)i.Value).HiddenText))
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
                return string.Join
                (
                    Environment.NewLine,
                    _functionsDataParser
                        .Parse(_xmlDocumentHelpers.ToXmlElement(ShapeXml))
                        .FunctionElements
                        .Select(e => e.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value)
                );
            }
        }

        public ApplicationTypeInfo Application => _application;

        public IDictionary<string, Function> FunctionDictionary { get; }

        public IList<TreeFolder> TreeFolders { get; }

        public void ClearInputControls()
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            _objectRichTextBox.Text = string.Empty;
            editVoidFunctionControl.ClearInputControls();
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

        public void UpdateFunctionsList(string xmlString)
        {
            ListBox.Items.Clear();
            if (!Application.AssemblyAvailable)
            {
                SetErrorMessage(Application.UnavailableMessage);
                return;
            }
            FunctionsData functionsData = _functionsDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(xmlString));
            if (functionsData.FunctionElements.Count == 0)
                return;

            IList<IFunctionListBoxItem> listBoxItems = GetListBoxItems();
            IList<string> errors = GetErrors();
            if (errors.Count > 0)
                SetErrorMessage(string.Join(Environment.NewLine, errors));

            //Allow the items with errors to be edited where possible so do not filter them out.
            ListBox.Items.AddRange
            (
                listBoxItems.Select(item => new RadListDataItem(item.VisibleText, item))
            );

            ValidateOk();

            IList<IFunctionListBoxItem> GetListBoxItems()
                => functionsData.FunctionElements.Select
                (
                    e => _functionListBoxItemFactory.GetFunctionListBoxItem
                    (
                        e.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value,
                        e.OuterXml,
                        typeof(object),
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

        public void UpdateInputControls(IFunctionListBoxItem item)
        {
            editVoidFunctionControl.UpdateInputControls(item.HiddenText);

            if (CurrentEditingControl?.IsValid == true)
            {
                _objectRichTextBox.Text = editVoidFunctionControl.VisibleText;
            }
        }

        private static EventHandler AddButtonClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnCopyXml.Click += btnCopyXmlClickHandler;
            btnPasteXml.Click += btnPasteXmlClickHandler;
            btnAdd.Click += btnAddClickHandler;
            btnUpdate.Click += btnUpdateClickHandler;
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(btnCopyXmlClickHandler),
        nameof(btnPasteXmlClickHandler),
        nameof(btnAddClickHandler),
        nameof(btnUpdateClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            ControlsLayoutUtility.CollapsePanelBorder(radScrollablePanelList);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelEdit);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelAddButton);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelFill);

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            editVoidFunctionControl.Changed += EditVoidFunctionControl_Changed;
            radListBoxManager.ListChanged += RadListBoxManager_ListChanged;
            Disposed += EditFunctionsForm_Disposed;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            _formInitializer.SetToEditSize(this);

            LayoutGroupBoxList();
            ControlsLayoutUtility.LayoutAddUpdateButtonPanel(radPanelAddButton, tableLayoutPanelAddUpdate);

            InitializeEditVoidFunctionControl();
            InitializeEditControl();

            btnCopyXmlClickHandler = AddButtonClickCommand(_editFunctionsCommandFactory.GetEditFFunctionsFormCopyXmlCommand(this));
            btnPasteXmlClickHandler = AddButtonClickCommand(_editFunctionsCommandFactory.GetEditFunctionsFormXmlCommand(this));

            btnAddClickHandler = AddButtonClickCommand(_editFunctionsCommandFactory.GetAddFunctionListBoxItemCommand(this));
            btnUpdateClickHandler = AddButtonClickCommand(_editFunctionsCommandFactory.GetUpdateFunctionListBoxItemCommand(this));
            managedListBoxControl.CreateCommands(radListBoxManager);
            ValidateOk();
            AddClickCommands();
        }

        private void InitializeApplicationDropDownList()
        {
            ControlsLayoutUtility.LayoutApplicationGroupBox(this, radPanelApplication, radGroupBoxApplication, _applicationDropDownList);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
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

        private void InitializeEditVoidFunctionControl()
        {
            ((ISupportInitialize)radPanelFill).BeginInit();
            radPanelFill.SuspendLayout();
            ((ISupportInitialize)this).BeginInit();
            this.SuspendLayout();

            Control control = (Control)editVoidFunctionControl;
            control.Dock = DockStyle.Fill;
            radPanelFill.Controls.Add(control);

            ((ISupportInitialize)radPanelFill).EndInit();
            radPanelFill.ResumeLayout(false);
            ((ISupportInitialize)this).EndInit();
            this.ResumeLayout(true);
        }

        private void LayoutGroupBoxList()
        {
            ((ISupportInitialize)radGroupBoxList).BeginInit();
            radGroupBoxList.SuspendLayout();
            ((ISupportInitialize)radScrollablePanelList).BeginInit();
            radScrollablePanelList.PanelContainer.SuspendLayout();
            radScrollablePanelList.SuspendLayout();

            this.SuspendLayout();

            managedListBoxControl.Margin = new Padding(0);
            radScrollablePanelList.Margin = new Padding(0);
            radGroupBoxList.Margin = new Padding(0);
            radGroupBoxList.Padding = PerFontSizeConstants.GroupBoxPadding;
            radGroupBoxList.Size = new Size
            (
                radGroupBoxList.Width,
                (int)PerFontSizeConstants.BottomPanelHeight
                    + radGroupBoxList.Padding.Top
                    + radGroupBoxList.Padding.Bottom
            );

            ((ISupportInitialize)radGroupBoxList).EndInit();
            radGroupBoxList.ResumeLayout(false);
            radScrollablePanelList.PanelContainer.ResumeLayout(false);
            ((ISupportInitialize)radScrollablePanelList).EndInit();
            radScrollablePanelList.ResumeLayout(false);
            this.ResumeLayout(true);
        }

        private void RemoveClickCommands()
        {
            btnCopyXml.Click -= btnCopyXmlClickHandler;
            btnPasteXml.Click -= btnPasteXmlClickHandler;
            btnAdd.Click -= btnAddClickHandler;
            btnUpdate.Click -= btnUpdateClickHandler;
        }

        private void RemoveEventHandlers()
        {
            _applicationDropDownList.ApplicationChanged -= ApplicationDropDownList_ApplicationChanged;
            editVoidFunctionControl.Changed -= EditVoidFunctionControl_Changed;
            radListBoxManager.ListChanged -= RadListBoxManager_ListChanged;
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

        private void EditFunctionsForm_Disposed(object? sender, EventArgs e)
        {
            RemoveClickCommands();
            RemoveEventHandlers();
        }

        private void EditVoidFunctionControl_Changed(object? sender, EventArgs e)
        {
            if (CurrentEditingControl == null)
                return;

            _objectRichTextBox.Text = CurrentEditingControl.VisibleText;
        }

        private void RadListBoxManager_ListChanged(object? sender, EventArgs e) => ValidateOk();
        #endregion Event Handlers
    }
}
