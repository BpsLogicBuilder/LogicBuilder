namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    partial class HelperButtonTextBox
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
            this.radButtonTextBox1 = new Telerik.WinControls.UI.RadButtonTextBox();
            this.radButtonHelper = new Telerik.WinControls.UI.RadButtonElement();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonTextBox1
            // 
            this.radButtonTextBox1.AutoSize = false;
            this.radButtonTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radButtonTextBox1.Location = new System.Drawing.Point(0, 0);
            this.radButtonTextBox1.Name = "radButtonTextBox1";
            this.radButtonTextBox1.RightButtonItems.AddRange(new Telerik.WinControls.RadItem[] {
            this.radButtonHelper});
            this.radButtonTextBox1.Size = new System.Drawing.Size(350, 24);
            this.radButtonTextBox1.TabIndex = 0;
            // 
            // radButtonHelper
            // 
            this.radButtonHelper.AutoSize = true;
            this.radButtonHelper.Image = global::ABIS.LogicBuilder.FlowBuilder.Properties.Resources.more;
            this.radButtonHelper.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.radButtonHelper.Name = "radButtonHelper";
            this.radButtonHelper.Padding = new System.Windows.Forms.Padding(0);
            this.radButtonHelper.Text = "";
            // 
            // HelperButtonTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.radButtonTextBox1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "HelperButtonTextBox";
            this.Size = new System.Drawing.Size(350, 24);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButtonTextBox radButtonTextBox1;
        private Telerik.WinControls.UI.RadButtonElement radButtonHelper;
    }
}
