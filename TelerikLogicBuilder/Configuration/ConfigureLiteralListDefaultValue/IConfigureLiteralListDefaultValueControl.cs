using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue
{
    internal interface IConfigureLiteralListDefaultValueControl
    {
        IRadListBoxManager<LiteralListDefaultValueItem> RadListBoxManager { get; }
        RadTextBox TxtDefaultValueItem { get; }
        RadListControl ListBox { get; }
        Type Type { get; }

        IList<string> GetDefaultValueItems();
        void SetDefaultValueItems(IList<string> domainItems);
    }
}
