namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment
{
    partial class SelectFragmentDropDownViewControl
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
            radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
            ((System.ComponentModel.ISupportInitialize)radDropDownList1).BeginInit();
            SuspendLayout();
            // 
            // radDropDownList1
            // 
            radDropDownList1.Dock = System.Windows.Forms.DockStyle.Top;
            radDropDownList1.DropDownAnimationEnabled = true;
            radDropDownList1.Location = new System.Drawing.Point(0, 0);
            radDropDownList1.Name = "radDropDownList1";
            radDropDownList1.Size = new System.Drawing.Size(422, 20);
            radDropDownList1.TabIndex = 2;
            // 
            // SelectFragmentDropDownViewControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            Controls.Add(radDropDownList1);
            Name = "SelectFragmentDropDownViewControl";
            Size = new System.Drawing.Size(422, 193);
            ((System.ComponentModel.ISupportInitialize)radDropDownList1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
    }
}
