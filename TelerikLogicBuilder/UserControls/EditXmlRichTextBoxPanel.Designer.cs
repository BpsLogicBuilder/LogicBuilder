namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    partial class EditXmlRichTextBoxPanel
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
            radPanel1 = new Telerik.WinControls.UI.RadPanel();
            richTextBox1 = new Components.TextOnlyRichTextBox();
            ((System.ComponentModel.ISupportInitialize)radPanel1).BeginInit();
            radPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // radPanel1
            // 
            radPanel1.Controls.Add(richTextBox1);
            radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanel1.Location = new System.Drawing.Point(0, 0);
            radPanel1.Name = "radPanel1";
            radPanel1.Size = new System.Drawing.Size(681, 429);
            radPanel1.TabIndex = 2;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.Color.White;
            richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            richTextBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            richTextBox1.ForeColor = System.Drawing.Color.Black;
            richTextBox1.Location = new System.Drawing.Point(0, 0);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(681, 429);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // EditXmlRichTextBoxPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            Controls.Add(radPanel1);
            Name = "EditXmlRichTextBoxPanel";
            Size = new System.Drawing.Size(681, 429);
            ((System.ComponentModel.ISupportInitialize)radPanel1).EndInit();
            radPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Components.TextOnlyRichTextBox richTextBox1;
    }
}
