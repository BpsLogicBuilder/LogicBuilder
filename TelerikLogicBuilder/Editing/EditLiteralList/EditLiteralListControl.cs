using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Services.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using MediaFoundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList
{
    internal partial class EditLiteralListControl : UserControl, IListBoxHost<ILiteralListBoxItem>, IEditLiteralListControl
    {
        private readonly IEditLiteralListCommandFactory _editLiteralListCommandFactory;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ILiteralListBoxItemFactory _literalListBoxItemFactory;
        private readonly ILiteralListItemEditorControlFactory _literalListItemEditorControlFactory;
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IServiceFactory _serviceFactory;
        private readonly ITypeHelper _typeHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingForm dataGraphEditingForm;
        private readonly Type assignedTo;
        private readonly LiteralListParameterElementInfo literalListElementInfo;
        private readonly int? selectedIndex;
        private readonly XmlDocument xmlDocument;
        private readonly IRadListBoxManager<ILiteralListBoxItem> radListBoxManager;

        private ILiteralListItemValueControl? valueControl;

        public EditLiteralListControl(
            IEditLiteralListCommandFactory editLiteralListCommandFactory,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            ILiteralListBoxItemFactory literalListBoxItemFactory,
            ILiteralListItemEditorControlFactory literalListItemEditorControlFactory,
            ILiteralListDataParser literalListDataParser,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            ITypeHelper typeHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingForm dataGraphEditingForm,
            LiteralListParameterElementInfo literalListElementInfo,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            int? selectedIndex)
        {
            InitializeComponent();
            _editLiteralListCommandFactory = editLiteralListCommandFactory;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _literalListBoxItemFactory = literalListBoxItemFactory;
            _literalListItemEditorControlFactory = literalListItemEditorControlFactory;
            _literalListDataParser = literalListDataParser;
            _radDropDownListHelper = radDropDownListHelper;
            _serviceFactory = serviceFactory;
            _typeHelper = typeHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingForm = dataGraphEditingForm;
            this.literalListElementInfo = literalListElementInfo;
            this.xmlDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(formDocument, treeNodeXPath)
            );
            this.assignedTo = assignedTo;
            this.selectedIndex = selectedIndex;
            radListBoxManager = new RadListBoxManager<ILiteralListBoxItem>(this);
            Initialize();
        }

        private Type ClosedListType
        {
            get
            {
                if (cmbLiteralType.SelectedIndex == -1 || cmbListType.SelectedIndex == -1)
                    throw _exceptionHelper.CriticalException("{0DB7D528-9ACE-43F6-A7A2-BF3F78DDBEC7}");

                return _enumHelper.GetSystemType
                (
                    (ListType)cmbListType.SelectedValue,
                    LiteralType
                );
            }
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

        public IRadListBoxManager<ILiteralListBoxItem> RadListBoxManager => radListBoxManager;

        public ApplicationTypeInfo Application => dataGraphEditingForm.Application;

        public IApplicationForm ApplicationForm => dataGraphEditingForm;

        public bool DenySpecialCharacters => dataGraphEditingForm.DenySpecialCharacters;

        public bool DisplayNotCheckBox => dataGraphEditingForm.DisplayNotCheckBox;

        public bool IsValid
        {
            get
            {
                foreach (ILiteralListBoxItem literalListBoxItem in ListBox.Items.Select(i => i.Value).OfType<ILiteralListBoxItem>())
                {
                    if (literalListBoxItem.Errors.Count > 0)
                        return false;
                }

                return true;
            }
        }

        public ListParameterInputStyle ListControl => literalListElementInfo.ListControl;

        public Type LiteralType
        {
            get
            {
                if (cmbLiteralType.SelectedIndex == -1)
                    throw _exceptionHelper.CriticalException("{EF78ABC5-CC1D-4EEA-9B45-23C6D4D778CA}");

                return _enumHelper.GetSystemType((LiteralParameterType)cmbLiteralType.SelectedValue);
            }
        }

        public ILiteralListItemValueControl ValueControl => valueControl ?? throw new Exception();

        public XmlElement XmlResult
        {
            get
            {
                ListType listType = (ListType)cmbListType.SelectedValue;
                LiteralParameterType literalParameterType = (LiteralParameterType)cmbLiteralType.SelectedValue;
                return _xmlDocumentHelpers.ToXmlElement
                (
                    _xmlDataHelper.BuildLiteralListXml
                    (
                        literalParameterType,
                        listType,
                        string.Format
                        (
                            CultureInfo.CurrentCulture,
                            Strings.listParameterCountFormat,
                            literalListElementInfo.HasParameter 
                                ? literalListElementInfo.Parameter.Name 
                                : _enumHelper.GetTypeDescription(listType, _enumHelper.GetVisibleEnumText(literalParameterType)),
                            ListBox.Items.Count
                        ),
                        GetInnerXml()
                    )
                );

                string GetInnerXml()
                {
                    if (ListBox.Items.Count == 0)
                        return string.Empty;

                    StringBuilder stringBuilder = new();
                    using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
                    {
                        foreach (string innerXml in ListBox.Items.Select(i => ((ILiteralListBoxItem)i.Value).HiddenText))
                        {
                            xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALELEMENT);
                                xmlTextWriter.WriteRaw(innerXml);
                            xmlTextWriter.WriteEndElement();
                        }
                        xmlTextWriter.Flush();
                    }
                    return stringBuilder.ToString();
                }
            }
        }

        public void ClearInputControls() => ValueControl.ResetControl();

        public void ClearMessage() => dataGraphEditingForm.ClearMessage();

        public void DisableControlsDuringEdit(bool disable) => dataGraphEditingForm.DisableControlsDuringEdit(disable);

        public void RequestDocumentUpdate() => dataGraphEditingForm.RequestDocumentUpdate();

        public void SetErrorMessage(string message) => dataGraphEditingForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => dataGraphEditingForm.SetMessage(message, title);

        public void UpdateInputControls(ILiteralListBoxItem item) 
            => ValueControl.Update
            (
                _xmlDocumentHelpers.ToXmlElement(_xmlDataHelper.BuildLiteralXml(item.HiddenText))
            );

        public void ValidateFields()
        {
            foreach(ILiteralListBoxItem literalListBoxItem in ListBox.Items.Select(i => i.Value).OfType<ILiteralListBoxItem>())
            {
                var itemErrors = literalListBoxItem.Errors;
                if (itemErrors.Count > 0)
                {
                    ListBox.SelectedValue = literalListBoxItem;
                    throw new LogicBuilderException(string.Join(Environment.NewLine, itemErrors));
                }
            }
        }

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

        private void CheckAssignability()
        {
            if (cmbLiteralType.SelectedIndex == -1 || cmbListType.SelectedIndex == -1)
            {
                EnableEditing(false);
                return;
            }

            if (!_typeHelper.AssignableFrom(assignedTo, ClosedListType))
            {
                SetErrorMessage
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.typeNotAssignableFormat,
                        ClosedListType.ToString(),
                        assignedTo.ToString()
                    )
                );

                EnableEditing(false);
                return;
            }

            EnableEditing(true);
            ClearMessage();
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void EnableEditing(bool enable)
        {
            if (enable)
            {
                ValueControl.EnableControls();
                managedListBoxControl.EnableControls();
            }
            else
            {
                ValueControl.DisableControls();
                managedListBoxControl.DisableControls();
            }

            btnAdd.Enabled = enable;
            btnUpdate.Enabled = enable;
        }

        private ILiteralListItemValueControl GetEditItemControl()
        {
            if (!literalListElementInfo.HasParameter)
                return _literalListItemEditorControlFactory.GetListOfLiteralsItemRichInputBoxControl(this, literalListElementInfo);

            switch (literalListElementInfo.Parameter.ElementControl)
            {
                case LiteralParameterInputStyle.DomainAutoComplete:
                    return _literalListItemEditorControlFactory.GetListOfLiteralsItemDomainAutoCompleteControl(this, literalListElementInfo.Parameter);
                case LiteralParameterInputStyle.DropDown:
                    return _literalListItemEditorControlFactory.GetListOfLiteralsItemDropDownListControl(this, literalListElementInfo.Parameter);
                case LiteralParameterInputStyle.MultipleLineTextBox:
                    return literalListElementInfo.Parameter.Domain.Any()
                                        ? _literalListItemEditorControlFactory.GetListOfLiteralsItemDomainMultilineControl(this, literalListElementInfo.Parameter)
                                        : _literalListItemEditorControlFactory.GetListOfLiteralsItemMultilineControl(this, literalListElementInfo.Parameter);
                case LiteralParameterInputStyle.ParameterSourcedPropertyInput:
                    return _literalListItemEditorControlFactory.GetListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl(this, literalListElementInfo);//needs ParameterSourceClassName
                case LiteralParameterInputStyle.ParameterSourceOnly:
                case LiteralParameterInputStyle.TypeAutoComplete:
                    IListOfLiteralsItemTypeAutoCompleteControl typeAutoCompleteControl = _literalListItemEditorControlFactory.GetListOfLiteralsItemTypeAutoCompleteControl();
                    ITypeAutoCompleteManager typeAutoCompleteManager = _serviceFactory.GetTypeAutoCompleteManager(dataGraphEditingForm, typeAutoCompleteControl);
                    typeAutoCompleteManager.Setup();
                    return typeAutoCompleteControl;
                case LiteralParameterInputStyle.PropertyInput:
                    return _literalListItemEditorControlFactory.GetListOfLiteralsItemPropertyInputRichInputBoxControl(this, literalListElementInfo.Parameter);
                case LiteralParameterInputStyle.SingleLineTextBox:
                    return literalListElementInfo.Parameter.Domain.Any()
                                        ? _literalListItemEditorControlFactory.GetListOfLiteralsItemDomainRichInputBoxControl(this, literalListElementInfo.Parameter)
                                        : _literalListItemEditorControlFactory.GetListOfLiteralsItemRichInputBoxControl(this, literalListElementInfo);
                default:
                    throw _exceptionHelper.CriticalException("{2036C474-5E25-4F43-9A87-0D720BA4F40A}");
            }
        }

        private void Initialize()
        {
            InitializeTableLayoutPanel();
            CollapsePanelBorder(radScrollablePanelType);
            CollapsePanelBorder(radPanelTableParent);
            CollapsePanelBorder(radPanelEdit);
            CollapsePanelBorder(radPanelAddButton);
            LoadDropDownLists();

            radGroupBoxEdit.Text = literalListElementInfo.Name;
            radGroupBoxList.Text = GetListTitle(literalListElementInfo.ListControl);

            if (!dataGraphEditingForm.Application.AssemblyAvailable)
            {
                SetErrorMessage(dataGraphEditingForm.Application.UnavailableMessage);
                return;
            }

            InitializeValueControl();
            SetValueControlToolTip();

            cmbLiteralType.SelectedValue = literalListElementInfo.LiteralType;
            cmbListType.SelectedValue = literalListElementInfo.ListType;

            CheckAssignability();

            cmbLiteralType.SelectedIndexChanged += CmbLiteralType_SelectedIndexChanged;
            cmbListType.SelectedIndexChanged += CmbListType_SelectedIndexChanged;
            radListBoxManager.ListChanged += RadListBoxManager_ListChanged;
            AddButtonClickCommand
            (
                BtnAdd,
                _editLiteralListCommandFactory.GetAddLiteralListBoxItemCommand(this)
            );
            AddButtonClickCommand
            (
                BtnUpdate,
                _editLiteralListCommandFactory.GetUpdateLiteralListBoxItemCommand(this)
            );
            managedListBoxControl.CreateCommands(radListBoxManager);

            UpdateListItems
            (
                _literalListDataParser.Parse
                (
                    _xmlDocumentHelpers.GetDocumentElement(xmlDocument)
                )
            );

            string GetListTitle(ListParameterInputStyle listInputStyle) 
                => listInputStyle switch
                {
                    ListParameterInputStyle.HashSetForm => Strings.hashSetFormGroupBoxTitle,
                    ListParameterInputStyle.ListForm => Strings.listFormGroupBoxTitle,
                    _ => throw _exceptionHelper.CriticalException("{0CA63025-4C10-419E-B6CE-852790CE14CF}"),
                };
        }

        private void InitializeTableLayoutPanel()
        {
            float size_20 = 20F / 112 * 100;
            float size_30 = 30F / 112 * 100;
            float size_6 = 6F / 112 * 100;

            ((ISupportInitialize)this.radPanelTableParent).BeginInit();
            this.radPanelTableParent.SuspendLayout();

            this.tableLayoutPanel.RowStyles[0] = new RowStyle(SizeType.Percent, size_20);
            this.tableLayoutPanel.RowStyles[1] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[2] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[3] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[4] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[5] = new RowStyle(SizeType.Percent, size_20);

            ((ISupportInitialize)this.radPanelTableParent).EndInit();
            this.radPanelTableParent.ResumeLayout(true);
        }

        private void InitializeValueControl()
        {
            valueControl = GetEditItemControl();

            Control control = (Control)valueControl;
            ((ISupportInitialize)this.radPanelEdit).BeginInit();
            this.radPanelEdit.SuspendLayout();
            ((ISupportInitialize)this.radGroupBoxEdit).BeginInit();
            this.radGroupBoxEdit.SuspendLayout();
            this.SuspendLayout();

            radGroupBoxEdit.Size = literalListElementInfo.ElementControl == LiteralParameterInputStyle.MultipleLineTextBox
                                    ? new Size(radGroupBoxEdit.Width, 120)
                                    : new Size(radGroupBoxEdit.Width, 55);

            control.Name = "valueControl";
            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0);
            control.Location = new Point(0, 0);

            this.radPanelEdit.Controls.Add(control);
            ((ISupportInitialize)this.radPanelEdit).EndInit();
            this.radPanelEdit.ResumeLayout(false);
            ((ISupportInitialize)this.radGroupBoxEdit).EndInit();
            this.radGroupBoxEdit.ResumeLayout(false);
            this.ResumeLayout(true);
        }

        private void LoadDropDownLists()
        {
            _radDropDownListHelper.LoadComboItems<LiteralParameterType>(this.cmbLiteralType, RadDropDownStyle.DropDownList);
            _radDropDownListHelper.LoadComboItems(this.cmbListType, RadDropDownStyle.DropDownList, new ListType[] { Enums.ListType.IGenericEnumerable, Enums.ListType.IGenericCollection, Enums.ListType.IGenericList });
        }

        private void SetValueControlToolTip()
        {
            if (literalListElementInfo.HasParameter
                && literalListElementInfo.Parameter.Comments.Trim().Length > 0)
            {
                ValueControl.SetToolTipHelp(literalListElementInfo.Parameter.Comments);
            }
        }

        private void UpdateListItems(LiteralListData literalListData)
        {
            ListBox.Items.Clear();
            cmbLiteralType.SelectedValue = literalListData.LiteralType;
            cmbListType.SelectedValue = literalListData.ListType;
            ListBox.Items.AddRange
            (
                GetListBoxItems().Select(lbi => new RadListDataItem(lbi.VisibleText, lbi))
            );
            ListBox.SelectedIndex = this.selectedIndex ?? -1;

            IList<ILiteralListBoxItem> GetListBoxItems()
            {
                if (!_typeHelper.AssignableFrom(this.assignedTo, ClosedListType))
                    return Array.Empty<ILiteralListBoxItem>();

                Type elementType = LiteralType;//only convert once
                List<string> errors = new();
                IList<ILiteralListBoxItem> literalListBoxItems = literalListData.ChildElements.Select
                (
                    e =>
                    {
                        ILiteralListBoxItem literalListBoxItem = _literalListBoxItemFactory.GetParameterLiteralListBoxItem
                        (
                            _xmlDocumentHelpers.GetVisibleText(e),
                            e.InnerXml,
                            elementType,
                            dataGraphEditingForm,
                            literalListElementInfo.ListControl
                        );
                        errors.AddRange(literalListBoxItem.Errors);
                        return literalListBoxItem;
                    }
                )
                .ToArray();

                if (errors.Count > 0)
                    SetErrorMessage(string.Join(Environment.NewLine, errors));

                return literalListBoxItems;
            }
        }

        #region Event Handlers
        private void CmbListType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e) 
            => CheckAssignability();

        private void CmbLiteralType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (cmbLiteralType.SelectedIndex == -1)
            {
                EnableEditing(false);
                return;
            }

            ValueControl.SetAssignedToType(LiteralType);
            CheckAssignability();
        }

        private void RadListBoxManager_ListChanged(object? sender, EventArgs e) => RequestDocumentUpdate();
        #endregion Event Handlers
    }
}
