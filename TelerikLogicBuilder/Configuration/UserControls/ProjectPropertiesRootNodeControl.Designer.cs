namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls
{
    partial class ProjectPropertiesRootNodeControl
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
            this.radGroupBoxProjectProperties = new Telerik.WinControls.UI.RadGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelFill)).BeginInit();
            this.radPanelFill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxProjectProperties)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelFill
            // 
            this.radPanelFill.Controls.Add(this.radGroupBoxProjectProperties);
            this.radPanelFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelFill.Location = new System.Drawing.Point(0, 0);
            this.radPanelFill.Name = "radPanelFill";
            this.radPanelFill.Size = new System.Drawing.Size(692, 517);
            this.radPanelFill.TabIndex = 0;
            // 
            // radGroupBoxProjectProperties
            // 
            this.radGroupBoxProjectProperties.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxProjectProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBoxProjectProperties.HeaderText = "Project Properties";
            this.radGroupBoxProjectProperties.Location = new System.Drawing.Point(0, 0);
            this.radGroupBoxProjectProperties.Name = "radGroupBoxProjectProperties";
            this.radGroupBoxProjectProperties.Size = new System.Drawing.Size(692, 517);
            this.radGroupBoxProjectProperties.TabIndex = 0;
            this.radGroupBoxProjectProperties.Text = "Project Properties";
            // 
            // RootNodeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanelFill);
            this.Name = "RootNodeControl";
            this.Size = new System.Drawing.Size(692, 517);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelFill)).EndInit();
            this.radPanelFill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxProjectProperties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelFill;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxProjectProperties;
    }
}
