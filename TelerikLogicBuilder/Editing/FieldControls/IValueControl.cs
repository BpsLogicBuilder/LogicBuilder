using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal interface IValueControl
    {
        event EventHandler? Changed;

        DockStyle Dock { set; }
        bool IsEmpty { get; }
        Point Location { set; }
        string MixedXml { get; }
        string VisibleText { get; }

        void Focus();
        void InvokeChanged();
        void HideControls();
        void SetErrorBackColor();
        void ShowControls();
        void SetNormalBackColor();
        void SetToolTipHelp(string toolTipText);
        void Update(XmlElement xmlElement);
    }
}
