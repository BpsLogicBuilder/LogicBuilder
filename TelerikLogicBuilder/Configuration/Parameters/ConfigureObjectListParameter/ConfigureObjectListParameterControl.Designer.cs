namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectListParameter
{
    partial class ConfigureObjectListParameterControl
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
            this.groupBoxParameter = new Telerik.WinControls.UI.RadGroupBox();
            this.radPanelParameter = new Telerik.WinControls.UI.RadScrollablePanel();
            this.radPanelTableParent = new Telerik.WinControls.UI.RadPanel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblListCpName = new Telerik.WinControls.UI.RadLabel();
            this.lblListCpObjectType = new Telerik.WinControls.UI.RadLabel();
            this.lblListCpListType = new Telerik.WinControls.UI.RadLabel();
            this.lblListCpControl = new Telerik.WinControls.UI.RadLabel();
            this.lblListCpOptional = new Telerik.WinControls.UI.RadLabel();
            this.lblListCpComments = new Telerik.WinControls.UI.RadLabel();
            this.cmbListCpObjectType = new ABIS.LogicBuilder.FlowBuilder.UserControls.AutoCompleteRadDropDownList();
            this.txtListCpName = new Telerik.WinControls.UI.RadTextBox();
            this.txtListCpComments = new Telerik.WinControls.UI.RadTextBox();
            this.cmbListCpListType = new Telerik.WinControls.UI.RadDropDownList();
            this.cmbListCpControl = new Telerik.WinControls.UI.RadDropDownList();
            this.cmbListCpOptional = new Telerik.WinControls.UI.RadDropDownList();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxParameter)).BeginInit();
            this.groupBoxParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelParameter)).BeginInit();
            this.radPanelParameter.PanelContainer.SuspendLayout();
            this.radPanelParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTableParent)).BeginInit();
            this.radPanelTableParent.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblListCpName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblListCpObjectType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblListCpListType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblListCpControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblListCpOptional)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblListCpComments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtListCpName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtListCpComments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbListCpListType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbListCpControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbListCpOptional)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxParameter
            // 
            this.groupBoxParameter.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.groupBoxParameter.Controls.Add(this.radPanelParameter);
            this.groupBoxParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxParameter.HeaderText = "List of Objects Parameter";
            this.groupBoxParameter.Location = new System.Drawing.Point(0, 0);
            this.groupBoxParameter.Name = "groupBoxParameter";
            this.groupBoxParameter.Size = new System.Drawing.Size(855, 300);
            this.groupBoxParameter.TabIndex = 0;
            this.groupBoxParameter.Text = "List of Objects Parameter";
            // 
            // radPanelParameter
            // 
            this.radPanelParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelParameter.Location = new System.Drawing.Point(2, 18);
            this.radPanelParameter.Name = "radPanelParameter";
            // 
            // radPanelParameter.PanelContainer
            // 
            this.radPanelParameter.PanelContainer.Controls.Add(this.radPanelTableParent);
            this.radPanelParameter.PanelContainer.Size = new System.Drawing.Size(849, 278);
            this.radPanelParameter.Size = new System.Drawing.Size(851, 280);
            this.radPanelParameter.TabIndex = 0;
            // 
            // radPanelTableParent
            // 
            this.radPanelTableParent.Controls.Add(this.tableLayoutPanel);
            this.radPanelTableParent.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanelTableParent.Location = new System.Drawing.Point(0, 0);
            this.radPanelTableParent.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelTableParent.Name = "radPanelTableParent";
            this.radPanelTableParent.Size = new System.Drawing.Size(851, 256);
            this.radPanelTableParent.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tableLayoutPanel.Controls.Add(this.cmbListCpListType, 2, 5);
            this.tableLayoutPanel.Controls.Add(this.cmbListCpControl, 2, 7);
            this.tableLayoutPanel.Controls.Add(this.cmbListCpOptional, 2, 9);
            this.tableLayoutPanel.Controls.Add(this.txtListCpComments, 2, 11);
            this.tableLayoutPanel.Controls.Add(this.lblListCpObjectType, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.lblListCpListType, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.lblListCpControl, 1, 7);
            this.tableLayoutPanel.Controls.Add(this.lblListCpOptional, 1, 9);
            this.tableLayoutPanel.Controls.Add(this.lblListCpComments, 1, 11);
            this.tableLayoutPanel.Controls.Add(this.lblListCpName, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.cmbListCpObjectType, 2, 3);
            this.tableLayoutPanel.Controls.Add(this.txtListCpName, 2, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 14;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(851, 256);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lblListCpName
            // 
            this.lblListCpName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblListCpName.Location = new System.Drawing.Point(28, 23);
            this.lblListCpName.Name = "lblListCpName";
            this.lblListCpName.Size = new System.Drawing.Size(39, 18);
            this.lblListCpName.TabIndex = 0;
            this.lblListCpName.Text = "Name:";
            // 
            // lblListCpObjectType
            // 
            this.lblListCpObjectType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblListCpObjectType.Location = new System.Drawing.Point(28, 59);
            this.lblListCpObjectType.Name = "lblListCpObjectType";
            this.lblListCpObjectType.Size = new System.Drawing.Size(69, 18);
            this.lblListCpObjectType.TabIndex = 2;
            this.lblListCpObjectType.Text = "Object Type:";
            // 
            // lblListCpListType
            // 
            this.lblListCpListType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblListCpListType.Location = new System.Drawing.Point(28, 95);
            this.lblListCpListType.Name = "lblListCpListType";
            this.lblListCpListType.Size = new System.Drawing.Size(52, 18);
            this.lblListCpListType.TabIndex = 4;
            this.lblListCpListType.Text = "List Type:";
            // 
            // lblListCpControl
            // 
            this.lblListCpControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblListCpControl.Location = new System.Drawing.Point(28, 131);
            this.lblListCpControl.Name = "lblListCpControl";
            this.lblListCpControl.Size = new System.Drawing.Size(46, 18);
            this.lblListCpControl.TabIndex = 6;
            this.lblListCpControl.Text = "Control:";
            // 
            // lblListCpOptional
            // 
            this.lblListCpOptional.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblListCpOptional.Location = new System.Drawing.Point(28, 167);
            this.lblListCpOptional.Name = "lblListCpOptional";
            this.lblListCpOptional.Size = new System.Drawing.Size(52, 18);
            this.lblListCpOptional.TabIndex = 8;
            this.lblListCpOptional.Text = "Optional:";
            // 
            // lblListCpComments
            // 
            this.lblListCpComments.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblListCpComments.Location = new System.Drawing.Point(28, 203);
            this.lblListCpComments.Name = "lblListCpComments";
            this.lblListCpComments.Size = new System.Drawing.Size(63, 18);
            this.lblListCpComments.TabIndex = 10;
            this.lblListCpComments.Text = "Comments:";
            // 
            // cmbListCpObjectType
            // 
            this.cmbListCpObjectType.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbListCpObjectType.Location = new System.Drawing.Point(288, 56);
            this.cmbListCpObjectType.Margin = new System.Windows.Forms.Padding(0);
            this.cmbListCpObjectType.Name = "cmbListCpObjectType";
            this.cmbListCpObjectType.SelectedText = "";
            this.cmbListCpObjectType.Size = new System.Drawing.Size(536, 28);
            this.cmbListCpObjectType.TabIndex = 3;
            // 
            // txtListCpName
            // 
            this.txtListCpName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtListCpName.Location = new System.Drawing.Point(291, 23);
            this.txtListCpName.Name = "txtListCpName";
            this.txtListCpName.Size = new System.Drawing.Size(530, 20);
            this.txtListCpName.TabIndex = 1;
            // 
            // txtListCpComments
            // 
            this.txtListCpComments.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtListCpComments.Location = new System.Drawing.Point(291, 203);
            this.txtListCpComments.Name = "txtListCpComments";
            this.txtListCpComments.Size = new System.Drawing.Size(530, 20);
            this.txtListCpComments.TabIndex = 11;
            // 
            // cmbListCpListType
            // 
            this.cmbListCpListType.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbListCpListType.DropDownAnimationEnabled = true;
            this.cmbListCpListType.Location = new System.Drawing.Point(291, 95);
            this.cmbListCpListType.Name = "cmbListCpListType";
            this.cmbListCpListType.Size = new System.Drawing.Size(530, 20);
            this.cmbListCpListType.TabIndex = 5;
            // 
            // cmbListCpControl
            // 
            this.cmbListCpControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbListCpControl.DropDownAnimationEnabled = true;
            this.cmbListCpControl.Location = new System.Drawing.Point(291, 131);
            this.cmbListCpControl.Name = "cmbListCpControl";
            this.cmbListCpControl.Size = new System.Drawing.Size(530, 20);
            this.cmbListCpControl.TabIndex = 7;
            // 
            // cmbListCpOptional
            // 
            this.cmbListCpOptional.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbListCpOptional.DropDownAnimationEnabled = true;
            this.cmbListCpOptional.Location = new System.Drawing.Point(291, 167);
            this.cmbListCpOptional.Name = "cmbListCpOptional";
            this.cmbListCpOptional.Size = new System.Drawing.Size(530, 20);
            this.cmbListCpOptional.TabIndex = 9;
            // 
            // ConfigureObjectListParameterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxParameter);
            this.Name = "ConfigureObjectListParameterControl";
            this.Size = new System.Drawing.Size(855, 300);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxParameter)).EndInit();
            this.groupBoxParameter.ResumeLayout(false);
            this.radPanelParameter.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelParameter)).EndInit();
            this.radPanelParameter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTableParent)).EndInit();
            this.radPanelTableParent.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblListCpName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblListCpObjectType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblListCpListType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblListCpControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblListCpOptional)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblListCpComments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtListCpName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtListCpComments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbListCpListType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbListCpControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbListCpOptional)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox groupBoxParameter;
        private Telerik.WinControls.UI.RadScrollablePanel radPanelParameter;
        private Telerik.WinControls.UI.RadPanel radPanelTableParent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private Telerik.WinControls.UI.RadDropDownList cmbListCpListType;
        private Telerik.WinControls.UI.RadDropDownList cmbListCpControl;
        private Telerik.WinControls.UI.RadDropDownList cmbListCpOptional;
        private Telerik.WinControls.UI.RadTextBox txtListCpComments;
        private Telerik.WinControls.UI.RadLabel lblListCpObjectType;
        private Telerik.WinControls.UI.RadLabel lblListCpListType;
        private Telerik.WinControls.UI.RadLabel lblListCpControl;
        private Telerik.WinControls.UI.RadLabel lblListCpOptional;
        private Telerik.WinControls.UI.RadLabel lblListCpComments;
        private Telerik.WinControls.UI.RadLabel lblListCpName;
        private UserControls.AutoCompleteRadDropDownList cmbListCpObjectType;
        private Telerik.WinControls.UI.RadTextBox txtListCpName;
    }
}
