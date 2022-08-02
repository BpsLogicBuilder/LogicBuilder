using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.Forms;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal partial class TableControl : UserControl, IDocumentEditor, ITableControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly UiNotificationService _uiNotificationService;

        public TableControl(
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IMainWindow mainWindow,
            IPathHelper pathHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            UiNotificationService uiNotificationService,
            string tableSourceFile,
            bool openedAsReadOnly)
        {
            _exceptionHelper = exceptionHelper; 
            _formInitializer = formInitializer;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _uiNotificationService = uiNotificationService;

            InitializeComponent();
            this.openedAsReadOnly = openedAsReadOnly;
            this.parentForm = (IMDIParent)_mainWindow.Instance;
            Initialize();

            this.parentForm.CommandBarButtonSave.Enabled = !openedAsReadOnly;
            this.parentForm.RadMenuItemSave.Enabled = !openedAsReadOnly;
            this.parentForm.RadMenuItemDelete.Enabled = !openedAsReadOnly;
            this.parentForm.RadMenuItemChaining.Enabled = !openedAsReadOnly;
            this.parentForm.RadMenuItemToggleActivateAll.Enabled = !openedAsReadOnly;
            this.parentForm.RadMenuItemToggleReevaluateAll.Enabled = !openedAsReadOnly;

            this.tableSourceFile = tableSourceFile;
            CheckDocumentAvailability();
            LoadSourceFile();
            LockDocument();

            this.Text = openedAsReadOnly
                ? string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.formCaptionReadOnlyFormat,
                    _pathHelper.GetFileName(tableSourceFile)
                )
                : _pathHelper.GetFileName(tableSourceFile);

            toolTip.SetToolTip(titleBar1, tableSourceFile);
            titleBar1.Text = this.Text;
        }

        #region Constants
        private const int ROWHEIGHT = 20;
        private const int MINIMUMROWHEIGHT = 40;
        private const int COLUMNCOUNT = 8;
        private const int CONDITIONCOLUMNWIDTH = 300;
        private const int ACTIONCOLUMNWIDTH = 400;
        private const int PRIORITYCOLUMNWIDTH = 70;
        private const int ROWHEADERSWIDTH = 70;
        #endregion Constants

        private readonly RadToolTip toolTip = new();

        private readonly IMDIParent parentForm;
        private readonly bool openedAsReadOnly;
        private readonly string tableSourceFile = string.Empty;

        private DataSet dataSet;
        private BindingSource bindingSource1;
        private FileStream? fileStream;
        private DataRow? copiedRow;
        private Cell? copiedCell;

        private RadContextMenu mnuFunctions;
        private RadMenuItem mnuItemCopyCell;
        private RadMenuItem mnuItemCopyRow;
        private RadMenuItem mnuItemPasteCell;
        private RadMenuItem mnuItemPasteRow;
        private RadMenuItem mnuItemAddNewRow;
        private RadMenuItem mnuItemInsertNewRow;
        private RadMenuItem mnuItemInsertCopiedRow;
        private RadMenuItem mnuItemDeleteRow;

        #region Properties
        bool IDocumentEditor.IsOpenReadOnly => openedAsReadOnly;
        string IDocumentEditor.SourceFile => tableSourceFile;
        string IDocumentEditor.Caption => this.titleBar1.Text;

        private DataTable RulesTable
        {
            get
            {
                DataTable? dataTable = this.dataSet.Tables[TableName.RULESTABLE];
                if (dataTable == null)
                    throw _exceptionHelper.CriticalException("{ECA3A18C-24FB-4951-A602-F912D1D602A7}");

                return dataTable;
            }
        }

        private DataTable RuleSetTable
        {
            get
            {
                DataTable? dataTable = this.dataSet.Tables[TableName.RULESETTABLE];
                if (dataTable == null)
                    throw _exceptionHelper.CriticalException("{A360E400-7A39-4EE5-9564-EC84288E4CCC}");

                return dataTable;
            }
        }
        #endregion Properties

        #region Methods
        private void AddNewRow()
        {
            RulesTable.Rows.Add(RulesTable.NewRow());
        }

        private bool AllActiveChecked()
        {
            foreach (DataRow row in RulesTable.Rows)
            {
                _ = bool.TryParse(row[TableColumns.ACTIVECOLUMNINDEX].ToString(), out bool isChecked);
                if (!isChecked)
                    return false;
            }
            return true;
        }

        private bool AllReevaluateChecked()
        {
            foreach (DataRow row in RulesTable.Rows)
            {
                _ = bool.TryParse(row[TableColumns.REEVALUATECOLUMNINDEX].ToString(), out bool isChecked);
                if (!isChecked)
                    return false;
            }
            return true;
        }

        private void CheckDocumentAvailability()
        {
            FileStream? fStream = null;
            try
            {//throws IOException if the file is opened for editing
                if (openedAsReadOnly)
                    fStream = new FileStream(this.tableSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                else
                    fStream = new FileStream(this.tableSourceFile, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            finally
            {
                if (fStream != null)
                    fStream.Close();
            }
        }

        private void CheckMenuItem()
        {
            if (RuleSetTable.Rows.Count == 0)
                RuleSetTable.Rows.Add(RuleSetTable.NewRow());
            
            string chainingBehaviour = RuleSetTable.Rows[0][TableColumns.CHAININGCOLUMNINDEX].ToString()!;

            RuleChainingBehavior ruleChainingBehavior;
            if (Enum.IsDefined(typeof(RuleChainingBehavior), chainingBehaviour))
            {
                ruleChainingBehavior = (RuleChainingBehavior)Enum.Parse(typeof(RuleChainingBehavior), chainingBehaviour);
            }
            else
            {
                RuleSetTable.Rows[0][TableColumns.CHAININGCOLUMNINDEX] = Enum.GetName(typeof(RuleChainingBehavior), RuleChainingBehavior.Full);
                ruleChainingBehavior = RuleChainingBehavior.Full;
            }

            SetChainingMenuItems(ruleChainingBehavior);
        }

        private void ClearSelection()
        {
            dataGridView1.ClearSelection();
            dataGridView1.CurrentRow = null;
        }

        private DataRow CloneDataRow(DataRow row)
        {
            DataRow newRow = RulesTable.NewRow();
            for (int i = 0; i < COLUMNCOUNT; i++)
                newRow[i] = row[i];

            return newRow;
        }

        private void CloseControl()
        {
            SaveAndReleaseDocument();
            this.parentForm.RemoveEditControl();
            this.parentForm.CommandBar.Enabled = false;
            this.parentForm.SetEditControlMenuStates(false, false);
        }

        private void CopyCell()
        {
            if (dataGridView1.CurrentCell == null)
                throw _exceptionHelper.CriticalException("{D4D158A8-7B9E-4D8E-8B06-E9BAB74742AF}");

            copiedRow = null;

            copiedCell = new Cell(dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex);
        }

        private DataRow CopyGridRow(GridViewRowInfo row)
        {
            DataRow newRow = this.dataSet.Tables[TableName.RULESTABLE]!.NewRow();

            for (int i = 0; i < COLUMNCOUNT; i++)
                newRow[i] = row.Cells[i].Value;

            return newRow;
        }

        private void CopyRow()
        {
            if (dataGridView1.CurrentCell == null)
                throw _exceptionHelper.CriticalException("{7E51CC41-0F01-4127-9293-4C1863173018}");

            copiedCell = null;

            copiedRow = CopyGridRow(dataGridView1.CurrentCell.RowInfo);
        }

        [MemberNotNull(nameof(mnuFunctions),
            nameof(mnuItemCopyCell),
            nameof(mnuItemCopyRow),
            nameof(mnuItemPasteCell),
            nameof(mnuItemPasteRow),
            nameof(mnuItemAddNewRow),
            nameof(mnuItemInsertNewRow),
            nameof(mnuItemInsertCopiedRow),
            nameof(mnuItemDeleteRow))]
        private void CreateContextMenus()
        {
            mnuFunctions = new RadContextMenu();

            mnuItemCopyCell = new RadMenuItem(Strings.mnuItemCopyCellText);
            mnuItemCopyCell.Click += MnuItemCopyCell_Click;

            mnuItemCopyRow = new RadMenuItem(Strings.mnuItemCopyRowText);
            mnuItemCopyRow.Click += MnuItemCopyRow_Click;

            mnuItemPasteCell = new RadMenuItem(Strings.mnuItemPasteCellText);
            mnuItemPasteCell.Click += MnuItemPasteCell_Click;
            mnuItemPasteRow = new RadMenuItem(Strings.mnuItemPasteRowText);
            mnuItemPasteRow.Click += MnuItemPasteRow_Click;

            mnuItemAddNewRow = new RadMenuItem(Strings.mnuItemAddNewRowText);
            mnuItemAddNewRow.Click += MnuItemAddNewRow_Click;
            mnuItemInsertNewRow = new RadMenuItem(Strings.mnuItemInsertNewRowText);
            mnuItemInsertNewRow.Click += MnuItemInsertNewRow_Click;
            mnuItemInsertCopiedRow = new RadMenuItem(Strings.mnuItemInsertCopiedRowText);
            mnuItemInsertCopiedRow.Click += MnuItemInsertCopiedRow_Click;

            mnuItemDeleteRow = new RadMenuItem(Strings.mnuItemDeleteRowText);
            mnuItemDeleteRow.Click += MnuItemDeleteRow_Click;

            mnuFunctions.Items.Add(mnuItemCopyCell);
            mnuFunctions.Items.Add(mnuItemCopyRow);
            mnuFunctions.Items.Add(new RadMenuSeparatorItem());
            mnuFunctions.Items.Add(mnuItemPasteCell);
            mnuFunctions.Items.Add(mnuItemPasteRow);
            mnuFunctions.Items.Add(new RadMenuSeparatorItem());
            mnuFunctions.Items.Add(mnuItemAddNewRow);
            mnuFunctions.Items.Add(mnuItemInsertNewRow);
            mnuFunctions.Items.Add(mnuItemInsertCopiedRow);
            mnuFunctions.Items.Add(new RadMenuSeparatorItem());
            mnuFunctions.Items.Add(mnuItemDeleteRow);
        }

        private void DeleteSelectedRow()
        {
            if (dataGridView1.CurrentCell == null)
                throw _exceptionHelper.CriticalException("{50CE5ED0-B705-45D5-A253-455A4AF45370}");

            //Deleting from the data set won't work because of deleted rows
            //still exist in the data set until saved causing index mismatches.
            dataGridView1.Rows.Remove(dataGridView1.CurrentCell.RowInfo);
        }

        private void DoSave()
        {
            try
            {
                if (dataGridView1.CurrentCell?.ColumnIndex == TableColumns.REEVALUATECOLUMNINDEX
                    || dataGridView1.CurrentCell?.ColumnIndex == TableColumns.ACTIVECOLUMNINDEX)
                {
                    /*//This code does not seem useful.  Maybe the state was not persisted in System.Windows.Forms.DataGridView 
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    //get the row index before clearing the selection
                    ClearSelection();
                    dataGridView1.Rows[rowIndex].Cells[TableColumns.CONDITIONCOLUMNINDEX].IsSelected = true;
                    */
                }

                if (dataGridView1.CurrentCell?.IsInEditMode == true)
                    bindingSource1.CurrencyManager.EndCurrentEdit();
            }
            catch (NoNullAllowedException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(new LogicBuilderException(ex.Message, ex));
                return;
            }

            if (!this.dataSet.HasChanges())
                return;

            try
            {
                this.dataSet.AcceptChanges();
                using XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFormattedXmlWriter(tableSourceFile, Encoding.Unicode);
                this.dataSet.WriteXml(xmlTextWriter);
                xmlTextWriter.Close();
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        private void EditCell()
        {
            if (dataGridView1.CurrentCell == null)
                return;

            if (dataGridView1.CurrentCell.ColumnIndex == TableColumns.REEVALUATECOLUMNINDEX
                || dataGridView1.CurrentCell.ColumnIndex == TableColumns.ACTIVECOLUMNINDEX)
                return;

            this.Cursor = Cursors.WaitCursor;
            //CellEditor cellEditor = new
            //(
            //    dataGridView1.CurrentRow.Cells[dataGridView1.CurrentColumn.Index],
            //    this.dataSet,
            //    this,
            //    this.parentForm
            //);
            //cellEditor.EditCell();
            this.Cursor = Cursors.Default;
        }

        private void FindAndReplaceConstructor()
        {
            //using (FindReplaceConstructorInCell findText = new FindReplaceConstructorInCell(this.dataGridView1, this.dataSet)
            //{
            //    StartPosition = FormStartPosition.Manual,
            //    Location = new Point(100, 50)
            //})
            //{
            //    findText.ShowDialog(this);
            //    Save();
            //}
        }

        private void FindAndReplaceFunction()
        {
            //using (FindReplaceFunctionInCell findText = new FindReplaceFunctionInCell(this.dataGridView1, this.dataSet)
            //{
            //    StartPosition = FormStartPosition.Manual,
            //    Location = new Point(100, 50)
            //})
            //{
            //    findText.ShowDialog(this);
            //    Save();
            //}
        }

        private void FindAndReplaceText()
        {
            //using (FindReplaceCellText findText = new FindReplaceCellText(this.dataGridView1, this.dataSet)
            //{
            //    StartPosition = FormStartPosition.CenterParent
            //})
            //{
            //    findText.ShowDialog(this);
            //    Save();
            //}
        }

        private void FindAndReplaceVariable()
        {
            //using (FindReplaceVariableInCell findText = new FindReplaceVariableInCell(this.dataGridView1, this.dataSet)
            //{
            //    StartPosition = FormStartPosition.Manual,
            //    Location = new Point(100, 50)
            //})
            //{
            //    findText.ShowDialog(this);
            //    Save();
            //}
        }

        private void FindCell()
        {
            using IScopedDisposableManager<FindCell> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<FindCell>>();
            FindCell findCell = disposableManager.ScopedService;
            findCell.StartPosition = FormStartPosition.Manual;
            findCell.Location = new Point(100, 50);
            findCell.Setup(dataGridView1);
            findCell.ShowDialog(this);
        }

        /// <summary>
        /// Selects a cell given the 1-based row index and 1-based column index as seen by tye user
        /// </summary>
        /// <param name="userRowIndex">1st row index is 1</param>
        /// <param name="userColumnIndex">1st column index is 1</param>
        public void FindCell(int userRowIndex, int userColumnIndex)
        {
            if (dataGridView1.Rows.Count < userRowIndex)
            {
                DisplayMessage.Show
                (
                    (IWin32Window)this.parentForm,
                    string.Format(CultureInfo.CurrentCulture, Strings.rowIndexIsInvalidFormat, userRowIndex),
                    string.Empty,
                    this._mainWindow.RightToLeft
                );
                return;
            }

            if (userColumnIndex > TableColumns.ACTIVECOLUMNINDEXUSER || userColumnIndex < TableColumns.CONDITIONCOLUMNINDEXUSER)
            {
                DisplayMessage.Show
                (
                    (IWin32Window)this.parentForm,
                    string.Format(CultureInfo.CurrentCulture, Strings.columnIndexIsInvalidFormat, userColumnIndex),
                    string.Empty,
                    this._mainWindow.RightToLeft
                );
                return;
            }

            int rowIndex = userRowIndex - 1;
            var columnIndex = userColumnIndex switch
            {
                TableColumns.CONDITIONCOLUMNINDEXUSER => TableColumns.CONDITIONCOLUMNINDEX,
                TableColumns.ACTIONCOLUMNINDEXUSER => TableColumns.ACTIONCOLUMNINDEX,
                TableColumns.PRIORITYCOLUMNINDEXUSER => TableColumns.PRIORITYCOLUMNINDEX,
                TableColumns.REEVALUATECOLUMNINDEXUSER => TableColumns.REEVALUATECOLUMNINDEX,
                TableColumns.ACTIVECOLUMNINDEXUSER => TableColumns.ACTIVECOLUMNINDEX,
                _ => throw _exceptionHelper.CriticalException("{DADB78A7-A670-4D1C-ABCA-B97187DAD7B6}"),
            };
            ClearSelection();
            dataGridView1.Rows[rowIndex].IsCurrent = true;
            dataGridView1.Columns[columnIndex].IsCurrent = true;
            dataGridView1.Rows[rowIndex].Cells[columnIndex].EnsureVisible();
        }

        private void FindConstructor()
        {
            //using (FindConstructorInCell findText = new FindConstructorInCell(this.dataGridView1)
            //{
            //    StartPosition = FormStartPosition.Manual,
            //    Location = new Point(100, 50)
            //})
            //{
            //    findText.ShowDialog(this);
            //}
        }

        private void FindFunction()
        {
            //using (FindFunctionInCell findText = new FindFunctionInCell(this.dataGridView1)
            //{
            //    StartPosition = FormStartPosition.Manual,
            //    Location = new Point(100, 50)
            //})
            //{
            //    findText.ShowDialog(this);
            //}
        }

        private void FindText()
        {
            //using (TableFindText findText = new TableFindText(this.dataGridView1)
            //{
            //    StartPosition = FormStartPosition.CenterParent
            //})
            //{
            //    findText.ShowDialog(this);
            //}
        }

        private void FindVariable()
        {
            //using (FindVariableInCell findText = new FindVariableInCell(this.dataGridView1)
            //{
            //    StartPosition = FormStartPosition.Manual,
            //    Location = new Point(100, 50)
            //})
            //{
            //    findText.ShowDialog(this);
            //}
        }

        private DataSet GetEmptyDataset()
        {
            StringBuilder stringBuilder = new();
            using XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFormattedXmlWriter(stringBuilder);
            DataSet ds = new(XmlDataConstants.TABLESELEMENT)
            {
                Locale = CultureInfo.InvariantCulture
            };

            StringReader stringReader = new(Schemas.GetSchema(Schemas.TableSchema));
            try
            {
                ds.ReadXmlSchema(stringReader);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            stringReader.Close();

            xmlTextWriter.WriteElementString(XmlDataConstants.TABLESELEMENT, string.Empty);

            xmlTextWriter.Flush();
            xmlTextWriter.Close();

            XmlTextReader xmlReader = new(stringBuilder.ToString(), XmlNodeType.Element, null);
            ds.ReadXml(xmlReader);
            return ds;
        }

        private int GetRowHeight(int rowIndex)
        {
            if (dataGridView1.Rows[rowIndex].Cells[TableColumns.CONDITIONCOLUMNINDEXVISIBLE].Value.ToString()!.Length == 0
                && dataGridView1.Rows[rowIndex].Cells[TableColumns.ACTIONCOLUMNINDEXVISIBLE].Value.ToString()!.Length == 0)
                return MINIMUMROWHEIGHT;

            int height = MINIMUMROWHEIGHT;
            string[] items = dataGridView1.Rows[rowIndex].Cells[TableColumns.ACTIONCOLUMNINDEXVISIBLE].Value.ToString()!.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            height = height > ROWHEIGHT * (items.Length + 1) ? height : ROWHEIGHT * (items.Length + 1);
            items = dataGridView1.Rows[rowIndex].Cells[TableColumns.CONDITIONCOLUMNINDEXVISIBLE].Value.ToString()!.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            height = height > ROWHEIGHT * (items.Length + 1) ? height : ROWHEIGHT * (items.Length + 1);
            return height;
        }

        void IDocumentEditor.Close() => CloseControl();

        void IDocumentEditor.Delete() => DeleteSelectedRow();

        void IDocumentEditor.Edit() => EditCell();

        void IDocumentEditor.FindText() => FindText();

        void IDocumentEditor.FindConstructor() => FindConstructor();

        void IDocumentEditor.FindFunction() => FindFunction();

        void IDocumentEditor.FindVariable() => FindVariable();

        void IDocumentEditor.ReplaceText() => FindAndReplaceText();

        void IDocumentEditor.ReplaceConstructor() => FindAndReplaceConstructor();

        void IDocumentEditor.ReplaceFunction() => FindAndReplaceFunction();

        void IDocumentEditor.ReplaceVariable() => FindAndReplaceVariable();

        void IDocumentEditor.Save() => Save();

        [MemberNotNull(nameof(bindingSource1), 
            nameof(mnuFunctions),
            nameof(mnuItemCopyCell),
            nameof(mnuItemCopyRow),
            nameof(mnuItemPasteCell),
            nameof(mnuItemPasteRow),
            nameof(mnuItemAddNewRow),
            nameof(mnuItemInsertNewRow),
            nameof(mnuItemInsertCopiedRow),
            nameof(mnuItemDeleteRow))]
        private void Initialize()
        {
            parentForm.RadMenuItemIndexInformation.Enabled = false;
            parentForm.CommandBar.Enabled = true;
            _formInitializer.SetToolTipDefaults(toolTip);

            this.parentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);
            this.titleBar1.CloseClick += new Components.TitleBar.CloseClickHandler(TitleBar1_CloseClick);

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.ClipboardCopyMode = GridViewClipboardCopyMode.Disable;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = GridViewSelectionMode.CellSelect;
            dataGridView1.ShowGroupPanel = false;
            dataGridView1.AllowEditRow = false;
            dataGridView1.AllowDragToGroup = false;
            dataGridView1.AllowAddNewRow = false;
            dataGridView1.AllowDeleteRow = false;
            dataGridView1.AllowColumnReorder = false;
            dataGridView1.AllowMultiColumnSorting = false;

            dataGridView1.MasterTemplate.BestFitColumns(BestFitColumnMode.HeaderCells);
            dataGridView1.TableElement.RowHeaderColumnWidth = ROWHEADERSWIDTH;
            dataGridView1.Columns[TableColumns.CONDITIONCOLUMNINDEX].Width = CONDITIONCOLUMNWIDTH;
            dataGridView1.Columns[TableColumns.CONDITIONCOLUMNINDEX].AutoSizeMode = BestFitColumnMode.None;
            dataGridView1.Columns[TableColumns.ACTIONCOLUMNINDEX].Width = ACTIONCOLUMNWIDTH;
            dataGridView1.Columns[TableColumns.ACTIONCOLUMNINDEX].AutoSizeMode = BestFitColumnMode.None;
            dataGridView1.Columns[TableColumns.PRIORITYCOLUMNINDEX].Width = PRIORITYCOLUMNWIDTH;
            dataGridView1.Columns[TableColumns.PRIORITYCOLUMNINDEX].AutoSizeMode = BestFitColumnMode.None;
            dataGridView1.Columns[TableColumns.REEVALUATECOLUMNINDEX].AutoSizeMode = BestFitColumnMode.HeaderCells;
            dataGridView1.Columns[TableColumns.ACTIVECOLUMNINDEX].AutoSizeMode = BestFitColumnMode.HeaderCells;

            foreach (GridViewDataColumn column in dataGridView1.Columns)
                column.AllowSort = false;

            bindingSource1 = new BindingSource();
            dataGridView1.DataSource = bindingSource1;
            CreateContextMenus();
        }

        private void InsertCopiedRow()
        {
            if (dataGridView1.CurrentCell == null)
                throw _exceptionHelper.CriticalException("{C8167C73-D442-41B4-9CAB-D3931D4A40E9}");

            if (copiedRow == null)
                return;

            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            RulesTable.Rows.InsertAt(copiedRow, rowIndex);

            copiedRow = CloneDataRow(copiedRow);
        }

        private void InsertNewRow()
        {
            if (dataGridView1.CurrentCell == null)
                throw _exceptionHelper.CriticalException("{54A4283C-4977-4784-A9F4-602AE5057C96}");

            int rowIndex = dataGridView1.CurrentCell.RowIndex;

            RulesTable.Rows.InsertAt(RulesTable.NewRow(), rowIndex);
        }

        void ITableControl.FindCell() => FindCell();

        void ITableControl.SetEvaluationFull()
        {
            SetChainingMenuItems(RuleChainingBehavior.Full);
            UpdateRuleSetTable();
        }

        void ITableControl.SetEvaluationNone()
        {
            SetChainingMenuItems(RuleChainingBehavior.None);
            UpdateRuleSetTable();
        }

        void ITableControl.SetEvaluationUpdateOnly()
        {
            SetChainingMenuItems(RuleChainingBehavior.UpdateOnly);
            UpdateRuleSetTable();
        }

        void ITableControl.ToggleReevaluateAll() => ToggleReevaluateAll();

        void ITableControl.ToggleActivateAll() => ToggleActivateAll();

        [MemberNotNull(nameof(dataSet))]
        private void LoadSourceFile()
        {
            this.dataSet = new()
            {
                Locale = CultureInfo.InvariantCulture
            };

            StringReader stringReader = new(Schemas.GetSchema(Schemas.TableSchema));
            try
            {
                this.dataSet.ReadXmlSchema(stringReader);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            stringReader.Close();
            try
            {
                this.dataSet.ReadXml(this.tableSourceFile);
            }
            catch (ConstraintException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(new LogicBuilderException(ex.Message, ex));
                this.dataSet = GetEmptyDataset();
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }

            RulesTable.Columns[TableColumns.CONDITIONCOLUMNINDEX].DefaultValue = string.Empty;
            RulesTable.Columns[TableColumns.CONDITIONCOLUMNINDEXVISIBLE].DefaultValue = string.Empty;
            RulesTable.Columns[TableColumns.ACTIONCOLUMNINDEX].DefaultValue = string.Empty;
            RulesTable.Columns[TableColumns.ACTIONCOLUMNINDEXVISIBLE].DefaultValue = string.Empty;
            RulesTable.Columns[TableColumns.PRIORITYCOLUMNINDEX].DefaultValue = string.Empty;
            RulesTable.Columns[TableColumns.PRIORITYCOLUMNINDEXVISIBLE].DefaultValue = string.Empty;
            RulesTable.Columns[TableColumns.REEVALUATECOLUMNINDEX].DefaultValue = true;
            RulesTable.Columns[TableColumns.ACTIVECOLUMNINDEX].DefaultValue = true;

            bindingSource1.DataSource = this.dataSet;
            bindingSource1.DataMember = this.dataSet.Tables[TableName.RULESTABLE]!.TableName;

            RuleSetTable.Columns[TableColumns.CHAININGCOLUMNINDEX].DefaultValue = Enum.GetName(typeof(RuleChainingBehavior), RuleChainingBehavior.Full);

            this.dataSet.AcceptChanges();

            CheckMenuItem();
        }

        private void LockDocument()
        {
            try
            {
                if (openedAsReadOnly)
                    fileStream = new FileStream(this.tableSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                else
                    fileStream = new FileStream(this.tableSourceFile, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        private void PasteCell()
        {
            if (dataGridView1.CurrentCell == null)
                throw _exceptionHelper.CriticalException("{2CBA48A0-6CC7-4EFE-AF4E-E22E2C16FE5C}");

            if (dataGridView1.CurrentCell.ColumnIndex == TableColumns.REEVALUATECOLUMNINDEX
                || dataGridView1.CurrentCell.ColumnIndex == TableColumns.ACTIVECOLUMNINDEX)
            {
                PasteCheckBoxCell();
                return;
            }

            if (copiedCell == null)
                return;

            if (dataGridView1.CurrentCell.ColumnIndex != copiedCell.ColumnIndex)
                return;

            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            int columnIndex = dataGridView1.CurrentCell.ColumnIndex;

            RulesTable.Rows[rowIndex][columnIndex + 1] = dataGridView1.Rows[copiedCell.RowIndex].Cells[columnIndex + 1].Value;
            RulesTable.Rows[rowIndex][columnIndex] = dataGridView1.Rows[copiedCell.RowIndex].Cells[columnIndex].Value;
        }

        private void PasteCheckBoxCell()
        {
            if (copiedCell == null)
                return;

            if (dataGridView1.CurrentCell == null)
                throw _exceptionHelper.CriticalException("{8788F3E1-FFBB-4402-A44E-8F9D8AC76E0E}");

            if (dataGridView1.CurrentCell.ColumnIndex != copiedCell.ColumnIndex)
                return;

            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            int columnIndex = dataGridView1.CurrentCell.ColumnIndex;

            RulesTable.Rows[rowIndex][columnIndex] = dataGridView1.Rows[copiedCell.RowIndex].Cells[columnIndex].Value;
        }

        private void PasteRow()
        {
            if (dataGridView1.CurrentCell == null)
                throw _exceptionHelper.CriticalException("{683ADE89-026B-4905-A988-DE318F737AC2}");

            if (copiedRow == null)
                return;

            int rowIndex = dataGridView1.CurrentCell.RowIndex;

            for (int i = 0; i < this.dataSet.Tables[TableName.RULESTABLE]!.Columns.Count; i++)
                RulesTable.Rows[rowIndex][i] = copiedRow[i];
        }

        private void Save()
        {
            try
            {
                SaveStayOpen();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
        private void SaveAndReleaseDocument()
        {
            if (this.parentForm == null)
                return;

            if (this.parentForm.EditControl != this)
                return;

            if (!openedAsReadOnly && this.dataSet.HasChanges())
            {
                DialogResult dialogResult = DisplayMessage.ShowYesNo
                (
                    (IWin32Window)this.parentForm,
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.saveFormQuestionFormat,
                        _pathHelper.GetFileName(this.tableSourceFile)
                    ),
                    string.Empty,
                    _mainWindow.RightToLeft
                );
                if (dialogResult == DialogResult.Yes)
                    SaveOnClose();
                else
                    UnlockDocument();
            }
            else
            {
                UnlockDocument();
            }
        }

        private void SaveOnClose()
        {
            try
            {
                UnlockDocument();

                if (openedAsReadOnly)
                    return;

                DoSave();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }

        private void SaveStayOpen()
        {
            if (openedAsReadOnly)
                return;

            GridDataRowElement? firstRowElement = dataGridView1.TableElement.VisualRows.OfType<GridDataRowElement>().FirstOrDefault();
            int topRow = GetTopIndex(firstRowElement);
            int topColumn = GetFirstColumn(firstRowElement);

            int selectedRowIndex = dataGridView1.CurrentCell?.RowIndex ?? -1;
            int selectedColumnIndex = dataGridView1.CurrentCell?.ColumnIndex ?? -1;

            UnlockDocument();
            DoSave();
            LockDocument();

            ClearSelection();

            if (selectedRowIndex > -1 && selectedColumnIndex > -1)
            {
                dataGridView1.Rows[selectedRowIndex].IsCurrent = true;
                dataGridView1.Columns[selectedColumnIndex].IsCurrent = true;
                dataGridView1.Rows[selectedRowIndex].Cells[selectedColumnIndex].EnsureVisible();
            }
            else if (selectedRowIndex != -1)
            {
                dataGridView1.CurrentRow = dataGridView1.Rows[selectedRowIndex];
                dataGridView1.Rows[selectedRowIndex].EnsureVisible();
            }
            else if (dataGridView1.Rows.Count > topRow && topRow > -1)
            {
                dataGridView1.Rows[topRow].Cells[topColumn].EnsureVisible();
            }

            int GetTopIndex(GridDataRowElement? gridRowElement)
                => gridRowElement == null ? 0 : gridRowElement.RowInfo.Index;

            int GetFirstColumn(GridDataRowElement? gridRowElement)
            {
                if (gridRowElement == null)
                    return 0;

                var cellElement = gridRowElement.VisualCells.OfType<GridDataCellElement>().FirstOrDefault();
                if (cellElement == null)
                    return 0;

                return cellElement.ColumnIndex;
            }
        }

        void SetChainingMenuItems(RuleChainingBehavior ruleChainingBehavior)
        {
            if (
                !new HashSet<RuleChainingBehavior> 
                {
                    RuleChainingBehavior.Full,
                    RuleChainingBehavior.None,
                    RuleChainingBehavior.UpdateOnly
                }.Contains(ruleChainingBehavior)
               )
                throw _exceptionHelper.CriticalException("{1BECA964-6489-433A-907B-82A6FA6E4269}");

            this.parentForm.RadMenuItemFullChaining.IsChecked = ruleChainingBehavior == RuleChainingBehavior.Full;
            this.parentForm.RadMenuItemNoneChaining.IsChecked = ruleChainingBehavior == RuleChainingBehavior.None;
            this.parentForm.RadMenuItemUpdateOnlyChaining.IsChecked = ruleChainingBehavior == RuleChainingBehavior.UpdateOnly;
        }
        private void SetContextMenuState()
        {
            if (this.openedAsReadOnly)
            {
                foreach (RadMenuItem item in mnuFunctions.Items)
                    item.Enabled = false;

                return;
            }

            mnuItemCopyCell.Enabled = dataGridView1.CurrentCell != null
                                        && dataGridView1.SelectedCells.Count == 1;

            mnuItemCopyRow.Enabled = dataGridView1.CurrentCell != null;

            mnuItemPasteCell.Enabled = copiedCell != null
                                        && dataGridView1.SelectedCells.Count == 1
                                        && dataGridView1.CurrentCell?.ColumnIndex == copiedCell.ColumnIndex;

            mnuItemPasteRow.Enabled = copiedRow != null && dataGridView1.CurrentCell != null;

            mnuItemInsertNewRow.Enabled = dataGridView1.CurrentCell != null;

            mnuItemInsertCopiedRow.Enabled = copiedRow != null && dataGridView1.CurrentCell != null;

            mnuItemDeleteRow.Enabled = dataGridView1.CurrentCell != null;

            this.parentForm.RadMenuItemDelete.Enabled = dataGridView1.CurrentCell != null;
        }

        private void ToggleActivateAll()
        {
            if (AllActiveChecked())
            {
                foreach (DataRow row in RulesTable.Rows)
                    row[TableColumns.ACTIVECOLUMNINDEX] = false;
            }
            else
            {
                foreach (DataRow row in RulesTable.Rows)
                    row[TableColumns.ACTIVECOLUMNINDEX] = true;
            }
        }

        private void ToggleReevaluateAll()
        {
            if (AllReevaluateChecked())
            {
                foreach (DataRow row in RulesTable.Rows)
                    row[TableColumns.REEVALUATECOLUMNINDEX] = false;
            }
            else
            {
                foreach (DataRow row in RulesTable.Rows)
                    row[TableColumns.REEVALUATECOLUMNINDEX] = true;
            }
        }

        private void UnlockDocument()
        {
            if (fileStream != null)
                fileStream.Close();
        }

        private void UpdateRuleSetTable()
        {
            if (RuleSetTable.Rows.Count == 0)
                RuleSetTable.Rows.Add(RuleSetTable.NewRow());

            if (this.parentForm.RadMenuItemFullChaining.IsChecked)
                RuleSetTable.Rows[0][TableColumns.CHAININGCOLUMNINDEX] = Enum.GetName(typeof(RuleChainingBehavior), RuleChainingBehavior.Full);
            else if (this.parentForm.RadMenuItemNoneChaining.IsChecked)
                RuleSetTable.Rows[0][TableColumns.CHAININGCOLUMNINDEX] = Enum.GetName(typeof(RuleChainingBehavior), RuleChainingBehavior.None);
            else if (this.parentForm.RadMenuItemUpdateOnlyChaining.IsChecked)
                RuleSetTable.Rows[0][TableColumns.CHAININGCOLUMNINDEX] = Enum.GetName(typeof(RuleChainingBehavior), RuleChainingBehavior.UpdateOnly);
            else
                throw _exceptionHelper.CriticalException("{3A9381A0-2C21-4F98-BCD2-2A9F54C635E6}");
        }
        #endregion Methods

        #region Event Handlers
        private void MnuItemCopyCell_Click(object? mnuItem, EventArgs e)
        {
            CopyCell();
        }

        private void MnuItemCopyRow_Click(object? mnuItem, EventArgs e)
        {
            CopyRow();
        }

        private void MnuItemPasteCell_Click(object? mnuItem, EventArgs e)
        {
            PasteCell();
        }

        private void MnuItemPasteRow_Click(object? mnuItem, EventArgs e)
        {
            PasteRow();
        }

        private void MnuItemAddNewRow_Click(object? mnuItem, EventArgs e)
        {
            AddNewRow();
        }

        private void MnuItemInsertNewRow_Click(object? mnuItem, EventArgs e)
        {
            InsertNewRow();
        }

        private void MnuItemInsertCopiedRow_Click(object? mnuItem, EventArgs e)
        {
            InsertCopiedRow();
        }

        private void MnuItemDeleteRow_Click(object? mnuItem, EventArgs e)
        {
            DeleteSelectedRow();
        }

        void ParentForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            SaveAndReleaseDocument();
        }

        void TitleBar1_CloseClick()
        {
            CloseControl();
        }

        private void DataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SetContextMenuState();
                //need to be set in MouseUp not MouseDown
                //because dataGridView1.SelectedCells can be different
                //in MouseDown
                //Also note that the behavior is inconsistent if
                //the code in MouseDown (ClearSelection() and select a cell)
                //is moved to MouseUp. Only some cell in the row appear selected (column[0] and the column selected in the previously selected row)
            }
        }

        private void DataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            var element = dataGridView1.ElementTree.GetElementAtPoint(e.Location);
            if (element is not GridDataCellElement
                && element is not GridRowHeaderCellElement)
            {
                ClearSelection();
                return;
            }
        }

        private void DataGridView1_DataError(object sender, GridViewDataErrorEventArgs e)
        {
            DisplayMessage.Show
            (
                (IWin32Window)this.parentForm,
                e.Exception.Message,
                _mainWindow.RightToLeft
            );
        }

        private void DataGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            EditCell();
        }

        private void DataGridView1_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement is not GridDataCellElement)
                return;

            if (e.ColumnIndex == TableColumns.REEVALUATECOLUMNINDEX || e.ColumnIndex == TableColumns.ACTIVECOLUMNINDEX)
                return;

            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value != null)
            {
                dataGridView1.Rows[e.RowIndex].Height = GetRowHeight(e.RowIndex);
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].Height = MINIMUMROWHEIGHT;
            }

            e.CellElement.Text = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value.ToString();
            //We are using cell formatting to display the visible text from the next column - the visible text column
        }

        private void DataGridView1_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement is not GridRowHeaderCellElement gridRowHeaderCellElement
                || e.RowIndex == -1)
            {
                return;
            }

            if (e.Row.IsCurrent)
            {
                e.CellElement.ImageLayout = ImageLayout.None;
                gridRowHeaderCellElement.ImageAlignment = ContentAlignment.MiddleLeft;
            }

            gridRowHeaderCellElement.Text = (e.RowIndex + 1).ToString(CultureInfo.CurrentCulture);
            gridRowHeaderCellElement.ShouldHandleMouseInput = true;//set to false to disable selecting the row from the header
            gridRowHeaderCellElement.NotifyParentOnMouseInput = true;//Also set to false when ShouldHandleMouseInput is false
        }

        private void DataGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            
            if (e.ColumnIndex == TableColumns.REEVALUATECOLUMNINDEX 
                || e.ColumnIndex == TableColumns.ACTIVECOLUMNINDEX)
            {
                _ = bool.TryParse(RulesTable.Rows[e.RowIndex][e.ColumnIndex].ToString(), out bool boolValue);
                RulesTable.Rows[e.RowIndex][e.ColumnIndex] = !boolValue;
            }
        }

        private void DataGridView1_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.ContextMenu = mnuFunctions.DropDown;
        }
        #endregion Event Handlers

        private class Cell
        {
            internal Cell(int rowIndex, int columnIndex)
            {
                this.RowIndex = rowIndex;
                this.ColumnIndex = columnIndex;
            }

            #region Properties
            internal int RowIndex { get; }
            internal int ColumnIndex { get; }
            #endregion Properties
        }
    }
}
