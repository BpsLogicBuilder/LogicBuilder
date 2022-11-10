namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls
{
    partial class LoadAssemblyPathsControl
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
            this.radGroupBoxPaths = new Telerik.WinControls.UI.RadGroupBox();
            this.managedListBoxControl = new ABIS.LogicBuilder.FlowBuilder.UserControls.ManagedListBoxControl();
            this.btnAdd = new Telerik.WinControls.UI.RadButton();
            this.txtPath = new ABIS.LogicBuilder.FlowBuilder.UserControls.HelperButtonTextBox();
            this.radGroupBoxAddPath = new Telerik.WinControls.UI.RadGroupBox();
            this.radPanelTxtPath = new Telerik.WinControls.UI.RadPanel();
            this.radPanelAddButton = new Telerik.WinControls.UI.RadPanel();
            this.btnUpdate = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxPaths)).BeginInit();
            this.radGroupBoxPaths.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxAddPath)).BeginInit();
            this.radGroupBoxAddPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTxtPath)).BeginInit();
            this.radPanelTxtPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAddButton)).BeginInit();
            this.radPanelAddButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBoxPaths
            // 
            this.radGroupBoxPaths.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxPaths.Controls.Add(this.managedListBoxControl);
            this.radGroupBoxPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBoxPaths.HeaderText = "Paths";
            this.radGroupBoxPaths.Location = new System.Drawing.Point(0, 55);
            this.radGroupBoxPaths.Name = "radGroupBoxPaths";
            this.radGroupBoxPaths.Size = new System.Drawing.Size(729, 182);
            this.radGroupBoxPaths.TabIndex = 1;
            this.radGroupBoxPaths.Text = "Paths";
            // 
            // managedListBoxControl
            // 
            this.managedListBoxControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managedListBoxControl.Location = new System.Drawing.Point(2, 18);
            this.managedListBoxControl.Margin = new System.Windows.Forms.Padding(0);
            this.managedListBoxControl.Name = "managedListBoxControl";
            this.managedListBoxControl.Size = new System.Drawing.Size(725, 162);
            this.managedListBoxControl.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(110, 24);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            // 
            // txtPath
            // 
            this.txtPath.AutoSize = true;
            this.txtPath.Location = new System.Drawing.Point(4, 3);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = false;
            this.txtPath.Size = new System.Drawing.Size(581, 24);
            this.txtPath.TabIndex = 0;
            // 
            // radGroupBoxAddPath
            // 
            this.radGroupBoxAddPath.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxAddPath.Controls.Add(this.radPanelTxtPath);
            this.radGroupBoxAddPath.Controls.Add(this.radPanelAddButton);
            this.radGroupBoxAddPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.radGroupBoxAddPath.HeaderText = "Add Path";
            this.radGroupBoxAddPath.Location = new System.Drawing.Point(0, 0);
            this.radGroupBoxAddPath.Name = "radGroupBoxAddPath";
            this.radGroupBoxAddPath.Size = new System.Drawing.Size(729, 55);
            this.radGroupBoxAddPath.TabIndex = 0;
            this.radGroupBoxAddPath.Text = "Add Path";
            // 
            // radPanelTxtPath
            // 
            this.radPanelTxtPath.Controls.Add(this.txtPath);
            this.radPanelTxtPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelTxtPath.Location = new System.Drawing.Point(2, 18);
            this.radPanelTxtPath.Name = "radPanelTxtPath";
            this.radPanelTxtPath.Size = new System.Drawing.Size(591, 35);
            this.radPanelTxtPath.TabIndex = 1;
            // 
            // radPanelAddButton
            // 
            this.radPanelAddButton.Controls.Add(this.btnUpdate);
            this.radPanelAddButton.Controls.Add(this.btnAdd);
            this.radPanelAddButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelAddButton.Location = new System.Drawing.Point(593, 18);
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
            // LoadAssemblyPathsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radGroupBoxPaths);
            this.Controls.Add(this.radGroupBoxAddPath);
            this.Name = "LoadAssemblyPathsControl";
            this.Size = new System.Drawing.Size(729, 237);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxPaths)).EndInit();
            this.radGroupBoxPaths.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxAddPath)).EndInit();
            this.radGroupBoxAddPath.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTxtPath)).EndInit();
            this.radPanelTxtPath.ResumeLayout(false);
            this.radPanelTxtPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAddButton)).EndInit();
            this.radPanelAddButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox radGroupBoxPaths;
        private Telerik.WinControls.UI.RadButton btnAdd;
        private FlowBuilder.UserControls.HelperButtonTextBox txtPath;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxAddPath;
        private Telerik.WinControls.UI.RadPanel radPanelTxtPath;
        private Telerik.WinControls.UI.RadPanel radPanelAddButton;
        private FlowBuilder.UserControls.ManagedListBoxControl managedListBoxControl;
        private Telerik.WinControls.UI.RadButton btnUpdate;
    }
}
