namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects
{
    partial class ConfigureConnectorObjectsControl
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
            this.radGroupBoxAddType = new Telerik.WinControls.UI.RadGroupBox();
            this.radPanelTxtType = new Telerik.WinControls.UI.RadPanel();
            this.txtType = new ABIS.LogicBuilder.FlowBuilder.UserControls.AutoCompleteRadDropDownList();
            this.radPanelAddButton = new Telerik.WinControls.UI.RadPanel();
            this.btnUpdate = new Telerik.WinControls.UI.RadButton();
            this.btnAdd = new Telerik.WinControls.UI.RadButton();
            this.radGroupBoxTypes = new Telerik.WinControls.UI.RadGroupBox();
            this.managedListBoxControl = new ABIS.LogicBuilder.FlowBuilder.UserControls.ManagedListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxAddType)).BeginInit();
            this.radGroupBoxAddType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTxtType)).BeginInit();
            this.radPanelTxtType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAddButton)).BeginInit();
            this.radPanelAddButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxTypes)).BeginInit();
            this.radGroupBoxTypes.SuspendLayout();
            this.SuspendLayout();
            // 
            // radGroupBoxAddType
            // 
            this.radGroupBoxAddType.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxAddType.Controls.Add(this.radPanelTxtType);
            this.radGroupBoxAddType.Controls.Add(this.radPanelAddButton);
            this.radGroupBoxAddType.Dock = System.Windows.Forms.DockStyle.Top;
            this.radGroupBoxAddType.HeaderText = "Add Fully Qualified Class Name";
            this.radGroupBoxAddType.Location = new System.Drawing.Point(0, 0);
            this.radGroupBoxAddType.Name = "radGroupBoxAddType";
            this.radGroupBoxAddType.Size = new System.Drawing.Size(729, 55);
            this.radGroupBoxAddType.TabIndex = 1;
            this.radGroupBoxAddType.Text = "Add Fully Qualified Class Name";
            // 
            // radPanelTxtType
            // 
            this.radPanelTxtType.Controls.Add(this.txtType);
            this.radPanelTxtType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelTxtType.Location = new System.Drawing.Point(2, 18);
            this.radPanelTxtType.Name = "radPanelTxtType";
            this.radPanelTxtType.Padding = new System.Windows.Forms.Padding(12, 3, 12, 0);
            this.radPanelTxtType.Size = new System.Drawing.Size(591, 35);
            this.radPanelTxtType.TabIndex = 1;
            // 
            // txtType
            // 
            this.txtType.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtType.Location = new System.Drawing.Point(12, 3);
            this.txtType.Margin = new System.Windows.Forms.Padding(0);
            this.txtType.Name = "txtType";
            this.txtType.SelectedText = "";
            this.txtType.Size = new System.Drawing.Size(567, 24);
            this.txtType.TabIndex = 0;
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
            // radGroupBoxTypes
            // 
            this.radGroupBoxTypes.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxTypes.Controls.Add(this.managedListBoxControl);
            this.radGroupBoxTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBoxTypes.HeaderText = "Object Types";
            this.radGroupBoxTypes.Location = new System.Drawing.Point(0, 55);
            this.radGroupBoxTypes.Name = "radGroupBoxTypes";
            this.radGroupBoxTypes.Size = new System.Drawing.Size(729, 182);
            this.radGroupBoxTypes.TabIndex = 2;
            this.radGroupBoxTypes.Text = "Object Types";
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
            // ConfigureConnectorObjectsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radGroupBoxTypes);
            this.Controls.Add(this.radGroupBoxAddType);
            this.Name = "ConfigureConnectorObjectsControl";
            this.Size = new System.Drawing.Size(729, 237);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxAddType)).EndInit();
            this.radGroupBoxAddType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTxtType)).EndInit();
            this.radPanelTxtType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelAddButton)).EndInit();
            this.radPanelAddButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxTypes)).EndInit();
            this.radGroupBoxTypes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox radGroupBoxAddType;
        private Telerik.WinControls.UI.RadPanel radPanelTxtType;
        private Telerik.WinControls.UI.RadPanel radPanelAddButton;
        private Telerik.WinControls.UI.RadButton btnUpdate;
        private Telerik.WinControls.UI.RadButton btnAdd;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxTypes;
        private FlowBuilder.UserControls.ManagedListBoxControl managedListBoxControl;
        private FlowBuilder.UserControls.AutoCompleteRadDropDownList txtType;
    }
}
