using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor
{
    internal interface IConfigureConstructorControl : IConfigureConstructorsTreeNodeControl
    {
        RadLabel LblConstructorName { get; }
        RadTextBox TxtConstructorName { get; }
        RadTreeView TreeView { get; }
        XmlDocument XmlDocument { get; }

        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
        void ValidateFields();
    }
}
