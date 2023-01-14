using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter
{
    internal interface IConfigureLiteralListParameterControl : IConfigureParameterControl
    {
        AutoCompleteRadDropDownList CmbListLpPropertySource { get; }
        RadDropDownList CmbListLpElementControl { get; }
        RadDropDownList CmbListLpLiteralType { get; }
        RadDropDownList CmbListLpPropertySourceParameter { get; }
        RadLabel LblListLpElementControl { get; }
        RadLabel LblListLpName { get; }
        RadLabel LblListLpPropertySource { get; }
        RadLabel LblListLpPropertySourceParameter { get; }
        RadTextBox TxtListLpName { get; }
    }
}
