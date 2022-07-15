using ABIS.LogicBuilder.FlowBuilder.Components;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    partial class Messages
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
            this.components = new System.ComponentModel.Container();
            this.titleBar1 = new ABIS.LogicBuilder.FlowBuilder.Components.TitleBar();
            this.radPageView1 = new Telerik.WinControls.UI.RadPageView();
            this.radPageViewDocuments = new Telerik.WinControls.UI.RadPageViewPage();
            this.radPageViewRules = new Telerik.WinControls.UI.RadPageViewPage();
            this.radPageViewMessages = new Telerik.WinControls.UI.RadPageViewPage();
            this.radPageViewPageSearchResults = new Telerik.WinControls.UI.RadPageViewPage();
            ((System.ComponentModel.ISupportInitialize)(this.titleBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleBar1
            // 
            this.titleBar1.AllowResize = false;
            this.titleBar1.CanManageOwnerForm = false;
            this.titleBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar1.Location = new System.Drawing.Point(0, 0);
            this.titleBar1.Name = "titleBar1";
            this.titleBar1.Size = new System.Drawing.Size(674, 23);
            this.titleBar1.TabIndex = 0;
            this.titleBar1.TabStop = false;
            // 
            // radPageView1
            // 
            this.radPageView1.Controls.Add(this.radPageViewDocuments);
            this.radPageView1.Controls.Add(this.radPageViewRules);
            this.radPageView1.Controls.Add(this.radPageViewMessages);
            this.radPageView1.Controls.Add(this.radPageViewPageSearchResults);
            this.radPageView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPageView1.Location = new System.Drawing.Point(0, 23);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.SelectedPage = this.radPageViewDocuments;
            this.radPageView1.Size = new System.Drawing.Size(674, 294);
            this.radPageView1.TabIndex = 1;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(radPageView1.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(radPageView1.GetChildAt(0))).ShowItemCloseButton = false;
            // 
            // radPageViewDocuments
            // 
            this.radPageViewDocuments.ItemSize = new System.Drawing.SizeF(73F, 28F);
            this.radPageViewDocuments.Location = new System.Drawing.Point(10, 37);
            this.radPageViewDocuments.Name = "radPageViewDocuments";
            this.radPageViewDocuments.Size = new System.Drawing.Size(653, 246);
            this.radPageViewDocuments.Text = "Documents";
            // 
            // radPageViewRules
            // 
            this.radPageViewRules.ItemSize = new System.Drawing.SizeF(43F, 28F);
            this.radPageViewRules.Location = new System.Drawing.Point(0, 0);
            this.radPageViewRules.Name = "radPageViewRules";
            this.radPageViewRules.Size = new System.Drawing.Size(200, 100);
            this.radPageViewRules.Text = "Rules";
            // 
            // radPageViewMessages
            // 
            this.radPageViewMessages.ItemSize = new System.Drawing.SizeF(65F, 28F);
            this.radPageViewMessages.Location = new System.Drawing.Point(0, 0);
            this.radPageViewMessages.Name = "radPageViewMessages";
            this.radPageViewMessages.Size = new System.Drawing.Size(200, 100);
            this.radPageViewMessages.Text = "Messages";
            // 
            // radPageViewPageSearchResults
            // 
            this.radPageViewPageSearchResults.ItemSize = new System.Drawing.SizeF(88F, 28F);
            this.radPageViewPageSearchResults.Location = new System.Drawing.Point(0, 0);
            this.radPageViewPageSearchResults.Name = "radPageViewPageSearchResults";
            this.radPageViewPageSearchResults.Size = new System.Drawing.Size(200, 100);
            this.radPageViewPageSearchResults.Text = "Search Results";
            // 
            // Messages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPageView1);
            this.Controls.Add(this.titleBar1);
            this.Name = "Messages";
            this.Size = new System.Drawing.Size(674, 317);
            ((System.ComponentModel.ISupportInitialize)(this.titleBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            this.radPageView1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TitleBar titleBar1;
        private Telerik.WinControls.UI.RadPageView radPageView1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewDocuments;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewRules;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewMessages;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPageSearchResults;
    }
}
