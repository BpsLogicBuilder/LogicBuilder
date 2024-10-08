﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IValueControl
    {
        event EventHandler? Changed;

        DockStyle Dock { set; }
        bool IsEmpty { get; }
        Point Location { set; }
        Padding Margin { set; }
        string MixedXml { get; }
        string VisibleText { get; }
        XmlElement? XmlElement { get; }

        void Focus();
        void HideControls();
        void InvokeChanged();
        void ResetControl();
        void SetErrorBackColor();
        void SetNormalBackColor();
        void SetToolTipHelp(string toolTipText);
        void ShowControls();
        void Update(XmlElement xmlElement);
    }
}
