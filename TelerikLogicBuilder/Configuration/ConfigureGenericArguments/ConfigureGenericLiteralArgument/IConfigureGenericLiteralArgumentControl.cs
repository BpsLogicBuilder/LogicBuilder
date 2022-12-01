using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument
{
    internal interface IConfigureGenericLiteralArgumentControl : IConfigurationXmlElementControl
    {
        RadTreeView TreeView { get; }
        RadDropDownList CmbLpLiteralType { get; }
        XmlDocument XmlDocument { get; }
    }
}
