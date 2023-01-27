namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration
{
    partial class FieldConfigurationControl
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
            this.groupBoxField = new Telerik.WinControls.UI.RadGroupBox();
            this.radPanelField = new Telerik.WinControls.UI.RadScrollablePanel();
            this.radPanelTableParent = new Telerik.WinControls.UI.RadPanel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblCastVariableAs = new Telerik.WinControls.UI.RadLabel();
            this.cmbCastVariableAs = new ABIS.LogicBuilder.FlowBuilder.UserControls.AutoCompleteRadDropDownList();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxField)).BeginInit();
            this.groupBoxField.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelField)).BeginInit();
            this.radPanelField.PanelContainer.SuspendLayout();
            this.radPanelField.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTableParent)).BeginInit();
            this.radPanelTableParent.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblCastVariableAs)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxField
            // 
            this.groupBoxField.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.groupBoxField.Controls.Add(this.radPanelField);
            this.groupBoxField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxField.HeaderText = "Configure Field";
            this.groupBoxField.Location = new System.Drawing.Point(0, 0);
            this.groupBoxField.Name = "groupBoxField";
            this.groupBoxField.Size = new System.Drawing.Size(855, 200);
            this.groupBoxField.TabIndex = 1;
            this.groupBoxField.Text = "Configure Field";
            // 
            // radPanelField
            // 
            this.radPanelField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelField.Location = new System.Drawing.Point(2, 18);
            this.radPanelField.Name = "radPanelField";
            // 
            // radPanelField.PanelContainer
            // 
            this.radPanelField.PanelContainer.Controls.Add(this.radPanelTableParent);
            this.radPanelField.PanelContainer.Size = new System.Drawing.Size(849, 178);
            this.radPanelField.Size = new System.Drawing.Size(851, 180);
            this.radPanelField.TabIndex = 0;
            // 
            // radPanelTableParent
            // 
            this.radPanelTableParent.Controls.Add(this.tableLayoutPanel);
            this.radPanelTableParent.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanelTableParent.Location = new System.Drawing.Point(0, 0);
            this.radPanelTableParent.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelTableParent.Name = "radPanelTableParent";
            this.radPanelTableParent.Size = new System.Drawing.Size(851, 76);
            this.radPanelTableParent.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tableLayoutPanel.Controls.Add(this.lblCastVariableAs, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.cmbCastVariableAs, 2, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(851, 76);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lblCastVariableAs
            // 
            this.lblCastVariableAs.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCastVariableAs.Location = new System.Drawing.Point(28, 23);
            this.lblCastVariableAs.Name = "lblCastVariableAs";
            this.lblCastVariableAs.Size = new System.Drawing.Size(89, 18);
            this.lblCastVariableAs.TabIndex = 4;
            this.lblCastVariableAs.Text = "Cast Variable As:";
            // 
            // cmbCastVariableAs
            // 
            this.cmbCastVariableAs.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbCastVariableAs.Location = new System.Drawing.Point(288, 20);
            this.cmbCastVariableAs.Margin = new System.Windows.Forms.Padding(0);
            this.cmbCastVariableAs.Name = "cmbCastVariableAs";
            this.cmbCastVariableAs.SelectedText = "";
            this.cmbCastVariableAs.Size = new System.Drawing.Size(536, 27);
            this.cmbCastVariableAs.TabIndex = 5;
            // 
            // FieldConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxField);
            this.Name = "FieldConfigurationControl";
            this.Size = new System.Drawing.Size(855, 200);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxField)).EndInit();
            this.groupBoxField.ResumeLayout(false);
            this.radPanelField.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelField)).EndInit();
            this.radPanelField.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTableParent)).EndInit();
            this.radPanelTableParent.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblCastVariableAs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox groupBoxField;
        private Telerik.WinControls.UI.RadScrollablePanel radPanelField;
        private Telerik.WinControls.UI.RadPanel radPanelTableParent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private Telerik.WinControls.UI.RadLabel lblCastVariableAs;
        private UserControls.AutoCompleteRadDropDownList cmbCastVariableAs;
    }
}
