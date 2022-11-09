using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Forms
{
    internal interface IConfigureExcludedModules
    {
        IList<string> ExcludedModules { get; }
        RadListControl ListControl { get; }
        RadTreeView TreeView { get; }
        RadButton OkButton { get; }
    }
}