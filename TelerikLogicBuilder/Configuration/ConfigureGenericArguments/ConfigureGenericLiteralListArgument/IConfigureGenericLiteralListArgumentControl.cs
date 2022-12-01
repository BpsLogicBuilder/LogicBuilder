using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument
{
    internal interface IConfigureGenericLiteralListArgumentControl : IConfigurationXmlElementControl
    {
        RadTreeView TreeView { get; }
        RadDropDownList CmbListLpLiteralType { get; }
        XmlDocument XmlDocument { get; }
    }
}
