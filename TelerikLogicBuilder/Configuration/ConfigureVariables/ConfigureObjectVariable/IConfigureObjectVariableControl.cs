using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectVariable
{
    internal interface IConfigureObjectVariableControl : IConfigureVariableControl
    {
        AutoCompleteRadDropDownList CmbObjectType { get; }
        RadLabel LblObjectType { get; }
    }
}
