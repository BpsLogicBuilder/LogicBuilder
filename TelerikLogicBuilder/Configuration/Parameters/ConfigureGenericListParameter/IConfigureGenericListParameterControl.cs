using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericListParameter
{
    internal interface IConfigureGenericListParameterControl : IConfigureParameterControl
    {
        RadDropDownList CmbListGpGenericArgumentName { get; }
        RadLabel LblListGpGenericArgumentName { get; }
    }
}
