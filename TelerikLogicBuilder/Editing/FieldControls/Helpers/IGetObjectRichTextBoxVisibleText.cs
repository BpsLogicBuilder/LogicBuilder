using ABIS.LogicBuilder.FlowBuilder.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IGetObjectRichTextBoxVisibleText
    {
        string GetVisibleText(string xmlString, ApplicationTypeInfo application);
        string RefreshVisibleTexts(string xmlString, ApplicationTypeInfo application);
    }
}
