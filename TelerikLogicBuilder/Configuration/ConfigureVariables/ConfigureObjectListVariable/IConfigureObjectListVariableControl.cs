using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectListVariable
{
    internal interface IConfigureObjectListVariableControl : IConfigureVariableControl
    {
        AutoCompleteRadDropDownList CmbObjectType { get; }
        RadLabel LblObjectType { get; }
    }
}
