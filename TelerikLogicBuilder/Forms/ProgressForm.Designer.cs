namespace ABIS.LogicBuilder.FlowBuilder.Forms
{
    partial class ProgressForm
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
            this.radButtonCancel = new Telerik.WinControls.UI.RadButton();
            this.radPanelTop = new Telerik.WinControls.UI.RadPanel();
            this.radPanelProgressBar = new Telerik.WinControls.UI.RadPanel();
            this.radProgressBar = new Telerik.WinControls.UI.RadProgressBar();
            this.radPanelLabel = new Telerik.WinControls.UI.RadPanel();
            this.radLabelMessage = new Telerik.WinControls.UI.RadLabel();
            this.tableLayoutPanelBottom = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelTop = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelBottom)).BeginInit();
            this.radPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTop)).BeginInit();
            this.radPanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelProgressBar)).BeginInit();
            this.radPanelProgressBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelLabel)).BeginInit();
            this.radPanelLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelMessage)).BeginInit();
            this.tableLayoutPanelBottom.SuspendLayout();
            this.tableLayoutPanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelBottom
            // 
            this.radPanelBottom.Controls.Add(this.tableLayoutPanelBottom);
            this.radPanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radPanelBottom.Location = new System.Drawing.Point(0, 188);
            this.radPanelBottom.Name = "radPanelBottom";
            this.radPanelBottom.Size = new System.Drawing.Size(808, 100);
            this.radPanelBottom.TabIndex = 0;
            // 
            // radButtonCancel
            // 
            this.radButtonCancel.Dock = System.Windows.Forms.DockStyle.Top;
            this.radButtonCancel.Location = new System.Drawing.Point(342, 28);
            this.radButtonCancel.Name = "radButtonCancel";
            this.radButtonCancel.Size = new System.Drawing.Size(123, 24);
            this.radButtonCancel.TabIndex = 0;
            this.radButtonCancel.Text = "&Cancel";
            this.radButtonCancel.Click += new System.EventHandler(this.RadButtonCancel_Click);
            // 
            // radPanelTop
            // 
            this.radPanelTop.Controls.Add(this.tableLayoutPanelTop);
            this.radPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelTop.Location = new System.Drawing.Point(0, 0);
            this.radPanelTop.Name = "radPanelTop";
            this.radPanelTop.Size = new System.Drawing.Size(808, 188);
            this.radPanelTop.TabIndex = 1;
            // 
            // radPanelProgressBar
            // 
            this.radPanelProgressBar.Controls.Add(this.radProgressBar);
            this.radPanelProgressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanelProgressBar.Location = new System.Drawing.Point(27, 114);
            this.radPanelProgressBar.Name = "radPanelProgressBar";
            this.radPanelProgressBar.Size = new System.Drawing.Size(753, 28);
            this.radPanelProgressBar.TabIndex = 3;
            // 
            // radProgressBar
            // 
            this.radProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radProgressBar.Location = new System.Drawing.Point(0, 0);
            this.radProgressBar.Name = "radProgressBar";
            this.radProgressBar.Size = new System.Drawing.Size(753, 28);
            this.radProgressBar.TabIndex = 0;
            // 
            // radPanelLabel
            // 
            this.radPanelLabel.Controls.Add(this.radLabelMessage);
            this.radPanelLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanelLabel.Location = new System.Drawing.Point(27, 40);
            this.radPanelLabel.Name = "radPanelLabel";
            this.radPanelLabel.Size = new System.Drawing.Size(753, 28);
            this.radPanelLabel.TabIndex = 2;
            // 
            // radLabelMessage
            // 
            this.radLabelMessage.Location = new System.Drawing.Point(0, 0);
            this.radLabelMessage.Name = "radLabelMessage";
            this.radLabelMessage.Size = new System.Drawing.Size(2, 2);
            this.radLabelMessage.TabIndex = 0;
            // 
            // tableLayoutPanelBottom
            // 
            this.tableLayoutPanelBottom.ColumnCount = 3;
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.tableLayoutPanelBottom.Controls.Add(this.radButtonCancel, 1, 1);
            this.tableLayoutPanelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBottom.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBottom.Name = "tableLayoutPanelBottom";
            this.tableLayoutPanelBottom.RowCount = 2;
            this.tableLayoutPanelBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanelBottom.Size = new System.Drawing.Size(808, 100);
            this.tableLayoutPanelBottom.TabIndex = 0;
            // 
            // tableLayoutPanelTop
            // 
            this.tableLayoutPanelTop.ColumnCount = 3;
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 94F));
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tableLayoutPanelTop.Controls.Add(this.radPanelProgressBar, 1, 3);
            this.tableLayoutPanelTop.Controls.Add(this.radPanelLabel, 1, 1);
            this.tableLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTop.Name = "tableLayoutPanelTop";
            this.tableLayoutPanelTop.RowCount = 5;
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelTop.Size = new System.Drawing.Size(808, 188);
            this.tableLayoutPanelTop.TabIndex = 0;
            // 
            // ProgressForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(9, 21);
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 288);
            this.Controls.Add(this.radPanelTop);
            this.Controls.Add(this.radPanelBottom);
            this.Name = "ProgressForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Progress Form";
            ((System.ComponentModel.ISupportInitialize)(this.radPanelBottom)).EndInit();
            this.radPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelTop)).EndInit();
            this.radPanelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelProgressBar)).EndInit();
            this.radPanelProgressBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelLabel)).EndInit();
            this.radPanelLabel.ResumeLayout(false);
            this.radPanelLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelMessage)).EndInit();
            this.tableLayoutPanelBottom.ResumeLayout(false);
            this.tableLayoutPanelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelBottom;
        private Telerik.WinControls.UI.RadPanel radPanelTop;
        private Telerik.WinControls.UI.RadButton radButtonCancel;
        private Telerik.WinControls.UI.RadPanel radPanelProgressBar;
        private Telerik.WinControls.UI.RadProgressBar radProgressBar;
        private Telerik.WinControls.UI.RadPanel radPanelLabel;
        private Telerik.WinControls.UI.RadLabel radLabelMessage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBottom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTop;
    }
}
