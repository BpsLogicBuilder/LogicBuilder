namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.ConfigureFragmentsRootNode
{
    partial class ConfigureFragmentsRootNodeControl
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
            this.radGroupBoxRoot = new Telerik.WinControls.UI.RadGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelFill)).BeginInit();
            this.radPanelFill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxRoot)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelFill
            // 
            this.radPanelFill.Controls.Add(this.radGroupBoxRoot);
            this.radPanelFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelFill.Location = new System.Drawing.Point(0, 0);
            this.radPanelFill.Name = "radPanelFill";
            this.radPanelFill.Size = new System.Drawing.Size(687, 392);
            this.radPanelFill.TabIndex = 3;
            // 
            // radGroupBoxRoot
            // 
            this.radGroupBoxRoot.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBoxRoot.HeaderText = "";
            this.radGroupBoxRoot.Location = new System.Drawing.Point(0, 0);
            this.radGroupBoxRoot.Name = "radGroupBoxRoot";
            this.radGroupBoxRoot.Size = new System.Drawing.Size(687, 392);
            this.radGroupBoxRoot.TabIndex = 0;
            // 
            // ConfigureFragmentsRootNodeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.radPanelFill);
            this.Name = "ConfigureFragmentsRootNodeControl";
            this.Size = new System.Drawing.Size(687, 392);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelFill)).EndInit();
            this.radPanelFill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxRoot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelFill;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxRoot;
    }
}
