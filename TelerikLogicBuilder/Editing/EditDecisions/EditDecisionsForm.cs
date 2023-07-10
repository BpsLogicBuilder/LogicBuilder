using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories;
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
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions
{
    internal partial class EditDecisionsForm : Telerik.WinControls.UI.RadForm, IEditDecisionsForm, IListBoxHost<IDecisionListBoxItem>
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IDecisionsDataParser _decisionsDataParser;
        private readonly IDecisionListBoxItemFactory _decisionListBoxItemFactory;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEditDecisionsFormCommandFactory _editDecisionsFormCommandFactory;
        private readonly IFormInitializer _formInitializer;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private ApplicationTypeInfo _application;
        private EventHandler btnCopyXmlClickHandler;
        private EventHandler btnPasteXmlClickHandler;
        private EventHandler btnAddClickHandler;
        private EventHandler btnUpdateClickHandler;
        private EventHandler<EventArgs> txtEditDecisionButtonClickHandler;
        private readonly IRadListBoxManager<IDecisionListBoxItem> radListBoxManager;

        public EditDecisionsForm(
            IDialogFormMessageControl dialogFormMessageControl,
            IDecisionDataParser decisionDataParser,
            IDecisionsDataParser decisionsDataParser,
            IDecisionListBoxItemFactory decisionListBoxItemFactory,
            IEditDecisionsFormCommandFactory editDecisionsFormCommandFactory,
            IFormInitializer formInitializer,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            XmlDocument? decisionsXmlDocument)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _decisionDataParser = decisionDataParser;
            _decisionsDataParser = decisionsDataParser;
            _decisionListBoxItemFactory = decisionListBoxItemFactory;
            _editDecisionsFormCommandFactory = editDecisionsFormCommandFactory;
            _formInitializer = formInitializer;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            UpdateDecisionsList(decisionsXmlDocument?.DocumentElement?.OuterXml);
            radListBoxManager = new RadListBoxManager<IDecisionListBoxItem>(this);
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

        public HelperButtonTextBox TxtEditDecision => txtEditDecision;

        public IRadListBoxManager<IDecisionListBoxItem> RadListBoxManager => radListBoxManager;

        public string ShapeXml
        {
            get
            {
                return _refreshVisibleTextHelper.RefreshDecisionVisibleTexts
                (
                    _xmlDocumentHelpers.ToXmlElement(GetDecisionsXml()),
                    Application
                ).OuterXml;

                string GetDecisionsXml()
                {
                    if (ListBox.Items.Count < 2)
                        return _xmlDataHelper.BuildDecisionsXml(GetItemsFragment());

                    return rdoOr.IsChecked
                        ? _xmlDataHelper.BuildDecisionsXml(_xmlDataHelper.BuildOrXml(GetItemsFragment()))
                        : _xmlDataHelper.BuildDecisionsXml(_xmlDataHelper.BuildAndXml(GetItemsFragment()));
                }

                string GetItemsFragment()
                {
                    if (ListBox.Items.Count == 0)
                        return string.Empty;

                    StringBuilder stringBuilder = new();
                    using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
                    {
                        foreach (string innerXml in ListBox.Items.Select(i => ((IDecisionListBoxItem)i.Value).HiddenText))
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
                    _decisionsDataParser
                        .Parse(_xmlDocumentHelpers.ToXmlElement(ShapeXml))
                        .DecisionElements
                        .Select(e =>
                        {
                            
                            return _decisionDataParser.Parse(e).VisibleText;
                        })
                );
            }
        }

        public ApplicationTypeInfo Application => _application;

        public void ClearInputControls()
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            TxtEditDecision.Text = string.Empty;
            TxtEditDecision.Tag = null;
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

        public void UpdateDecisionsList(string? xmlString)
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

            DecisionsData decisionsData = _decisionsDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(xmlString));
            if (decisionsData.DecisionElements.Count == 0)
                return;

            IList<IDecisionListBoxItem> listBoxItems = GetListBoxItems();
            IList<string> errors = GetErrors();
            if (errors.Count > 0)
                SetErrorMessage(string.Join(Environment.NewLine, errors));

            //Allow the items with errors to be edited where possible so do not filter them out.
            ListBox.Items.AddRange
            (
                listBoxItems.Select(item => new RadListDataItem(item.VisibleText, item))
            );

            if (decisionsData.FirstChildElementName == XmlDataConstants.ORELEMENT)
                rdoOr.IsChecked = true;

            ValidateOk();

            IList<IDecisionListBoxItem> GetListBoxItems()
                => decisionsData.DecisionElements.Select
                (
                    e => _decisionListBoxItemFactory.GetDecisionListBoxItem
                    (
                        _decisionDataParser.Parse(e).VisibleText,
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

        public void UpdateInputControls(IDecisionListBoxItem item)
        {
            TxtEditDecision.Tag = item.HiddenText;
            TxtEditDecision.Text = item.VisibleText;
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
            txtEditDecision.ButtonClick += txtEditDecisionButtonClickHandler;
        }

        private static EventHandler<EventArgs> AddHelperButtonClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(btnCopyXmlClickHandler),
        nameof(btnPasteXmlClickHandler),
        nameof(btnAddClickHandler),
        nameof(btnUpdateClickHandler),
        nameof(txtEditDecisionButtonClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();

            ControlsLayoutUtility.CollapsePanelBorder(radScrollablePanelList);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelEdit);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelAddButton);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelRadioButtons);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelRadioTableLayoutParent);

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            radListBoxManager.ListChanged += RadListBoxManager_ListChanged;
            Disposed += EditDecisionsForm_Disposed;

            TxtEditDecision.ReadOnly = true;

            _formInitializer.SetFormDefaults(this, 719);
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
            if (ListBox.Items.Count == 0)
                rdoAnd.IsChecked = true;

            _formInitializer.SetToEditSize(this);

            LayoutGroupBoxList();
            ControlsLayoutUtility.LayoutAddUpdateButtonPanel(radPanelAddButton, tableLayoutPanelAddUpdate);

            btnCopyXmlClickHandler = AddButtonClickCommand(_editDecisionsFormCommandFactory.GetEditDecisionsFormCopyXmlCommand(this));
            btnPasteXmlClickHandler = AddButtonClickCommand(_editDecisionsFormCommandFactory.GetEditDecisionsFormEditXmlCommand(this));

            btnAddClickHandler = AddButtonClickCommand(_editDecisionsFormCommandFactory.GetAddDecisionListBoxItemCommand(this));
            btnUpdateClickHandler = AddButtonClickCommand(_editDecisionsFormCommandFactory.GetUpdateDecisionListBoxItemCommand(this));

            txtEditDecisionButtonClickHandler = AddHelperButtonClickCommand(_editDecisionsFormCommandFactory.GetEditDecisionCommand(this));
            managedListBoxControl.CreateCommands(radListBoxManager);

            ValidateOk();

            InitializeEditControl();
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
            ControlsLayoutUtility.LayoutListItemGroupBox
            (
                this,
                radGroupBoxEdit,
                radPanelEdit,
                radPanelEditControl,
                TxtEditDecision,
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
            ((ISupportInitialize)radPanelRadioTableLayoutParent).BeginInit();
            radPanelRadioTableLayoutParent.SuspendLayout();

            this.SuspendLayout();

            radPanelRadioTableLayoutParent.Dock = DockStyle.Top;
            radPanelRadioTableLayoutParent.Margin = new Padding(0);
            radPanelRadioTableLayoutParent.Padding = new Padding(0);
            radPanelRadioTableLayoutParent.Size = new Size(radPanelRadioTableLayoutParent.Width, (int)PerFontSizeConstants.BottomPanelHeight);

            managedListBoxControl.Margin = new Padding(0);
            radScrollablePanelList.Margin = new Padding(0);
            radPanelRadioButtons.Size = new Size(PerFontSizeConstants.ConditionsRadioButtonPanelWidth, radPanelRadioButtons.Height);

            ControlsLayoutUtility.SetLabelMargin(rdoAnd);
            ControlsLayoutUtility.SetLabelMargin(rdoOr);
            radGroupBoxList.Margin = new Padding(0);
            radGroupBoxList.Padding = PerFontSizeConstants.GroupBoxPadding;

            ((ISupportInitialize)radPanelRadioTableLayoutParent).EndInit();
            radPanelRadioTableLayoutParent.ResumeLayout(false);
            ((ISupportInitialize)radPanelRadioButtons).EndInit();
            radPanelRadioButtons.ResumeLayout(false);
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
            txtEditDecision.ButtonClick -= txtEditDecisionButtonClickHandler;
        }

        private void RemoveEventHandlers()
        {
            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            radListBoxManager.ListChanged += RadListBoxManager_ListChanged;
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

        private void EditDecisionsForm_Disposed(object? sender, EventArgs e)
        {
            RemoveClickCommands();
            RemoveEventHandlers();
        }

        private void RadListBoxManager_ListChanged(object? sender, EventArgs e) => ValidateOk();
        #endregion Event Handlers
    }
}
