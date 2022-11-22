using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument
{
    internal interface IConfigureGenericLiteralArgumentControl : IXmlElementControl
    {
        RadTreeView TreeView { get; }
        HelperButtonTextBox TxtLpDomain { get; }
        XmlDocument XmlDocument { get; }
    }
}
