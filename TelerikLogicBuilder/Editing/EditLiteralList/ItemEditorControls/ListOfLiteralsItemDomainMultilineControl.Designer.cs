namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls
{
    partial class ListOfLiteralsItemDomainMultilineControl
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
            this.radPanelRight = new Telerik.WinControls.UI.RadPanel();
            this.radPanelCommandBar = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelRichInputBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelRight)).BeginInit();
            this.radPanelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelCommandBar)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelRichInputBox
            // 
            this.radPanelRichInputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelRichInputBox.Location = new System.Drawing.Point(0, 0);
            this.radPanelRichInputBox.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelRichInputBox.Name = "radPanelRichInputBox";
            this.radPanelRichInputBox.Size = new System.Drawing.Size(260, 95);
            this.radPanelRichInputBox.TabIndex = 5;
            // 
            // radPanelRight
            // 
            this.radPanelRight.Controls.Add(this.radPanelCommandBar);
            this.radPanelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelRight.Location = new System.Drawing.Point(260, 0);
            this.radPanelRight.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelRight.Name = "radPanelRight";
            this.radPanelRight.Size = new System.Drawing.Size(90, 95);
            this.radPanelRight.TabIndex = 4;
            // 
            // radPanelCommandBar
            // 
            this.radPanelCommandBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanelCommandBar.Location = new System.Drawing.Point(0, 0);
            this.radPanelCommandBar.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelCommandBar.Name = "radPanelCommandBar";
            this.radPanelCommandBar.Size = new System.Drawing.Size(90, 28);
            this.radPanelCommandBar.TabIndex = 2;
            // 
            // ListOfLiteralsParameterDomainMultilineControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanelRichInputBox);
            this.Controls.Add(this.radPanelRight);
            this.Name = "ListOfLiteralsParameterDomainMultilineControl";
            this.Size = new System.Drawing.Size(350, 95);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelRichInputBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelRight)).EndInit();
            this.radPanelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelCommandBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelRichInputBox;
        private Telerik.WinControls.UI.RadPanel radPanelRight;
        private Telerik.WinControls.UI.RadPanel radPanelCommandBar;
    }
}
