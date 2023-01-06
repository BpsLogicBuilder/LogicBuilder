using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.CustomConfiguration
{
    internal interface IIntellisenseVariableConfigurationControl : IIntellisenseConfigurationControl
    {
        ApplicationTypeInfo Application { get; }
        AutoCompleteRadDropDownList CmbCastVariableAs { get; }

        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
    }
}
