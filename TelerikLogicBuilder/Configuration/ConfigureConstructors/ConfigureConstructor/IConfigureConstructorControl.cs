using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System.Collections.Generic;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor
{
    internal interface IConfigureConstructorControl : IConfigureConstructorsTreeNodeControl
    {
        ConstructorHelperStatus? HelperStatus { get; }
        RadLabel LblConstructorName { get; }
        RadTextBox TxtConstructorName { get; }
        RadTreeView TreeView { get; }
        XmlDocument XmlDocument { get; }
        IConfigureConstructorsForm Form { get; }
        IDictionary<string, Constructor> ConstructorsDictionary { get; }

        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
        void ValidateFields();
        void ValidateXmlDocument();
    }
}
