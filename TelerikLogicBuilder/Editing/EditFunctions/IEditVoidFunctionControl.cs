using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions
{
    internal interface IEditVoidFunctionControl : IDataGraphEditingHost
    {
        event EventHandler? Changed;
        HelperButtonDropDownList CmbSelectFunction { get; }
        IEditingControl? CurrentEditingControl { get; }
        IDictionary<string, Function> FunctionDictionary { get; }
        IList<TreeFolder> TreeFolders { get; }
        void ClearInputControls();
        void UpdateInputControls(string xmlString);
        void SetFunctionName(string functionName);
    }
}
