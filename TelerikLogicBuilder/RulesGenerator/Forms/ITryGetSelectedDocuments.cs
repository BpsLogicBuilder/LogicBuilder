using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal interface ITryGetSelectedDocuments
    {
        bool Try(out IList<string> selectedDocuments, IWin32Window dialogOwner);
    }
}
