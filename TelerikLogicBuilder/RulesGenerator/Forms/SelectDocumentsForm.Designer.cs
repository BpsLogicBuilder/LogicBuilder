﻿namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    partial class SelectDocumentsForm
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
            radPanelButtons = new Telerik.WinControls.UI.RadPanel();
            tableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            btnCancel = new Telerik.WinControls.UI.RadButton();
            btnOk = new Telerik.WinControls.UI.RadButton();
            radPanelTop = new Telerik.WinControls.UI.RadPanel();
            radGroupBoxTop = new Telerik.WinControls.UI.RadGroupBox();
            radTreeView = new Telerik.WinControls.UI.RadTreeView();
            ((System.ComponentModel.ISupportInitialize)radPanelBottom).BeginInit();
            radPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelMessages).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelButtons).BeginInit();
            radPanelButtons.SuspendLayout();
            tableLayoutPanelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnCancel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnOk).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelTop).BeginInit();
            radPanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radGroupBoxTop).BeginInit();
            radGroupBoxTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radTreeView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();
            SuspendLayout();
            // 
            // radPanelBottom
            // 
            radPanelBottom.Controls.Add(radPanelMessages);
            radPanelBottom.Controls.Add(radPanelButtons);
            radPanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            radPanelBottom.Location = new System.Drawing.Point(0, 450);
            radPanelBottom.Name = "radPanelBottom";
            radPanelBottom.Size = new System.Drawing.Size(961, 150);
            radPanelBottom.TabIndex = 1;
            // 
            // radPanelMessages
            // 
            radPanelMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelMessages.Location = new System.Drawing.Point(0, 0);
            radPanelMessages.Name = "radPanelMessages";
            radPanelMessages.Size = new System.Drawing.Size(801, 150);
            radPanelMessages.TabIndex = 2;
            // 
            // radPanelButtons
            // 
            radPanelButtons.Controls.Add(tableLayoutPanelButtons);
            radPanelButtons.Dock = System.Windows.Forms.DockStyle.Right;
            radPanelButtons.Location = new System.Drawing.Point(801, 0);
            radPanelButtons.Name = "radPanelButtons";
            radPanelButtons.Size = new System.Drawing.Size(160, 150);
            radPanelButtons.TabIndex = 2;
            // 
            // tableLayoutPanelButtons
            // 
            tableLayoutPanelButtons.ColumnCount = 3;
            tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            tableLayoutPanelButtons.Controls.Add(btnCancel, 1, 3);
            tableLayoutPanelButtons.Controls.Add(btnOk, 1, 1);
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
            tableLayoutPanelButtons.Size = new System.Drawing.Size(160, 150);
            tableLayoutPanelButtons.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            btnCancel.Location = new System.Drawing.Point(24, 42);
            btnCancel.Margin = new System.Windows.Forms.Padding(0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(112, 30);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "&Cancel";
            // 
            // btnOk
            // 
            btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            btnOk.Location = new System.Drawing.Point(24, 6);
            btnOk.Margin = new System.Windows.Forms.Padding(0);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(112, 30);
            btnOk.TabIndex = 0;
            btnOk.Text = "&Ok";
            // 
            // radPanelTop
            // 
            radPanelTop.Controls.Add(radGroupBoxTop);
            radPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelTop.Location = new System.Drawing.Point(0, 0);
            radPanelTop.Name = "radPanelTop";
            radPanelTop.Padding = new System.Windows.Forms.Padding(10);
            radPanelTop.Size = new System.Drawing.Size(961, 450);
            radPanelTop.TabIndex = 2;
            // 
            // radGroupBoxTop
            // 
            radGroupBoxTop.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            radGroupBoxTop.Controls.Add(radTreeView);
            radGroupBoxTop.Dock = System.Windows.Forms.DockStyle.Fill;
            radGroupBoxTop.HeaderMargin = new System.Windows.Forms.Padding(1);
            radGroupBoxTop.HeaderText = "Select Documents";
            radGroupBoxTop.Location = new System.Drawing.Point(10, 10);
            radGroupBoxTop.Name = "radGroupBoxTop";
            radGroupBoxTop.Padding = new System.Windows.Forms.Padding(2, 21, 2, 2);
            radGroupBoxTop.Size = new System.Drawing.Size(941, 430);
            radGroupBoxTop.TabIndex = 1;
            radGroupBoxTop.Text = "Select Documents";
            // 
            // radTreeView
            // 
            radTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            radTreeView.Location = new System.Drawing.Point(2, 21);
            radTreeView.Name = "radTreeView";
            radTreeView.Size = new System.Drawing.Size(937, 407);
            radTreeView.SpacingBetweenNodes = -1;
            radTreeView.TabIndex = 0;
            radTreeView.NodeExpandedChanged += RadTreeView_NodeExpandedChanged;
            // 
            // SelectDocumentsForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(9, 21);
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(961, 600);
            Controls.Add(radPanelTop);
            Controls.Add(radPanelBottom);
            Name = "SelectDocumentsForm";
            Text = "Select Documents";
            ((System.ComponentModel.ISupportInitialize)radPanelBottom).EndInit();
            radPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelMessages).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelButtons).EndInit();
            radPanelButtons.ResumeLayout(false);
            tableLayoutPanelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)btnCancel).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnOk).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelTop).EndInit();
            radPanelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radGroupBoxTop).EndInit();
            radGroupBoxTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radTreeView).EndInit();
            ((System.ComponentModel.ISupportInitialize)this).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelBottom;
        private Telerik.WinControls.UI.RadPanel radPanelMessages;
        private Telerik.WinControls.UI.RadPanel radPanelButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButtons;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadButton btnOk;
        private Telerik.WinControls.UI.RadPanel radPanelTop;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxTop;
        private Telerik.WinControls.UI.RadTreeView radTreeView;
    }
}
