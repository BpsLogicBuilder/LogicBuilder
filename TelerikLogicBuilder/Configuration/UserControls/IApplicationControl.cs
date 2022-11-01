using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls
{
    internal interface IApplicationControl : IXmlElementControl
    {
        RadTreeView TreeView { get; }
        HelperButtonTextBox TxtActivityAssembly { get; }
        HelperButtonTextBox TxtActivityAssemblyPath { get; }
        HelperButtonTextBox TxtResourceFilesDeployment { get; }
        HelperButtonTextBox TxtRulesDeployment { get; }
        XmlDocument XmlDocument { get; }
    }
}