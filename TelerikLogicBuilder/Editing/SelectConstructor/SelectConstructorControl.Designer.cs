namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor
{
    partial class SelectConstructorControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectConstructorControl));
            this.radCommandBar1 = new Telerik.WinControls.UI.RadCommandBar();
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.commandBarToggleButtonList = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarToggleButtonTreeView = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarToggleButtonDropDown = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.radPanelConstructorView = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelConstructorView)).BeginInit();
            this.SuspendLayout();
            // 
            // radCommandBar1
            // 
            this.radCommandBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radCommandBar1.Location = new System.Drawing.Point(0, 0);
            this.radCommandBar1.Name = "radCommandBar1";
            this.radCommandBar1.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1});
            this.radCommandBar1.Size = new System.Drawing.Size(633, 30);
            this.radCommandBar1.TabIndex = 0;
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
            this.commandBarStripElement1.DisplayName = "commandBarStripElement1";
            this.commandBarStripElement1.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.commandBarToggleButtonList,
            this.commandBarToggleButtonTreeView,
            this.commandBarToggleButtonDropDown});
            this.commandBarStripElement1.Name = "commandBarStripElement1";
            // 
            // commandBarToggleButtonList
            // 
            this.commandBarToggleButtonList.DisplayName = "commandBarToggleButton1";
            this.commandBarToggleButtonList.Image = ((System.Drawing.Image)(resources.GetObject("commandBarToggleButtonList.Image")));
            this.commandBarToggleButtonList.Name = "commandBarToggleButtonList";
            this.commandBarToggleButtonList.Text = "commandBarToggleButton1";
            // 
            // commandBarToggleButtonTreeView
            // 
            this.commandBarToggleButtonTreeView.DisplayName = "commandBarToggleButton2";
            this.commandBarToggleButtonTreeView.Image = ((System.Drawing.Image)(resources.GetObject("commandBarToggleButtonTreeView.Image")));
            this.commandBarToggleButtonTreeView.Name = "commandBarToggleButtonTreeView";
            this.commandBarToggleButtonTreeView.Text = "commandBarToggleButton2";
            // 
            // commandBarToggleButtonDropDown
            // 
            this.commandBarToggleButtonDropDown.DisplayName = "commandBarToggleButton3";
            this.commandBarToggleButtonDropDown.Image = ((System.Drawing.Image)(resources.GetObject("commandBarToggleButtonDropDown.Image")));
            this.commandBarToggleButtonDropDown.Name = "commandBarToggleButtonDropDown";
            this.commandBarToggleButtonDropDown.Text = "commandBarToggleButton3";
            // 
            // radPanelConstructorView
            // 
            this.radPanelConstructorView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelConstructorView.Location = new System.Drawing.Point(0, 30);
            this.radPanelConstructorView.Name = "radPanelConstructorView";
            this.radPanelConstructorView.Size = new System.Drawing.Size(633, 469);
            this.radPanelConstructorView.TabIndex = 1;
            // 
            // SelectConstructorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanelConstructorView);
            this.Controls.Add(this.radCommandBar1);
            this.Name = "SelectConstructorControl";
            this.Size = new System.Drawing.Size(633, 499);
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelConstructorView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadCommandBar radCommandBar1;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement1;
        private Telerik.WinControls.UI.CommandBarToggleButton commandBarToggleButtonList;
        private Telerik.WinControls.UI.CommandBarToggleButton commandBarToggleButtonTreeView;
        private Telerik.WinControls.UI.CommandBarToggleButton commandBarToggleButtonDropDown;
        private Telerik.WinControls.UI.RadPanel radPanelConstructorView;
    }
}
