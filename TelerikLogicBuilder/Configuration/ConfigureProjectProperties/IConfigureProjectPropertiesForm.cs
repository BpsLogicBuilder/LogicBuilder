using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties
{
    internal interface IConfigureProjectPropertiesForm
    {
        RadTreeView TreeView { get; }
        XmlDocument XmlDocument { get; }

        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
        void ValidateXmlDocument();
    }
}