using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal partial class SplashScreen : Telerik.WinControls.UI.RadForm, ISplashScreen
    {
        private readonly IFormInitializer _formInitializer;
        public SplashScreen(IFormInitializer formInitializer)
        {
            _formInitializer = formInitializer;
            InitializeComponent();
            Initialize();
            this.Activated += SplashScreen_Activated;
        }

        private void SplashScreen_Activated(object? sender, EventArgs e)
        {
            this.Refresh();
        }

        private void Initialize()
        {
            this.Text = Strings.applicationNameLogicBuilder;
            this.Icon = _formInitializer.GetLogicBuilderIcon();
            this.BackColor = Color.Black;
            this.BackgroundImage = Properties.Resources.logo;
            this.BackgroundImageLayout = ImageLayout.Center;
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
            _formInitializer.SetCenterScreen(this);
        }
    }
}
