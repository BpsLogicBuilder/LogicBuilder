using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    public partial class HelperButtonTextBox : UserControl
    {
        public HelperButtonTextBox()
        {
            InitializeComponent();
            Initialize();
        }

        public new string Text { get => radButtonTextBox1.Text; set => radButtonTextBox1.Text = value; }

        public new Font Font { get => radButtonTextBox1.Font; set => radButtonTextBox1.Font = value; }

        public bool ReadOnly { get => radButtonTextBox1.ReadOnly; set => radButtonTextBox1.ReadOnly = value; }

        public event EventHandler<EventArgs>? ButtonClick;

        public new event CancelEventHandler? Validating;

        private void Initialize()
        {
            radButtonHelper.Image = Properties.Resources.more;
            radButtonHelper.Click += RadButtonHelper_Click;
            radButtonTextBox1.Validating += TextBox_Validating;
            radButtonTextBox1.SizeChanged += RadButtonTextBox1_SizeChanged;
            
        }

        private void RadButtonTextBox1_SizeChanged(object? sender, EventArgs e)
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
    }
}
