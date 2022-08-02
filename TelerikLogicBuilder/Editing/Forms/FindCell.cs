using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Forms
{
    internal partial class FindCell : RadForm
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly DialogFormMessageControl _dialogFormMessageControl;
        private RadGridView? dataGridView;

        public FindCell(
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            DialogFormMessageControl dialogFormMessageControl)
        {
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _dialogFormMessageControl = dialogFormMessageControl;
            InitializeComponent();
            InitializeDialogFormMessageControl();
            Initialize();
        }

        #region Properties
        private int ColumnIndex 
            => int.Parse(radTextBoxColumnIndex.Text.Trim(), CultureInfo.CurrentCulture);

        private int RowIndex 
            => int.Parse(radTextBoxRowIndex.Text.Trim(), CultureInfo.CurrentCulture);
        #endregion Properties

        #region Methods
        public void Setup(RadGridView dataGridView)
        {
            this.dataGridView = dataGridView;
        }

        private void Find(int userRowIndex, int userColumnIndex)
        {
            if (dataGridView == null)
                return;

            int rowIndex = userRowIndex - 1;
            var columnIndex = userColumnIndex switch
            {
                TableColumns.CONDITIONCOLUMNINDEXUSER => TableColumns.CONDITIONCOLUMNINDEX,
                TableColumns.ACTIONCOLUMNINDEXUSER => TableColumns.ACTIONCOLUMNINDEX,
                TableColumns.PRIORITYCOLUMNINDEXUSER => TableColumns.PRIORITYCOLUMNINDEX,
                TableColumns.REEVALUATECOLUMNINDEXUSER => TableColumns.REEVALUATECOLUMNINDEX,
                TableColumns.ACTIVECOLUMNINDEXUSER => TableColumns.ACTIVECOLUMNINDEX,
                _ => throw _exceptionHelper.CriticalException("{7D86FD09-50B9-4F28-A639-119856036DC6}"),
            };


            this.dataGridView.ClearSelection();
            this.dataGridView.CurrentRow = null;

            this.dataGridView.Rows[rowIndex].IsCurrent = true;
            this.dataGridView.Columns[columnIndex].IsCurrent = true;
            this.dataGridView.Rows[rowIndex].Cells[columnIndex].EnsureVisible();
        }

        private void Initialize()
        {
            this.AcceptButton = radButtonFind;
            _formInitializer.SetFormDefaults(this, 276);
            this.radButtonCancel.DialogResult = DialogResult.Cancel;
            ValidateOk();
        }

        private void InitializeDialogFormMessageControl()
        {
            ((ISupportInitialize)(this.radPanelBottom)).BeginInit();
            this.radPanelBottom.SuspendLayout();
            ((ISupportInitialize)(this.radPanelMessages)).BeginInit();
            this.radPanelMessages.SuspendLayout();
            this.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new System.Drawing.Point(0, 0);
            this.radPanelMessages.Controls.Add(_dialogFormMessageControl);

            ((ISupportInitialize)(this.radPanelBottom)).EndInit();
            this.radPanelBottom.ResumeLayout(false);
            ((ISupportInitialize)(this.radPanelMessages)).EndInit();
            this.radPanelMessages.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void ValidateOk()
        {
            if (radTextBoxRowIndex.Text.Trim().Length > 0 && !TryParse(radTextBoxRowIndex.Text, out _))
            {
                _dialogFormMessageControl.SetErrorMessage(Strings.integersOnly);
                radButtonFind.Enabled = false;
            }
            else if (radTextBoxColumnIndex.Text.Trim().Length > 0 && !TryParse(radTextBoxColumnIndex.Text, out _))
            {
                _dialogFormMessageControl.SetErrorMessage(Strings.integersOnly);
                radButtonFind.Enabled = false;
            }
            else if (TryParse(radTextBoxRowIndex.Text, out _) && TryParse(radTextBoxColumnIndex.Text, out _))
            {
                _dialogFormMessageControl.ClearMessage();
                radButtonFind.Enabled = true;
            }
            else if(radTextBoxRowIndex.Text.Trim().Length == 0 || radTextBoxColumnIndex.Text.Trim().Length == 0)
            {
                _dialogFormMessageControl.ClearMessage();
                radButtonFind.Enabled = false;
            }
            else
            {
                throw _exceptionHelper.CriticalException("{28FAB7E2-3F41-4207-B818-887395512F06}");
            }

            if (this.dataGridView == null)
                return;

            if (!TryParse(radTextBoxRowIndex.Text, out int userRowIndex) || !TryParse(radTextBoxColumnIndex.Text, out int userColumnIndex))
                return;

            if (dataGridView.Rows.Count < userRowIndex)
            {
                _dialogFormMessageControl.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.rowIndexIsInvalidFormat, userRowIndex));
                radButtonFind.Enabled = false;
                return;
            }

            if (userColumnIndex > TableColumns.ACTIVECOLUMNINDEXUSER || userColumnIndex < TableColumns.CONDITIONCOLUMNINDEXUSER)
            {
                _dialogFormMessageControl.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.columnIndexIsInvalidFormat, userColumnIndex));
                radButtonFind.Enabled = false;
                return;
            }
        }

        private static bool TryParse(string text, out int number)
            => int.TryParse(text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out number);
        #endregion Methods

        #region Event Handlers
        private void RadButtonFind_Click(object sender, EventArgs e)
        {
            Find(RowIndex, ColumnIndex);
        }

        private void RadButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadTextBoxRowIndex_TextChanged(object sender, EventArgs e)
        {
            ValidateOk();
        }

        private void RadTextBoxColumnIndex_TextChanged(object sender, EventArgs e)
        {
            ValidateOk();
        }
        #endregion Event Handlers
    }
}
