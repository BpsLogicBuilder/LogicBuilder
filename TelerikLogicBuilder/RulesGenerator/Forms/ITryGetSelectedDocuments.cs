using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal interface ITryGetSelectedDocuments
    {
        bool Try(out IList<string> selectedDocuments);
    }
}
