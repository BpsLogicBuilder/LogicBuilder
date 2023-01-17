using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments
{
    internal interface IEditGenericArgumentsControl
    {
        Point Location { set; }
        DockStyle Dock { set; }

        IRadListBoxManager<GenericArgumentName> RadListBoxManager { get; }
        RadTextBox TxtArgument { get; }
        RadListControl ListBox { get; }

        IList<string> GetArguments();
        void SetArguments(IList<string> arguments);
    }
}
