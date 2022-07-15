namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    partial class TableControl
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn1 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn2 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.titleBar1 = new ABIS.LogicBuilder.FlowBuilder.Components.TitleBar();
            this.dataGridView1 = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.titleBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // titleBar1
            // 
            this.titleBar1.AllowResize = false;
            this.titleBar1.CanManageOwnerForm = false;
            this.titleBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar1.Location = new System.Drawing.Point(0, 0);
            this.titleBar1.Name = "titleBar1";
            this.titleBar1.Size = new System.Drawing.Size(1110, 23);
            this.titleBar1.TabIndex = 0;
            this.titleBar1.TabStop = false;
            this.titleBar1.Text = "titleBar1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 23);
            // 
            // 
            // 
            gridViewTextBoxColumn1.FieldName = "Condition";
            gridViewTextBoxColumn1.HeaderText = "Condition";
            gridViewTextBoxColumn1.Name = "Condition";
            gridViewTextBoxColumn1.WrapText = true;
            gridViewTextBoxColumn2.FieldName = "ConditionVisible";
            gridViewTextBoxColumn2.HeaderText = "ConditionVisible";
            gridViewTextBoxColumn2.IsVisible = false;
            gridViewTextBoxColumn2.Name = "ConditionVisible";
            gridViewTextBoxColumn3.FieldName = "Action";
            gridViewTextBoxColumn3.HeaderText = "Action";
            gridViewTextBoxColumn3.Name = "Action";
            gridViewTextBoxColumn3.WrapText = true;
            gridViewTextBoxColumn4.FieldName = "ActionVisible";
            gridViewTextBoxColumn4.HeaderText = "ActionVisible";
            gridViewTextBoxColumn4.IsVisible = false;
            gridViewTextBoxColumn4.Name = "ActionVisible";
            gridViewTextBoxColumn5.FieldName = "Priority";
            gridViewTextBoxColumn5.HeaderText = "Priority";
            gridViewTextBoxColumn5.Name = "Priority";
            gridViewTextBoxColumn6.FieldName = "PriorityVisible";
            gridViewTextBoxColumn6.HeaderText = "PriorityVisible";
            gridViewTextBoxColumn6.IsVisible = false;
            gridViewTextBoxColumn6.Name = "PriorityVisible";
            gridViewCheckBoxColumn1.FieldName = "ReEvaluate";
            gridViewCheckBoxColumn1.HeaderText = "ReEvaluate";
            gridViewCheckBoxColumn1.Name = "ReEvaluate";
            gridViewCheckBoxColumn2.FieldName = "Active";
            gridViewCheckBoxColumn2.HeaderText = "Active";
            gridViewCheckBoxColumn2.Name = "Active";
            this.dataGridView1.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewCheckBoxColumn1,
            gridViewCheckBoxColumn2});
            this.dataGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1110, 664);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.DataGridView1_CellFormatting);
            this.dataGridView1.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.DataGridView1_ViewCellFormatting);
            this.dataGridView1.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.DataGridView1_CellClick);
            this.dataGridView1.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.DataGridView1_CellDoubleClick);
            this.dataGridView1.DataError += new Telerik.WinControls.UI.GridViewDataErrorEventHandler(this.DataGridView1_DataError);
            this.dataGridView1.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.DataGridView1_ContextMenuOpening);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridView1_MouseDown);
            this.dataGridView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DataGridView1_MouseUp);
            // 
            // TableControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.titleBar1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TableControl";
            this.Size = new System.Drawing.Size(1110, 687);
            ((System.ComponentModel.ISupportInitialize)(this.titleBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Components.TitleBar titleBar1;
        private Telerik.WinControls.UI.RadGridView dataGridView1;
    }
}
