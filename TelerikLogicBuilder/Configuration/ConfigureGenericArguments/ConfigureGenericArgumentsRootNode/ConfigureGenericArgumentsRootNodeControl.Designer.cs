namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericArgumentsRootNode
{
    partial class ConfigureGenericArgumentsRootNodeControl
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
            this.radPanelFill = new Telerik.WinControls.UI.RadPanel();
            this.radGroupBoxType = new Telerik.WinControls.UI.RadGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelFill)).BeginInit();
            this.radPanelFill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxType)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelFill
            // 
            this.radPanelFill.Controls.Add(this.radGroupBoxType);
            this.radPanelFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelFill.Location = new System.Drawing.Point(0, 0);
            this.radPanelFill.Name = "radPanelFill";
            this.radPanelFill.Size = new System.Drawing.Size(692, 517);
            this.radPanelFill.TabIndex = 1;
            // 
            // radGroupBoxType
            // 
            this.radGroupBoxType.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBoxType.HeaderText = "Type";
            this.radGroupBoxType.Location = new System.Drawing.Point(0, 0);
            this.radGroupBoxType.Name = "radGroupBoxType";
            this.radGroupBoxType.Size = new System.Drawing.Size(692, 517);
            this.radGroupBoxType.TabIndex = 0;
            this.radGroupBoxType.Text = "Type";
            // 
            // ConfigureGenericArgumentsRootNodeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanelFill);
            this.Name = "ConfigureGenericArgumentsRootNodeControl";
            this.Size = new System.Drawing.Size(692, 517);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelFill)).EndInit();
            this.radPanelFill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelFill;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxType;
    }
}
