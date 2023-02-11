using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal interface ILoadParameterControlsDictionary
    {
        void Load(IDictionary<string, ParameterControlSet> editControlsSet, IList<ParameterBase> parameters);
    }
}
