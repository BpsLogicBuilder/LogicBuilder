using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper
{
    internal interface IConfigureConstructorsHelperForm : IApplicationForm
    {
        ApplicationDropDownList CmbApplication { get; }
        AutoCompleteRadDropDownList CmbClass { get; }
        Constructor Constructor { get; }
        ICollection<Constructor> ChildConstructors { get; }
        RadTreeView TreeView { get; }

        void ClearTreeView();
        void ValidateOk();
    }
}
