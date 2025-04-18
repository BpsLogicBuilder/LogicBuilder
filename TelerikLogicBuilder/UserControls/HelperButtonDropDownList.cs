﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    public partial class HelperButtonDropDownList : UserControl
    {
        private RadButton radButtonHelper;
        private RadDropDownList radDropDownList;

        public HelperButtonDropDownList()
        {
            InitializeComponent();
            Initialize();
        }

        public RadDropDownList DropDownList => radDropDownList;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Text { get => radDropDownList.Text; set => radDropDownList.Text = value; }

        public event EventHandler<EventArgs>? ButtonClick;

        public event EventHandler? Changed;

        public void SetErrorBackColor()
        {
            Color errorColor = ForeColorUtility.GetGroupBoxBorderErrorColor();
            SetPanelBorderForeColor(radPanelButton, errorColor);
            SetPanelBorderForeColor(radPanelDropDownList, errorColor);
            ShowPanelBorder(radPanelDropDownList);
            ShowPanelBorder(radPanelButton);
        }

        public void SetNormalBackColor()
        {
            Color normalColor = ForeColorUtility.GetGroupBoxBorderColor(ThemeResolutionService.ApplicationThemeName);
            SetPanelBorderForeColor(radPanelButton, normalColor);
            SetPanelBorderForeColor(radPanelDropDownList, normalColor);
            CollapsePanelBorder(radPanelDropDownList);
            CollapsePanelBorder(radPanelButton);
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        [MemberNotNull(nameof(radButtonHelper))]
        [MemberNotNull(nameof(radDropDownList))]
        private void Initialize()
        {
            InitializeButton();
            InitializeDropDownList();
            CollapsePanelBorder(radPanelDropDownList);
            CollapsePanelBorder(radPanelButton);

            ControlsLayoutUtility.SetDropDownListPadding(radDropDownList);
            Disposed += HelperButtonDropDownList_Disposed;
            radButtonHelper.Click += RadButtonHelper_Click;
            radDropDownList.TextChanged += RadDropDownList_TextChanged;
        }

        [MemberNotNull(nameof(radButtonHelper))]
        private void InitializeButton()
        {
            radButtonHelper = new()
            {
                Name = "btnHelper",
                ImageAlignment = ContentAlignment.MiddleCenter,
                Padding = new Padding(0),
                Margin = new Padding(1, 0, 1, 0),
                ImageIndex = ImageIndexes.MOREIMAGEINDEX,
                Dock = DockStyle.Fill,
                Image = Properties.Resources.more
            };

            ((ISupportInitialize)this.radPanelButton).BeginInit();
            this.radPanelButton.SuspendLayout();
            this.radPanelButton.Padding = new Padding(0, 1, 1, 1);
            this.radPanelButton.Size = new Size(PerFontSizeConstants.CommandButtonWidth, this.radPanelButton.Height);

            ((ISupportInitialize)radButtonHelper).BeginInit();

            radButtonHelper.Location = new Point(43, 69);
            radButtonHelper.Name = "radButtonHelper";
            radButtonHelper.Size = new Size(110, 24);
            radButtonHelper.TabIndex = 1;
            ((ISupportInitialize)radButtonHelper).EndInit();

            this.radPanelButton.Controls.Add(radButtonHelper);
            ((ISupportInitialize)this.radPanelButton).EndInit();
            this.radPanelButton.ResumeLayout(true);
        }

        [MemberNotNull(nameof(radDropDownList))]
        private void InitializeDropDownList()
        {
            this.radDropDownList = new RadDropDownList();

            ((ISupportInitialize)this.radPanelDropDownList).BeginInit();
            this.radPanelDropDownList.SuspendLayout();
            this.radPanelDropDownList.Padding = new Padding(1, 1, 0, 1);

            ((ISupportInitialize)this.radDropDownList).BeginInit();
            this.radDropDownList.Dock = DockStyle.Fill;
            this.radDropDownList.AutoSize = false;
            this.radDropDownList.DropDownAnimationEnabled = true;
            this.radDropDownList.Location = new Point(0, 0);
            this.radDropDownList.Name = "radDropDownList";
            this.radDropDownList.Size = new Size(350, 28);
            this.radDropDownList.TabIndex = 0;
            ((ISupportInitialize)this.radDropDownList).EndInit();

            this.radPanelDropDownList.Controls.Add(radDropDownList);
            ((ISupportInitialize)this.radPanelDropDownList).EndInit();
            this.radPanelDropDownList.ResumeLayout(true);
        }

        private void RemoveEventHandlers()
        {
            radButtonHelper.Click -= RadButtonHelper_Click;
            radDropDownList.TextChanged -= RadDropDownList_TextChanged;
        }

        private static void SetPanelBorderForeColor(RadPanel radPanel, Color color)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).ForeColor = color;

        private static void ShowPanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Visible;

        #region Event Handlers
        private void HelperButtonDropDownList_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
        }

        private void RadButtonHelper_Click(object? sender, EventArgs e)
        {
            ButtonClick?.Invoke(this, e);
        }

        private void RadDropDownList_TextChanged(object? sender, EventArgs e)
        {
            if (radDropDownList.Disposing || radDropDownList.IsDisposed)
                return;

            Changed?.Invoke(this, e);
        }
        #endregion Event Handlers
    }
}
