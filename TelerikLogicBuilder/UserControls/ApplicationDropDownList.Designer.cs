namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    partial class ApplicationDropDownList
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
            this.cmbApplication = new Telerik.WinControls.UI.RadDropDownList();
            ((System.ComponentModel.ISupportInitialize)(this.cmbApplication)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbApplication
            // 
            this.cmbApplication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbApplication.DropDownAnimationEnabled = true;
            this.cmbApplication.Location = new System.Drawing.Point(0, 0);
            this.cmbApplication.Margin = new System.Windows.Forms.Padding(0);
            this.cmbApplication.Name = "cmbApplication";
            this.cmbApplication.Size = new System.Drawing.Size(653, 20);
            this.cmbApplication.TabIndex = 0;
            // 
            // ApplicationDropDownList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.cmbApplication);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ApplicationDropDownList";
            this.Size = new System.Drawing.Size(653, 20);
            ((System.ComponentModel.ISupportInitialize)(this.cmbApplication)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadDropDownList cmbApplication;
    }
}
