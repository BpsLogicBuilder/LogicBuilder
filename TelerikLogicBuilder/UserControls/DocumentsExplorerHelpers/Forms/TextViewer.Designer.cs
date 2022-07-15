namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms
{
    partial class TextViewer
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
            this.richTextBoxViewerPanel1 = new ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxViewerPanel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBoxViewerPanel1
            // 
            this.richTextBoxViewerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxViewerPanel1.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxViewerPanel1.Name = "richTextBoxViewerPanel1";
            this.richTextBoxViewerPanel1.Size = new System.Drawing.Size(1184, 915);
            this.richTextBoxViewerPanel1.TabIndex = 0;
            // 
            // TextViewer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(9, 21);
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 915);
            this.Controls.Add(this.richTextBoxViewerPanel1);
            this.Name = "TextViewer";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Text Viewer";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBoxViewerPanel richTextBoxViewerPanel1;
    }
}
