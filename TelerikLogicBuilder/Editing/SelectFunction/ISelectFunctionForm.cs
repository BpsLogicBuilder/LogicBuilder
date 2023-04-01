using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction
{
    internal interface ISelectFunctionForm : IApplicationForm
    {
        IDictionary<string, Function> FunctionDictionary { get; }
        string FunctionName { get; }
        IList<TreeFolder> TreeFolders { get; }

        void SetFunction(string functionName);
    }
}
