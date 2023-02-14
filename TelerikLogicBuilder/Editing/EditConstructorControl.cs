using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal partial class EditConstructorControl : UserControl, IEditConstructorControl
    {
        private readonly ILoadParameterControlsDictionary _loadParameterControlsDictionary;
        private readonly ITableLayoutPanelHelper _tableLayoutPanelHelper;
        private readonly IEditingForm editingForm;
        private readonly Constructor constructor;
        private readonly Type assignedTo;
        private readonly IDictionary<string, ParameterControlSet> editControlsSet = new Dictionary<string, ParameterControlSet>();

        private readonly RadGroupBox groupBoxConstructor;
        private readonly RadScrollablePanel radPanelConstructor;
        private readonly RadPanel radPanelTableParent;
        private readonly TableLayoutPanel tableLayoutPanel;
        private readonly RadLabel lblConstructor;
        private readonly RadLabel? lblGenericArguments;

        public bool IsValid => throw new NotImplementedException();

        public EditConstructorControl(
            IEditingControlHelperFactory editingControlFactory,
            ITableLayoutPanelHelper tableLayoutPanelHelper,
            IEditingForm editingForm,
            Constructor constructor,
            Type assignedTo)
        {
            InitializeComponent();
            _tableLayoutPanelHelper = tableLayoutPanelHelper;
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
                    Dock = DockStyle.Fill,
                    Location = new Point(0, 0),
                    Name = "lblGenericArguments",
                    Text = Strings.lblGenericArgumentsText
                };
            }

            InitializeControls();
        }

        private void InitializeControls()
        {
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
            if (constructor.HasGenericArguments)
            {
                ((ISupportInitialize)(this.lblGenericArguments!)).BeginInit();
            }
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
            //radPanelTableParent
            //tableLayoutPanel
            _tableLayoutPanelHelper.SetUp(tableLayoutPanel, radPanelTableParent, constructor.Parameters, constructor.HasGenericArguments);

            int currentRow = 1;//constructor name row
            this.tableLayoutPanel.Controls.Add(this.lblConstructor, 3, currentRow);
            currentRow += 2;

            if (constructor.HasGenericArguments)
            {
                this.tableLayoutPanel.Controls.Add(this.lblGenericArguments, 2, currentRow);
                currentRow += 2;
            }

            if (ValidateGenericArgs())
            {
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
            }

            // 
            // lblConstructorName
            // 
            this.lblConstructor.Dock = DockStyle.Fill;
            this.lblConstructor.Location = new Point(0, 0);
            this.lblConstructor.Name = "lblConstructor";
            this.lblConstructor.Size = new Size(39, 18);
            this.lblConstructor.TabIndex = 0;
            this.lblConstructor.Text = constructor.Name;
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
            if (constructor.HasGenericArguments)
            {
                ((ISupportInitialize)(this.lblGenericArguments!)).EndInit();
            }

            this.ResumeLayout(false);

            CollapsePanelBorder(radPanelTableParent);
            CollapsePanelBorder(radPanelConstructor);
        }

        public void ClearMessage() => editingForm.ClearMessage();

        public void RequestDocumentUpdate() => editingForm.RequestDocumentUpdate();

        public void SetErrorMessage(string message) => editingForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => editingForm.SetMessage(message, title);

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        bool ValidateGenericArgs() => true;
    }
}
