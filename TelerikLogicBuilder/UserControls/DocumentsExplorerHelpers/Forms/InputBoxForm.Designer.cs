﻿namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms
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
            radPanelBottom = new Telerik.WinControls.UI.RadPanel();
            radPanelMessages = new Telerik.WinControls.UI.RadPanel();
            radPanelCommandButtons = new Telerik.WinControls.UI.RadPanel();
            tableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            radButtonCancel = new Telerik.WinControls.UI.RadButton();
            radButtonOk = new Telerik.WinControls.UI.RadButton();
            radPanelFill = new Telerik.WinControls.UI.RadPanel();
            radGroupBoxPrompt = new Telerik.WinControls.UI.RadGroupBox();
            radTextBoxInput = new Telerik.WinControls.UI.RadTextBox();
            radPanelTop = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)radPanelBottom).BeginInit();
            radPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelMessages).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelCommandButtons).BeginInit();
            radPanelCommandButtons.SuspendLayout();
            tableLayoutPanelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radButtonCancel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radButtonOk).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelFill).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radGroupBoxPrompt).BeginInit();
            radGroupBoxPrompt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radTextBoxInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelTop).BeginInit();
            radPanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();
            SuspendLayout();
            // 
            // radPanelBottom
            // 
            radPanelBottom.Controls.Add(radPanelMessages);
            radPanelBottom.Controls.Add(radPanelCommandButtons);
            radPanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            radPanelBottom.Location = new System.Drawing.Point(0, 39);
            radPanelBottom.Name = "radPanelBottom";
            radPanelBottom.Size = new System.Drawing.Size(1000, 137);
            radPanelBottom.TabIndex = 1;
            // 
            // radPanelMessages
            // 
            radPanelMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelMessages.Location = new System.Drawing.Point(0, 0);
            radPanelMessages.Name = "radPanelMessages";
            radPanelMessages.Size = new System.Drawing.Size(839, 137);
            radPanelMessages.TabIndex = 3;
            // 
            // radPanelCommandButtons
            // 
            radPanelCommandButtons.Controls.Add(tableLayoutPanelButtons);
            radPanelCommandButtons.Dock = System.Windows.Forms.DockStyle.Right;
            radPanelCommandButtons.Location = new System.Drawing.Point(839, 0);
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
            // radPanelFill
            // 
            radPanelFill.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelFill.Location = new System.Drawing.Point(0, 73);
            radPanelFill.Name = "radPanelFill";
            radPanelFill.Size = new System.Drawing.Size(1000, 0);
            radPanelFill.TabIndex = 2;
            // 
            // radGroupBoxPrompt
            // 
            radGroupBoxPrompt.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            radGroupBoxPrompt.Controls.Add(radTextBoxInput);
            radGroupBoxPrompt.Dock = System.Windows.Forms.DockStyle.Fill;
            radGroupBoxPrompt.HeaderText = "File Type";
            radGroupBoxPrompt.Location = new System.Drawing.Point(0, 0);
            radGroupBoxPrompt.Name = "radGroupBoxPrompt";
            radGroupBoxPrompt.Padding = new System.Windows.Forms.Padding(15, 32, 15, 2);
            radGroupBoxPrompt.Size = new System.Drawing.Size(1000, 73);
            radGroupBoxPrompt.TabIndex = 0;
            radGroupBoxPrompt.Text = "File Type";
            // 
            // radTextBoxInput
            // 
            radTextBoxInput.Dock = System.Windows.Forms.DockStyle.Top;
            radTextBoxInput.Location = new System.Drawing.Point(15, 32);
            radTextBoxInput.Name = "radTextBoxInput";
            radTextBoxInput.Size = new System.Drawing.Size(970, 20);
            radTextBoxInput.TabIndex = 3;
            radTextBoxInput.TextChanged += RadTextBoxInput_TextChanged;
            // 
            // radPanelTop
            // 
            radPanelTop.Controls.Add(radGroupBoxPrompt);
            radPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            radPanelTop.Location = new System.Drawing.Point(0, 0);
            radPanelTop.Name = "radPanelTop";
            radPanelTop.Size = new System.Drawing.Size(1000, 73);
            radPanelTop.TabIndex = 3;
            // 
            // InputBoxForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(9, 21);
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1000, 176);
            Controls.Add(radPanelFill);
            Controls.Add(radPanelTop);
            Controls.Add(radPanelBottom);
            Name = "InputBoxForm";
            Text = "InputBoxForm";
            ((System.ComponentModel.ISupportInitialize)radPanelBottom).EndInit();
            radPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelMessages).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelCommandButtons).EndInit();
            radPanelCommandButtons.ResumeLayout(false);
            tableLayoutPanelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radButtonCancel).EndInit();
            ((System.ComponentModel.ISupportInitialize)radButtonOk).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelFill).EndInit();
            ((System.ComponentModel.ISupportInitialize)radGroupBoxPrompt).EndInit();
            radGroupBoxPrompt.ResumeLayout(false);
            radGroupBoxPrompt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)radTextBoxInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelTop).EndInit();
            radPanelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this).EndInit();
            ResumeLayout(false);
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButtons;
        private Telerik.WinControls.UI.RadPanel radPanelTop;
    }
}
