namespace ABIS.LogicBuilder.FlowBuilder.Editing.Forms
{
    partial class FindShape
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
            this.radGroupBoxShape = new Telerik.WinControls.UI.RadGroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radLabelPageIndex = new Telerik.WinControls.UI.RadLabel();
            this.radLabelShapeIndex = new Telerik.WinControls.UI.RadLabel();
            this.radTextBoxPageIndex = new Telerik.WinControls.UI.RadTextBox();
            this.radTextBoxShapeIndex = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelBottom)).BeginInit();
            this.radPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelCommandButtons)).BeginInit();
            this.radPanelCommandButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonFind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMain)).BeginInit();
            this.radPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxShape)).BeginInit();
            this.radGroupBoxShape.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelPageIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelShapeIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxPageIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxShapeIndex)).BeginInit();
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
            this.radPanelBottom.TabIndex = 0;
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
            this.radPanelMain.Controls.Add(this.radGroupBoxShape);
            this.radPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelMain.Location = new System.Drawing.Point(0, 0);
            this.radPanelMain.Name = "radPanelMain";
            this.radPanelMain.Size = new System.Drawing.Size(671, 123);
            this.radPanelMain.TabIndex = 1;
            // 
            // radGroupBoxShape
            // 
            this.radGroupBoxShape.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxShape.Controls.Add(this.tableLayoutPanel1);
            this.radGroupBoxShape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBoxShape.HeaderText = "Shape";
            this.radGroupBoxShape.Location = new System.Drawing.Point(0, 0);
            this.radGroupBoxShape.Name = "radGroupBoxShape";
            this.radGroupBoxShape.Padding = new System.Windows.Forms.Padding(20, 30, 20, 2);
            this.radGroupBoxShape.Size = new System.Drawing.Size(671, 123);
            this.radGroupBoxShape.TabIndex = 2;
            this.radGroupBoxShape.Text = "Shape";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.radLabelPageIndex, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.radLabelShapeIndex, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.radTextBoxPageIndex, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.radTextBoxShapeIndex, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(20, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(631, 69);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radLabelPageIndex
            // 
            this.radLabelPageIndex.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radLabelPageIndex.Location = new System.Drawing.Point(3, 13);
            this.radLabelPageIndex.Name = "radLabelPageIndex";
            this.radLabelPageIndex.Size = new System.Drawing.Size(144, 18);
            this.radLabelPageIndex.TabIndex = 0;
            this.radLabelPageIndex.Text = "PageI Index";
            // 
            // radLabelShapeIndex
            // 
            this.radLabelShapeIndex.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radLabelShapeIndex.Location = new System.Drawing.Point(3, 48);
            this.radLabelShapeIndex.Name = "radLabelShapeIndex";
            this.radLabelShapeIndex.Size = new System.Drawing.Size(144, 18);
            this.radLabelShapeIndex.TabIndex = 1;
            this.radLabelShapeIndex.Text = "Shape Index";
            // 
            // radTextBoxPageIndex
            // 
            this.radTextBoxPageIndex.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radTextBoxPageIndex.Location = new System.Drawing.Point(153, 11);
            this.radTextBoxPageIndex.Name = "radTextBoxPageIndex";
            this.radTextBoxPageIndex.Size = new System.Drawing.Size(475, 20);
            this.radTextBoxPageIndex.TabIndex = 2;
            this.radTextBoxPageIndex.TextChanged += new System.EventHandler(this.RadTextBoxPageIndex_TextChanged);
            // 
            // radTextBoxShapeIndex
            // 
            this.radTextBoxShapeIndex.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radTextBoxShapeIndex.Location = new System.Drawing.Point(153, 46);
            this.radTextBoxShapeIndex.Name = "radTextBoxShapeIndex";
            this.radTextBoxShapeIndex.Size = new System.Drawing.Size(475, 20);
            this.radTextBoxShapeIndex.TabIndex = 3;
            this.radTextBoxShapeIndex.TextChanged += new System.EventHandler(this.RadTextBoxShapeIndex_TextChanged);
            // 
            // FindShape
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(9, 21);
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 244);
            this.Controls.Add(this.radPanelMain);
            this.Controls.Add(this.radPanelBottom);
            this.Name = "FindShape";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Find Shape";
            ((System.ComponentModel.ISupportInitialize)(this.radPanelBottom)).EndInit();
            this.radPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelCommandButtons)).EndInit();
            this.radPanelCommandButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonFind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMain)).EndInit();
            this.radPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxShape)).EndInit();
            this.radGroupBoxShape.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelPageIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelShapeIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxPageIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxShapeIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelBottom;
        private Telerik.WinControls.UI.RadPanel radPanelMain;
        private Telerik.WinControls.UI.RadPanel radPanelMessages;
        private Telerik.WinControls.UI.RadPanel radPanelCommandButtons;
        private Telerik.WinControls.UI.RadButton radButtonCancel;
        private Telerik.WinControls.UI.RadButton radButtonFind;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxShape;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel radLabelPageIndex;
        private Telerik.WinControls.UI.RadLabel radLabelShapeIndex;
        private Telerik.WinControls.UI.RadTextBox radTextBoxPageIndex;
        private Telerik.WinControls.UI.RadTextBox radTextBoxShapeIndex;
    }
}
