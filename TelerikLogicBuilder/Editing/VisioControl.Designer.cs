
using ABIS.LogicBuilder.FlowBuilder.Components;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    partial class VisioControl
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
            this.axDrawingControl1 = new Components.VisioDrawingControl();
            ((System.ComponentModel.ISupportInitialize)(this.titleBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axDrawingControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // titleBar1
            // 
            this.titleBar1.AllowResize = false;
            this.titleBar1.CanManageOwnerForm = false;
            this.titleBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar1.Location = new System.Drawing.Point(0, 0);
            this.titleBar1.Name = "titleBar1";
            this.titleBar1.Size = new System.Drawing.Size(1110, 23);
            this.titleBar1.TabIndex = 0;
            this.titleBar1.TabStop = false;
            this.titleBar1.Text = "titleBar1";
            // 
            // axDrawingControl1
            // 
            this.axDrawingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axDrawingControl1.Enabled = true;
            this.axDrawingControl1.Location = new System.Drawing.Point(0, 23);
            this.axDrawingControl1.Name = "axDrawingControl1";
            this.axDrawingControl1.Size = new System.Drawing.Size(1110, 664);
            this.axDrawingControl1.TabIndex = 1;
            this.axDrawingControl1.SelectionChanged += new AxMicrosoft.Office.Interop.VisOcx.EVisOcx_SelectionChangedEventHandler(this.AxDrawingControl1_SelectionChanged);
            this.axDrawingControl1.MouseDownEvent += new AxMicrosoft.Office.Interop.VisOcx.EVisOcx_MouseDownEventHandler(AxDrawingControl1_MouseDownEvent);
            this.axDrawingControl1.MouseUpEvent += new AxMicrosoft.Office.Interop.VisOcx.EVisOcx_MouseUpEventHandler(AxDrawingControl1_MouseUpEvent);
            this.axDrawingControl1.DocumentOpened += new AxMicrosoft.Office.Interop.VisOcx.EVisOcx_DocumentOpenedEventHandler(this.AxDrawingControl1_DocumentOpened);
            // 
            // VisioControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axDrawingControl1);
            this.Controls.Add(this.titleBar1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "VisioControl";
            this.Size = new System.Drawing.Size(1110, 687);
            ((System.ComponentModel.ISupportInitialize)(this.titleBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axDrawingControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TitleBar titleBar1;
        private Components.VisioDrawingControl axDrawingControl1;
    }
}
