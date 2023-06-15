namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment
{
    partial class SelectFragmentForm
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
            radPanelApplication = new Telerik.WinControls.UI.RadPanel();
            radGroupBoxApplication = new Telerik.WinControls.UI.RadGroupBox();
            radPanelFragment = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)radPanelBottom).BeginInit();
            radPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelMessages).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelButtons).BeginInit();
            radPanelButtons.SuspendLayout();
            tableLayoutPanelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnCancel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnOk).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelApplication).BeginInit();
            radPanelApplication.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radGroupBoxApplication).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelFragment).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();
            SuspendLayout();
            // 
            // radPanelBottom
            // 
            radPanelBottom.Controls.Add(radPanelMessages);
            radPanelBottom.Controls.Add(radPanelButtons);
            radPanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            radPanelBottom.Location = new System.Drawing.Point(0, 356);
            radPanelBottom.Name = "radPanelBottom";
            radPanelBottom.Size = new System.Drawing.Size(841, 150);
            radPanelBottom.TabIndex = 4;
            // 
            // radPanelMessages
            // 
            radPanelMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelMessages.Location = new System.Drawing.Point(0, 0);
            radPanelMessages.Name = "radPanelMessages";
            radPanelMessages.Size = new System.Drawing.Size(681, 150);
            radPanelMessages.TabIndex = 0;
            // 
            // radPanelButtons
            // 
            radPanelButtons.Controls.Add(tableLayoutPanelButtons);
            radPanelButtons.Dock = System.Windows.Forms.DockStyle.Right;
            radPanelButtons.Location = new System.Drawing.Point(681, 0);
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
            btnCancel.TabIndex = 2;
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
            // radPanelApplication
            // 
            radPanelApplication.Controls.Add(radGroupBoxApplication);
            radPanelApplication.Dock = System.Windows.Forms.DockStyle.Top;
            radPanelApplication.Location = new System.Drawing.Point(0, 0);
            radPanelApplication.Name = "radPanelApplication";
            radPanelApplication.Size = new System.Drawing.Size(841, 60);
            radPanelApplication.TabIndex = 5;
            // 
            // radGroupBoxApplication
            // 
            radGroupBoxApplication.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            radGroupBoxApplication.Dock = System.Windows.Forms.DockStyle.Fill;
            radGroupBoxApplication.HeaderMargin = new System.Windows.Forms.Padding(1);
            radGroupBoxApplication.HeaderText = "Application";
            radGroupBoxApplication.Location = new System.Drawing.Point(0, 0);
            radGroupBoxApplication.Name = "radGroupBoxApplication";
            radGroupBoxApplication.Padding = new System.Windows.Forms.Padding(18, 24, 18, 2);
            radGroupBoxApplication.Size = new System.Drawing.Size(841, 60);
            radGroupBoxApplication.TabIndex = 0;
            radGroupBoxApplication.Text = "Application";
            // 
            // radPanelFragment
            // 
            radPanelFragment.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelFragment.Location = new System.Drawing.Point(0, 60);
            radPanelFragment.Name = "radPanelFragment";
            radPanelFragment.Size = new System.Drawing.Size(841, 296);
            radPanelFragment.TabIndex = 6;
            // 
            // SelectFragmentForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(9, 21);
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(841, 506);
            Controls.Add(radPanelFragment);
            Controls.Add(radPanelApplication);
            Controls.Add(radPanelBottom);
            Name = "SelectFragmentForm";
            Text = "Select Fragment";
            ((System.ComponentModel.ISupportInitialize)radPanelBottom).EndInit();
            radPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelMessages).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelButtons).EndInit();
            radPanelButtons.ResumeLayout(false);
            tableLayoutPanelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)btnCancel).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnOk).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelApplication).EndInit();
            radPanelApplication.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radGroupBoxApplication).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelFragment).EndInit();
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
        private Telerik.WinControls.UI.RadPanel radPanelApplication;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxApplication;
        private Telerik.WinControls.UI.RadPanel radPanelFragment;
    }
}
