namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.LiteralListItemEditor
{
    partial class ListOfLiteralsItemTypeAutoCompleteControl
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
            this.radPanelDropDownList = new Telerik.WinControls.UI.RadPanel();
            this.radPanelButton = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDropDownList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelButton)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelDropDownList
            // 
            this.radPanelDropDownList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelDropDownList.Location = new System.Drawing.Point(0, 0);
            this.radPanelDropDownList.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelDropDownList.Name = "radPanelDropDownList";
            this.radPanelDropDownList.Size = new System.Drawing.Size(320, 28);
            this.radPanelDropDownList.TabIndex = 2;
            // 
            // radPanelButton
            // 
            this.radPanelButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelButton.Location = new System.Drawing.Point(320, 0);
            this.radPanelButton.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelButton.Name = "radPanelButton";
            this.radPanelButton.Size = new System.Drawing.Size(30, 28);
            this.radPanelButton.TabIndex = 3;
            // 
            // ListOfLiteralsParameterTypeAutoCompleteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanelDropDownList);
            this.Controls.Add(this.radPanelButton);
            this.Name = "ListOfLiteralsParameterTypeAutoCompleteControl";
            this.Size = new System.Drawing.Size(350, 28);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDropDownList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelDropDownList;
        private Telerik.WinControls.UI.RadPanel radPanelButton;
    }
}
