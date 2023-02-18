using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal interface IObjectRichTextBoxValueControl : IValueControl
    {
        ApplicationTypeInfo Application { get; }
        Type? AssignedTo { get; }
        ObjectRichTextBox RichTextBox { get; }
        void RequestDocumentUpdate();
        void ResetControl();
        void UpdateXmlElement(string innerXml);
    }
}
