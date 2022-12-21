using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable
{
    internal interface IConfigureLiteralListVariableControl : IConfigureVariableControl
    {
        RadDropDownList CmbLiteralType { get; }
        RadLabel LblElementControl { get; }
        RadDropDownList CmbElementControl { get; }
        RadLabel LblPropertySource { get; }
        AutoCompleteRadDropDownList CmbPropertySource { get; }
        RadTreeView TreeView { get; }
        XmlDocument XmlDocument { get; }
    }
}
