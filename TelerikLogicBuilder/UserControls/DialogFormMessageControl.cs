using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class DialogFormMessageControl : UserControl, IDialogFormMessageControl
    {
        private readonly IDialogFormMessageCommandFactory _dialogFormMessageCommandFactory;
        private readonly IImageListService _imageListService;

        private readonly RadMenuItem mnuItemCopy = new(Strings.mnuItemCopyToClipboardText) { ImageIndex = ImageIndexes.COPYIMAGEINDEX };
        private readonly RadMenuItem mnuItemOpen = new(Strings.mnuItemOpenInTextViewerText) { ImageIndex = ImageIndexes.OPENIMAGEINDEX };
        private readonly RadContextMenuManager radContextMenuManager;

        public DialogFormMessageControl(
            IDialogFormMessageCommandFactory dialogFormMessageCommandFactory,
            IImageListService imageListService)
        {
            _dialogFormMessageCommandFactory = dialogFormMessageCommandFactory;
            _imageListService = imageListService;
            radContextMenuManager = new RadContextMenuManager();
            InitializeComponent();
            Initialize();

            radLabelMessages.MouseDown += Control_MouseDown;
            radPanelMessages.MouseDown += Control_MouseDown;
            radGroupBoxMessages.MouseDown += Control_MouseDown;
            radPanelMessages.ClientSizeChanged += RadPanelMessages_ClientSizeChanged;
            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
        }

        private State _state;
        private EventHandler mnuItemCopyClickHandler;
        private EventHandler mnuItemOpenClickHandler;

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

        private static EventHandler AddClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void AddClickCommands()
        {
            RemoveClickCommands();
            mnuItemCopy.Click += mnuItemCopyClickHandler;
            mnuItemOpen.Click += mnuItemOpenClickHandler;
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemCopyClickHandler),
            nameof(mnuItemOpenClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void CreateContextMenu()
        {
            mnuItemCopyClickHandler = AddClickCommand(_dialogFormMessageCommandFactory.GetCopyToClipboardCommand(radLabelMessages));
            mnuItemOpenClickHandler = AddClickCommand(_dialogFormMessageCommandFactory.GetOpenInTextViewerCommand(radLabelMessages));

            RadContextMenu radContextMenu = new()
            {
                ImageList = _imageListService.ImageList,
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

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemCopyClickHandler),
            nameof(mnuItemOpenClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            ControlsLayoutUtility.LayoutGroupBox(this, radGroupBoxMessages);
            radPanelMessages.AutoScroll = true;
            ((BorderPrimitive)radPanelMessages.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

            this.Disposed += DialogFormMessageControl_Disposed;

            radLabelMessages.AutoSize = true;
            radLabelMessages.Padding = new Padding(10);
            radLabelMessages.MaximumSize = new Size(radPanelMessages.Size.Width, int.MaxValue);
            radLabelMessages.TextWrap = true;
            radLabelMessages.TextAlignment = ContentAlignment.TopLeft;

            ClearMessage();
            CreateContextMenu();
            AddClickCommands();
        }

        private void RemoveClickCommands()
        {
            mnuItemCopy.Click -= mnuItemCopyClickHandler;
            mnuItemOpen.Click -= mnuItemOpenClickHandler;
        }

        private void RemoveEventHandlers()
        {
            radLabelMessages.MouseDown -= Control_MouseDown;
            radPanelMessages.MouseDown -= Control_MouseDown;
            radGroupBoxMessages.MouseDown -= Control_MouseDown;
            radPanelMessages.ClientSizeChanged -= RadPanelMessages_ClientSizeChanged;
            ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
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
            RemoveClickCommands();
            RemoveEventHandlers();
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
