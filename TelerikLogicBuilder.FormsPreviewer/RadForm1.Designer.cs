namespace TelerikLogicBuilder.FormsPreviewer
{
    partial class RadForm1
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
            this.btnSelectVariableForm = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectVariableForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonSelectVariableForm
            // 
            this.btnSelectVariableForm.Location = new System.Drawing.Point(39, 12);
            this.btnSelectVariableForm.Name = "radButtonSelectVariableForm";
            this.btnSelectVariableForm.Size = new System.Drawing.Size(151, 24);
            this.btnSelectVariableForm.TabIndex = 0;
            this.btnSelectVariableForm.Text = "Select Variable Form";
            // 
            // RadForm1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(9, 21);
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 448);
            this.Controls.Add(this.btnSelectVariableForm);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "RadForm1";
            this.Text = "RadForm1";
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectVariableForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton btnSelectVariableForm;
    }
}