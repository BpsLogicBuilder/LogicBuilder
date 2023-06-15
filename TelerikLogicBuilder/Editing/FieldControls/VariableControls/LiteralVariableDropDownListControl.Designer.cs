namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls
{
    partial class LiteralVariableDropDownListControl
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
            radPanelDropDownList = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)radPanelDropDownList).BeginInit();
            SuspendLayout();
            // 
            // radPanelDropDownList
            // 
            radPanelDropDownList.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelDropDownList.Location = new System.Drawing.Point(0, 0);
            radPanelDropDownList.Margin = new System.Windows.Forms.Padding(0);
            radPanelDropDownList.Name = "radPanelDropDownList";
            radPanelDropDownList.Size = new System.Drawing.Size(350, 28);
            radPanelDropDownList.TabIndex = 1;
            // 
            // LiteralVariableDropDownListControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            Controls.Add(radPanelDropDownList);
            Name = "LiteralVariableDropDownListControl";
            Size = new System.Drawing.Size(350, 28);
            ((System.ComponentModel.ISupportInitialize)radPanelDropDownList).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelDropDownList;
    }
}
