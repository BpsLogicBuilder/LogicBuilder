using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects
{
    internal interface IConfigureConnectorObjectsControl
    {
        AutoCompleteRadDropDownList TxtType { get; }
        IRadListBoxManager<ConnectorObjectListBoxItem> RadListBoxManager { get; }
        RadListControl ListBox { get; }

        Point Location { set; }
        DockStyle Dock { set; }

        IList<string> GetObjectTypes();
        void SetObjectTypes(IList<string> objectTypes);
    }
}
