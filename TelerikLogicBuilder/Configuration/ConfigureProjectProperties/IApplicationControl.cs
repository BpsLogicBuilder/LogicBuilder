using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties
{
    internal interface IApplicationControl : IConfigurationXmlElementControl
    {
        RadTreeView TreeView { get; }
        HelperButtonTextBox TxtActivityAssembly { get; }
        HelperButtonTextBox TxtActivityAssemblyPath { get; }
        HelperButtonTextBox TxtResourceFilesDeployment { get; }
        HelperButtonTextBox TxtRulesDeployment { get; }
        XmlDocument XmlDocument { get; }
    }
}