namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration
{
    partial class PropertyConfigurationControl
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
            groupBoxProperty = new Telerik.WinControls.UI.RadGroupBox();
            radPanelProperty = new Telerik.WinControls.UI.RadScrollablePanel();
            radPanelTableParent = new Telerik.WinControls.UI.RadPanel();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            lblCastVariableAs = new Telerik.WinControls.UI.RadLabel();
            cmbCastVariableAs = new UserControls.AutoCompleteRadDropDownList();
            ((System.ComponentModel.ISupportInitialize)groupBoxProperty).BeginInit();
            groupBoxProperty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelProperty).BeginInit();
            radPanelProperty.PanelContainer.SuspendLayout();
            radPanelProperty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelTableParent).BeginInit();
            radPanelTableParent.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lblCastVariableAs).BeginInit();
            SuspendLayout();
            // 
            // groupBoxProperty
            // 
            groupBoxProperty.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            groupBoxProperty.Controls.Add(radPanelProperty);
            groupBoxProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxProperty.HeaderText = "Configure Property";
            groupBoxProperty.Location = new System.Drawing.Point(0, 0);
            groupBoxProperty.Name = "groupBoxProperty";
            groupBoxProperty.Size = new System.Drawing.Size(855, 200);
            groupBoxProperty.TabIndex = 2;
            groupBoxProperty.Text = "Configure Property";
            // 
            // radPanelProperty
            // 
            radPanelProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelProperty.Location = new System.Drawing.Point(2, 18);
            radPanelProperty.Name = "radPanelProperty";
            // 
            // radPanelProperty.PanelContainer
            // 
            radPanelProperty.PanelContainer.Controls.Add(radPanelTableParent);
            radPanelProperty.PanelContainer.Size = new System.Drawing.Size(849, 178);
            radPanelProperty.Size = new System.Drawing.Size(851, 180);
            radPanelProperty.TabIndex = 0;
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
            lblCastVariableAs.Size = new System.Drawing.Size(257, 18);
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
            // PropertyConfigurationControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBoxProperty);
            Name = "PropertyConfigurationControl";
            Size = new System.Drawing.Size(855, 200);
            ((System.ComponentModel.ISupportInitialize)groupBoxProperty).EndInit();
            groupBoxProperty.ResumeLayout(false);
            radPanelProperty.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelProperty).EndInit();
            radPanelProperty.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)lblCastVariableAs).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox groupBoxProperty;
        private Telerik.WinControls.UI.RadScrollablePanel radPanelProperty;
        private Telerik.WinControls.UI.RadPanel radPanelTableParent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private Telerik.WinControls.UI.RadLabel lblCastVariableAs;
        private UserControls.AutoCompleteRadDropDownList cmbCastVariableAs;
    }
}
