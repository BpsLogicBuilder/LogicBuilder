using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class EditXmlRichTextBoxPanel : UserControl
    {
        private readonly IImageListService _imageListService;
        private readonly IRichTextBoxPanelCommandFactory _richTextBoxPanelCommandFactory;

        public EditXmlRichTextBoxPanel(
            IImageListService imageListService,
            IRichTextBoxPanelCommandFactory richTextBoxPanelCommandFactory)
        {
            InitializeComponent();
            _imageListService = imageListService;
            _richTextBoxPanelCommandFactory = richTextBoxPanelCommandFactory;
            radContextMenuManager = new RadContextMenuManager();
            Initialize();
        }

        private readonly RadMenuItem mnuItemCopy = new(Strings.mnuItemCopyText) { ImageIndex = ImageIndexes.COPYIMAGEINDEX };
        private readonly RadMenuItem mnuItemCut = new(Strings.mnuItemCutText) { ImageIndex = ImageIndexes.CUTIMAGEINDEX };
        private readonly RadMenuItem mnuItemPaste = new(Strings.mnuItemPasteText);
        private readonly RadMenuItem mnuItemSelectAll = new(Strings.mnuItemSelectAllText);
        private readonly RadMenuItem mnuItemInsertXmlFragment = new(Strings.mnuItemInsertXmlFragment);
        private readonly RadContextMenuManager radContextMenuManager;
        private RadContextMenu radContextMenu;
        private EventHandler mnuItemCopyClickHandler;
        private EventHandler mnuItemCutClickHandler;
        private EventHandler mnuItemPasteClickHandler;
        private EventHandler mnuItemSelectAllClickHandler;
        private EventHandler mnuItemInsertXmlFragmentClickHandler;

        public new event EventHandler? TextChanged;

        public string[] Lines { get => richTextBox1.Lines; set => richTextBox1.Lines = value; }

        public RichTextBox RichTextBox => this.richTextBox1;

        public new string Text { get => richTextBox1.Text; set => richTextBox1.Text = value; }

        private static EventHandler AddClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void AddClickCommands()
        {
            RemoveClickCommands();
            mnuItemCopy.Click += mnuItemCopyClickHandler;
            mnuItemCut.Click += mnuItemCutClickHandler;
            mnuItemPaste.Click += mnuItemPasteClickHandler;
            mnuItemSelectAll.Click += mnuItemSelectAllClickHandler;
            mnuItemInsertXmlFragment.Click += mnuItemInsertXmlFragmentClickHandler;
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemCopyClickHandler),
            nameof(mnuItemCutClickHandler),
            nameof(mnuItemPasteClickHandler),
            nameof(mnuItemSelectAllClickHandler),
            nameof(mnuItemInsertXmlFragmentClickHandler),
            nameof(radContextMenu))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void CreateContextMenu()
        {
            mnuItemCopyClickHandler = AddClickCommand(_richTextBoxPanelCommandFactory.GetRichTextBoxCopySelectedTextCommand(richTextBox1));
            mnuItemCutClickHandler = AddClickCommand(_richTextBoxPanelCommandFactory.GetRichTextBoxCutSelectedTextCommand(richTextBox1));
            mnuItemPasteClickHandler = AddClickCommand(_richTextBoxPanelCommandFactory.GetRichTextBoxPasteTextCommand(richTextBox1));
            mnuItemSelectAllClickHandler = AddClickCommand(_richTextBoxPanelCommandFactory.GetRichTextBoxSelectAllCommand(richTextBox1));
            mnuItemInsertXmlFragmentClickHandler = AddClickCommand(_richTextBoxPanelCommandFactory.GetSelectFragmentCommand(richTextBox1));

            radContextMenu = new()
            {
                ImageList = _imageListService.ImageList,
                Items =
                {
                    mnuItemCopy,
                    mnuItemCut,
                    mnuItemPaste,
                    new RadMenuSeparatorItem(),
                    mnuItemSelectAll,
                    mnuItemInsertXmlFragment
                }
            };

            radContextMenuManager.SetRadContextMenu(richTextBox1, radContextMenu);
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemCopyClickHandler),
            nameof(mnuItemCutClickHandler),
            nameof(mnuItemPasteClickHandler),
            nameof(mnuItemSelectAllClickHandler),
            nameof(mnuItemInsertXmlFragmentClickHandler),
            nameof(radContextMenu))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            this.richTextBox1.WordWrap = false;
            this.richTextBox1.BorderStyle = BorderStyle.None;
            this.richTextBox1.ScrollBars = RichTextBoxScrollBars.Both;
            this.richTextBox1.Select(0, 0);

            this.richTextBox1.BackColor = ForeColorUtility.GetTextBoxBackColor(ThemeResolutionService.ApplicationThemeName);
            this.richTextBox1.ForeColor = ForeColorUtility.GetTextBoxForeColor(ThemeResolutionService.ApplicationThemeName);
            this.richTextBox1.Font = ForeColorUtility.GetDefaultFont(ThemeResolutionService.ApplicationThemeName);

            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            this.Disposed += RichTextBoxPanel_Disposed;
            this.MouseDown += RichTextBoxPanel_MouseDown;
            this.RichTextBox.MouseDown += RichTextBox_MouseDown;
            this.RichTextBox.TextChanged += RichTextBox_TextChanged;

            CreateContextMenu();
            AddClickCommands();
        }

        private void RemoveClickCommands()
        {
            mnuItemCopy.Click -= mnuItemCopyClickHandler;
            mnuItemCut.Click -= mnuItemCutClickHandler;
            mnuItemPaste.Click -= mnuItemPasteClickHandler;
            mnuItemSelectAll.Click -= mnuItemSelectAllClickHandler;
            mnuItemInsertXmlFragment.Click -= mnuItemInsertXmlFragmentClickHandler;
        }

        private void RemoveEventHanlders()
        {
            ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
            this.MouseDown -= RichTextBoxPanel_MouseDown;
            this.RichTextBox.MouseDown -= RichTextBox_MouseDown;
            this.RichTextBox.TextChanged -= RichTextBox_TextChanged;
        }

        private void SetContextMenuState()
        {
            mnuItemPaste.Enabled = richTextBox1.Enabled && Clipboard.GetText() != null;
            mnuItemCut.Enabled = !string.IsNullOrEmpty(richTextBox1.SelectedText);
            mnuItemCopy.Enabled = !string.IsNullOrEmpty(richTextBox1.SelectedText);
            mnuItemSelectAll.Enabled = richTextBox1.Enabled;
            mnuItemInsertXmlFragment.Enabled = richTextBox1.Enabled;
        }

        #region Event Handlers
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
            RemoveEventHanlders();
            RemoveClickCommands();
            radContextMenu.ImageList = null;
            radContextMenu.Dispose();
            radContextMenuManager.Dispose();
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args)
        {
            if (this.IsDisposed)
                return;

            this.richTextBox1.BackColor = ForeColorUtility.GetTextBoxBackColor(args.ThemeName);
            this.richTextBox1.ForeColor = ForeColorUtility.GetTextBoxForeColor(args.ThemeName);
            this.richTextBox1.Font = ForeColorUtility.GetDefaultFont(args.ThemeName);
        }
        #endregion Event Handlers
    }
}
