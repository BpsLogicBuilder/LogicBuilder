namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
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
            this.radSplitContainerMain = new Telerik.WinControls.UI.RadSplitContainer();
            this.splitPanelTop = new Telerik.WinControls.UI.SplitPanel();
            this.radGroupBoxTop = new Telerik.WinControls.UI.RadGroupBox();
            this.radTreeView = new Telerik.WinControls.UI.RadTreeView();
            this.splitPanelBottom = new Telerik.WinControls.UI.SplitPanel();
            this.radPanelMessages = new Telerik.WinControls.UI.RadPanel();
            this.radPanelCommandButtons = new Telerik.WinControls.UI.RadPanel();
            this.radButtonCancel = new Telerik.WinControls.UI.RadButton();
            this.radButtonOk = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radSplitContainerMain)).BeginInit();
            this.radSplitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelTop)).BeginInit();
            this.splitPanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxTop)).BeginInit();
            this.radGroupBoxTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelBottom)).BeginInit();
            this.splitPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelCommandButtons)).BeginInit();
            this.radPanelCommandButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radSplitContainerMain
            // 
            this.radSplitContainerMain.Controls.Add(this.splitPanelTop);
            this.radSplitContainerMain.Controls.Add(this.splitPanelBottom);
            this.radSplitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radSplitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.radSplitContainerMain.Name = "radSplitContainerMain";
            this.radSplitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // 
            // 
            this.radSplitContainerMain.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.radSplitContainerMain.Size = new System.Drawing.Size(943, 648);
            this.radSplitContainerMain.SplitterWidth = 8;
            this.radSplitContainerMain.TabIndex = 1;
            this.radSplitContainerMain.TabStop = false;
            // 
            // splitPanelTop
            // 
            this.splitPanelTop.Controls.Add(this.radGroupBoxTop);
            this.splitPanelTop.Location = new System.Drawing.Point(0, 0);
            this.splitPanelTop.Name = "splitPanelTop";
            this.splitPanelTop.Padding = new System.Windows.Forms.Padding(10);
            // 
            // 
            // 
            this.splitPanelTop.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.splitPanelTop.Size = new System.Drawing.Size(943, 470);
            this.splitPanelTop.SizeInfo.AutoSizeScale = new System.Drawing.SizeF(0F, 0.2349206F);
            this.splitPanelTop.SizeInfo.SplitterCorrection = new System.Drawing.Size(0, 148);
            this.splitPanelTop.TabIndex = 0;
            this.splitPanelTop.TabStop = false;
            this.splitPanelTop.Text = "splitPanel1";
            // 
            // radGroupBoxTop
            // 
            this.radGroupBoxTop.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBoxTop.Controls.Add(this.radTreeView);
            this.radGroupBoxTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBoxTop.HeaderMargin = new System.Windows.Forms.Padding(1);
            this.radGroupBoxTop.HeaderText = "Select Documents";
            this.radGroupBoxTop.Location = new System.Drawing.Point(10, 10);
            this.radGroupBoxTop.Name = "radGroupBoxTop";
            this.radGroupBoxTop.Padding = new System.Windows.Forms.Padding(2, 21, 2, 2);
            this.radGroupBoxTop.Size = new System.Drawing.Size(923, 450);
            this.radGroupBoxTop.TabIndex = 0;
            this.radGroupBoxTop.Text = "Select Documents";
            // 
            // radTreeView
            // 
            this.radTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTreeView.Location = new System.Drawing.Point(2, 21);
            this.radTreeView.Name = "radTreeView";
            this.radTreeView.Size = new System.Drawing.Size(919, 427);
            this.radTreeView.SpacingBetweenNodes = -1;
            this.radTreeView.TabIndex = 0;
            this.radTreeView.NodeExpandedChanged += new Telerik.WinControls.UI.RadTreeView.TreeViewEventHandler(this.RadTreeView_NodeExpandedChanged);
            // 
            // splitPanelBottom
            // 
            this.splitPanelBottom.Controls.Add(this.radPanelMessages);
            this.splitPanelBottom.Controls.Add(this.radPanelCommandButtons);
            this.splitPanelBottom.Location = new System.Drawing.Point(0, 478);
            this.splitPanelBottom.Name = "splitPanelBottom";
            // 
            // 
            // 
            this.splitPanelBottom.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.splitPanelBottom.Size = new System.Drawing.Size(943, 170);
            this.splitPanelBottom.SizeInfo.AutoSizeScale = new System.Drawing.SizeF(0F, -0.2349206F);
            this.splitPanelBottom.SizeInfo.SplitterCorrection = new System.Drawing.Size(0, -148);
            this.splitPanelBottom.TabIndex = 1;
            this.splitPanelBottom.TabStop = false;
            this.splitPanelBottom.Text = "splitPanel2";
            // 
            // radPanelMessages
            // 
            this.radPanelMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelMessages.Location = new System.Drawing.Point(0, 0);
            this.radPanelMessages.Name = "radPanelMessages";
            this.radPanelMessages.Size = new System.Drawing.Size(782, 170);
            this.radPanelMessages.TabIndex = 1;
            // 
            // radPanelCommandButtons
            // 
            this.radPanelCommandButtons.Controls.Add(this.radButtonCancel);
            this.radPanelCommandButtons.Controls.Add(this.radButtonOk);
            this.radPanelCommandButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelCommandButtons.Location = new System.Drawing.Point(782, 0);
            this.radPanelCommandButtons.Name = "radPanelCommandButtons";
            this.radPanelCommandButtons.Size = new System.Drawing.Size(161, 170);
            this.radPanelCommandButtons.TabIndex = 0;
            // 
            // radButtonCancel
            // 
            this.radButtonCancel.Location = new System.Drawing.Point(25, 61);
            this.radButtonCancel.Name = "radButtonCancel";
            this.radButtonCancel.Size = new System.Drawing.Size(110, 24);
            this.radButtonCancel.TabIndex = 1;
            this.radButtonCancel.Text = "&Cancel";
            // 
            // radButtonOk
            // 
            this.radButtonOk.Location = new System.Drawing.Point(25, 22);
            this.radButtonOk.Name = "radButtonOk";
            this.radButtonOk.Size = new System.Drawing.Size(110, 24);
            this.radButtonOk.TabIndex = 0;
            this.radButtonOk.Text = "&Ok";
            // 
            // SelectDocumentsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(9, 21);
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 648);
            this.Controls.Add(this.radSplitContainerMain);
            this.Name = "SelectDocumentsForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Select Docunents";
            ((System.ComponentModel.ISupportInitialize)(this.radSplitContainerMain)).EndInit();
            this.radSplitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelTop)).EndInit();
            this.splitPanelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBoxTop)).EndInit();
            this.radGroupBoxTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelBottom)).EndInit();
            this.splitPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelCommandButtons)).EndInit();
            this.radPanelCommandButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadSplitContainer radSplitContainerMain;
        private Telerik.WinControls.UI.SplitPanel splitPanelTop;
        private Telerik.WinControls.UI.RadGroupBox radGroupBoxTop;
        private Telerik.WinControls.UI.RadTreeView radTreeView;
        private Telerik.WinControls.UI.SplitPanel splitPanelBottom;
        private Telerik.WinControls.UI.RadPanel radPanelMessages;
        private Telerik.WinControls.UI.RadPanel radPanelCommandButtons;
        private Telerik.WinControls.UI.RadButton radButtonCancel;
        private Telerik.WinControls.UI.RadButton radButtonOk;
    }
}
