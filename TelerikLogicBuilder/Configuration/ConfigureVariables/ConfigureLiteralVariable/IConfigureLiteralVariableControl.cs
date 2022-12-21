using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable
{
    internal interface IConfigureLiteralVariableControl : IConfigureVariableControl
    {
        RadDropDownList CmbLiteralType { get; }
        RadLabel LblControl { get; }
        RadDropDownList CmbControl { get; }
        RadLabel LblPropertySource { get; }
        AutoCompleteRadDropDownList CmbPropertySource { get; }
        RadLabel LblDefaultValue { get; }
        RadTextBox TxtDefaultValue { get; }
        RadTreeView TreeView { get; }
        XmlDocument XmlDocument { get; }
    }
}
