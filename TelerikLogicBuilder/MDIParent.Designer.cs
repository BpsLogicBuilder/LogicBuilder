namespace TelerikLogicBuilder
{
    partial class MDIParent
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIParent));
            this.radMenu1 = new Telerik.WinControls.UI.RadMenu();
            this.radMenuItemFile = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItemEdit = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItemView = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItemProject = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItemRules = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItemTools = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItemHelp = new Telerik.WinControls.UI.RadMenuItem();
            this.radCommandBar1 = new Telerik.WinControls.UI.RadCommandBar();
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.commandBarButtonEdit = new Telerik.WinControls.UI.CommandBarButton();
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radMenu1
            // 
            this.radMenu1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItemFile,
            this.radMenuItemEdit,
            this.radMenuItemView,
            this.radMenuItemProject,
            this.radMenuItemRules,
            this.radMenuItemTools,
            this.radMenuItemHelp});
            resources.ApplyResources(this.radMenu1, "radMenu1");
            this.radMenu1.Name = "radMenu1";
            this.radMenu1.Click += new System.EventHandler(this.RadMenu1_Click);
            // 
            // radMenuItemFile
            // 
            this.radMenuItemFile.Name = "radMenuItemFile";
            resources.ApplyResources(this.radMenuItemFile, "radMenuItemFile");
            // 
            // radMenuItemEdit
            // 
            this.radMenuItemEdit.Name = "radMenuItemEdit";
            resources.ApplyResources(this.radMenuItemEdit, "radMenuItemEdit");
            // 
            // radMenuItemView
            // 
            this.radMenuItemView.Name = "radMenuItemView";
            resources.ApplyResources(this.radMenuItemView, "radMenuItemView");
            // 
            // radMenuItemProject
            // 
            this.radMenuItemProject.Name = "radMenuItemProject";
            resources.ApplyResources(this.radMenuItemProject, "radMenuItemProject");
            // 
            // radMenuItemRules
            // 
            this.radMenuItemRules.Name = "radMenuItemRules";
            resources.ApplyResources(this.radMenuItemRules, "radMenuItemRules");
            // 
            // radMenuItemTools
            // 
            this.radMenuItemTools.Name = "radMenuItemTools";
            resources.ApplyResources(this.radMenuItemTools, "radMenuItemTools");
            // 
            // radMenuItemHelp
            // 
            this.radMenuItemHelp.Name = "radMenuItemHelp";
            resources.ApplyResources(this.radMenuItemHelp, "radMenuItemHelp");
            // 
            // radCommandBar1
            // 
            resources.ApplyResources(this.radCommandBar1, "radCommandBar1");
            this.radCommandBar1.Name = "radCommandBar1";
            this.radCommandBar1.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1});
            // 
            // commandBarRowElement1
            // 
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Name = "commandBarRowElement1";
            this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement1});
            // 
            // commandBarStripElement1
            // 
            resources.ApplyResources(this.commandBarStripElement1, "commandBarStripElement1");
            this.commandBarStripElement1.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.commandBarButtonEdit});
            this.commandBarStripElement1.Name = "commandBarStripElement1";
            // 
            // commandBarButtonEdit
            // 
            resources.ApplyResources(this.commandBarButtonEdit, "commandBarButtonEdit");
            this.commandBarButtonEdit.Image = global::TelerikLogicBuilder.Properties.Resources.Edit;
            this.commandBarButtonEdit.Name = "commandBarButtonEdit";
            // 
            // MDIParent
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radCommandBar1);
            this.Controls.Add(this.radMenu1);
            this.IsMdiContainer = true;
            this.Name = "MDIParent";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadMenu radMenu1;
        private Telerik.WinControls.UI.RadMenuItem radMenuItemFile;
        private Telerik.WinControls.UI.RadMenuItem radMenuItemEdit;
        private Telerik.WinControls.UI.RadMenuItem radMenuItemView;
        private Telerik.WinControls.UI.RadMenuItem radMenuItemProject;
        private Telerik.WinControls.UI.RadMenuItem radMenuItemRules;
        private Telerik.WinControls.UI.RadMenuItem radMenuItemTools;
        private Telerik.WinControls.UI.RadMenuItem radMenuItemHelp;
        private Telerik.WinControls.UI.RadCommandBar radCommandBar1;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement1;
        private Telerik.WinControls.UI.CommandBarButton commandBarButtonEdit;
    }
}
