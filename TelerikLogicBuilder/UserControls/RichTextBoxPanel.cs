using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers;
using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class RichTextBoxPanel : UserControl
    {
        private readonly IImageListService _imageListService;

        public RichTextBoxPanel(
            IImageListService imageListService)
        {
            InitializeComponent();
            _imageListService = imageListService;
            radContextMenuManager = new RadContextMenuManager();
            Initialize();
        }

        private readonly RadMenuItem mnuItemCopy = new(Strings.mnuItemCopyText) { ImageIndex = ImageIndexes.COPYIMAGEINDEX };
        private readonly RadMenuItem mnuItemCut = new(Strings.mnuItemCutText) { ImageIndex = ImageIndexes.CUTIMAGEINDEX };
        private readonly RadMenuItem mnuItemPaste = new(Strings.mnuItemPasteText);
        private readonly RadMenuItem mnuItemSelectAll = new(Strings.mnuItemSelectAllText);
        private readonly RadContextMenuManager radContextMenuManager;

        public new event EventHandler? TextChanged;

        public string[] Lines { get => richTextBox1.Lines; set => richTextBox1.Lines = value; }

        public RichTextBox RichTextBox => this.richTextBox1;

        public new string Text { get => richTextBox1.Text; set => richTextBox1.Text = value; }

        private static void AddClickCommand(RadMenuItem radMenuItem, IClickCommand command)
        {
            radMenuItem.Click += (sender, args) => command.Execute();
        }

        private void CreateContextMenu()
        {
            AddClickCommand(mnuItemCopy, new RichTextBoxCopySelectedTextCommand(richTextBox1));
            AddClickCommand(mnuItemCut, new RichTextBoxCutSelectedTextCommand(richTextBox1));
            AddClickCommand(mnuItemPaste, new RichTextBoxPasteTextCommand(richTextBox1));
            AddClickCommand(mnuItemSelectAll, new RichTextBoxSelectAllCommand(richTextBox1));

            RadContextMenu radContextMenu = new()
            {
                ImageList = _imageListService.ImageList,
                Items = 
                {
                    mnuItemCopy,
                    mnuItemCut,
                    mnuItemPaste,
                    new RadMenuSeparatorItem(),
                    mnuItemSelectAll
                }
            };

            radContextMenuManager.SetRadContextMenu(richTextBox1, radContextMenu);
        }

        private void Initialize()
        {
            this.richTextBox1.WordWrap = false;
            this.richTextBox1.BorderStyle = BorderStyle.None;
            this.richTextBox1.ScrollBars = RichTextBoxScrollBars.Both;
            this.richTextBox1.Select(0, 0);

            this.richTextBox1.BackColor = ForeColorUtility.GetTextBoxBackColor(ThemeResolutionService.ApplicationThemeName);
            this.richTextBox1.ForeColor = ForeColorUtility.GetTextBoxForeColor(ThemeResolutionService.ApplicationThemeName);

            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            this.Disposed += RichTextBoxPanel_Disposed;
            this.MouseDown += RichTextBoxPanel_MouseDown;
            this.RichTextBox.MouseDown += RichTextBox_MouseDown;
            this.RichTextBox.TextChanged += RichTextBox_TextChanged;

            CreateContextMenu();
        }

        private void SetContextMenuState()
        {
            mnuItemPaste.Enabled = richTextBox1.Enabled && Clipboard.GetText() != null;
            mnuItemCut.Enabled = !string.IsNullOrEmpty(richTextBox1.SelectedText);
            mnuItemCopy.Enabled = !string.IsNullOrEmpty(richTextBox1.SelectedText);
            mnuItemSelectAll.Enabled = richTextBox1.Enabled;
        }

        private void RichTextBox_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                return;

            SetContextMenuState();
        }

        private void RichTextBox_TextChanged(object? sender, EventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }

        private void RichTextBoxPanel_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                return;

            SetContextMenuState();
        }

        private void RichTextBoxPanel_Disposed(object? sender, EventArgs e)
        {
            radContextMenuManager.Dispose();
            ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args)
        {
            if (this.IsDisposed)
                return;

            this.richTextBox1.BackColor = ForeColorUtility.GetTextBoxBackColor(args.ThemeName);
            this.richTextBox1.ForeColor = ForeColorUtility.GetTextBoxForeColor(args.ThemeName);
        }
    }
}
