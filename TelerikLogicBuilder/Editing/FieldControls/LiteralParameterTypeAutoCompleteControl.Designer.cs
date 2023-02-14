namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    partial class LiteralParameterTypeAutoCompleteControl
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
            this.radPanelRight = new Telerik.WinControls.UI.RadPanel();
            this.radPanelButton = new Telerik.WinControls.UI.RadPanel();
            this.radPanelDropDownList = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelRight)).BeginInit();
            this.radPanelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDropDownList)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelRight
            // 
            this.radPanelRight.Controls.Add(this.radPanelButton);
            this.radPanelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelRight.Location = new System.Drawing.Point(320, 0);
            this.radPanelRight.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelRight.Name = "radPanelRight";
            this.radPanelRight.Size = new System.Drawing.Size(30, 30);
            this.radPanelRight.TabIndex = 1;
            // 
            // radPanelButton
            // 
            this.radPanelButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanelButton.Location = new System.Drawing.Point(0, 0);
            this.radPanelButton.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelButton.Name = "radPanelButton";
            this.radPanelButton.Size = new System.Drawing.Size(30, 28);
            this.radPanelButton.TabIndex = 2;
            // 
            // radPanelDropDownList
            // 
            this.radPanelDropDownList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelDropDownList.Location = new System.Drawing.Point(0, 0);
            this.radPanelDropDownList.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelDropDownList.Name = "radPanelDropDownList";
            this.radPanelDropDownList.Size = new System.Drawing.Size(320, 30);
            this.radPanelDropDownList.TabIndex = 2;
            // 
            // LiteralParameterTypeAutoCompleteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanelDropDownList);
            this.Controls.Add(this.radPanelRight);
            this.Name = "LiteralParameterTypeAutoCompleteControl";
            this.Size = new System.Drawing.Size(350, 30);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelRight)).EndInit();
            this.radPanelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDropDownList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelRight;
        private Telerik.WinControls.UI.RadPanel radPanelButton;
        private Telerik.WinControls.UI.RadPanel radPanelDropDownList;
    }
}
