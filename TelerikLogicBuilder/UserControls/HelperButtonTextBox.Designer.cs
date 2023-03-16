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
            radPanelTextBox = new Telerik.WinControls.UI.RadPanel();
            radPanelButton = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)radPanelTextBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelButton).BeginInit();
            SuspendLayout();
            // 
            // radPanelTextBox
            // 
            radPanelTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelTextBox.Location = new System.Drawing.Point(0, 0);
            radPanelTextBox.Margin = new System.Windows.Forms.Padding(0);
            radPanelTextBox.Name = "radPanelTextBox";
            radPanelTextBox.Size = new System.Drawing.Size(320, 28);
            radPanelTextBox.TabIndex = 2;
            // 
            // radPanelButton
            // 
            radPanelButton.Dock = System.Windows.Forms.DockStyle.Right;
            radPanelButton.Location = new System.Drawing.Point(320, 0);
            radPanelButton.Margin = new System.Windows.Forms.Padding(0);
            radPanelButton.Name = "radPanelButton";
            radPanelButton.Size = new System.Drawing.Size(30, 28);
            radPanelButton.TabIndex = 3;
            // 
            // HelperButtonTextBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(radPanelTextBox);
            Controls.Add(radPanelButton);
            Margin = new System.Windows.Forms.Padding(0);
            Name = "HelperButtonTextBox";
            Size = new System.Drawing.Size(350, 28);
            ((System.ComponentModel.ISupportInitialize)radPanelTextBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelButton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelTextBox;
        private Telerik.WinControls.UI.RadPanel radPanelButton;
    }
}
