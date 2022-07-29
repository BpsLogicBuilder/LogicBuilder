namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms
{
    partial class InputBoxForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radPanelBottom = new Telerik.WinControls.UI.RadPanel();
            this.radPanelCommandButtons = new Telerik.WinControls.UI.RadPanel();
            this.radButtonCancel = new Telerik.WinControls.UI.RadButton();
            this.radButtonOk = new Telerik.WinControls.UI.RadButton();
            this.radPanelFill = new Telerik.WinControls.UI.RadPanel();
            this.radGroupBoxPrompt = new Telerik.WinControls.UI.RadGroupBox();
            this.radTextBoxInput = new Telerik.WinControls.UI.RadTextBox();
            this.radPanelMessages = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelBottom)).BeginInit();
            this.radPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelCommandButtons)).BeginInit();
            this.radPanelCommandButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelFill)).BeginInit();
            this.radPanelFill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxPrompt)).BeginInit();
            this.radGroupBoxPrompt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelBottom
            // 
            this.radPanelBottom.Controls.Add(this.radPanelMessages);
            this.radPanelBottom.Controls.Add(this.radPanelCommandButtons);
            this.radPanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radPanelBottom.Location = new System.Drawing.Point(0, 97);
            this.radPanelBottom.Name = "radPanelBottom";
            this.radPanelBottom.Size = new System.Drawing.Size(754, 137);
            this.radPanelBottom.TabIndex = 1;
            // 
            // radPanelCommandButtons
            // 
            this.radPanelCommandButtons.Controls.Add(this.radButtonCancel);
            this.radPanelCommandButtons.Controls.Add(this.radButtonOk);
            this.radPanelCommandButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelCommandButtons.Location = new System.Drawing.Point(593, 0);
            this.radPanelCommandButtons.Name = "radPanelCommandButtons";
            this.radPanelCommandButtons.Size = new System.Drawing.Size(161, 137);
            this.radPanelCommandButtons.TabIndex = 2;
            // 
            // radButtonCancel
            // 
            this.radButtonCancel.Location = new System.Drawing.Point(20, 63);
            this.radButtonCancel.Name = "radButtonCancel";
            this.radButtonCancel.Size = new System.Drawing.Size(110, 24);
            this.radButtonCancel.TabIndex = 1;
            this.radButtonCancel.Text = "&Cancel";
            // 
            // radButtonOk
            // 
            this.radButtonOk.Location = new System.Drawing.Point(20, 21);
            this.radButtonOk.Name = "radButtonOk";
            this.radButtonOk.Size = new System.Drawing.Size(110, 24);
            this.radButtonOk.TabIndex = 0;
            this.radButtonOk.Text = "&Ok";
            // 
            // radPanelFill
            // 
            this.radPanelFill.Controls.Add(this.radGroupBoxPrompt);
            this.radPanelFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelFill.Location = new System.Drawing.Point(0, 0);
            this.radPanelFill.Name = "radPanelFill";
            this.radPanelFill.Padding = new System.Windows.Forms.Padding(10);
            this.radPanelFill.Size = new System.Drawing.Size(754, 234);
            this.radPanelFill.TabIndex = 2;
            // 
            // radGroupBoxPrompt
            // 
            this.radGroupBoxPrompt.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxPrompt.Controls.Add(this.radTextBoxInput);
            this.radGroupBoxPrompt.HeaderText = "File Type";
            this.radGroupBoxPrompt.Location = new System.Drawing.Point(12, 10);
            this.radGroupBoxPrompt.Name = "radGroupBoxPrompt";
            this.radGroupBoxPrompt.Size = new System.Drawing.Size(729, 77);
            this.radGroupBoxPrompt.TabIndex = 0;
            this.radGroupBoxPrompt.Text = "File Type";
            // 
            // radTextBoxInput
            // 
            this.radTextBoxInput.Location = new System.Drawing.Point(11, 32);
            this.radTextBoxInput.Name = "radTextBoxInput";
            this.radTextBoxInput.Size = new System.Drawing.Size(713, 20);
            this.radTextBoxInput.TabIndex = 3;
            this.radTextBoxInput.TextChanged += new System.EventHandler(this.RadTextBoxInput_TextChanged);
            // 
            // radPanelMessages
            // 
            this.radPanelMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelMessages.Location = new System.Drawing.Point(0, 0);
            this.radPanelMessages.Name = "radPanelMessages";
            this.radPanelMessages.Size = new System.Drawing.Size(593, 137);
            this.radPanelMessages.TabIndex = 3;
            // 
            // InputBoxForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(9, 21);
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 234);
            this.Controls.Add(this.radPanelBottom);
            this.Controls.Add(this.radPanelFill);
            this.Name = "InputBoxForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "InputBoxForm";
            ((System.ComponentModel.ISupportInitialize)(this.radPanelBottom)).EndInit();
            this.radPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelCommandButtons)).EndInit();
            this.radPanelCommandButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelFill)).EndInit();
            this.radPanelFill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxPrompt)).EndInit();
            this.radGroupBoxPrompt.ResumeLayout(false);
            this.radGroupBoxPrompt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelBottom;
        private Telerik.WinControls.UI.RadPanel radPanelCommandButtons;
        private Telerik.WinControls.UI.RadButton radButtonCancel;
        private Telerik.WinControls.UI.RadButton radButtonOk;
        private Telerik.WinControls.UI.RadPanel radPanelFill;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxPrompt;
        private Telerik.WinControls.UI.RadTextBox radTextBoxInput;
        private Telerik.WinControls.UI.RadPanel radPanelMessages;
    }
}
