namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector
{
    partial class EditDecisionConnectorForm
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
            radPanelBottom = new Telerik.WinControls.UI.RadPanel();
            radPanelMessages = new Telerik.WinControls.UI.RadPanel();
            radPanelCommandButtons = new Telerik.WinControls.UI.RadPanel();
            tableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            radButtonCancel = new Telerik.WinControls.UI.RadButton();
            radButtonOk = new Telerik.WinControls.UI.RadButton();
            radPanelTop = new Telerik.WinControls.UI.RadPanel();
            radGroupBoxPrompt = new Telerik.WinControls.UI.RadGroupBox();
            cmbIndex = new Telerik.WinControls.UI.RadDropDownList();
            radPanelFill = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)radPanelBottom).BeginInit();
            radPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelMessages).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelCommandButtons).BeginInit();
            radPanelCommandButtons.SuspendLayout();
            tableLayoutPanelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radButtonCancel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radButtonOk).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelTop).BeginInit();
            radPanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radGroupBoxPrompt).BeginInit();
            radGroupBoxPrompt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmbIndex).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelFill).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();
            SuspendLayout();
            // 
            // radPanelBottom
            // 
            radPanelBottom.Controls.Add(radPanelMessages);
            radPanelBottom.Controls.Add(radPanelCommandButtons);
            radPanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            radPanelBottom.Location = new System.Drawing.Point(0, 82);
            radPanelBottom.Name = "radPanelBottom";
            radPanelBottom.Size = new System.Drawing.Size(1048, 137);
            radPanelBottom.TabIndex = 2;
            // 
            // radPanelMessages
            // 
            radPanelMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelMessages.Location = new System.Drawing.Point(0, 0);
            radPanelMessages.Name = "radPanelMessages";
            radPanelMessages.Size = new System.Drawing.Size(887, 137);
            radPanelMessages.TabIndex = 3;
            // 
            // radPanelCommandButtons
            // 
            radPanelCommandButtons.Controls.Add(tableLayoutPanelButtons);
            radPanelCommandButtons.Dock = System.Windows.Forms.DockStyle.Right;
            radPanelCommandButtons.Location = new System.Drawing.Point(887, 0);
            radPanelCommandButtons.Name = "radPanelCommandButtons";
            radPanelCommandButtons.Size = new System.Drawing.Size(161, 137);
            radPanelCommandButtons.TabIndex = 2;
            // 
            // tableLayoutPanelButtons
            // 
            tableLayoutPanelButtons.ColumnCount = 3;
            tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            tableLayoutPanelButtons.Controls.Add(radButtonCancel, 1, 3);
            tableLayoutPanelButtons.Controls.Add(radButtonOk, 1, 1);
            tableLayoutPanelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelButtons.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            tableLayoutPanelButtons.RowCount = 9;
            tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            tableLayoutPanelButtons.Size = new System.Drawing.Size(161, 137);
            tableLayoutPanelButtons.TabIndex = 1;
            // 
            // radButtonCancel
            // 
            radButtonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            radButtonCancel.Location = new System.Drawing.Point(24, 37);
            radButtonCancel.Margin = new System.Windows.Forms.Padding(0);
            radButtonCancel.Name = "radButtonCancel";
            radButtonCancel.Size = new System.Drawing.Size(112, 27);
            radButtonCancel.TabIndex = 1;
            radButtonCancel.Text = "&Cancel";
            // 
            // radButtonOk
            // 
            radButtonOk.Dock = System.Windows.Forms.DockStyle.Fill;
            radButtonOk.Location = new System.Drawing.Point(24, 5);
            radButtonOk.Margin = new System.Windows.Forms.Padding(0);
            radButtonOk.Name = "radButtonOk";
            radButtonOk.Size = new System.Drawing.Size(112, 27);
            radButtonOk.TabIndex = 0;
            radButtonOk.Text = "&Ok";
            // 
            // radPanelTop
            // 
            radPanelTop.Controls.Add(radGroupBoxPrompt);
            radPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            radPanelTop.Location = new System.Drawing.Point(0, 0);
            radPanelTop.Name = "radPanelTop";
            radPanelTop.Size = new System.Drawing.Size(1048, 73);
            radPanelTop.TabIndex = 4;
            // 
            // radGroupBoxPrompt
            // 
            radGroupBoxPrompt.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            radGroupBoxPrompt.Controls.Add(cmbIndex);
            radGroupBoxPrompt.Dock = System.Windows.Forms.DockStyle.Fill;
            radGroupBoxPrompt.HeaderText = "Update Connector";
            radGroupBoxPrompt.Location = new System.Drawing.Point(0, 0);
            radGroupBoxPrompt.Name = "radGroupBoxPrompt";
            radGroupBoxPrompt.Padding = new System.Windows.Forms.Padding(15, 32, 15, 2);
            radGroupBoxPrompt.Size = new System.Drawing.Size(1048, 73);
            radGroupBoxPrompt.TabIndex = 0;
            radGroupBoxPrompt.Text = "Update Connector";
            // 
            // cmbIndex
            // 
            cmbIndex.AutoSize = false;
            cmbIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            cmbIndex.DropDownAnimationEnabled = true;
            cmbIndex.Location = new System.Drawing.Point(15, 32);
            cmbIndex.Name = "cmbIndex";
            cmbIndex.Size = new System.Drawing.Size(1018, 39);
            cmbIndex.TabIndex = 0;
            // 
            // radPanelFill
            // 
            radPanelFill.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelFill.Location = new System.Drawing.Point(0, 73);
            radPanelFill.Name = "radPanelFill";
            radPanelFill.Size = new System.Drawing.Size(1048, 9);
            radPanelFill.TabIndex = 5;
            // 
            // EditDecisionConnectorForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(9, 21);
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1048, 219);
            Controls.Add(radPanelFill);
            Controls.Add(radPanelTop);
            Controls.Add(radPanelBottom);
            Name = "EditDecisionConnectorForm";
            Text = "Edit Connector";
            ((System.ComponentModel.ISupportInitialize)radPanelBottom).EndInit();
            radPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelMessages).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelCommandButtons).EndInit();
            radPanelCommandButtons.ResumeLayout(false);
            tableLayoutPanelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radButtonCancel).EndInit();
            ((System.ComponentModel.ISupportInitialize)radButtonOk).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelTop).EndInit();
            radPanelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radGroupBoxPrompt).EndInit();
            radGroupBoxPrompt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cmbIndex).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelFill).EndInit();
            ((System.ComponentModel.ISupportInitialize)this).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelBottom;
        private Telerik.WinControls.UI.RadPanel radPanelMessages;
        private Telerik.WinControls.UI.RadPanel radPanelCommandButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButtons;
        private Telerik.WinControls.UI.RadButton radButtonCancel;
        private Telerik.WinControls.UI.RadButton radButtonOk;
        private Telerik.WinControls.UI.RadPanel radPanelTop;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxPrompt;
        private Telerik.WinControls.UI.RadDropDownList cmbIndex;
        private Telerik.WinControls.UI.RadPanel radPanelFill;
    }
}
