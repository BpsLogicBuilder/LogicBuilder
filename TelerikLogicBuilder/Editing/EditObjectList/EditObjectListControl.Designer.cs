namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList
{
    partial class EditObjectListControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radGroupBoxType = new Telerik.WinControls.UI.RadGroupBox();
            this.radScrollablePanelType = new Telerik.WinControls.UI.RadScrollablePanel();
            this.radPanelTableParent = new Telerik.WinControls.UI.RadPanel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.cmbListType = new Telerik.WinControls.UI.RadDropDownList();
            this.lblObjectType = new Telerik.WinControls.UI.RadLabel();
            this.lblListType = new Telerik.WinControls.UI.RadLabel();
            this.radGroupBoxEdit = new Telerik.WinControls.UI.RadGroupBox();
            this.radPanelEdit = new Telerik.WinControls.UI.RadPanel();
            this.radPanelAddButton = new Telerik.WinControls.UI.RadPanel();
            this.btnUpdate = new Telerik.WinControls.UI.RadButton();
            this.btnAdd = new Telerik.WinControls.UI.RadButton();
            this.radGroupBoxList = new Telerik.WinControls.UI.RadGroupBox();
            this.managedListBoxControl = new ABIS.LogicBuilder.FlowBuilder.UserControls.ManagedListBoxControl();
            this.cmbObjectType = new ABIS.LogicBuilder.FlowBuilder.UserControls.AutoCompleteRadDropDownList();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxType)).BeginInit();
            this.radGroupBoxType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanelType)).BeginInit();
            this.radScrollablePanelType.PanelContainer.SuspendLayout();
            this.radScrollablePanelType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTableParent)).BeginInit();
            this.radPanelTableParent.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbListType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblObjectType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblListType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxEdit)).BeginInit();
            this.radGroupBoxEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAddButton)).BeginInit();
            this.radPanelAddButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxList)).BeginInit();
            this.radGroupBoxList.SuspendLayout();
            this.SuspendLayout();
            // 
            // radGroupBoxType
            // 
            this.radGroupBoxType.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxType.Controls.Add(this.radScrollablePanelType);
            this.radGroupBoxType.Dock = System.Windows.Forms.DockStyle.Top;
            this.radGroupBoxType.HeaderText = "Type";
            this.radGroupBoxType.Location = new System.Drawing.Point(0, 0);
            this.radGroupBoxType.Name = "radGroupBoxType";
            this.radGroupBoxType.Size = new System.Drawing.Size(855, 137);
            this.radGroupBoxType.TabIndex = 0;
            this.radGroupBoxType.Text = "Type";
            // 
            // radScrollablePanelType
            // 
            this.radScrollablePanelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radScrollablePanelType.Location = new System.Drawing.Point(2, 18);
            this.radScrollablePanelType.Name = "radScrollablePanelType";
            // 
            // radScrollablePanelType.PanelContainer
            // 
            this.radScrollablePanelType.PanelContainer.Controls.Add(this.radPanelTableParent);
            this.radScrollablePanelType.PanelContainer.Size = new System.Drawing.Size(849, 115);
            this.radScrollablePanelType.Size = new System.Drawing.Size(851, 117);
            this.radScrollablePanelType.TabIndex = 0;
            // 
            // radPanelTableParent
            // 
            this.radPanelTableParent.Controls.Add(this.tableLayoutPanel);
            this.radPanelTableParent.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanelTableParent.Location = new System.Drawing.Point(0, 0);
            this.radPanelTableParent.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelTableParent.Name = "radPanelTableParent";
            this.radPanelTableParent.Size = new System.Drawing.Size(849, 112);
            this.radPanelTableParent.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tableLayoutPanel.Controls.Add(this.cmbListType, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.lblObjectType, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.lblListType, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.cmbObjectType, 2, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(849, 112);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // cmbListType
            // 
            this.cmbListType.AutoSize = false;
            this.cmbListType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbListType.DropDownAnimationEnabled = true;
            this.cmbListType.Location = new System.Drawing.Point(291, 23);
            this.cmbListType.Name = "cmbListType";
            this.cmbListType.Size = new System.Drawing.Size(528, 24);
            this.cmbListType.TabIndex = 1;
            // 
            // lblObjectType
            // 
            this.lblObjectType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblObjectType.Location = new System.Drawing.Point(28, 59);
            this.lblObjectType.Name = "lblObjectType";
            this.lblObjectType.Size = new System.Drawing.Size(69, 18);
            this.lblObjectType.TabIndex = 2;
            this.lblObjectType.Text = "Object Type:";
            // 
            // lblListType
            // 
            this.lblListType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblListType.Location = new System.Drawing.Point(28, 23);
            this.lblListType.Name = "lblListType";
            this.lblListType.Size = new System.Drawing.Size(257, 18);
            this.lblListType.TabIndex = 0;
            this.lblListType.Text = "List Type:";
            // 
            // radGroupBoxEdit
            // 
            this.radGroupBoxEdit.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxEdit.Controls.Add(this.radPanelEdit);
            this.radGroupBoxEdit.Controls.Add(this.radPanelAddButton);
            this.radGroupBoxEdit.Dock = System.Windows.Forms.DockStyle.Top;
            this.radGroupBoxEdit.HeaderText = "Edit";
            this.radGroupBoxEdit.Location = new System.Drawing.Point(0, 137);
            this.radGroupBoxEdit.Name = "radGroupBoxEdit";
            this.radGroupBoxEdit.Size = new System.Drawing.Size(855, 55);
            this.radGroupBoxEdit.TabIndex = 1;
            this.radGroupBoxEdit.Text = "Edit";
            // 
            // radPanelEdit
            // 
            this.radPanelEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelEdit.Location = new System.Drawing.Point(2, 18);
            this.radPanelEdit.Name = "radPanelEdit";
            this.radPanelEdit.Padding = new System.Windows.Forms.Padding(12, 3, 12, 6);
            this.radPanelEdit.Size = new System.Drawing.Size(717, 35);
            this.radPanelEdit.TabIndex = 0;
            // 
            // radPanelAddButton
            // 
            this.radPanelAddButton.Controls.Add(this.btnUpdate);
            this.radPanelAddButton.Controls.Add(this.btnAdd);
            this.radPanelAddButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelAddButton.Location = new System.Drawing.Point(719, 18);
            this.radPanelAddButton.Name = "radPanelAddButton";
            this.radPanelAddButton.Size = new System.Drawing.Size(134, 35);
            this.radPanelAddButton.TabIndex = 0;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(110, 24);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Update";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(110, 24);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            // 
            // radGroupBoxList
            // 
            this.radGroupBoxList.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxList.Controls.Add(this.managedListBoxControl);
            this.radGroupBoxList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBoxList.HeaderText = "List";
            this.radGroupBoxList.Location = new System.Drawing.Point(0, 192);
            this.radGroupBoxList.Name = "radGroupBoxList";
            this.radGroupBoxList.Size = new System.Drawing.Size(855, 339);
            this.radGroupBoxList.TabIndex = 2;
            this.radGroupBoxList.Text = "List";
            // 
            // managedListBoxControl
            // 
            this.managedListBoxControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managedListBoxControl.Location = new System.Drawing.Point(2, 18);
            this.managedListBoxControl.Margin = new System.Windows.Forms.Padding(0);
            this.managedListBoxControl.Name = "managedListBoxControl";
            this.managedListBoxControl.Size = new System.Drawing.Size(851, 319);
            this.managedListBoxControl.TabIndex = 0;
            // 
            // cmbObjectType
            // 
            this.cmbObjectType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbObjectType.Location = new System.Drawing.Point(291, 59);
            this.cmbObjectType.Name = "cmbObjectType";
            this.cmbObjectType.SelectedText = "";
            this.cmbObjectType.Size = new System.Drawing.Size(528, 24);
            this.cmbObjectType.TabIndex = 3;
            // 
            // EditObjectListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radGroupBoxList);
            this.Controls.Add(this.radGroupBoxEdit);
            this.Controls.Add(this.radGroupBoxType);
            this.Name = "EditObjectListControl";
            this.Size = new System.Drawing.Size(855, 531);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxType)).EndInit();
            this.radGroupBoxType.ResumeLayout(false);
            this.radScrollablePanelType.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanelType)).EndInit();
            this.radScrollablePanelType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTableParent)).EndInit();
            this.radPanelTableParent.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbListType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblObjectType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblListType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxEdit)).EndInit();
            this.radGroupBoxEdit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAddButton)).EndInit();
            this.radPanelAddButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxList)).EndInit();
            this.radGroupBoxList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox radGroupBoxType;
        private Telerik.WinControls.UI.RadScrollablePanel radScrollablePanelType;
        private Telerik.WinControls.UI.RadPanel radPanelTableParent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private Telerik.WinControls.UI.RadDropDownList cmbListType;
        private Telerik.WinControls.UI.RadLabel lblObjectType;
        private Telerik.WinControls.UI.RadLabel lblListType;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxEdit;
        private Telerik.WinControls.UI.RadPanel radPanelEdit;
        private Telerik.WinControls.UI.RadPanel radPanelAddButton;
        private Telerik.WinControls.UI.RadButton btnUpdate;
        private Telerik.WinControls.UI.RadButton btnAdd;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxList;
        private UserControls.ManagedListBoxControl managedListBoxControl;
        private UserControls.AutoCompleteRadDropDownList cmbObjectType;
    }
}
