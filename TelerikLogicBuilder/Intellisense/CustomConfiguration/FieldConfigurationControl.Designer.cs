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
            groupBoxField = new Telerik.WinControls.UI.RadGroupBox();
            radPanelField = new Telerik.WinControls.UI.RadScrollablePanel();
            radPanelTableParent = new Telerik.WinControls.UI.RadPanel();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            lblCastVariableAs = new Telerik.WinControls.UI.RadLabel();
            cmbCastVariableAs = new UserControls.AutoCompleteRadDropDownList();
            ((System.ComponentModel.ISupportInitialize)groupBoxField).BeginInit();
            groupBoxField.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelField).BeginInit();
            radPanelField.PanelContainer.SuspendLayout();
            radPanelField.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelTableParent).BeginInit();
            radPanelTableParent.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lblCastVariableAs).BeginInit();
            SuspendLayout();
            // 
            // groupBoxField
            // 
            groupBoxField.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            groupBoxField.Controls.Add(radPanelField);
            groupBoxField.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxField.HeaderText = "Configure Field";
            groupBoxField.Location = new System.Drawing.Point(0, 0);
            groupBoxField.Name = "groupBoxField";
            groupBoxField.Size = new System.Drawing.Size(855, 200);
            groupBoxField.TabIndex = 1;
            groupBoxField.Text = "Configure Field";
            // 
            // radPanelField
            // 
            radPanelField.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelField.Location = new System.Drawing.Point(2, 18);
            radPanelField.Name = "radPanelField";
            // 
            // radPanelField.PanelContainer
            // 
            radPanelField.PanelContainer.Controls.Add(radPanelTableParent);
            radPanelField.PanelContainer.Size = new System.Drawing.Size(849, 178);
            radPanelField.Size = new System.Drawing.Size(851, 180);
            radPanelField.TabIndex = 0;
            // 
            // radPanelTableParent
            // 
            radPanelTableParent.Controls.Add(tableLayoutPanel);
            radPanelTableParent.Dock = System.Windows.Forms.DockStyle.Top;
            radPanelTableParent.Location = new System.Drawing.Point(0, 0);
            radPanelTableParent.Margin = new System.Windows.Forms.Padding(0);
            radPanelTableParent.Name = "radPanelTableParent";
            radPanelTableParent.Size = new System.Drawing.Size(849, 76);
            radPanelTableParent.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 4;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            tableLayoutPanel.Controls.Add(lblCastVariableAs, 1, 1);
            tableLayoutPanel.Controls.Add(cmbCastVariableAs, 2, 1);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 4;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.Size = new System.Drawing.Size(849, 76);
            tableLayoutPanel.TabIndex = 0;
            // 
            // lblCastVariableAs
            // 
            lblCastVariableAs.Dock = System.Windows.Forms.DockStyle.Top;
            lblCastVariableAs.Location = new System.Drawing.Point(28, 23);
            lblCastVariableAs.Name = "lblCastVariableAs";
            lblCastVariableAs.Size = new System.Drawing.Size(89, 18);
            lblCastVariableAs.TabIndex = 4;
            lblCastVariableAs.Text = "Cast Variable As:";
            // 
            // cmbCastVariableAs
            // 
            cmbCastVariableAs.Dock = System.Windows.Forms.DockStyle.Fill;
            cmbCastVariableAs.Location = new System.Drawing.Point(288, 20);
            cmbCastVariableAs.Margin = new System.Windows.Forms.Padding(0);
            cmbCastVariableAs.Name = "cmbCastVariableAs";
            cmbCastVariableAs.SelectedText = "";
            cmbCastVariableAs.Size = new System.Drawing.Size(534, 30);
            cmbCastVariableAs.TabIndex = 5;
            // 
            // FieldConfigurationControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            Controls.Add(groupBoxField);
            Name = "FieldConfigurationControl";
            Size = new System.Drawing.Size(855, 200);
            ((System.ComponentModel.ISupportInitialize)groupBoxField).EndInit();
            groupBoxField.ResumeLayout(false);
            radPanelField.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelField).EndInit();
            radPanelField.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)lblCastVariableAs).EndInit();
            ResumeLayout(false);
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
