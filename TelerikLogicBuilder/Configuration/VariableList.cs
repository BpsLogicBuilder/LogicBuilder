using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class VariableList
    {
        public VariableList(IDictionary<string, VariableBase> variables, TreeFolder variablesTreeFolder)
        {
            Variables = variables;
            VariablesTreeFolder = variablesTreeFolder;
        }

        internal IDictionary<string, VariableBase> Variables { get; }
        internal TreeFolder VariablesTreeFolder { get; }
    }
}
