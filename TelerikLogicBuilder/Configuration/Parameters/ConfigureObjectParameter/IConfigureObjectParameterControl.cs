using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectParameter
{
    internal interface IConfigureObjectParameterControl : IConfigureParameterControl
    {
        RadLabel LblCpName { get; }
        RadTextBox TxtCpName { get; }
    }
}
