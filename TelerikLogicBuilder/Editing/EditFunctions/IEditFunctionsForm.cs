using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions
{
    internal interface IEditFunctionsForm : IShapeEditForm, IApplicationForm
    {
        IEditingControl? CurrentEditingControl { get; }
        IEditVoidFunctionControl EditVoidFunctionControl { get; }
        IDictionary<string, Function> FunctionDictionary { get; }
        IRadListBoxManager<IFunctionListBoxItem> RadListBoxManager { get; }
        IList<TreeFolder> TreeFolders { get; }
        void UpdateFunctionsList(string xmlString);
    }
}
