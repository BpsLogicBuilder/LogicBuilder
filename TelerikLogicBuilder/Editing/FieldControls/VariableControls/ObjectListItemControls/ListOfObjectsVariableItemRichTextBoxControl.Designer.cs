namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.ObjectListItemControls
{
    partial class ListOfObjectsVariableItemRichTextBoxControl
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
            radPanelRichTextBox = new Telerik.WinControls.UI.RadPanel();
            radPanelCommandBar = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)radPanelRichTextBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelCommandBar).BeginInit();
            SuspendLayout();
            // 
            // radPanelRichTextBox
            // 
            radPanelRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelRichTextBox.Location = new System.Drawing.Point(0, 0);
            radPanelRichTextBox.Margin = new System.Windows.Forms.Padding(0);
            radPanelRichTextBox.Name = "radPanelRichTextBox";
            radPanelRichTextBox.Size = new System.Drawing.Size(275, 28);
            radPanelRichTextBox.TabIndex = 8;
            // 
            // radPanelCommandBar
            // 
            radPanelCommandBar.Dock = System.Windows.Forms.DockStyle.Right;
            radPanelCommandBar.Location = new System.Drawing.Point(275, 0);
            radPanelCommandBar.Margin = new System.Windows.Forms.Padding(0);
            radPanelCommandBar.Name = "radPanelCommandBar";
            radPanelCommandBar.Size = new System.Drawing.Size(75, 28);
            radPanelCommandBar.TabIndex = 7;
            // 
            // ListOfObjectsVariableItemRichTextBoxControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(radPanelRichTextBox);
            Controls.Add(radPanelCommandBar);
            Name = "ListOfObjectsVariableItemRichTextBoxControl";
            Size = new System.Drawing.Size(350, 28);
            ((System.ComponentModel.ISupportInitialize)radPanelRichTextBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelCommandBar).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelRichTextBox;
        private Telerik.WinControls.UI.RadPanel radPanelCommandBar;
    }
}
