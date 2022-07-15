using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    public partial class DialogFormMessageControl : UserControl
    {
        public DialogFormMessageControl()
        {
            InitializeComponent();
            Initialize();

            radPanelMessages.ClientSizeChanged += RadPanelMessages_ClientSizeChanged;
            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
        }

        private State _state;

        #region Methods
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

        public void ClearMessage()
        {
            _state = State.Ok;
            radLabelMessages.Text = string.Empty;
            radGroupBoxMessages.Text = string.Empty;
        }

        private void Initialize()
        {
            radPanelMessages.AutoScroll = true;
            radLabelMessages.AutoSize = true;
            radLabelMessages.Padding = new Padding(10);
            radLabelMessages.MaximumSize = new Size(radPanelMessages.Size.Width, int.MaxValue);
            radLabelMessages.TextWrap = true;
            radLabelMessages.TextAlignment = ContentAlignment.TopLeft;

            ClearMessage();
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
            Color color = ForeColorUtility.GetOkForeColor(themeName);
            radLabelMessages.ForeColor = color;
            radGroupBoxMessages.ForeColor = color;
        }

        private void SetErrorColor(string themeName)
        {
            Color color = ForeColorUtility.GetErrorForeColor(themeName);
            radLabelMessages.ForeColor = color;
            radGroupBoxMessages.ForeColor = color;
        }
        #endregion Methods

        #region Event Handlers
        private void RadPanelMessages_ClientSizeChanged(object? sender, EventArgs e)
        {
            radLabelMessages.MaximumSize = new Size(radPanelMessages.Size.Width, int.MaxValue);
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args)
        {
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
