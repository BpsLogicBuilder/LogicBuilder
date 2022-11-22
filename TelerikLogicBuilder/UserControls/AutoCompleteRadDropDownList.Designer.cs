namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    partial class AutoCompleteRadDropDownList
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
            this.radPanelButton = new Telerik.WinControls.UI.RadPanel();
            this.radButtonHelper = new Telerik.WinControls.UI.RadButton();
            this.radPanelDropDownList = new Telerik.WinControls.UI.RadPanel();
            this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelButton)).BeginInit();
            this.radPanelButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonHelper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDropDownList)).BeginInit();
            this.radPanelDropDownList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelButton
            // 
            this.radPanelButton.Controls.Add(this.radButtonHelper);
            this.radPanelButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelButton.Location = new System.Drawing.Point(326, 0);
            this.radPanelButton.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelButton.Name = "radPanelButton";
            this.radPanelButton.Padding = new System.Windows.Forms.Padding(1, 0, 1, 2);
            this.radPanelButton.Size = new System.Drawing.Size(24, 24);
            this.radPanelButton.TabIndex = 0;
            // 
            // radButtonHelper
            // 
            this.radButtonHelper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radButtonHelper.Image = global::ABIS.LogicBuilder.FlowBuilder.Properties.Resources.more;
            this.radButtonHelper.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonHelper.Location = new System.Drawing.Point(1, 0);
            this.radButtonHelper.Margin = new System.Windows.Forms.Padding(0);
            this.radButtonHelper.Name = "radButtonHelper";
            this.radButtonHelper.Size = new System.Drawing.Size(22, 22);
            this.radButtonHelper.TabIndex = 0;
            // 
            // radPanelDropDownList
            // 
            this.radPanelDropDownList.Controls.Add(this.radDropDownList1);
            this.radPanelDropDownList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelDropDownList.Location = new System.Drawing.Point(0, 0);
            this.radPanelDropDownList.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelDropDownList.Name = "radPanelDropDownList";
            this.radPanelDropDownList.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.radPanelDropDownList.Size = new System.Drawing.Size(326, 24);
            this.radPanelDropDownList.TabIndex = 1;
            // 
            // radDropDownList1
            // 
            this.radDropDownList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDropDownList1.DropDownAnimationEnabled = true;
            this.radDropDownList1.Location = new System.Drawing.Point(0, 0);
            this.radDropDownList1.Margin = new System.Windows.Forms.Padding(0);
            this.radDropDownList1.Name = "radDropDownList1";
            this.radDropDownList1.Size = new System.Drawing.Size(324, 24);
            this.radDropDownList1.TabIndex = 0;
            // 
            // AutoCompleteRadDropDownList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanelDropDownList);
            this.Controls.Add(this.radPanelButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AutoCompleteRadDropDownList";
            this.Size = new System.Drawing.Size(350, 24);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelButton)).EndInit();
            this.radPanelButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonHelper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDropDownList)).EndInit();
            this.radPanelDropDownList.ResumeLayout(false);
            this.radPanelDropDownList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelButton;
        private Telerik.WinControls.UI.RadButton radButtonHelper;
        private Telerik.WinControls.UI.RadPanel radPanelDropDownList;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
    }
}
