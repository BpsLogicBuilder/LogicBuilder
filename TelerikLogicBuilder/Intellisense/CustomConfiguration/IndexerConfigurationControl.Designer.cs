namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration
{
    partial class IndexerConfigurationControl
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
            groupBoxIndexer = new Telerik.WinControls.UI.RadGroupBox();
            radPanelIndexer = new Telerik.WinControls.UI.RadScrollablePanel();
            radPanelTableParent = new Telerik.WinControls.UI.RadPanel();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            cmbVariableCategory = new Telerik.WinControls.UI.RadDropDownList();
            lblVariableCategory = new Telerik.WinControls.UI.RadLabel();
            lblCastVariableAs = new Telerik.WinControls.UI.RadLabel();
            lblMemberName = new Telerik.WinControls.UI.RadLabel();
            txtMemberName = new Telerik.WinControls.UI.RadTextBox();
            cmbCastVariableAs = new UserControls.AutoCompleteRadDropDownList();
            ((System.ComponentModel.ISupportInitialize)groupBoxIndexer).BeginInit();
            groupBoxIndexer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelIndexer).BeginInit();
            radPanelIndexer.PanelContainer.SuspendLayout();
            radPanelIndexer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelTableParent).BeginInit();
            radPanelTableParent.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmbVariableCategory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lblVariableCategory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lblCastVariableAs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lblMemberName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtMemberName).BeginInit();
            SuspendLayout();
            // 
            // groupBoxIndexer
            // 
            groupBoxIndexer.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            groupBoxIndexer.Controls.Add(radPanelIndexer);
            groupBoxIndexer.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxIndexer.HeaderText = "Configure Indexer";
            groupBoxIndexer.Location = new System.Drawing.Point(0, 0);
            groupBoxIndexer.Name = "groupBoxIndexer";
            groupBoxIndexer.Size = new System.Drawing.Size(855, 200);
            groupBoxIndexer.TabIndex = 0;
            groupBoxIndexer.Text = "Configure Indexer";
            // 
            // radPanelIndexer
            // 
            radPanelIndexer.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelIndexer.Location = new System.Drawing.Point(2, 18);
            radPanelIndexer.Name = "radPanelIndexer";
            // 
            // radPanelIndexer.PanelContainer
            // 
            radPanelIndexer.PanelContainer.Controls.Add(radPanelTableParent);
            radPanelIndexer.PanelContainer.Size = new System.Drawing.Size(849, 178);
            radPanelIndexer.Size = new System.Drawing.Size(851, 180);
            radPanelIndexer.TabIndex = 0;
            // 
            // radPanelTableParent
            // 
            radPanelTableParent.Controls.Add(tableLayoutPanel);
            radPanelTableParent.Dock = System.Windows.Forms.DockStyle.Top;
            radPanelTableParent.Location = new System.Drawing.Point(0, 0);
            radPanelTableParent.Margin = new System.Windows.Forms.Padding(0);
            radPanelTableParent.Name = "radPanelTableParent";
            radPanelTableParent.Size = new System.Drawing.Size(849, 148);
            radPanelTableParent.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 4;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            tableLayoutPanel.Controls.Add(cmbVariableCategory, 2, 3);
            tableLayoutPanel.Controls.Add(lblVariableCategory, 1, 3);
            tableLayoutPanel.Controls.Add(lblCastVariableAs, 1, 5);
            tableLayoutPanel.Controls.Add(lblMemberName, 1, 1);
            tableLayoutPanel.Controls.Add(txtMemberName, 2, 1);
            tableLayoutPanel.Controls.Add(cmbCastVariableAs, 2, 5);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 8;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.Size = new System.Drawing.Size(849, 148);
            tableLayoutPanel.TabIndex = 0;
            // 
            // cmbVariableCategory
            // 
            cmbVariableCategory.AutoSize = false;
            cmbVariableCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            cmbVariableCategory.DropDownAnimationEnabled = true;
            cmbVariableCategory.Location = new System.Drawing.Point(288, 56);
            cmbVariableCategory.Margin = new System.Windows.Forms.Padding(0);
            cmbVariableCategory.Name = "cmbVariableCategory";
            cmbVariableCategory.Size = new System.Drawing.Size(534, 30);
            cmbVariableCategory.TabIndex = 3;
            // 
            // lblVariableCategory
            // 
            lblVariableCategory.Dock = System.Windows.Forms.DockStyle.Top;
            lblVariableCategory.Location = new System.Drawing.Point(28, 59);
            lblVariableCategory.Name = "lblVariableCategory";
            lblVariableCategory.Size = new System.Drawing.Size(98, 18);
            lblVariableCategory.TabIndex = 2;
            lblVariableCategory.Text = "Variable Category:";
            // 
            // lblCastVariableAs
            // 
            lblCastVariableAs.Dock = System.Windows.Forms.DockStyle.Top;
            lblCastVariableAs.Location = new System.Drawing.Point(28, 95);
            lblCastVariableAs.Name = "lblCastVariableAs";
            lblCastVariableAs.Size = new System.Drawing.Size(89, 18);
            lblCastVariableAs.TabIndex = 4;
            lblCastVariableAs.Text = "Cast Variable As:";
            // 
            // lblMemberName
            // 
            lblMemberName.Dock = System.Windows.Forms.DockStyle.Top;
            lblMemberName.Location = new System.Drawing.Point(28, 23);
            lblMemberName.Name = "lblMemberName";
            lblMemberName.Size = new System.Drawing.Size(84, 18);
            lblMemberName.TabIndex = 0;
            lblMemberName.Text = "Member Name:";
            // 
            // txtMemberName
            // 
            txtMemberName.AutoSize = false;
            txtMemberName.Dock = System.Windows.Forms.DockStyle.Fill;
            txtMemberName.Location = new System.Drawing.Point(288, 20);
            txtMemberName.Margin = new System.Windows.Forms.Padding(0);
            txtMemberName.Name = "txtMemberName";
            txtMemberName.Size = new System.Drawing.Size(534, 30);
            txtMemberName.TabIndex = 1;
            // 
            // cmbCastVariableAs
            // 
            cmbCastVariableAs.Dock = System.Windows.Forms.DockStyle.Fill;
            cmbCastVariableAs.Location = new System.Drawing.Point(288, 92);
            cmbCastVariableAs.Margin = new System.Windows.Forms.Padding(0);
            cmbCastVariableAs.Name = "cmbCastVariableAs";
            cmbCastVariableAs.SelectedText = "";
            cmbCastVariableAs.Size = new System.Drawing.Size(534, 30);
            cmbCastVariableAs.TabIndex = 5;
            // 
            // IndexerConfigurationControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBoxIndexer);
            Name = "IndexerConfigurationControl";
            Size = new System.Drawing.Size(855, 200);
            ((System.ComponentModel.ISupportInitialize)groupBoxIndexer).EndInit();
            groupBoxIndexer.ResumeLayout(false);
            radPanelIndexer.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelIndexer).EndInit();
            radPanelIndexer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cmbVariableCategory).EndInit();
            ((System.ComponentModel.ISupportInitialize)lblVariableCategory).EndInit();
            ((System.ComponentModel.ISupportInitialize)lblCastVariableAs).EndInit();
            ((System.ComponentModel.ISupportInitialize)lblMemberName).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtMemberName).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox groupBoxIndexer;
        private Telerik.WinControls.UI.RadScrollablePanel radPanelIndexer;
        private Telerik.WinControls.UI.RadPanel radPanelTableParent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private Telerik.WinControls.UI.RadDropDownList cmbVariableCategory;
        private Telerik.WinControls.UI.RadLabel lblVariableCategory;
        private Telerik.WinControls.UI.RadLabel lblCastVariableAs;
        private Telerik.WinControls.UI.RadLabel lblMemberName;
        private Telerik.WinControls.UI.RadTextBox txtMemberName;
        private UserControls.AutoCompleteRadDropDownList cmbCastVariableAs;
    }
}
