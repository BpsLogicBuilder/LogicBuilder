namespace ABIS.LogicBuilder.FlowBuilder.Forms
{
    partial class NewProjectForm
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
            radGroupBoxNewProject = new Telerik.WinControls.UI.RadGroupBox();
            radPanelNewProject = new Telerik.WinControls.UI.RadScrollablePanel();
            radPanelTableParent = new Telerik.WinControls.UI.RadPanel();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            txtProjectName = new Telerik.WinControls.UI.RadTextBox();
            lblFolder = new Telerik.WinControls.UI.RadLabel();
            lblProjectName = new Telerik.WinControls.UI.RadLabel();
            txtProjectPath = new UserControls.HelperButtonTextBox();
            ((System.ComponentModel.ISupportInitialize)radPanelBottom).BeginInit();
            radPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelMessages).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelButtons).BeginInit();
            radPanelButtons.SuspendLayout();
            tableLayoutPanelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnCancel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnOk).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radGroupBoxNewProject).BeginInit();
            radGroupBoxNewProject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelNewProject).BeginInit();
            radPanelNewProject.PanelContainer.SuspendLayout();
            radPanelNewProject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelTableParent).BeginInit();
            radPanelTableParent.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtProjectName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lblFolder).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lblProjectName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();
            SuspendLayout();
            // 
            // radPanelBottom
            // 
            radPanelBottom.Controls.Add(radPanelMessages);
            radPanelBottom.Controls.Add(radPanelButtons);
            radPanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            radPanelBottom.Location = new System.Drawing.Point(0, 216);
            radPanelBottom.Name = "radPanelBottom";
            radPanelBottom.Size = new System.Drawing.Size(857, 150);
            radPanelBottom.TabIndex = 5;
            // 
            // radPanelMessages
            // 
            radPanelMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelMessages.Location = new System.Drawing.Point(0, 0);
            radPanelMessages.Name = "radPanelMessages";
            radPanelMessages.Size = new System.Drawing.Size(697, 150);
            radPanelMessages.TabIndex = 2;
            // 
            // radPanelButtons
            // 
            radPanelButtons.Controls.Add(tableLayoutPanelButtons);
            radPanelButtons.Dock = System.Windows.Forms.DockStyle.Right;
            radPanelButtons.Location = new System.Drawing.Point(697, 0);
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
            // radGroupBoxNewProject
            // 
            radGroupBoxNewProject.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            radGroupBoxNewProject.Controls.Add(radPanelNewProject);
            radGroupBoxNewProject.Dock = System.Windows.Forms.DockStyle.Fill;
            radGroupBoxNewProject.HeaderText = "New Project";
            radGroupBoxNewProject.Location = new System.Drawing.Point(0, 0);
            radGroupBoxNewProject.Name = "radGroupBoxNewProject";
            radGroupBoxNewProject.Size = new System.Drawing.Size(857, 216);
            radGroupBoxNewProject.TabIndex = 7;
            radGroupBoxNewProject.Text = "New Project";
            // 
            // radPanelNewProject
            // 
            radPanelNewProject.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelNewProject.Location = new System.Drawing.Point(2, 18);
            radPanelNewProject.Name = "radPanelNewProject";
            // 
            // radPanelNewProject.PanelContainer
            // 
            radPanelNewProject.PanelContainer.Controls.Add(radPanelTableParent);
            radPanelNewProject.PanelContainer.Size = new System.Drawing.Size(851, 194);
            radPanelNewProject.Size = new System.Drawing.Size(853, 196);
            radPanelNewProject.TabIndex = 0;
            // 
            // radPanelTableParent
            // 
            radPanelTableParent.Controls.Add(tableLayoutPanel);
            radPanelTableParent.Dock = System.Windows.Forms.DockStyle.Top;
            radPanelTableParent.Location = new System.Drawing.Point(0, 0);
            radPanelTableParent.Margin = new System.Windows.Forms.Padding(0);
            radPanelTableParent.Name = "radPanelTableParent";
            radPanelTableParent.Size = new System.Drawing.Size(851, 112);
            radPanelTableParent.TabIndex = 1;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 4;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            tableLayoutPanel.Controls.Add(txtProjectName, 2, 1);
            tableLayoutPanel.Controls.Add(lblFolder, 1, 3);
            tableLayoutPanel.Controls.Add(lblProjectName, 1, 1);
            tableLayoutPanel.Controls.Add(txtProjectPath, 2, 3);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 6;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.Size = new System.Drawing.Size(851, 112);
            tableLayoutPanel.TabIndex = 0;
            // 
            // txtProjectName
            // 
            txtProjectName.AutoSize = false;
            txtProjectName.Dock = System.Windows.Forms.DockStyle.Fill;
            txtProjectName.Location = new System.Drawing.Point(288, 20);
            txtProjectName.Margin = new System.Windows.Forms.Padding(0);
            txtProjectName.Name = "txtProjectName";
            txtProjectName.Size = new System.Drawing.Size(536, 30);
            txtProjectName.TabIndex = 0;
            // 
            // lblFolder
            // 
            lblFolder.Dock = System.Windows.Forms.DockStyle.Top;
            lblFolder.Location = new System.Drawing.Point(28, 59);
            lblFolder.Name = "lblFolder";
            lblFolder.Size = new System.Drawing.Size(51, 18);
            lblFolder.TabIndex = 4;
            lblFolder.Text = "Location:";
            // 
            // lblProjectName
            // 
            lblProjectName.Dock = System.Windows.Forms.DockStyle.Top;
            lblProjectName.Location = new System.Drawing.Point(28, 23);
            lblProjectName.Name = "lblProjectName";
            lblProjectName.Size = new System.Drawing.Size(39, 18);
            lblProjectName.TabIndex = 0;
            lblProjectName.Text = "Name:";
            // 
            // txtProjectPath
            // 
            txtProjectPath.Dock = System.Windows.Forms.DockStyle.Fill;
            txtProjectPath.Location = new System.Drawing.Point(288, 56);
            txtProjectPath.Margin = new System.Windows.Forms.Padding(0);
            txtProjectPath.Name = "txtProjectPath";
            txtProjectPath.ReadOnly = false;
            txtProjectPath.Size = new System.Drawing.Size(536, 30);
            txtProjectPath.TabIndex = 5;
            // 
            // NewProjectForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(9, 21);
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(857, 366);
            Controls.Add(radGroupBoxNewProject);
            Controls.Add(radPanelBottom);
            Name = "NewProjectForm";
            Text = "New Project";
            ((System.ComponentModel.ISupportInitialize)radPanelBottom).EndInit();
            radPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelMessages).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelButtons).EndInit();
            radPanelButtons.ResumeLayout(false);
            tableLayoutPanelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)btnCancel).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnOk).EndInit();
            ((System.ComponentModel.ISupportInitialize)radGroupBoxNewProject).EndInit();
            radGroupBoxNewProject.ResumeLayout(false);
            radPanelNewProject.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelNewProject).EndInit();
            radPanelNewProject.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtProjectName).EndInit();
            ((System.ComponentModel.ISupportInitialize)lblFolder).EndInit();
            ((System.ComponentModel.ISupportInitialize)lblProjectName).EndInit();
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
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxNewProject;
        private Telerik.WinControls.UI.RadScrollablePanel radPanelNewProject;
        private Telerik.WinControls.UI.RadPanel radPanelTableParent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private Telerik.WinControls.UI.RadTextBox txtProjectName;
        private Telerik.WinControls.UI.RadLabel lblFolder;
        private Telerik.WinControls.UI.RadLabel lblProjectName;
        private UserControls.HelperButtonTextBox txtProjectPath;
    }
}
