namespace ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments
{
    partial class EditGenericArgumentsControl
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
            this.radGroupBoxEditArgument = new Telerik.WinControls.UI.RadGroupBox();
            this.radPanelTxtArgument = new Telerik.WinControls.UI.RadPanel();
            this.txtArgument = new Telerik.WinControls.UI.RadTextBox();
            this.radPanelAddButton = new Telerik.WinControls.UI.RadPanel();
            this.btnUpdate = new Telerik.WinControls.UI.RadButton();
            this.btnAdd = new Telerik.WinControls.UI.RadButton();
            this.radGroupBoxArguments = new Telerik.WinControls.UI.RadGroupBox();
            this.managedListBoxControl = new ABIS.LogicBuilder.FlowBuilder.UserControls.ManagedListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxEditArgument)).BeginInit();
            this.radGroupBoxEditArgument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTxtArgument)).BeginInit();
            this.radPanelTxtArgument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtArgument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAddButton)).BeginInit();
            this.radPanelAddButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxArguments)).BeginInit();
            this.radGroupBoxArguments.SuspendLayout();
            this.SuspendLayout();
            // 
            // radGroupBoxEditArgument
            // 
            this.radGroupBoxEditArgument.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxEditArgument.Controls.Add(this.radPanelTxtArgument);
            this.radGroupBoxEditArgument.Controls.Add(this.radPanelAddButton);
            this.radGroupBoxEditArgument.Dock = System.Windows.Forms.DockStyle.Top;
            this.radGroupBoxEditArgument.HeaderText = "Edit Argument";
            this.radGroupBoxEditArgument.Location = new System.Drawing.Point(0, 0);
            this.radGroupBoxEditArgument.Name = "radGroupBoxEditArgument";
            this.radGroupBoxEditArgument.Size = new System.Drawing.Size(729, 55);
            this.radGroupBoxEditArgument.TabIndex = 2;
            this.radGroupBoxEditArgument.Text = "Edit Argument";
            // 
            // radPanelTxtDomainItem
            // 
            this.radPanelTxtArgument.Controls.Add(this.txtArgument);
            this.radPanelTxtArgument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelTxtArgument.Location = new System.Drawing.Point(2, 18);
            this.radPanelTxtArgument.Name = "radPanelTxtDomainItem";
            this.radPanelTxtArgument.Padding = new System.Windows.Forms.Padding(12, 3, 12, 0);
            this.radPanelTxtArgument.Size = new System.Drawing.Size(591, 35);
            this.radPanelTxtArgument.TabIndex = 1;
            // 
            // txtDomainItem
            // 
            this.txtArgument.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtArgument.Location = new System.Drawing.Point(12, 3);
            this.txtArgument.Name = "txtDomainItem";
            this.txtArgument.Size = new System.Drawing.Size(567, 20);
            this.txtArgument.TabIndex = 0;
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
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(110, 24);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            // 
            // radGroupBoxArguments
            // 
            this.radGroupBoxArguments.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxArguments.Controls.Add(this.managedListBoxControl);
            this.radGroupBoxArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBoxArguments.HeaderText = "Arguments";
            this.radGroupBoxArguments.Location = new System.Drawing.Point(0, 55);
            this.radGroupBoxArguments.Name = "radGroupBoxArguments";
            this.radGroupBoxArguments.Size = new System.Drawing.Size(729, 182);
            this.radGroupBoxArguments.TabIndex = 3;
            this.radGroupBoxArguments.Text = "Arguments";
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
            // EditGenericArgumentsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radGroupBoxArguments);
            this.Controls.Add(this.radGroupBoxEditArgument);
            this.Name = "EditGenericArgumentsControl";
            this.Size = new System.Drawing.Size(729, 237);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxEditArgument)).EndInit();
            this.radGroupBoxEditArgument.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTxtArgument)).EndInit();
            this.radPanelTxtArgument.ResumeLayout(false);
            this.radPanelTxtArgument.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtArgument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAddButton)).EndInit();
            this.radPanelAddButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxArguments)).EndInit();
            this.radGroupBoxArguments.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox radGroupBoxEditArgument;
        private Telerik.WinControls.UI.RadPanel radPanelTxtArgument;
        private Telerik.WinControls.UI.RadTextBox txtArgument;
        private Telerik.WinControls.UI.RadPanel radPanelAddButton;
        private Telerik.WinControls.UI.RadButton btnUpdate;
        private Telerik.WinControls.UI.RadButton btnAdd;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxArguments;
        private UserControls.ManagedListBoxControl managedListBoxControl;
    }
}
