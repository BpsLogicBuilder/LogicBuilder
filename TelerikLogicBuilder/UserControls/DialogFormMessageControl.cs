using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class DialogFormMessageControl : UserControl
    {
        private readonly IImageListService _imageImageListService;
        private readonly Func<RadLabel, CopyToClipboardCommand> _getCopyToClipboardCommand;
        private readonly Func<RadLabel, OpenInTextViewerCommand> _getOpenInTextViewerCommand;

        private readonly RadMenuItem mnuItemCopy = new(Strings.mnuItemCopyToClipboardText) { ImageIndex = ImageIndexes.COPYIMAGEINDEX };
        private readonly RadMenuItem mnuItemOpen = new(Strings.mnuItemOpenInTextViewerText) { ImageIndex = ImageIndexes.OPENIMAGEINDEX };
        private readonly RadContextMenuManager radContextMenuManager;

        public DialogFormMessageControl(IImageListService imageImageListService, Func<RadLabel, CopyToClipboardCommand> getCopyToClipboardCommand, Func<RadLabel, OpenInTextViewerCommand> getOpenInTextViewerCommand)
        {
            _imageImageListService = imageImageListService;
            _getCopyToClipboardCommand = getCopyToClipboardCommand;
            _getOpenInTextViewerCommand = getOpenInTextViewerCommand;
            radContextMenuManager = new RadContextMenuManager();
            InitializeComponent();
            Initialize();

            radLabelMessages.MouseDown += Control_MouseDown;
            radPanelMessages.MouseDown += Control_MouseDown;
            radGroupBoxMessages.MouseDown += Control_MouseDown;
            radPanelMessages.ClientSizeChanged += RadPanelMessages_ClientSizeChanged;
            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            this.Disposed += DialogFormMessageControl_Disposed;
        }

        private State _state;

        #region Methods
        public void ClearMessage()
        {
            _state = State.Ok;
            SetColorOk(ThemeResolutionService.ApplicationThemeName);
            radGroupBoxMessages.Text = Strings.dialogFormMessageControlMessagesGroupBoxHeader;
            radLabelMessages.Text = string.Empty;
        }

        public void SetErrorMessage(string message)
        {
            _state = State.Error;
            SetErrorColor(ThemeResolutionService.ApplicationThemeName);
            radGroupBoxMessages.Text = Strings.dialogFormMessageControlErrorsGroupBoxHeader;
            radLabelMessages.Text = message;
        }

        public void SetMessage(string message, string title = "")
        {
            _state = State.Ok;
            SetColorOk(ThemeResolutionService.ApplicationThemeName);
            radGroupBoxMessages.Text = string.IsNullOrEmpty(title) 
                ? Strings.dialogFormMessageControlMessagesGroupBoxHeader 
                : title;
            radLabelMessages.Text = message;
        }

        private static void AddClickCommand(RadMenuItem radMenuItem, IClickCommand command)
        {
            radMenuItem.Click += (sender, args) => command.Execute();
        }

        private void CreateContextMenu()
        {
            AddClickCommand(mnuItemCopy, _getCopyToClipboardCommand(radLabelMessages));
            AddClickCommand(mnuItemOpen, _getOpenInTextViewerCommand(radLabelMessages));

            RadContextMenu radContextMenu = new()
            {
                ImageList = _imageImageListService.ImageList,
                Items =
                {
                    new RadMenuSeparatorItem(),
                    mnuItemCopy,
                    new RadMenuSeparatorItem(),
                    mnuItemOpen,
                    new RadMenuSeparatorItem()
                }
            };

            radContextMenuManager.SetRadContextMenu(this.radLabelMessages, radContextMenu);
            radContextMenuManager.SetRadContextMenu(this.radPanelMessages, radContextMenu);
            radContextMenuManager.SetRadContextMenu(this.radGroupBoxMessages, radContextMenu);

            this.radLabelMessages.ContextMenuStrip = null;
            this.radPanelMessages.ContextMenuStrip = null;
            this.radGroupBoxMessages.ContextMenuStrip = null;
        }

        private void Initialize()
        {
            radPanelMessages.AutoScroll = true;
            ((BorderPrimitive)radPanelMessages.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

            radLabelMessages.AutoSize = true;
            radLabelMessages.Padding = new Padding(10);
            radLabelMessages.MaximumSize = new Size(radPanelMessages.Size.Width, int.MaxValue);
            radLabelMessages.TextWrap = true;
            radLabelMessages.TextAlignment = ContentAlignment.TopLeft;

            ClearMessage();
            CreateContextMenu();
        }

        private void SetTheme(string themeName)
        {
            switch (_state)
            {
                case State.Ok:
                    SetColorOk(themeName);
                    break;
                case State.Error:
                    SetErrorColor(themeName);
                    break;
                default:
                    throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{7E305565-C94F-4625-A891-3D3964E5F3A3}"));
            }
        }

        private void SetColorOk(string themeName)
        {
            ((BorderPrimitive)radGroupBoxMessages.GroupBoxElement.Content.Children[1]).ForeColor = ForeColorUtility.GetGroupBoxBorderColor(themeName);
            Color color = ForeColorUtility.GetOkForeColor(themeName);
            radLabelMessages.ForeColor = color;
            radGroupBoxMessages.ForeColor = color;
        }

        private void SetContextMenuState()
        {
            Set(!string.IsNullOrEmpty(radLabelMessages.Text));
            void Set(bool enable)
            {
                mnuItemCopy.Enabled = enable;
                mnuItemOpen.Enabled = enable;
            }
        }

        private void SetErrorColor(string themeName)
        {
            ((BorderPrimitive)radGroupBoxMessages.GroupBoxElement.Content.Children[1]).ForeColor = ForeColorUtility.GetGroupBoxBorderErrorColor();
            Color color = ForeColorUtility.GetErrorForeColor(themeName);
            radLabelMessages.ForeColor = color;
            radGroupBoxMessages.ForeColor = color;
        }
        #endregion Methods

        #region Event Handlers
        private void Control_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                return;

            SetContextMenuState();
        }

        private void DialogFormMessageControl_Disposed(object? sender, EventArgs e)
        {
            ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
            radContextMenuManager.Dispose();
        }

        private void RadPanelMessages_ClientSizeChanged(object? sender, EventArgs e)
        {
            radLabelMessages.MaximumSize = new Size(radPanelMessages.Size.Width, int.MaxValue);
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args)
        {
            if (this.IsDisposed)
                return;

            SetTheme(args.ThemeName);
        }
        #endregion Event Handlers

        private enum State
        {
            Ok,
            Error
        }
    }
}
