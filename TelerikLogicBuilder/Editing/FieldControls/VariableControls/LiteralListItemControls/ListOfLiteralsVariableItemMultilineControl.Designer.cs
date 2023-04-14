namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.LiteralListItemControls
{
    partial class ListOfLiteralsVariableItemMultilineControl
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
            radPanelRichInputBox = new Telerik.WinControls.UI.RadPanel();
            radPanelRight = new Telerik.WinControls.UI.RadPanel();
            radPanelCommandBar = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)radPanelRichInputBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelRight).BeginInit();
            radPanelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelCommandBar).BeginInit();
            SuspendLayout();
            // 
            // radPanelRichInputBox
            // 
            radPanelRichInputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelRichInputBox.Location = new System.Drawing.Point(0, 0);
            radPanelRichInputBox.Margin = new System.Windows.Forms.Padding(0);
            radPanelRichInputBox.Name = "radPanelRichInputBox";
            radPanelRichInputBox.Size = new System.Drawing.Size(260, 95);
            radPanelRichInputBox.TabIndex = 5;
            // 
            // radPanelRight
            // 
            radPanelRight.Controls.Add(radPanelCommandBar);
            radPanelRight.Dock = System.Windows.Forms.DockStyle.Right;
            radPanelRight.Location = new System.Drawing.Point(260, 0);
            radPanelRight.Margin = new System.Windows.Forms.Padding(0);
            radPanelRight.Name = "radPanelRight";
            radPanelRight.Size = new System.Drawing.Size(90, 95);
            radPanelRight.TabIndex = 4;
            // 
            // radPanelCommandBar
            // 
            radPanelCommandBar.Dock = System.Windows.Forms.DockStyle.Top;
            radPanelCommandBar.Location = new System.Drawing.Point(0, 0);
            radPanelCommandBar.Margin = new System.Windows.Forms.Padding(0);
            radPanelCommandBar.Name = "radPanelCommandBar";
            radPanelCommandBar.Size = new System.Drawing.Size(90, 28);
            radPanelCommandBar.TabIndex = 2;
            // 
            // ListOfLiteralsVariableItemMultilineControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(radPanelRichInputBox);
            Controls.Add(radPanelRight);
            Name = "ListOfLiteralsVariableItemMultilineControl";
            Size = new System.Drawing.Size(350, 95);
            ((System.ComponentModel.ISupportInitialize)radPanelRichInputBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelRight).EndInit();
            radPanelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelCommandBar).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelRichInputBox;
        private Telerik.WinControls.UI.RadPanel radPanelRight;
        private Telerik.WinControls.UI.RadPanel radPanelCommandBar;
    }
}
