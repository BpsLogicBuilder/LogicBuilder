using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    public partial class RichTextBoxViewerPanel : UserControl
    {
        public RichTextBoxViewerPanel()
        {
            InitializeComponent();
            this.office2007BlackTheme1 = new Telerik.WinControls.Themes.Office2007BlackTheme();
            this.office2007SilverTheme1 = new Telerik.WinControls.Themes.Office2007SilverTheme();
            this.office2010BlackTheme1 = new Telerik.WinControls.Themes.Office2010BlackTheme();
            this.office2010BlueTheme1 = new Telerik.WinControls.Themes.Office2010BlueTheme();
            this.office2010SilverTheme1 = new Telerik.WinControls.Themes.Office2010SilverTheme();
            this.office2013DarkTheme1 = new Telerik.WinControls.Themes.Office2013DarkTheme();
            this.office2013LightTheme1 = new Telerik.WinControls.Themes.Office2013LightTheme();
            this.office2019GrayTheme1 = new Telerik.WinControls.Themes.Office2019GrayTheme();
            this.office2019LightTheme1 = new Telerik.WinControls.Themes.Office2019LightTheme();
            this.office2019DarkTheme1 = new Telerik.WinControls.Themes.Office2019DarkTheme();
            Initialize();
        }

        private Telerik.WinControls.Themes.Office2007BlackTheme office2007BlackTheme1;
        private Telerik.WinControls.Themes.Office2007SilverTheme office2007SilverTheme1;
        private Telerik.WinControls.Themes.Office2010BlackTheme office2010BlackTheme1;
        private Telerik.WinControls.Themes.Office2010BlueTheme office2010BlueTheme1;
        private Telerik.WinControls.Themes.Office2010SilverTheme office2010SilverTheme1;
        private Telerik.WinControls.Themes.Office2013DarkTheme office2013DarkTheme1;
        private Telerik.WinControls.Themes.Office2013LightTheme office2013LightTheme1;
        private Telerik.WinControls.Themes.Office2019GrayTheme office2019GrayTheme1;
        private Telerik.WinControls.Themes.Office2019DarkTheme office2019DarkTheme1;
        private Telerik.WinControls.Themes.Office2019LightTheme office2019LightTheme1;

        public RichTextBox RichTextBox => this.richTextBox1;

        private void Initialize()
        {
            this.richTextBox1.WordWrap = false;
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.BorderStyle = BorderStyle.None;
            this.richTextBox1.ScrollBars = RichTextBoxScrollBars.Both;
            this.richTextBox1.Select(0, 0);

            this.richTextBox1.BackColor = ForeColorUtility.GetTextBoxBackColor(ThemeResolutionService.ApplicationThemeName);
            this.richTextBox1.ForeColor = ForeColorUtility.GetTextBoxForeColor(ThemeResolutionService.ApplicationThemeName);

            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args)
        {
            this.richTextBox1.BackColor = ForeColorUtility.GetTextBoxBackColor(args.ThemeName);
            this.richTextBox1.ForeColor = ForeColorUtility.GetTextBoxForeColor(args.ThemeName);
        }
    }
}
