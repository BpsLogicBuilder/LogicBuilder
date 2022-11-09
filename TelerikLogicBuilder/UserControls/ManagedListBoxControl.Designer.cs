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
            this.radPanelUpDownButtons = new Telerik.WinControls.UI.RadPanel();
            this.btnDown = new Telerik.WinControls.UI.RadButton();
            this.btnUp = new Telerik.WinControls.UI.RadButton();
            this.radPanelEditButtons = new Telerik.WinControls.UI.RadPanel();
            this.btnUpdate = new Telerik.WinControls.UI.RadButton();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.btnCopy = new Telerik.WinControls.UI.RadButton();
            this.btnEdit = new Telerik.WinControls.UI.RadButton();
            this.btnRemove = new Telerik.WinControls.UI.RadButton();
            this.radPanelListBox = new Telerik.WinControls.UI.RadPanel();
            this.listBox = new Telerik.WinControls.UI.RadListControl();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelUpDownButtons)).BeginInit();
            this.radPanelUpDownButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelEditButtons)).BeginInit();
            this.radPanelEditButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCopy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRemove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelListBox)).BeginInit();
            this.radPanelListBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBox)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelUpDownButtons
            // 
            this.radPanelUpDownButtons.Controls.Add(this.btnDown);
            this.radPanelUpDownButtons.Controls.Add(this.btnUp);
            this.radPanelUpDownButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelUpDownButtons.Location = new System.Drawing.Point(576, 0);
            this.radPanelUpDownButtons.Name = "radPanelUpDownButtons";
            this.radPanelUpDownButtons.Size = new System.Drawing.Size(33, 168);
            this.radPanelUpDownButtons.TabIndex = 3;
            // 
            // btnDown
            // 
            this.btnDown.Image = global::ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Down;
            this.btnDown.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDown.Location = new System.Drawing.Point(6, 93);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(24, 24);
            this.btnDown.TabIndex = 1;
            this.btnDown.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUp
            // 
            this.btnUp.Image = global::ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Up;
            this.btnUp.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUp.Location = new System.Drawing.Point(6, 63);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(24, 24);
            this.btnUp.TabIndex = 0;
            // 
            // radPanelEditButtons
            // 
            this.radPanelEditButtons.Controls.Add(this.btnUpdate);
            this.radPanelEditButtons.Controls.Add(this.btnCancel);
            this.radPanelEditButtons.Controls.Add(this.btnCopy);
            this.radPanelEditButtons.Controls.Add(this.btnEdit);
            this.radPanelEditButtons.Controls.Add(this.btnRemove);
            this.radPanelEditButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanelEditButtons.Location = new System.Drawing.Point(609, 0);
            this.radPanelEditButtons.Name = "radPanelEditButtons";
            this.radPanelEditButtons.Size = new System.Drawing.Size(134, 168);
            this.radPanelEditButtons.TabIndex = 4;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(9, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(110, 24);
            this.btnUpdate.TabIndex = 0;
            this.btnUpdate.Text = "Update";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(9, 33);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(9, 63);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(110, 24);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "Copy";
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(9, 93);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(110, 24);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Edit";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(9, 123);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(110, 24);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "Remove";
            // 
            // radPanelListBox
            // 
            this.radPanelListBox.Controls.Add(this.listBox);
            this.radPanelListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelListBox.Location = new System.Drawing.Point(0, 0);
            this.radPanelListBox.Name = "radPanelListBox";
            this.radPanelListBox.Size = new System.Drawing.Size(576, 168);
            this.radPanelListBox.TabIndex = 5;
            // 
            // listBox
            // 
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.Location = new System.Drawing.Point(0, 0);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(576, 168);
            this.listBox.TabIndex = 0;
            // 
            // ManagedListBoxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanelListBox);
            this.Controls.Add(this.radPanelUpDownButtons);
            this.Controls.Add(this.radPanelEditButtons);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ManagedListBoxControl";
            this.Size = new System.Drawing.Size(743, 168);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelUpDownButtons)).EndInit();
            this.radPanelUpDownButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelEditButtons)).EndInit();
            this.radPanelEditButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCopy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRemove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelListBox)).EndInit();
            this.radPanelListBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelUpDownButtons;
        private Telerik.WinControls.UI.RadButton btnDown;
        private Telerik.WinControls.UI.RadButton btnUp;
        private Telerik.WinControls.UI.RadPanel radPanelEditButtons;
        private Telerik.WinControls.UI.RadButton btnUpdate;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadButton btnCopy;
        private Telerik.WinControls.UI.RadButton btnEdit;
        private Telerik.WinControls.UI.RadButton btnRemove;
        private Telerik.WinControls.UI.RadPanel radPanelListBox;
        private Telerik.WinControls.UI.RadListControl listBox;
    }
}
