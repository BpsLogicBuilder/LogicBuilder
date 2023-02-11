using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal partial class EditConstructorControl : UserControl, IEditConstructorControl
    {
        private readonly ILoadParameterControlsDictionary _loadParameterControlsDictionary;
        private readonly IEditingForm editingForm;
        private readonly Constructor constructor;
        private readonly Type assignedTo;
        private readonly IDictionary<string, ParameterControlSet> editControlsSet = new Dictionary<string, ParameterControlSet>();

        private readonly RadGroupBox groupBoxConstructor;
        private readonly RadScrollablePanel radPanelConstructor;
        private readonly RadPanel radPanelTableParent;
        private readonly TableLayoutPanel tableLayoutPanel;
        private readonly RadLabel lblConstructor;
        private RadLabel? lblGenericArguments;

        public bool IsValid => throw new NotImplementedException();

        public EditConstructorControl(
            IEditingControlHelperFactory editingControlFactory,
            IEditingForm editingForm,
            Constructor constructor,
            Type assignedTo)
        {
            InitializeComponent();
            this.editingForm = editingForm;
            this.constructor = constructor;
            this.assignedTo = assignedTo;
            _loadParameterControlsDictionary = editingControlFactory.GetLoadParameterControlsDictionary(this);
            this.groupBoxConstructor = new RadGroupBox();
            this.radPanelConstructor = new RadScrollablePanel();
            this.radPanelTableParent = new RadPanel();
            this.tableLayoutPanel = new TableLayoutPanel();
            this.lblConstructor = new RadLabel();
            if (constructor.HasGenericArguments)
            {
                lblGenericArguments = new()
                {
                    Name = "lblGenericArguments",
                    Text = Strings.lblGenericArgumentsText
                };

            }
            InitializeControls();
        }

        private void InitializeControls()
        {
            const float boundaryWidth = 20F;
            const float singleLineHeight = 33F;
            const float multiLineHeight = 100F;
            const float separatorLineHeight = 3F;
            int rowCount = 4;
            rowCount = constructor.Parameters.Aggregate(rowCount, (current, nextParameter) =>
            {
                if (nextParameter is not LiteralParameter literalParameter || literalParameter.Control != Enums.LiteralParameterInputStyle.SingleLineTextBox)
                    return current;

                current += 2;
                return current;
            });

            float total = (2 * boundaryWidth) + singleLineHeight + separatorLineHeight; //top + bottom + constructorName rows;
            total = constructor.Parameters.Aggregate(total, (current, nextParameter) =>
            {
                if (nextParameter is not LiteralParameter literalParameter 
                    || literalParameter.Control != Enums.LiteralParameterInputStyle.SingleLineTextBox)
                    return current;

                current += literalParameter.Control == Enums.LiteralParameterInputStyle.MultipleLineTextBox  
                            ? multiLineHeight
                            : singleLineHeight;

                current += separatorLineHeight;
                return current;
            });


            if (constructor.HasGenericArguments)
            {
                rowCount += 2;
                total += singleLineHeight;
                total += separatorLineHeight;
            }

            float size_Boundary = boundaryWidth / total * 100;
            float size_SingleLine = singleLineHeight / total * 100;
            float size_MultiLine = multiLineHeight / total * 100;
            float size_Separator = separatorLineHeight / total * 100;

            int totalTableLayoutHeight = (int)Math.Ceiling(total);

            ((ISupportInitialize)(this.groupBoxConstructor)).BeginInit();
            this.groupBoxConstructor.SuspendLayout();
            ((ISupportInitialize)(this.radPanelConstructor)).BeginInit();
            this.radPanelConstructor.PanelContainer.SuspendLayout();
            this.radPanelConstructor.SuspendLayout();
            ((ISupportInitialize)(this.radPanelTableParent)).BeginInit();
            this.radPanelTableParent.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();

            this.SuspendLayout();
            ((ISupportInitialize)(this.lblConstructor)).BeginInit();

            // 
            // groupBoxConstructor
            // 
            this.groupBoxConstructor.AccessibleRole = AccessibleRole.Grouping;
            this.groupBoxConstructor.Controls.Add(this.radPanelConstructor);
            this.groupBoxConstructor.Dock = DockStyle.Fill;
            this.groupBoxConstructor.HeaderText = Strings.editConstructorGroupBoxHeaderText;
            this.groupBoxConstructor.Location = new Point(0, 0);
            this.groupBoxConstructor.Name = "groupBoxConstructor";
            this.groupBoxConstructor.Size = new Size(855, 300);
            this.groupBoxConstructor.TabIndex = 0;
            this.groupBoxConstructor.Text = Strings.editConstructorGroupBoxHeaderText;
            // 
            // radPanelConstructor
            // 
            this.radPanelConstructor.Dock = DockStyle.Fill;
            this.radPanelConstructor.Location = new Point(2, 18);
            this.radPanelConstructor.Name = "radPanelConstructor";
            // 
            // radPanelConstructor.PanelContainer
            // 
            this.radPanelConstructor.PanelContainer.Controls.Add(this.radPanelTableParent);
            this.radPanelConstructor.PanelContainer.Size = new Size(849, 278);
            this.radPanelConstructor.Size = new Size(851, 280);
            this.radPanelConstructor.TabIndex = 0;
            // 
            // radPanelTableParent
            // 
            this.radPanelTableParent.Controls.Add(this.tableLayoutPanel);
            this.radPanelTableParent.Dock = DockStyle.Top;
            this.radPanelTableParent.Location = new Point(0, 0);
            this.radPanelTableParent.Margin = new Padding(0);
            this.radPanelTableParent.Name = "radPanelTableParent";
            this.radPanelTableParent.Size = new Size(851, totalTableLayoutHeight);
            this.radPanelTableParent.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3F));//boundary column - 0
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));//image column - 1
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));//parameter label column - 2
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 63F));//entry column column - 3
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3F));//boundary column - 4

            int currentRow = 1;//constructor name row
            this.tableLayoutPanel.Controls.Add(this.lblConstructor, 3, currentRow);
            currentRow += 2;

            if (constructor.HasGenericArguments)
            {
                this.tableLayoutPanel.Controls.Add(this.lblGenericArguments, 2, currentRow);
                currentRow += 2;
            }

            _loadParameterControlsDictionary.Load(editControlsSet, constructor.Parameters);
            foreach (ParameterBase parameter in constructor.Parameters)
            {
                if (!editControlsSet.ContainsKey(parameter.Name))
                {
                    continue;
                }

                ParameterControlSet parameterControlSet = editControlsSet[parameter.Name];
                this.tableLayoutPanel.Controls.Add(parameterControlSet.ImageLabel, 1, currentRow);
                this.tableLayoutPanel.Controls.Add(parameterControlSet.ChkInclude, 2, currentRow);
                this.tableLayoutPanel.Controls.Add(parameterControlSet.Control, 3, currentRow);
                currentRow += 2;
            }

            this.tableLayoutPanel.Dock = DockStyle.Fill;
            this.tableLayoutPanel.Location = new Point(0, 0);
            this.tableLayoutPanel.Margin = new Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = rowCount;

            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Boundary));//boundary row

            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_SingleLine));//Constructor Name
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Separator));

            if (constructor.HasGenericArguments)
            {
                this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_SingleLine));//Generic Arguments
                this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Separator));
            }

            foreach (ParameterBase parameter in constructor.Parameters)
            {
                if (!editControlsSet.ContainsKey(parameter.Name))
                {
                    continue;
                }

                float size = parameter is LiteralParameter literalParameter && literalParameter.Control == Enums.LiteralParameterInputStyle.MultipleLineTextBox
                            ? size_MultiLine
                            : size_SingleLine;

                this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size));//parameter
                this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Separator));
            }

            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, size_Boundary));//boundary row
            this.tableLayoutPanel.Size = new Size(851, totalTableLayoutHeight);
            this.tableLayoutPanel.TabIndex = 0;

            // 
            // lblConstructorName
            // 
            this.lblConstructor.Dock = DockStyle.Fill;
            this.lblConstructor.Location = new Point(0, 0);
            this.lblConstructor.Name = "lblConstructor";
            this.lblConstructor.Size = new Size(39, 18);
            this.lblConstructor.TabIndex = 0;
            this.lblConstructor.Text = constructor.Name;
            //this.lblConstructor.Text = $"{radPanelTableParent.Height} {totalTableLayoutHeight} {rowCount}";
            lblConstructor.TextAlignment = ContentAlignment.MiddleLeft;
            lblConstructor.Font = new Font(lblConstructor.Font, FontStyle.Bold);

            // 
            // EditConstructorControl
            // 
            this.AutoScaleDimensions = new SizeF(9F, 21F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxConstructor);
            this.Name = "ConfigureConstructorControl";
            this.Size = new Size(855, 300);
            ((ISupportInitialize)(this.groupBoxConstructor)).EndInit();
            this.groupBoxConstructor.ResumeLayout(false);
            this.radPanelConstructor.PanelContainer.ResumeLayout(false);
            ((ISupportInitialize)(this.radPanelConstructor)).EndInit();
            this.radPanelConstructor.ResumeLayout(false);
            ((ISupportInitialize)(this.radPanelTableParent)).EndInit();
            this.radPanelTableParent.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();

            ((ISupportInitialize)(this.lblConstructor)).EndInit();

            this.ResumeLayout(false);
        }

        public void ClearMessage() => editingForm.ClearMessage();

        public void RequestDocumentUpdate() => editingForm.RequestDocumentUpdate();

        public void SetErrorMessage(string message) => editingForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => editingForm.SetMessage(message, title);
    }
}
