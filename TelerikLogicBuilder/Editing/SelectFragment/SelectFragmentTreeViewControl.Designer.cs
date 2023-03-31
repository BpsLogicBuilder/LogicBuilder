namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment
{
    partial class SelectFragmentTreeViewControl
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
            radTreeView1 = new Telerik.WinControls.UI.RadTreeView();
            ((System.ComponentModel.ISupportInitialize)radTreeView1).BeginInit();
            SuspendLayout();
            // 
            // radTreeView1
            // 
            radTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            radTreeView1.Location = new System.Drawing.Point(0, 0);
            radTreeView1.Name = "radTreeView1";
            radTreeView1.Size = new System.Drawing.Size(481, 343);
            radTreeView1.SpacingBetweenNodes = -1;
            radTreeView1.TabIndex = 2;
            // 
            // SelectFragmentTreeViewControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(radTreeView1);
            Name = "SelectFragmentTreeViewControl";
            Size = new System.Drawing.Size(481, 343);
            ((System.ComponentModel.ISupportInitialize)radTreeView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadTreeView radTreeView1;
    }
}
