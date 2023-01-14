using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters
{
    internal interface IConfigureParameterControl : IConfigureConstructorsTreeNodeControl
    {
        RadTreeView TreeView { get; }
        XmlDocument XmlDocument { get; }

        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
        void ValidateFields();
        void ValidateXmlDocument();
    }
}
