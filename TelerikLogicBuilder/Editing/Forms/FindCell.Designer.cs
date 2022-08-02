namespace ABIS.LogicBuilder.FlowBuilder.Editing.Forms
{
    partial class FindCell
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
            this.radPanelMessages = new Telerik.WinControls.UI.RadPanel();
            this.radPanelCommandButtons = new Telerik.WinControls.UI.RadPanel();
            this.radButtonCancel = new Telerik.WinControls.UI.RadButton();
            this.radButtonFind = new Telerik.WinControls.UI.RadButton();
            this.radPanelMain = new Telerik.WinControls.UI.RadPanel();
            this.radGroupBoxCell = new Telerik.WinControls.UI.RadGroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radLabelRowIndex = new Telerik.WinControls.UI.RadLabel();
            this.radLabelColumnIndex = new Telerik.WinControls.UI.RadLabel();
            this.radTextBoxRowIndex = new Telerik.WinControls.UI.RadTextBox();
            this.radTextBoxColumnIndex = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelBottom)).BeginInit();
            this.radPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelCommandButtons)).BeginInit();
            this.radPanelCommandButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonFind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMain)).BeginInit();
            this.radPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxCell)).BeginInit();
            this.radGroupBoxCell.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelRowIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelColumnIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxRowIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxColumnIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelBottom
            // 
            this.radPanelBottom.Controls.Add(this.radPanelMessages);
            this.radPanelBottom.Controls.Add(this.radPanelCommandButtons);
            this.radPanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radPanelBottom.Location = new System.Drawing.Point(0, 123);
            this.radPanelBottom.Name = "radPanelBottom";
            this.radPanelBottom.Size = new System.Drawing.Size(671, 121);
            this.radPanelBottom.TabIndex = 1;
            // 
            // radPanelMessages
            // 
            this.radPanelMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelMessages.Location = new System.Drawing.Point(0, 0);
            this.radPanelMessages.Name = "radPanelMessages";
            this.radPanelMessages.Size = new System.Drawing.Size(510, 121);
            this.radPanelMessages.TabIndex = 2;
            // 
            // radPanelCommandButtons
            // 
            this.radPanelCommandButtons.Controls.Add(this.radButtonCancel);
            this.radPanelCommandButtons.Controls.Add(this.radButtonFind);
            this.radPanelCommandButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelCommandButtons.Location = new System.Drawing.Point(510, 0);
            this.radPanelCommandButtons.Name = "radPanelCommandButtons";
            this.radPanelCommandButtons.Size = new System.Drawing.Size(161, 121);
            this.radPanelCommandButtons.TabIndex = 1;
            // 
            // radButtonCancel
            // 
            this.radButtonCancel.Location = new System.Drawing.Point(25, 61);
            this.radButtonCancel.Name = "radButtonCancel";
            this.radButtonCancel.Size = new System.Drawing.Size(110, 24);
            this.radButtonCancel.TabIndex = 1;
            this.radButtonCancel.Text = "&Cancel";
            this.radButtonCancel.Click += new System.EventHandler(this.RadButtonCancel_Click);
            // 
            // radButtonFind
            // 
            this.radButtonFind.Location = new System.Drawing.Point(25, 22);
            this.radButtonFind.Name = "radButtonFind";
            this.radButtonFind.Size = new System.Drawing.Size(110, 24);
            this.radButtonFind.TabIndex = 0;
            this.radButtonFind.Text = "&Find";
            this.radButtonFind.Click += new System.EventHandler(this.RadButtonFind_Click);
            // 
            // radPanelMain
            // 
            this.radPanelMain.Controls.Add(this.radGroupBoxCell);
            this.radPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelMain.Location = new System.Drawing.Point(0, 0);
            this.radPanelMain.Name = "radPanelMain";
            this.radPanelMain.Size = new System.Drawing.Size(671, 123);
            this.radPanelMain.TabIndex = 2;
            // 
            // radGroupBoxCell
            // 
            this.radGroupBoxCell.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxCell.Controls.Add(this.tableLayoutPanel1);
            this.radGroupBoxCell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBoxCell.HeaderText = "Cell";
            this.radGroupBoxCell.Location = new System.Drawing.Point(0, 0);
            this.radGroupBoxCell.Name = "radGroupBoxCell";
            this.radGroupBoxCell.Padding = new System.Windows.Forms.Padding(20, 30, 20, 2);
            this.radGroupBoxCell.Size = new System.Drawing.Size(671, 123);
            this.radGroupBoxCell.TabIndex = 2;
            this.radGroupBoxCell.Text = "Cell";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.radLabelRowIndex, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.radLabelColumnIndex, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.radTextBoxRowIndex, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.radTextBoxColumnIndex, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(20, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(631, 69);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radLabelRowIndex
            // 
            this.radLabelRowIndex.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radLabelRowIndex.Location = new System.Drawing.Point(3, 13);
            this.radLabelRowIndex.Name = "radLabelRowIndex";
            this.radLabelRowIndex.Size = new System.Drawing.Size(144, 18);
            this.radLabelRowIndex.TabIndex = 0;
            this.radLabelRowIndex.Text = "Row Index";
            // 
            // radLabelColumnIndex
            // 
            this.radLabelColumnIndex.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radLabelColumnIndex.Location = new System.Drawing.Point(3, 48);
            this.radLabelColumnIndex.Name = "radLabelColumnIndex";
            this.radLabelColumnIndex.Size = new System.Drawing.Size(144, 18);
            this.radLabelColumnIndex.TabIndex = 1;
            this.radLabelColumnIndex.Text = "Column Index";
            // 
            // radTextBoxRowIndex
            // 
            this.radTextBoxRowIndex.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radTextBoxRowIndex.Location = new System.Drawing.Point(153, 11);
            this.radTextBoxRowIndex.Name = "radTextBoxRowIndex";
            this.radTextBoxRowIndex.Size = new System.Drawing.Size(475, 20);
            this.radTextBoxRowIndex.TabIndex = 2;
            this.radTextBoxRowIndex.TextChanged += new System.EventHandler(this.RadTextBoxRowIndex_TextChanged);
            // 
            // radTextBoxColumnIndex
            // 
            this.radTextBoxColumnIndex.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radTextBoxColumnIndex.Location = new System.Drawing.Point(153, 46);
            this.radTextBoxColumnIndex.Name = "radTextBoxColumnIndex";
            this.radTextBoxColumnIndex.Size = new System.Drawing.Size(475, 20);
            this.radTextBoxColumnIndex.TabIndex = 3;
            this.radTextBoxColumnIndex.TextChanged += new System.EventHandler(this.RadTextBoxColumnIndex_TextChanged);
            // 
            // FindCell
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(9, 21);
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 244);
            this.Controls.Add(this.radPanelMain);
            this.Controls.Add(this.radPanelBottom);
            this.Name = "FindCell";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Find Cell";
            ((System.ComponentModel.ISupportInitialize)(this.radPanelBottom)).EndInit();
            this.radPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelCommandButtons)).EndInit();
            this.radPanelCommandButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonFind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMain)).EndInit();
            this.radPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxCell)).EndInit();
            this.radGroupBoxCell.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelRowIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelColumnIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxRowIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxColumnIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelBottom;
        private Telerik.WinControls.UI.RadPanel radPanelMessages;
        private Telerik.WinControls.UI.RadPanel radPanelCommandButtons;
        private Telerik.WinControls.UI.RadButton radButtonCancel;
        private Telerik.WinControls.UI.RadButton radButtonFind;
        private Telerik.WinControls.UI.RadPanel radPanelMain;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxCell;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel radLabelRowIndex;
        private Telerik.WinControls.UI.RadLabel radLabelColumnIndex;
        private Telerik.WinControls.UI.RadTextBox radTextBoxRowIndex;
        private Telerik.WinControls.UI.RadTextBox radTextBoxColumnIndex;
    }
}
