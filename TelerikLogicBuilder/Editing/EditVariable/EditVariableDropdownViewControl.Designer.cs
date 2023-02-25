namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable
{
    partial class EditVariableDropdownViewControl
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
            this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit();
            this.SuspendLayout();
            // 
            // radDropDownList1
            // 
            this.radDropDownList1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radDropDownList1.DropDownAnimationEnabled = true;
            this.radDropDownList1.Location = new System.Drawing.Point(0, 0);
            this.radDropDownList1.Name = "radDropDownList1";
            this.radDropDownList1.Size = new System.Drawing.Size(422, 20);
            this.radDropDownList1.TabIndex = 0;
            // 
            // SelectVariableDropdownControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radDropDownList1);
            this.Name = "SelectVariableDropDownControl";
            this.Size = new System.Drawing.Size(422, 193);
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
    }
}
