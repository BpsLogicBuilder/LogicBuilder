namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector
{
    partial class EditDialogConnectorControl
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
            groupBoxConnector = new Telerik.WinControls.UI.RadGroupBox();
            radPanelConnector = new Telerik.WinControls.UI.RadScrollablePanel();
            radPanelTableParent = new Telerik.WinControls.UI.RadPanel();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            lblText = new Telerik.WinControls.UI.RadLabel();
            lblConnectorObjectType = new Telerik.WinControls.UI.RadLabel();
            lblConnectorObject = new Telerik.WinControls.UI.RadLabel();
            lblIndex = new Telerik.WinControls.UI.RadLabel();
            cmbIndex = new Telerik.WinControls.UI.RadDropDownList();
            radPanelText = new Telerik.WinControls.UI.RadPanel();
            radPanelConnectorObject = new Telerik.WinControls.UI.RadPanel();
            cmbConnectorObjectType = new UserControls.AutoCompleteRadDropDownList();
            ((System.ComponentModel.ISupportInitialize)groupBoxConnector).BeginInit();
            groupBoxConnector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelConnector).BeginInit();
            radPanelConnector.PanelContainer.SuspendLayout();
            radPanelConnector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)radPanelTableParent).BeginInit();
            radPanelTableParent.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lblText).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lblConnectorObjectType).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lblConnectorObject).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lblIndex).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cmbIndex).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelText).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radPanelConnectorObject).BeginInit();
            SuspendLayout();
            // 
            // groupBoxConnector
            // 
            groupBoxConnector.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            groupBoxConnector.Controls.Add(radPanelConnector);
            groupBoxConnector.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxConnector.HeaderText = "Update Connector";
            groupBoxConnector.Location = new System.Drawing.Point(0, 0);
            groupBoxConnector.Name = "groupBoxConnector";
            groupBoxConnector.Size = new System.Drawing.Size(943, 445);
            groupBoxConnector.TabIndex = 1;
            groupBoxConnector.Text = "Update Connector";
            // 
            // radPanelConstructor
            // 
            radPanelConnector.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelConnector.Location = new System.Drawing.Point(2, 18);
            radPanelConnector.Name = "radPanelConstructor";
            // 
            // radPanelConstructor.PanelContainer
            // 
            radPanelConnector.PanelContainer.Controls.Add(radPanelTableParent);
            radPanelConnector.PanelContainer.Size = new System.Drawing.Size(937, 423);
            radPanelConnector.Size = new System.Drawing.Size(939, 425);
            radPanelConnector.TabIndex = 0;
            // 
            // radPanelTableParent
            // 
            radPanelTableParent.Controls.Add(tableLayoutPanel);
            radPanelTableParent.Dock = System.Windows.Forms.DockStyle.Top;
            radPanelTableParent.Location = new System.Drawing.Point(0, 0);
            radPanelTableParent.Margin = new System.Windows.Forms.Padding(0);
            radPanelTableParent.Name = "radPanelTableParent";
            radPanelTableParent.Size = new System.Drawing.Size(937, 184);
            radPanelTableParent.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 4;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3F));
            tableLayoutPanel.Controls.Add(radPanelConnectorObject, 2, 7);
            tableLayoutPanel.Controls.Add(lblText, 1, 3);
            tableLayoutPanel.Controls.Add(lblConnectorObjectType, 1, 5);
            tableLayoutPanel.Controls.Add(lblConnectorObject, 1, 7);
            tableLayoutPanel.Controls.Add(lblIndex, 1, 1);
            tableLayoutPanel.Controls.Add(cmbIndex, 2, 1);
            tableLayoutPanel.Controls.Add(radPanelText, 2, 3);
            tableLayoutPanel.Controls.Add(cmbConnectorObjectType, 2, 5);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 10;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel.Size = new System.Drawing.Size(937, 184);
            tableLayoutPanel.TabIndex = 0;
            // 
            // lblText
            // 
            lblText.Dock = System.Windows.Forms.DockStyle.Top;
            lblText.Location = new System.Drawing.Point(31, 59);
            lblText.Name = "lblText";
            lblText.Size = new System.Drawing.Size(30, 18);
            lblText.TabIndex = 2;
            lblText.Text = "Text:";
            // 
            // lblConnectorObjectType
            // 
            lblConnectorObjectType.Dock = System.Windows.Forms.DockStyle.Top;
            lblConnectorObjectType.Location = new System.Drawing.Point(31, 95);
            lblConnectorObjectType.Name = "lblConnectorObjectType";
            lblConnectorObjectType.Size = new System.Drawing.Size(124, 18);
            lblConnectorObjectType.TabIndex = 4;
            lblConnectorObjectType.Text = "Connector Object Type:";
            // 
            // lblConnectorObject
            // 
            lblConnectorObject.Dock = System.Windows.Forms.DockStyle.Top;
            lblConnectorObject.Location = new System.Drawing.Point(31, 131);
            lblConnectorObject.Name = "lblConnectorObject";
            lblConnectorObject.Size = new System.Drawing.Size(97, 18);
            lblConnectorObject.TabIndex = 6;
            lblConnectorObject.Text = "Connector Object:";
            // 
            // lblIndex
            // 
            lblIndex.Dock = System.Windows.Forms.DockStyle.Top;
            lblIndex.Location = new System.Drawing.Point(31, 23);
            lblIndex.Name = "lblIndex";
            lblIndex.Size = new System.Drawing.Size(36, 18);
            lblIndex.TabIndex = 0;
            lblIndex.Text = "Index:";
            // 
            // cmbIndex
            // 
            cmbIndex.AutoSize = false;
            cmbIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            cmbIndex.DropDownAnimationEnabled = true;
            cmbIndex.Location = new System.Drawing.Point(318, 20);
            cmbIndex.Margin = new System.Windows.Forms.Padding(0);
            cmbIndex.Name = "cmbIndex";
            cmbIndex.Size = new System.Drawing.Size(590, 30);
            cmbIndex.TabIndex = 7;
            // 
            // radPanelText
            // 
            radPanelText.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelText.Location = new System.Drawing.Point(318, 56);
            radPanelText.Margin = new System.Windows.Forms.Padding(0);
            radPanelText.Name = "radPanelText";
            radPanelText.Size = new System.Drawing.Size(590, 30);
            radPanelText.TabIndex = 8;
            // 
            // radPanelConnectorObject
            // 
            radPanelConnectorObject.Dock = System.Windows.Forms.DockStyle.Fill;
            radPanelConnectorObject.Location = new System.Drawing.Point(318, 128);
            radPanelConnectorObject.Margin = new System.Windows.Forms.Padding(0);
            radPanelConnectorObject.Name = "radPanelConnectorObject";
            radPanelConnectorObject.Size = new System.Drawing.Size(590, 30);
            radPanelConnectorObject.TabIndex = 9;
            // 
            // cmbConnectorObjectType
            // 
            cmbConnectorObjectType.Dock = System.Windows.Forms.DockStyle.Fill;
            cmbConnectorObjectType.Location = new System.Drawing.Point(318, 92);
            cmbConnectorObjectType.Margin = new System.Windows.Forms.Padding(0);
            cmbConnectorObjectType.Name = "cmbConnectorObjectType";
            cmbConnectorObjectType.SelectedText = "";
            cmbConnectorObjectType.Size = new System.Drawing.Size(590, 30);
            cmbConnectorObjectType.TabIndex = 10;
            // 
            // EditDialogConnectorControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBoxConnector);
            Name = "EditDialogConnectorControl";
            Size = new System.Drawing.Size(943, 445);
            ((System.ComponentModel.ISupportInitialize)groupBoxConnector).EndInit();
            groupBoxConnector.ResumeLayout(false);
            radPanelConnector.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelConnector).EndInit();
            radPanelConnector.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)lblText).EndInit();
            ((System.ComponentModel.ISupportInitialize)lblConnectorObjectType).EndInit();
            ((System.ComponentModel.ISupportInitialize)lblConnectorObject).EndInit();
            ((System.ComponentModel.ISupportInitialize)lblIndex).EndInit();
            ((System.ComponentModel.ISupportInitialize)cmbIndex).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelText).EndInit();
            ((System.ComponentModel.ISupportInitialize)radPanelConnectorObject).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox groupBoxConnector;
        private Telerik.WinControls.UI.RadScrollablePanel radPanelConnector;
        private Telerik.WinControls.UI.RadPanel radPanelTableParent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private Telerik.WinControls.UI.RadLabel lblText;
        private Telerik.WinControls.UI.RadLabel lblConnectorObjectType;
        private Telerik.WinControls.UI.RadLabel lblConnectorObject;
        private Telerik.WinControls.UI.RadLabel lblIndex;
        private Telerik.WinControls.UI.RadDropDownList cmbIndex;
        private Telerik.WinControls.UI.RadPanel radPanelText;
        private Telerik.WinControls.UI.RadPanel radPanelConnectorObject;
        private UserControls.AutoCompleteRadDropDownList cmbConnectorObjectType;
    }
}
