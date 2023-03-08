using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.ItemEditorControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList
{
    internal partial class EditObjectListControl : UserControl, IListBoxHost<IObjectListBoxItem>, IEditObjectListControl
    {
        private readonly ITypeAutoCompleteManager _cmbObjectTypeAutoCompleteManager;
        private readonly IEditObjectListCommandFactory _editObjectListCommandFactory;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGetObjectRichTextBoxVisibleText _getObjectRichTextBoxVisibleText;
        private readonly IObjectListBoxItemFactory _objectListBoxItemFactory;
        private readonly IObjectListItemEditorControlFactory _objectListItemEditorControlFactory;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingForm dataGraphEditingForm;
        private readonly Type assignedTo;
        private readonly ObjectListParameterElementInfo objectListElementInfo;
        private readonly int? selectedIndex;
        private readonly XmlDocument xmlDocument;
        private readonly IRadListBoxManager<IObjectListBoxItem> radListBoxManager;

        private IObjectListItemValueControl? valueControl;

        public EditObjectListControl(
            IEditObjectListCommandFactory editObjectListCommandFactory,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IGetObjectRichTextBoxVisibleText getObjectRichTextBoxVisibleText,
            IObjectListBoxItemFactory objectListBoxItemFactory,
            IObjectListItemEditorControlFactory objectListItemEditorControlFactory,
            IObjectListDataParser objectListDataParser,
            IRadDropDownListHelper radDropDownListHelper,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IServiceFactory serviceFactory,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingForm dataGraphEditingForm,
            ObjectListParameterElementInfo objectListElementInfo,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            int? selectedIndex)
        {
            InitializeComponent();
            _cmbObjectTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                dataGraphEditingForm,
                cmbObjectType
            );
            _editObjectListCommandFactory = editObjectListCommandFactory;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _getObjectRichTextBoxVisibleText = getObjectRichTextBoxVisibleText;
            _objectListBoxItemFactory = objectListBoxItemFactory;
            _objectListItemEditorControlFactory = objectListItemEditorControlFactory;
            _objectListDataParser = objectListDataParser;
            _radDropDownListHelper = radDropDownListHelper;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingForm = dataGraphEditingForm;
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
                    throw _exceptionHelper.CriticalException("{4EDE3CF9-FD3D-42E3-88A7-00A6552C075E}");

                if (!_typeLoadHelper.TryGetSystemType(cmbObjectType.Text, Application, out Type? _))
                    throw _exceptionHelper.CriticalException("{92CF9CC3-B5A2-4FD6-8735-0A6E8FF1E158}");

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

        public ApplicationTypeInfo Application => dataGraphEditingForm.Application;

        public IApplicationForm ApplicationForm => dataGraphEditingForm;

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

        public ListParameterInputStyle ListControl => objectListElementInfo.ListControl;

        public Type ObjectType
        {
            get
            {
                if (!_typeLoadHelper.TryGetSystemType(cmbObjectType.Text, Application, out Type? type))
                    throw _exceptionHelper.CriticalException("{485FBB31-FA6F-4AE5-8A84-CF5C3C19CF63}");

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
                            Strings.listParameterCountFormat,
                            objectListElementInfo.HasParameter
                                ? objectListElementInfo.Parameter.Name
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

        public void ClearInputControls() => ValueControl.ResetControl();

        public void ClearMessage() => dataGraphEditingForm.ClearMessage();

        public void DisableControlsDuringEdit(bool disable) => dataGraphEditingForm.DisableControlsDuringEdit(disable);

        public void RequestDocumentUpdate() => dataGraphEditingForm.RequestDocumentUpdate();

        public void SetErrorMessage(string message) => dataGraphEditingForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => dataGraphEditingForm.SetMessage(message, title);

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

        private static void AddButtonClickCommand(RadButton radButton, IClickCommand command)
        {
            radButton.Click += (sender, args) => command.Execute();
        }

        private void CheckAssignability()
        {
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

        private IObjectListItemValueControl GetEditItemControl() 
            => _objectListItemEditorControlFactory.GetListOfObjectsItemRichTextBoxControl(this, objectListElementInfo);

        private void Initialize()
        {
            InitializeTableLayoutPanel();
            CollapsePanelBorder(radScrollablePanelType);
            CollapsePanelBorder(radPanelTableParent);
            CollapsePanelBorder(radPanelEdit);
            CollapsePanelBorder(radPanelAddButton);
            LoadDropDownLists();

            radGroupBoxEdit.Text = objectListElementInfo.Name;
            radGroupBoxList.Text = GetListTitle(objectListElementInfo.ListControl);

            if (!dataGraphEditingForm.Application.AssemblyAvailable)
            {
                SetErrorMessage(dataGraphEditingForm.Application.UnavailableMessage);
                return;
            }

            InitializeValueControl();
            SetValueControlToolTip();

            cmbObjectType.Text = objectListElementInfo.ObjectType;
            cmbListType.SelectedValue = objectListElementInfo.ListType;

            CheckAssignability();

            cmbListType.SelectedIndexChanged += CmbListType_SelectedIndexChanged;
            cmbObjectType.TextChanged += CmbObjectType_TextChanged;
            radListBoxManager.ListChanged += RadListBoxManager_ListChanged;

            AddButtonClickCommand
            (
                BtnAdd,
                _editObjectListCommandFactory.GetAddObjectListBoxItemCommand(this)
            );
            AddButtonClickCommand
            (
                BtnUpdate,
                _editObjectListCommandFactory.GetUpdateObjectListBoxItemCommand(this)
            );
            managedListBoxControl.CreateCommands(radListBoxManager);

            UpdateListItems
            (
                _objectListDataParser.Parse
                (
                    _xmlDocumentHelpers.GetDocumentElement(xmlDocument)
                )
            );

            string GetListTitle(ListParameterInputStyle listInputStyle)
                => listInputStyle switch
                {
                    ListParameterInputStyle.HashSetForm => Strings.hashSetFormGroupBoxTitle,
                    ListParameterInputStyle.ListForm => Strings.listFormGroupBoxTitle,
                    _ => throw _exceptionHelper.CriticalException("{AE8C9DBD-F5E7-47A2-A7C7-760DD517950A}"),
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
            _cmbObjectTypeAutoCompleteManager.Setup();
            _radDropDownListHelper.LoadComboItems(this.cmbListType, RadDropDownStyle.DropDownList, new ListType[] { Enums.ListType.IGenericEnumerable, Enums.ListType.IGenericCollection, Enums.ListType.IGenericList });
        }

        private void SetValueControlToolTip()
        {
            if (objectListElementInfo.HasParameter
                && objectListElementInfo.Parameter.Comments.Trim().Length > 0)
            {
                ValueControl.SetToolTipHelp(objectListElementInfo.Parameter.Comments);
            }
        }

        private void UpdateListItems(ObjectListData objectListData)
        {
            cmbObjectType.Text = objectListData.ObjectType;
            cmbListType.SelectedValue = objectListData.ListType;

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
                        IObjectListBoxItem objectListBoxItem = _objectListBoxItemFactory.GetParameterObjectListBoxItem
                        (
                            _getObjectRichTextBoxVisibleText.GetVisibleText
                            (
                                _refreshVisibleTextHelper.RefreshAllVisibleTexts(e, Application).InnerXml, //inner XML is constructor,function,variable,literalList or objectList
                                Application
                            ),
                            e.InnerXml,
                            elementType,
                            dataGraphEditingForm,
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

        private void RadListBoxManager_ListChanged(object? sender, EventArgs e) => RequestDocumentUpdate();
        #endregion Event Handlers
    }
}
