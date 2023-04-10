using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions
{
    internal interface IEditVoidFunctionControl : IDataGraphEditingHost
    {
        event EventHandler? Changed;
        HelperButtonDropDownList CmbSelectFunction { get; }
        IEditingControl? CurrentEditingControl { get; }
        string VisibleText { get; }
        void ClearInputControls();
        void UpdateInputControls(string xmlString);
        void SetFunctionName(string functionName);
    }
}
