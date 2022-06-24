using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers
{
    internal class DocumentExplorerErrorsList : List<string>
    {
        internal delegate void ErrorCountChangedHandler(int errorCount);
        internal event ErrorCountChangedHandler? ErrorCountChanged;

        internal new void Add(string errorMessage)
        {
            base.Add(errorMessage);
            ErrorCountChanged?.Invoke(this.Count);
        }

        internal new void Clear()
        {
            base.Clear();
            ErrorCountChanged?.Invoke(this.Count);
        }
    }
}
