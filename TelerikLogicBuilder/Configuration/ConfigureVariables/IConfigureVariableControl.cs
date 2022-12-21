using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables
{
    internal interface IConfigureVariableControl : IConfigurationXmlElementControl
    {
        RadLabel LblName { get; }
        RadTextBox TxtName { get; }
        RadTextBox TxtMemberName { get; }
        RadDropDownList CmbVariableCategory { get; }
        RadLabel LblCastVariableAs { get; }
        RadTextBox TxtCastVariableAs { get; }
        RadLabel LblTypeName { get; }
        RadTextBox TxtTypeName { get; }
        RadTextBox TxtReferenceName { get; }
        RadDropDownList CmbReferenceDefinition { get; }
        RadTextBox TxtCastReferenceAs { get; }
        RadDropDownList CmbReferenceCategory { get; }

        ApplicationTypeInfo Application { get; }
        IDictionary<string, VariableBase> VariablesDictionary { get; }
        HashSet<string> VariableNames { get; }

        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
        void ValidateXmlDocument();
    }
}
