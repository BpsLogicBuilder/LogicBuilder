using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IApplicationDropDownList
    {
        event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        ApplicationTypeInfo Application { get; }
        Point Location { set; }
        DockStyle Dock { set; }
    }
}