namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    partial class ManagedListBoxControl
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
            radPanelUpDownButtons = new Telerik.WinControls.UI.RadPanel();
            btnDown = new Telerik.WinControls.UI.RadButton();
            btnUp = new Telerik.WinControls.UI.RadButton();
            radPanelEditButtons = new Telerik.WinControls.UI.RadPanel();
            radPanelTableParent = new Telerik.WinControls.UI.RadPanel();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            btnRemove = new Telerik.WinControls.UI.RadButton();
            btnEdit = new Telerik.WinControls.UI.RadButton();
            btnCopy = new Telerik.WinControls.UI.RadButton();
            btnCancel = new Telerik.WinControls.UI.RadButton();
            radPanelListBox = new Telerik.WinControls.UI.RadPanel();
            listBox = new Telerik.WinControls.UI.RadListControl();
            ((System.ComponentModel.ISupportInitialize)radPanelUpDownButtons).BeginInit();
            radPanelUpDownButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnUp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelEditButtons).BeginInit();
            radPanelEditButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelTableParent).BeginInit();
            radPanelTableParent.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnRemove).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnEdit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnCopy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnCancel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelListBox).BeginInit();
            radPanelListBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)listBox).BeginInit();
            SuspendLayout();
            // 
            // radPanelUpDownButtons
            // 
            radPanelUpDownButtons.Controls.Add(btnDown);
            radPanelUpDownButtons.Controls.Add(btnUp);
            radPanelUpDownButtons.Dock = System.Windows.Forms.DockStyle.Right;
            radPanelUpDownButtons.Location = new System.Drawing.Point(550, 0);
            radPanelUpDownButtons.Name = "radPanelUpDownButtons";
            radPanelUpDownButtons.Size = new System.Drawing.Size(33, 168);
            radPanelUpDownButtons.TabIndex = 3;
            // 
            // btnDown
            // 
            btnDown.Image = Properties.Resources.Down;
            btnDown.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            btnDown.Location = new System.Drawing.Point(6, 63);
            btnDown.Name = "btnDown";
            btnDown.Size = new System.Drawing.Size(24, 24);
            btnDown.TabIndex = 1;
            btnDown.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUp
            // 
            btnUp.Image = Properties.Resources.Up;
            btnUp.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            btnUp.Location = new System.Drawing.Point(6, 33);
            btnUp.Name = "btnUp";
            btnUp.Size = new System.Drawing.Size(24, 24);
            btnUp.TabIndex = 0;
            // 
            // radPanelEditButtons
            // 
            radPanelEditButtons.Controls.Add(radPanelTableParent);
            radPanelEditButtons.Dock = System.Windows.Forms.DockStyle.Right;
            radPanelEditButtons.Location = new System.Drawing.Point(583, 0);
            radPanelEditButtons.Name = "radPanelEditButtons";
            radPanelEditButtons.Size = new System.Drawing.Size(160, 168);
            radPanelEditButtons.TabIndex = 4;
            // 
            // radPanelTableParent
            // 
            radPanelTableParent.Controls.Add(tableLayoutPanel);
            radPanelTableParent.Dock = System.Windows.Forms.DockStyle.Top;
            radPanelTableParent.Location = new System.Drawing.Point(0, 0);
            radPanelTableParent.Margin = new System.Windows.Forms.Padding(0);
            radPanelTableParent.Name = "radPanelTableParent";
            radPanelTableParent.Size = new System.Drawing.Size(160, 150);
            radPanelTableParent.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            tableLayoutPanel.Controls.Add(btnRemove, 1, 7);
            tableLayoutPanel.Controls.Add(btnEdit, 1, 5);
            tableLayoutPanel.Controls.Add(btnCopy, 1, 3);
            tableLayoutPanel.Controls.Add(btnCancel, 1, 1);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 9;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            tableLayoutPanel.Size = new System.Drawing.Size(160, 150);
            tableLayoutPanel.TabIndex = 0;
            // 
            // btnRemove
            // 
            btnRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            btnRemove.Location = new System.Drawing.Point(24, 114);
            btnRemove.Margin = new System.Windows.Forms.Padding(0);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new System.Drawing.Size(112, 30);
            btnRemove.TabIndex = 4;
            btnRemove.Text = "Remove";
            // 
            // btnEdit
            // 
            btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            btnEdit.Location = new System.Drawing.Point(24, 78);
            btnEdit.Margin = new System.Windows.Forms.Padding(0);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(112, 30);
            btnEdit.TabIndex = 3;
            btnEdit.Text = "Edit";
            // 
            // btnCopy
            // 
            btnCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            btnCopy.Location = new System.Drawing.Point(24, 42);
            btnCopy.Margin = new System.Windows.Forms.Padding(0);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new System.Drawing.Size(112, 30);
            btnCopy.TabIndex = 2;
            btnCopy.Text = "Copy";
            // 
            // btnCancel
            // 
            btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            btnCancel.Location = new System.Drawing.Point(24, 6);
            btnCancel.Margin = new System.Windows.Forms.Padding(0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(112, 30);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            // 
            // radPanelListBox
            // 
            radPanelListBox.Controls.Add(listBox);
            radPanelListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelListBox.Location = new System.Drawing.Point(0, 0);
            radPanelListBox.Name = "radPanelListBox";
            radPanelListBox.Padding = new System.Windows.Forms.Padding(9, 9, 3, 9);
            radPanelListBox.Size = new System.Drawing.Size(550, 168);
            radPanelListBox.TabIndex = 5;
            // 
            // listBox
            // 
            listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            listBox.Location = new System.Drawing.Point(9, 9);
            listBox.Name = "listBox";
            listBox.Size = new System.Drawing.Size(538, 150);
            listBox.TabIndex = 0;
            // 
            // ManagedListBoxControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            Controls.Add(radPanelListBox);
            Controls.Add(radPanelUpDownButtons);
            Controls.Add(radPanelEditButtons);
            Margin = new System.Windows.Forms.Padding(0);
            Name = "ManagedListBoxControl";
            Size = new System.Drawing.Size(743, 168);
            ((System.ComponentModel.ISupportInitialize)radPanelUpDownButtons).EndInit();
            radPanelUpDownButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)btnDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnUp).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelEditButtons).EndInit();
            radPanelEditButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)btnRemove).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnEdit).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnCopy).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnCancel).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelListBox).EndInit();
            radPanelListBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)listBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelUpDownButtons;
        private Telerik.WinControls.UI.RadButton btnDown;
        private Telerik.WinControls.UI.RadButton btnUp;
        private Telerik.WinControls.UI.RadPanel radPanelEditButtons;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadButton btnCopy;
        private Telerik.WinControls.UI.RadButton btnEdit;
        private Telerik.WinControls.UI.RadButton btnRemove;
        private Telerik.WinControls.UI.RadPanel radPanelListBox;
        private Telerik.WinControls.UI.RadListControl listBox;
        private Telerik.WinControls.UI.RadPanel radPanelTableParent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}
