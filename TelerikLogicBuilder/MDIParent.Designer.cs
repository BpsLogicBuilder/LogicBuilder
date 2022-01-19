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
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.commandBarButtonEdit = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButtonSave = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButtonSaveAll = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButtonValidate = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButtonBuild = new Telerik.WinControls.UI.CommandBarButton();
            this.radCommandBar1 = new Telerik.WinControls.UI.RadCommandBar();
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
            // commandBarRowElement1
            // 
            resources.ApplyResources(this.commandBarRowElement1, "commandBarRowElement1");
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Name = "commandBarRowElement1";
            this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement1});
            // 
            // commandBarStripElement1
            // 
            resources.ApplyResources(this.commandBarStripElement1, "commandBarStripElement1");
            this.commandBarStripElement1.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.commandBarButtonEdit,
            this.commandBarButtonSave,
            this.commandBarButtonSaveAll,
            this.commandBarButtonValidate,
            this.commandBarButtonBuild});
            this.commandBarStripElement1.Name = "commandBarStripElement1";
            // 
            // commandBarButtonEdit
            // 
            this.commandBarButtonEdit.ClipText = false;
            resources.ApplyResources(this.commandBarButtonEdit, "commandBarButtonEdit");
            this.commandBarButtonEdit.Image = global::TelerikLogicBuilder.Properties.Resources.Edit;
            this.commandBarButtonEdit.Name = "commandBarButtonEdit";
            this.commandBarButtonEdit.Click += new System.EventHandler(this.commandBarButtonEdit_Click);
            // 
            // commandBarButtonSave
            // 
            this.commandBarButtonSave.AccessibleName = "commandBarButton1";
            resources.ApplyResources(this.commandBarButtonSave, "commandBarButtonSave");
            this.commandBarButtonSave.Image = global::TelerikLogicBuilder.Properties.Resources.Save;
            this.commandBarButtonSave.Name = "commandBarButtonSave";
            // 
            // commandBarButtonSaveAll
            // 
            this.commandBarButtonSaveAll.AccessibleName = "commandBarButton";
            resources.ApplyResources(this.commandBarButtonSaveAll, "commandBarButtonSaveAll");
            this.commandBarButtonSaveAll.Image = global::TelerikLogicBuilder.Properties.Resources.SaveAll;
            this.commandBarButtonSaveAll.Name = "commandBarButtonSaveAll";
            // 
            // commandBarButtonValidate
            // 
            resources.ApplyResources(this.commandBarButtonValidate, "commandBarButtonValidate");
            this.commandBarButtonValidate.Image = global::TelerikLogicBuilder.Properties.Resources.Validate;
            this.commandBarButtonValidate.Name = "commandBarButtonValidate";
            // 
            // commandBarButtonBuild
            // 
            resources.ApplyResources(this.commandBarButtonBuild, "commandBarButtonBuild");
            this.commandBarButtonBuild.Image = global::TelerikLogicBuilder.Properties.Resources.Build;
            this.commandBarButtonBuild.Name = "commandBarButtonBuild";
            // 
            // radCommandBar1
            // 
            resources.ApplyResources(this.radCommandBar1, "radCommandBar1");
            this.radCommandBar1.Name = "radCommandBar1";
            this.radCommandBar1.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1});
            this.radCommandBar1.Click += new System.EventHandler(this.radCommandBar1_Click);
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
        private Telerik.WinControls.UI.CommandBarButton commandBarButtonSave;
        private Telerik.WinControls.UI.CommandBarButton commandBarButtonSaveAll;
        private Telerik.WinControls.UI.CommandBarButton commandBarButtonValidate;
        private Telerik.WinControls.UI.CommandBarButton commandBarButtonBuild;
    }
}
