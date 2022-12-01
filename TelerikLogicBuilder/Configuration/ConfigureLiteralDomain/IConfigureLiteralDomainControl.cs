using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain
{
    internal interface IConfigureLiteralDomainControl
    {
        Point Location { set; }
        DockStyle Dock { set; }

        IRadListBoxManager<LiteralDomainItem> RadListBoxManager { get; }
        RadTextBox TxtDomainItem { get; }
        RadListControl ListBox { get; }
        Type Type { get; }

        IList<string> GetDomainItems();
        void SetDomainItems(IList<string> domainItems);
    }
}
