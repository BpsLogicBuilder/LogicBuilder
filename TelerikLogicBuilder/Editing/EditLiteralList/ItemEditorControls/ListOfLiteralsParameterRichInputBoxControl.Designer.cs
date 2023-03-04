namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls
{
    partial class ListOfLiteralsParameterRichInputBoxControl
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
            this.radPanelRichInputBox = new Telerik.WinControls.UI.RadPanel();
            this.radPanelCommandBar = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelRichInputBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelCommandBar)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelRichInputBox
            // 
            this.radPanelRichInputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelRichInputBox.Location = new System.Drawing.Point(0, 0);
            this.radPanelRichInputBox.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelRichInputBox.Name = "radPanelRichInputBox";
            this.radPanelRichInputBox.Size = new System.Drawing.Size(275, 28);
            this.radPanelRichInputBox.TabIndex = 4;
            // 
            // radPanelCommandBar
            // 
            this.radPanelCommandBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelCommandBar.Location = new System.Drawing.Point(275, 0);
            this.radPanelCommandBar.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelCommandBar.Name = "radPanelCommandBar";
            this.radPanelCommandBar.Size = new System.Drawing.Size(75, 28);
            this.radPanelCommandBar.TabIndex = 3;
            // 
            // ListOfLiteralsParameterRichInputBoxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanelRichInputBox);
            this.Controls.Add(this.radPanelCommandBar);
            this.Name = "ListOfLiteralsParameterRichInputBoxControl";
            this.Size = new System.Drawing.Size(350, 28);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelRichInputBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelCommandBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelRichInputBox;
        private Telerik.WinControls.UI.RadPanel radPanelCommandBar;
    }
}
