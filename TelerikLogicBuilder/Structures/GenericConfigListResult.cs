using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class GenericConfigListResult
    {
        public GenericConfigListResult(DialogResult dialogResult, IList<GenericConfigBase> genericConfigs)
        {
            DialogResult = dialogResult;
            GenericConfigs = genericConfigs;
        }

        public DialogResult DialogResult { get; set; }
        public IList<GenericConfigBase> GenericConfigs { get; set; }
    }
}
