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

            TitleBarElement.CloseButton.Click += new EventHandler(CloseButton_Click);
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

            TitleBarElement.TitleBarFill.BackColor4 = System.Drawing.SystemColors.ActiveCaption;
            TitleBarElement.TitlePrimitive.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            base.OnLoad(desiredSize);
        }

        void CloseButton_Click(object? sender, EventArgs e)
        {
            CloseClick?.Invoke();
        }
    }
}
