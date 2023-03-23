using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Components
{
    internal class TitleBar : RadTitleBar
    {
        public TitleBar()
        {
            radMenuItemClose = new RadMenuItem(Strings.titleBarCloseContextMenuText)
            {
                Image = Properties.Resources.Close
            };
            radMenuItemClose.Click += CloseButton_Click;

            radContextMenu = new RadContextMenu()
            {
                Items = { new RadMenuSeparatorItem(), radMenuItemClose, new RadMenuSeparatorItem() }
            };

            radContextMenuManager = new RadContextMenuManager();
            radContextMenuManager.SetRadContextMenu(this, radContextMenu);

            ContextMenuStrip = null;

            Telerik.WinControls.ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            TitleBarElement.CloseButton.Click += new EventHandler(CloseButton_Click);
            this.Disposed += TitleBar_Disposed;

            this.Font = ForeColorUtility.GetDefaultFont(Telerik.WinControls.ThemeResolutionService.ApplicationThemeName);
        }

        private readonly RadContextMenuManager radContextMenuManager;
        private readonly RadContextMenu radContextMenu;
        private readonly RadMenuItem radMenuItemClose;

        public delegate void CloseClickHandler();
        public event CloseClickHandler? CloseClick;

        protected override void OnLoad(System.Drawing.Size desiredSize)
        {
            CanManageOwnerForm = false;
            TitleBarElement.MaximizeButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            TitleBarElement.MinimizeButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.Size = new System.Drawing.Size(this.Width, PerFontSizeConstants.TitleBarHeight);
            base.OnLoad(desiredSize);
        }

        #region Event Handlers
        private void CloseButton_Click(object? sender, EventArgs e)
        {
            CloseClick?.Invoke();
        }

        private void TitleBar_Disposed(object? sender, EventArgs e)
        {
            radContextMenuManager.Dispose();
            Telerik.WinControls.ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, Telerik.WinControls.ThemeChangedEventArgs args)
        {
            if (this.IsDisposed)
                return;

            this.Font = ForeColorUtility.GetDefaultFont(args.ThemeName);
            this.Size = new System.Drawing.Size(this.Width, PerFontSizeConstants.TitleBarHeight);
        }
        #endregion Event Handlers
    }
}
