using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter
{
    internal interface IConfigureLiteralParameterControl : IConfigureParameterControl
    {
        RadDropDownList CmbLpControl { get; }
        RadDropDownList CmbLpLiteralType { get; }
        AutoCompleteRadDropDownList CmbLpPropertySource { get; }
        RadDropDownList CmbLpPropertySourceParameter { get; }
        RadLabel LblLpControl { get; }
        RadLabel LblLpDefaultValue { get; }
        RadLabel LblLpName { get; }
        RadLabel LblLpPropertySource { get; }
        RadLabel LblLpPropertySourceParameter { get; }
        RadTextBox TxtLpDefaultValue { get; }
        RadTextBox TxtLpName { get; }
    }
}
