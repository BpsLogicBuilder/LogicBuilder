using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericParameter
{
    internal interface IConfigureGenericParameterControl : IConfigureParameterControl
    {
        RadDropDownList CmbGpGenericArgumentName { get; }
        RadLabel LblGpGenericArgumentName { get; }
    }
}
