using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction
{
    internal interface IEditConditionFunctionControl : IDataGraphEditingHost
    {
        event EventHandler? Changed;
        HelperButtonDropDownList CmbSelectFunction { get; }
        IEditingControl? CurrentEditingControl { get; }
        void ClearInputControls();
        void UpdateInputControls(string xmlString);
        void SetFunctionName(string functionName);
    }
}
