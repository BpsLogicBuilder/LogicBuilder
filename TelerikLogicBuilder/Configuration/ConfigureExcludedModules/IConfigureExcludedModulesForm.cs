using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules
{
    internal interface IConfigureExcludedModulesForm : IForm
    {
        IList<string> ExcludedModules { get; }
        RadListControl ListControl { get; }
        RadTreeView TreeView { get; }
        RadButton OkButton { get; }
    }
}