namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment
{
    partial class SelectFragmentListViewControl
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
            radListControl1 = new Telerik.WinControls.UI.RadListControl();
            ((System.ComponentModel.ISupportInitialize)radListControl1).BeginInit();
            SuspendLayout();
            // 
            // radListControl1
            // 
            radListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            radListControl1.Location = new System.Drawing.Point(0, 0);
            radListControl1.Name = "radListControl1";
            radListControl1.Size = new System.Drawing.Size(471, 259);
            radListControl1.TabIndex = 2;
            // 
            // SelectFragmentListViewControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(radListControl1);
            Name = "SelectFragmentListViewControl";
            Size = new System.Drawing.Size(471, 259);
            ((System.ComponentModel.ISupportInitialize)radListControl1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadListControl radListControl1;
    }
}
