using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    public partial class HelperButtonTextBox : UserControl
    {
        private RadButton radButtonHelper;
        private RadTextBox radTextBox1;

        public HelperButtonTextBox()
        {
            InitializeComponent();
            Initialize();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Text { get => radTextBox1.Text; set => radTextBox1.Text = value; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Font Font { get => radTextBox1.Font; set => radTextBox1.Font = value; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ReadOnly { get => radTextBox1.ReadOnly; set => radTextBox1.ReadOnly = value; }

        public event EventHandler<EventArgs>? ButtonClick;
        public event EventHandler<EventArgs>? TextClick;

        public new event CancelEventHandler? Validating;

        internal void SetPaddingType(PaddingType paddingType)
        {
            if (paddingType == PaddingType.Bold)
                radTextBox1.TextBoxElement.Padding = new Padding(0);/*Makes the text more visible.  Unclear why because the original padding also appears to be zero.*/

            if (paddingType == PaddingType.Normal)
                radTextBox1.TextBoxElement.Padding = new Padding(2);
        }

        [MemberNotNull(nameof(radButtonHelper))]
        [MemberNotNull(nameof(radTextBox1))]
        private void Initialize()
        {
            InitializeButton();
            InitializeTextBox();

            ControlsLayoutUtility.SetTextBoxPadding(radTextBox1);
            radButtonHelper.Click += RadButtonHelper_Click;
            radTextBox1.Click += RadTextBox1_Click;

            radTextBox1.Validating += TextBox_Validating;
            radTextBox1.SizeChanged += RadTextBox_SizeChanged;

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
                Image = Properties.Resources.more,
                TabStop = false
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

        [MemberNotNull(nameof(radTextBox1))]
        private void InitializeTextBox()
        {
            this.radTextBox1 = new RadTextBox();

            ((ISupportInitialize)this.radPanelTextBox).BeginInit();
            this.radPanelTextBox.SuspendLayout();
            this.radPanelTextBox.Padding = new Padding(0, 1, 3, 0);

            ((ISupportInitialize)this.radTextBox1).BeginInit();
            this.radTextBox1.AutoSize = false;
            this.radTextBox1.Dock = DockStyle.Fill;
            this.radTextBox1.Location = new Point(0, 0);
            this.radTextBox1.Name = "radTextBox1";
            this.radTextBox1.Size = new Size(350, 28);
            this.radTextBox1.TabIndex = 0;
            ((ISupportInitialize)(this.radTextBox1)).EndInit();

            this.radPanelTextBox.Controls.Add(radTextBox1);
            ((ISupportInitialize)this.radPanelTextBox).EndInit();
            this.radPanelTextBox.ResumeLayout(true);
        }

        private void RadTextBox_SizeChanged(object? sender, EventArgs e)
        {
            //this.Size = new Size(radButtonTextBox1.Size.Width, radButtonTextBox1.Size.Height);
            //radButtonHelper.Size = new Size(radButtonHelper.Size.Width, radButtonTextBox1.Size.Height - 4);
        }

        private void TextBox_Validating(object? sender, CancelEventArgs e)
        {
            Validating?.Invoke(this, e);
        }

        private void RadButtonHelper_Click(object? sender, EventArgs e)
        {
            ButtonClick?.Invoke(this, e);
        }

        private void RadTextBox1_Click(object? sender, EventArgs e)
        {
            TextClick?.Invoke(this, e);
        }

        internal enum PaddingType
        {
            Bold,
            Normal
        }
    }
}
