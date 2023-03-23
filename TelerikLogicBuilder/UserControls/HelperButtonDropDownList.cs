using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    public partial class HelperButtonDropDownList : UserControl
    {
        private RadButton radButtonHelper;
        private RadDropDownList radDropDownList;

        public HelperButtonDropDownList()
        {
            InitializeComponent();
            Initialize();
        }

        public RadDropDownList DropDownList => radDropDownList;

        public new string Text { get => radDropDownList.Text; set => radDropDownList.Text = value; }

        public event EventHandler<EventArgs>? ButtonClick;

        public event EventHandler? Changed;

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        [MemberNotNull(nameof(radButtonHelper))]
        [MemberNotNull(nameof(radDropDownList))]
        private void Initialize()
        {
            InitializeButton();
            InitializeDropDownList();
            CollapsePanelBorder(radPanelDropDownList);
            CollapsePanelBorder(radPanelButton);

            ControlsLayoutUtility.SetDropDownListPadding(radDropDownList);
            radButtonHelper.Click += RadButtonHelper_Click;
            radDropDownList.TextChanged += RadDropDownList_TextChanged;
        }

        [MemberNotNull(nameof(radButtonHelper))]
        private void InitializeButton()
        {
            radButtonHelper = new()
            {
                Name = "btnHelper",
                ImageAlignment = ContentAlignment.MiddleCenter,
                Padding = new Padding(0),
                Margin = new Padding(1, 0, 1, 0),
                ImageIndex = ImageIndexes.MOREIMAGEINDEX,
                Dock = DockStyle.Fill,
                Image = Properties.Resources.more
            };

            ((ISupportInitialize)this.radPanelButton).BeginInit();
            this.radPanelButton.SuspendLayout();
            this.radPanelButton.Padding = new Padding(0, 1, 3, 0);
            this.radPanelButton.Size = new Size(PerFontSizeConstants.CommandButtonWidth, this.radPanelButton.Height);

            ((ISupportInitialize)radButtonHelper).BeginInit();

            radButtonHelper.Location = new Point(43, 69);
            radButtonHelper.Name = "radButtonHelper";
            radButtonHelper.Size = new Size(110, 24);
            radButtonHelper.TabIndex = 1;
            ((ISupportInitialize)radButtonHelper).EndInit();

            this.radPanelButton.Controls.Add(radButtonHelper);
            ((ISupportInitialize)this.radPanelButton).EndInit();
            this.radPanelButton.ResumeLayout(true);
        }

        [MemberNotNull(nameof(radDropDownList))]
        private void InitializeDropDownList()
        {
            this.radDropDownList = new RadDropDownList();

            ((ISupportInitialize)this.radPanelDropDownList).BeginInit();
            this.radPanelDropDownList.SuspendLayout();

            ((ISupportInitialize)this.radDropDownList).BeginInit();
            this.radDropDownList.Dock = DockStyle.Fill;
            this.radDropDownList.AutoSize = false;
            this.radDropDownList.DropDownAnimationEnabled = true;
            this.radDropDownList.Location = new Point(0, 0);
            this.radDropDownList.Name = "radDropDownList";
            this.radDropDownList.Size = new Size(350, 28);
            this.radDropDownList.TabIndex = 0;
            ((ISupportInitialize)this.radDropDownList).EndInit();

            this.radPanelDropDownList.Controls.Add(radDropDownList);
            ((ISupportInitialize)this.radPanelDropDownList).EndInit();
            this.radPanelDropDownList.ResumeLayout(true);
        }

        #region Event Handlers
        private void RadButtonHelper_Click(object? sender, EventArgs e)
        {
            ButtonClick?.Invoke(this, e);
        }

        private void RadDropDownList_TextChanged(object? sender, EventArgs e)
        {
            Changed?.Invoke(this, e);
        }
        #endregion Event Handlers
    }
}
