﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.LiteralListItemControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.LiteralListItemControls.Factories;
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
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
    internal partial class EditParameterLiteralListControl : UserControl, IListBoxHost<ILiteralListBoxItem>, IEditParameterLiteralListControl
    {
        private readonly IEditLiteralListCommandFactory _editLiteralListCommandFactory;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ILiteralListBoxItemFactory _literalListBoxItemFactory;
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly IParameterLiteralListItemEditorControlFactory _parameterLiteralListItemEditorControlFactory;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IServiceFactory _serviceFactory;
        private readonly ITypeHelper _typeHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingHost dataGraphEditingHost;
        private readonly Type assignedTo;
        private readonly LiteralListParameterElementInfo literalListElementInfo;
        private readonly int? selectedIndex;
        private readonly XmlDocument xmlDocument;
        private readonly IRadListBoxManager<ILiteralListBoxItem> radListBoxManager;

        private ILiteralListItemValueControl? valueControl;
        private EventHandler btnAddClickHandler;
        private EventHandler btnUpdateClickHandler;

        public EditParameterLiteralListControl(
            IEditLiteralListCommandFactory editLiteralListCommandFactory,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            ILiteralListBoxItemFactory literalListBoxItemFactory,
            ILiteralListDataParser literalListDataParser,
            IParameterLiteralListItemEditorControlFactory parameterLiteralListItemEditorControlFactory,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            ITypeHelper typeHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingHost dataGraphEditingHost,
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
            _parameterLiteralListItemEditorControlFactory = parameterLiteralListItemEditorControlFactory;
            _literalListDataParser = literalListDataParser;
            _radDropDownListHelper = radDropDownListHelper;
            _serviceFactory = serviceFactory;
            _typeHelper = typeHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingHost = dataGraphEditingHost;
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

        public ApplicationTypeInfo Application => dataGraphEditingHost.Application;

        public IApplicationControl ApplicationControl => dataGraphEditingHost;

        public bool DenySpecialCharacters => dataGraphEditingHost.DenySpecialCharacters;

        public bool DisplayNotCheckBox => dataGraphEditingHost.DisplayNotCheckBox;

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

                return _enumHelper.GetSystemType((LiteralListElementType)cmbLiteralType.SelectedValue);
            }
        }

        public ILiteralListItemValueControl ValueControl => valueControl ?? throw new Exception();

        public XmlElement XmlResult
        {
            get
            {
                ListType listType = (ListType)cmbListType.SelectedValue;
                LiteralListElementType literalParameterType = (LiteralListElementType)cmbLiteralType.SelectedValue;
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

        public string VisibleText => XmlResult.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value;

        public void ClearInputControls() => ValueControl.ResetControl();

        public void ClearMessage() => dataGraphEditingHost.ClearMessage();

        public void DisableControlsDuringEdit(bool disable) => dataGraphEditingHost.DisableControlsDuringEdit(disable);

        public void RequestDocumentUpdate() => dataGraphEditingHost.RequestDocumentUpdate(this);

        public void SetErrorMessage(string message) => dataGraphEditingHost.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => dataGraphEditingHost.SetMessage(message, title);

        public void UpdateInputControls(ILiteralListBoxItem item)
            => ValueControl.Update
            (
                _xmlDocumentHelpers.ToXmlElement(_xmlDataHelper.BuildLiteralXml(item.HiddenText))
            );

        public void ValidateFields()
        {
            foreach (ILiteralListBoxItem literalListBoxItem in ListBox.Items.Select(i => i.Value).OfType<ILiteralListBoxItem>())
            {
                var itemErrors = literalListBoxItem.Errors;
                if (itemErrors.Count > 0)
                {
                    ListBox.SelectedValue = literalListBoxItem;
                    throw new LogicBuilderException(string.Join(Environment.NewLine, itemErrors));
                }
            }
        }

        private static EventHandler AddButtonClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void AddClickCommands()
        {
            RemoveClickCommands();
            btnAdd.Click += btnAddClickHandler;
            btnUpdate.Click += btnUpdateClickHandler;
        }

        private void CheckAssignability()
        {
            if (!dataGraphEditingHost.Application.AssemblyAvailable)
            {
                SetErrorMessage(dataGraphEditingHost.Application.UnavailableMessage);
                return;
            }

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
                managedListBoxControl.EnableControls(radListBoxManager);
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
                return _parameterLiteralListItemEditorControlFactory.GetListOfLiteralsParameterItemRichInputBoxControl(this, literalListElementInfo);

            switch (literalListElementInfo.Parameter.ElementControl)
            {
                case LiteralParameterInputStyle.DomainAutoComplete:
                    return _parameterLiteralListItemEditorControlFactory.GetListOfLiteralsParameterItemDomainAutoCompleteControl(literalListElementInfo.Parameter);
                case LiteralParameterInputStyle.DropDown:
                    return _parameterLiteralListItemEditorControlFactory.GetListOfLiteralsParameterItemDropDownListControl(literalListElementInfo.Parameter);
                case LiteralParameterInputStyle.MultipleLineTextBox:
                    return literalListElementInfo.Parameter.Domain.Any()
                                        ? _parameterLiteralListItemEditorControlFactory.GetListOfLiteralsParameterItemDomainMultilineControl(this, literalListElementInfo.Parameter)
                                        : _parameterLiteralListItemEditorControlFactory.GetListOfLiteralsParameterItemMultilineControl(this, literalListElementInfo.Parameter);
                case LiteralParameterInputStyle.ParameterSourcedPropertyInput:
                    return _parameterLiteralListItemEditorControlFactory.GetListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl(this, literalListElementInfo);//needs ParameterSourceClassName
                case LiteralParameterInputStyle.ParameterSourceOnly:
                case LiteralParameterInputStyle.TypeAutoComplete:
                    IListOfLiteralsParameterItemTypeAutoCompleteControl typeAutoCompleteControl = _parameterLiteralListItemEditorControlFactory.GetListOfLiteralsParameterItemTypeAutoCompleteControl();
                    ITypeAutoCompleteManager typeAutoCompleteManager = _serviceFactory.GetTypeAutoCompleteManager(dataGraphEditingHost, typeAutoCompleteControl);
                    typeAutoCompleteManager.Setup();
                    return typeAutoCompleteControl;
                case LiteralParameterInputStyle.PropertyInput:
                    return _parameterLiteralListItemEditorControlFactory.GetListOfLiteralsParameterItemPropertyInputRichInputBoxControl(this, literalListElementInfo.Parameter);
                case LiteralParameterInputStyle.SingleLineTextBox:
                    return literalListElementInfo.Parameter.Domain.Any()
                                        ? _parameterLiteralListItemEditorControlFactory.GetListOfLiteralsParameterItemDomainRichInputBoxControl(this, literalListElementInfo.Parameter)
                                        : _parameterLiteralListItemEditorControlFactory.GetListOfLiteralsParameterItemRichInputBoxControl(this, literalListElementInfo);
                default:
                    throw _exceptionHelper.CriticalException("{2036C474-5E25-4F43-9A87-0D720BA4F40A}");
            }
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(btnAddClickHandler),
        nameof(btnUpdateClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            InitializeTableLayoutPanel();
            CollapsePanelBorder(radScrollablePanelList);
            CollapsePanelBorder(radScrollablePanelType);
            CollapsePanelBorder(radPanelTableParent);
            CollapsePanelBorder(radPanelEdit);
            CollapsePanelBorder(radPanelAddButton);
            LoadDropDownLists();

            managedListBoxControl.Size = new Size(managedListBoxControl.Width, Math.Max((int)PerFontSizeConstants.BottomPanelHeight, radScrollablePanelList.Height));
            radGroupBoxEdit.Text = literalListElementInfo.Name;
            radGroupBoxList.Text = GetListTitle(literalListElementInfo.ListControl);
            radScrollablePanelList.VerticalScrollBarState = ScrollState.AlwaysShow;

            ControlsLayoutUtility.LayoutGroupBox(this, radGroupBoxList);
            ControlsLayoutUtility.LayoutAddUpdateButtonPanel(radPanelAddButton, tableLayoutPanelAddUpdate);
            InitializeValueControl();
            SetValueControlToolTip();

            cmbLiteralType.SelectedValue = literalListElementInfo.LiteralType;
            cmbListType.SelectedValue = _enumHelper.GetConcreteListType(literalListElementInfo.ListType);

            CheckAssignability();

            cmbLiteralType.SelectedIndexChanged += CmbLiteralType_SelectedIndexChanged;
            cmbListType.SelectedIndexChanged += CmbListType_SelectedIndexChanged;
            radListBoxManager.ListChanged += RadListBoxManager_ListChanged;
            radScrollablePanelList.SizeChanged += RadScrollablePanelList_SizeChanged;
            Disposed += EditParameterLiteralListControl_Disposed;
            btnAddClickHandler = AddButtonClickCommand
            (
                _editLiteralListCommandFactory.GetAddParameterLiteralListBoxItemCommand(this)
            );
            btnUpdateClickHandler = AddButtonClickCommand
            (
                _editLiteralListCommandFactory.GetUpdateParameterLiteralListBoxItemCommand(this)
            );
            managedListBoxControl.CreateCommands(radListBoxManager);

            UpdateListItems
            (
                _literalListDataParser.Parse
                (
                    _xmlDocumentHelpers.GetDocumentElement(xmlDocument)
                )
            );
            AddClickCommands();

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
            this.SuspendLayout();
            ControlsLayoutUtility.LayoutControls
            (
                radGroupBoxType,
                radScrollablePanelType,
                radPanelTableParent,
                tableLayoutPanel,
                2,
                false
            );

            //must adjust height because radGroupBoxType.Dock is not Fill.
            ControlsLayoutUtility.LayoutTwoRowGroupBox(this, radGroupBoxType, false);
            this.ResumeLayout(true);
        }

        private void InitializeValueControl()
        {
            valueControl = GetEditItemControl();
            ControlsLayoutUtility.LayoutListItemGroupBox
            (
                this,
                radGroupBoxEdit,
                radPanelEdit,
                radPanelEditControl,
                (Control)valueControl,
                multiLine: literalListElementInfo.ElementControl == LiteralParameterInputStyle.MultipleLineTextBox
            );
        }

        private void LoadDropDownLists()
        {
            _radDropDownListHelper.LoadComboItems<LiteralListElementType>(this.cmbLiteralType, RadDropDownStyle.DropDownList);
            _radDropDownListHelper.LoadComboItems(this.cmbListType, RadDropDownStyle.DropDownList, new ListType[] { Enums.ListType.IGenericEnumerable, Enums.ListType.IGenericCollection, Enums.ListType.IGenericList });
        }

        private void RemoveClickCommands()
        {
            btnAdd.Click -= btnAddClickHandler;
            btnUpdate.Click -= btnUpdateClickHandler;
        }

        private void RemoveEventHandlers()
        {
            cmbLiteralType.SelectedIndexChanged -= CmbLiteralType_SelectedIndexChanged;
            cmbListType.SelectedIndexChanged -= CmbListType_SelectedIndexChanged;
            radListBoxManager.ListChanged -= RadListBoxManager_ListChanged;
            radScrollablePanelList.SizeChanged -= RadScrollablePanelList_SizeChanged;
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
            if (!dataGraphEditingHost.Application.AssemblyAvailable)
            {
                SetErrorMessage(dataGraphEditingHost.Application.UnavailableMessage);
                return;
            }

            ListBox.Items.Clear();
            cmbLiteralType.SelectedValue = literalListData.LiteralType;
            cmbListType.SelectedValue = _enumHelper.GetConcreteListType(literalListData.ListType);
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
                            dataGraphEditingHost,
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

        private void EditParameterLiteralListControl_Disposed(object? sender, EventArgs e)
        {
            RemoveClickCommands();
            RemoveEventHandlers();
        }

        private void RadScrollablePanelList_SizeChanged(object? sender, EventArgs e)
        {
            managedListBoxControl.Size = new Size(managedListBoxControl.Width, Math.Max((int)PerFontSizeConstants.BottomPanelHeight, radScrollablePanelList.Height));
        }

        private void RadListBoxManager_ListChanged(object? sender, EventArgs e) => RequestDocumentUpdate();
        #endregion Event Handlers
    }
}
