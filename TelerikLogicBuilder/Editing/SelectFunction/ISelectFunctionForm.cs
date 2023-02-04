using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction
{
    internal interface ISelectFunctionForm : IEditingForm
    {
        IDictionary<string, Function> FunctionDictionary { get; }
        IList<TreeFolder> TreeFolders { get; }
    }
}
