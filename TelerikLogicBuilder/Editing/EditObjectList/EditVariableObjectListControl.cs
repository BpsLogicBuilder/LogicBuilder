using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.ObjectListItemControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList
{
    internal partial class EditVariableObjectListControl : UserControl, IListBoxHost<IObjectListBoxItem>, IEditVariableObjectListControl
    {
        private readonly ITypeAutoCompleteManager _cmbObjectTypeAutoCompleteManager;
        private readonly IEditObjectListCommandFactory _editObjectListCommandFactory;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGetObjectRichTextBoxVisibleText _getObjectRichTextBoxVisibleText;
        private readonly IObjectListBoxItemFactory _objectListBoxItemFactory;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IVariableObjectListItemEditorControlFactory _variableObjectListItemEditorControlFactory;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingHost dataGraphEditingHost;
        private readonly Type assignedTo;
        private readonly ObjectListVariableElementInfo objectListElementInfo;
        private readonly int? selectedIndex;
        private readonly XmlDocument xmlDocument;
        private readonly IRadListBoxManager<IObjectListBoxItem> radListBoxManager;

        private IObjectListItemValueControl? valueControl;
        private EventHandler btnAddClickHandler;
        private EventHandler btnUpdateClickHandler;

        public EditVariableObjectListControl(
            IEditObjectListCommandFactory editObjectListCommandFactory,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IGetObjectRichTextBoxVisibleText getObjectRichTextBoxVisibleText,
            IObjectListBoxItemFactory objectListBoxItemFactory,
            IObjectListDataParser objectListDataParser,
            IVariableObjectListItemEditorControlFactory variableObjectListItemEditorControlFactory,
            IRadDropDownListHelper radDropDownListHelper,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingHost dataGraphEditingHost,
            ObjectListVariableElementInfo objectListElementInfo,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            int? selectedIndex)
        {
            InitializeComponent();
            _cmbObjectTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                dataGraphEditingHost,
                cmbObjectType
            );
            _editObjectListCommandFactory = editObjectListCommandFactory;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _getObjectRichTextBoxVisibleText = getObjectRichTextBoxVisibleText;
            _objectListBoxItemFactory = objectListBoxItemFactory;
            _variableObjectListItemEditorControlFactory = variableObjectListItemEditorControlFactory;
            _objectListDataParser = objectListDataParser;
            _radDropDownListHelper = radDropDownListHelper;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingHost = dataGraphEditingHost;
            this.objectListElementInfo = objectListElementInfo;
            this.xmlDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(formDocument, treeNodeXPath)
            );
            this.assignedTo = assignedTo;
            this.selectedIndex = selectedIndex;
            radListBoxManager = new RadListBoxManager<IObjectListBoxItem>(this);
            Initialize();
        }

        private Type ClosedListType
        {
            get
            {
                if (cmbListType.SelectedIndex == -1)
                    throw _exceptionHelper.CriticalException("{DE489373-5AC3-4533-95EA-C8FEEC0097AE}");

                if (!_typeLoadHelper.TryGetSystemType(cmbObjectType.Text, Application, out Type? _))
                    throw _exceptionHelper.CriticalException("{DE489373-5AC3-4533-95EA-C8FEEC0097AE}");

                return _enumHelper.GetSystemType
                (
                    (ListType)cmbListType.SelectedValue,
                    ObjectType
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

        public IRadListBoxManager<IObjectListBoxItem> RadListBoxManager => radListBoxManager;

        public ApplicationTypeInfo Application => dataGraphEditingHost.Application;

        public IApplicationControl ApplicationControl => dataGraphEditingHost;

        public bool DenySpecialCharacters => dataGraphEditingHost.DenySpecialCharacters;

        public bool DisplayNotCheckBox => dataGraphEditingHost.DisplayNotCheckBox;

        public bool IsValid
        {
            get
            {
                foreach (IObjectListBoxItem objectListBoxItem in ListBox.Items.Select(i => i.Value).OfType<IObjectListBoxItem>())
                {
                    if (objectListBoxItem.Errors.Count > 0)
                        return false;
                }

                return true;
            }
        }

        public ListVariableInputStyle ListControl => objectListElementInfo.ListControl;

        public Type ObjectType
        {
            get
            {
                if (!_typeLoadHelper.TryGetSystemType(cmbObjectType.Text, Application, out Type? type))
                    throw _exceptionHelper.CriticalException("{597383B8-C428-4CC6-8D79-802FF12BF033}");

                return type;
            }
        }

        public IObjectListItemValueControl ValueControl => valueControl ?? throw new Exception();

        public XmlElement XmlResult
        {
            get
            {
                ListType listType = (ListType)cmbListType.SelectedValue;
                string objectType = cmbObjectType.Text;

                return _xmlDocumentHelpers.ToXmlElement
                (
                    _xmlDataHelper.BuildObjectListXml
                    (
                        objectType,
                        listType,
                        string.Format
                        (
                            CultureInfo.CurrentCulture,
                            Strings.listVariableCountFormat,
                            objectListElementInfo.HasVariable
                                ? objectListElementInfo.Variable.Name
                                : _enumHelper.GetTypeDescription(listType, objectType),
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
                        foreach (string innerXml in ListBox.Items.Select(i => ((IObjectListBoxItem)i.Value).HiddenText))
                        {
                            xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTELEMENT);
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

        public void UpdateInputControls(IObjectListBoxItem item)
            => ValueControl.Update
            (
                _xmlDocumentHelpers.ToXmlElement(_xmlDataHelper.BuildObjectXml(item.HiddenText))
            );

        public void ValidateFields()
        {
            foreach (IObjectListBoxItem objectListBoxItem in ListBox.Items.Select(i => i.Value).OfType<IObjectListBoxItem>())
            {
                var itemErrors = objectListBoxItem.Errors;
                if (itemErrors.Count > 0)
                {
                    ListBox.SelectedValue = objectListBoxItem;
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

            if (cmbListType.SelectedIndex == -1)
            {
                EnableEditing(false);
                return;
            }

            if (!_typeLoadHelper.TryGetSystemType(cmbObjectType.Text, Application, out Type? _))
            {
                SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, cmbObjectType.Text));
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

        private IObjectListItemValueControl GetEditItemControl()
            => _variableObjectListItemEditorControlFactory.GetListOfObjectsVariableItemRichTextBoxControl(this, objectListElementInfo);

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
            radGroupBoxEdit.Text = objectListElementInfo.Name;
            radGroupBoxList.Text = GetListTitle(objectListElementInfo.ListControl);
            radScrollablePanelList.VerticalScrollBarState = ScrollState.AlwaysShow;

            ControlsLayoutUtility.LayoutGroupBox(this, radGroupBoxList);
            ControlsLayoutUtility.LayoutAddUpdateButtonPanel(radPanelAddButton, tableLayoutPanelAddUpdate);
            InitializeValueControl();
            SetValueControlToolTip();

            cmbObjectType.Text = objectListElementInfo.ObjectType;
            cmbListType.SelectedValue = _enumHelper.GetConcreteListType(objectListElementInfo.ListType);

            CheckAssignability();

            cmbListType.SelectedIndexChanged += CmbListType_SelectedIndexChanged;
            cmbObjectType.TextChanged += CmbObjectType_TextChanged;
            radListBoxManager.ListChanged += RadListBoxManager_ListChanged;
            radScrollablePanelList.SizeChanged += RadScrollablePanelList_SizeChanged;
            Disposed += EditVariableObjectListControl_Disposed;

            btnAddClickHandler = AddButtonClickCommand
            (
                _editObjectListCommandFactory.GetAddVariableObjectListBoxItemCommand(this)
            );
            btnUpdateClickHandler = AddButtonClickCommand
            (
                _editObjectListCommandFactory.GetUpdateVariableObjectListBoxItemCommand(this)
            );
            managedListBoxControl.CreateCommands(radListBoxManager);

            UpdateListItems
            (
                _objectListDataParser.Parse
                (
                    _xmlDocumentHelpers.GetDocumentElement(xmlDocument)
                )
            );

            AddClickCommands();

            string GetListTitle(ListVariableInputStyle listInputStyle)
                => listInputStyle switch
                {
                    ListVariableInputStyle.HashSetForm => Strings.hashSetFormGroupBoxTitle,
                    ListVariableInputStyle.ListForm => Strings.listFormGroupBoxTitle,
                    _ => throw _exceptionHelper.CriticalException("{503CF370-63F6-4DCA-8EF7-054C867F7F5A}"),
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
                multiLine: false
            );
        }

        private void LoadDropDownLists()
        {
            _cmbObjectTypeAutoCompleteManager.Setup();
            _radDropDownListHelper.LoadComboItems(this.cmbListType, RadDropDownStyle.DropDownList, new ListType[] { Enums.ListType.IGenericEnumerable, Enums.ListType.IGenericCollection, Enums.ListType.IGenericList });
        }

        private void RemoveClickCommands()
        {
            btnAdd.Click -= btnAddClickHandler;
            btnUpdate.Click -= btnUpdateClickHandler;
        }

        private void RemoveEventHandlers()
        {
            cmbListType.SelectedIndexChanged -= CmbListType_SelectedIndexChanged;
            cmbObjectType.TextChanged -= CmbObjectType_TextChanged;
            radListBoxManager.ListChanged -= RadListBoxManager_ListChanged;
            radScrollablePanelList.SizeChanged -= RadScrollablePanelList_SizeChanged;
        }

        private void SetValueControlToolTip()
        {
            if (objectListElementInfo.HasVariable
                && objectListElementInfo.Variable.Comments.Trim().Length > 0)
            {
                ValueControl.SetToolTipHelp(objectListElementInfo.Variable.Comments);
            }
        }

        private void UpdateListItems(ObjectListData objectListData)
        {
            if (!dataGraphEditingHost.Application.AssemblyAvailable)
            {
                SetErrorMessage(dataGraphEditingHost.Application.UnavailableMessage);
                return;
            }

            cmbObjectType.Text = objectListData.ObjectType;
            cmbListType.SelectedValue = _enumHelper.GetConcreteListType(objectListData.ListType);

            if (!_typeLoadHelper.TryGetSystemType(cmbObjectType.Text, Application, out Type? type))
            {
                SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, cmbObjectType.Text));
                return;
            }

            ListBox.Items.Clear();
            ListBox.Items.AddRange
            (
                GetListBoxItems().Select(lbi => new RadListDataItem(lbi.VisibleText, lbi))
            );
            ListBox.SelectedIndex = this.selectedIndex ?? -1;

            IList<IObjectListBoxItem> GetListBoxItems()
            {
                if (!_typeHelper.AssignableFrom(this.assignedTo, ClosedListType))
                    return Array.Empty<IObjectListBoxItem>();

                Type elementType = ObjectType;//only convert once
                List<string> errors = new();
                IList<IObjectListBoxItem> objectListBoxItems = objectListData.ChildElements.Select
                (
                    e =>
                    {
                        IObjectListBoxItem objectListBoxItem = _objectListBoxItemFactory.GetVariableObjectListBoxItem
                        (
                            _getObjectRichTextBoxVisibleText.GetVisibleText
                            (
                                _refreshVisibleTextHelper.RefreshAllVisibleTexts(e, Application).InnerXml, //inner XML is constructor,function,variable,literalList or objectList
                                Application
                            ),
                            e.InnerXml,
                            elementType,
                            dataGraphEditingHost,
                            objectListElementInfo.ListControl
                        );
                        errors.AddRange(objectListBoxItem.Errors);
                        return objectListBoxItem;
                    }
                )
                .ToArray();

                if (errors.Count > 0)
                    SetErrorMessage(string.Join(Environment.NewLine, errors));

                return objectListBoxItems;
            }
        }

        #region Event Handlers
        private void CmbListType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
            => CheckAssignability();

        private void CmbObjectType_TextChanged(object? sender, EventArgs e)
        {
            if (!_typeLoadHelper.TryGetSystemType(cmbObjectType.Text, Application, out Type? _))
            {
                SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeFormat2, cmbObjectType.Text));
                EnableEditing(false);
                return;
            }

            ValueControl.SetAssignedToType(ObjectType);
            CheckAssignability();
        }

        private void EditVariableObjectListControl_Disposed(object? sender, EventArgs e)
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
