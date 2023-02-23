namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    partial class ConstructorGenericParametersControl
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
            this.radPanelRichTextBox = new Telerik.WinControls.UI.RadPanel();
            this.radPanelButton = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelRichTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelButton)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelRichTextBox
            // 
            this.radPanelRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.radPanelRichTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelRichTextBox.Name = "radPanelRichTextBox";
            this.radPanelRichTextBox.Size = new System.Drawing.Size(275, 28);
            this.radPanelRichTextBox.TabIndex = 0;
            // 
            // radPanelCommandBar
            // 
            this.radPanelButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelButton.Location = new System.Drawing.Point(275, 0);
            this.radPanelButton.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelButton.Name = "radPanelCommandBar";
            this.radPanelButton.Size = new System.Drawing.Size(75, 28);
            this.radPanelButton.TabIndex = 1;
            // 
            // ConstructorGenericParametersControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanelRichTextBox);
            this.Controls.Add(this.radPanelButton);
            this.Name = "ConstructorGenericParametersControl";
            this.Size = new System.Drawing.Size(350, 28);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelRichTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelRichTextBox;
        private Telerik.WinControls.UI.RadPanel radPanelButton;
    }
}
