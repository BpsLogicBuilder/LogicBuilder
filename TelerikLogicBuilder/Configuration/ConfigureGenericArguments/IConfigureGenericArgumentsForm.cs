using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments
{
    internal interface IConfigureGenericArgumentsForm : IApplicationForm
    {
        IList<string> ConfiguredGenericArgumentNames { get; }
        IList<ParameterBase> MemberParameters { get; }
        RadTreeView TreeView { get; }
        XmlDocument XmlDocument { get; }

        void ValidateXmlDocument();
    }
}
