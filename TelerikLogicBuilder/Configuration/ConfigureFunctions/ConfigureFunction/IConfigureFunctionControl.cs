using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction
{
    internal interface IConfigureFunctionControl : IConfigureFunctionsTreeNodeControl
    {
        RadDropDownList CmbFunctionCategory { get; }
        RadDropDownList CmbReferenceCategory { get; }
        RadDropDownList CmbReferenceDefinition { get; }
        RadLabel LblFunctionName { get; }
        RadLabel LblMemberName { get; }
        RadLabel LblTypeName { get; }
        RadTextBox TxtCastReferenceAs { get; }
        RadTextBox TxtFunctionName { get; }
        RadTextBox TxtMemberName { get; }
        RadTextBox TxtReferenceName { get; }
        AutoCompleteRadDropDownList TxtTypeName { get; }
        RadTreeView TreeView { get; }
        XmlDocument XmlDocument { get; }

        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
        void ValidateFields();
        void ValidateXmlDocument();
    }
}
