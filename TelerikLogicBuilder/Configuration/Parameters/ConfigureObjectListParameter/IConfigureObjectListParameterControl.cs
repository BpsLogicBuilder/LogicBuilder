using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectListParameter
{
    internal interface IConfigureObjectListParameterControl : IConfigureParameterControl
    {
        RadLabel LblListCpName { get; }
        RadTextBox TxtListCpName { get; }
    }
}
