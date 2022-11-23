using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument
{
    internal interface IConfigureGenericLiteralListArgumentControl : IConfigurationXmlElementControl
    {
        RadTreeView TreeView { get; }
        HelperButtonTextBox TxtListLpDomain { get; }
        HelperButtonTextBox TxtListLpDefaultValue { get; }
        XmlDocument XmlDocument { get; }
    }
}
