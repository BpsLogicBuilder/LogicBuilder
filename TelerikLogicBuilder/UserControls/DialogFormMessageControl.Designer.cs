namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    partial class DialogFormMessageControl
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
            this.radGroupBoxMessages = new Telerik.WinControls.UI.RadGroupBox();
            this.radPanelMessages = new Telerik.WinControls.UI.RadScrollablePanel();
            this.radLabelMessages = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxMessages)).BeginInit();
            this.radGroupBoxMessages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).BeginInit();
            this.radPanelMessages.PanelContainer.SuspendLayout();
            this.radPanelMessages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelMessages)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBoxMessages
            // 
            this.radGroupBoxMessages.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxMessages.Controls.Add(this.radPanelMessages);
            this.radGroupBoxMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBoxMessages.HeaderText = "Messages";
            this.radGroupBoxMessages.Location = new System.Drawing.Point(0, 0);
            this.radGroupBoxMessages.Name = "radGroupBoxMessages";
            this.radGroupBoxMessages.Size = new System.Drawing.Size(508, 193);
            this.radGroupBoxMessages.TabIndex = 0;
            this.radGroupBoxMessages.Text = "Messages";
            // 
            // radPanelMessages
            // 
            this.radPanelMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelMessages.Location = new System.Drawing.Point(2, 18);
            this.radPanelMessages.Name = "radPanelMessages";
            // 
            // radPanelMessages.PanelContainer
            // 
            this.radPanelMessages.PanelContainer.Controls.Add(this.radLabelMessages);
            this.radPanelMessages.PanelContainer.Size = new System.Drawing.Size(502, 171);
            this.radPanelMessages.Size = new System.Drawing.Size(504, 173);
            this.radPanelMessages.TabIndex = 0;
            // 
            // radLabelMessages
            // 
            this.radLabelMessages.Location = new System.Drawing.Point(0, 0);
            this.radLabelMessages.Name = "radLabelMessages";
            this.radLabelMessages.Size = new System.Drawing.Size(2, 2);
            this.radLabelMessages.TabIndex = 0;
            // 
            // DialogFormMessageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.radGroupBoxMessages);
            this.Name = "DialogFormMessageControl";
            this.Size = new System.Drawing.Size(508, 193);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxMessages)).EndInit();
            this.radGroupBoxMessages.ResumeLayout(false);
            this.radPanelMessages.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).EndInit();
            this.radPanelMessages.ResumeLayout(false);
            this.radPanelMessages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelMessages)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox radGroupBoxMessages;
        private Telerik.WinControls.UI.RadScrollablePanel radPanelMessages;
        private Telerik.WinControls.UI.RadLabel radLabelMessages;
    }
}
