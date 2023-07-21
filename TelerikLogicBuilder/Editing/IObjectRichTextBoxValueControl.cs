using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IObjectRichTextBoxValueControl : IValueControl
    {
        ApplicationTypeInfo Application { get; }
        Type? AssignedTo { get; }
        IObjectRichTextBox RichTextBox { get; }

        void ClearMessage();
        void RequestDocumentUpdate();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
        void UpdateXmlElement(string innerXml);
    }
}
