using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface ILayoutFieldControlButtons
    {
        void Layout(RadPanel panelButtons, IList<RadButton> buttons, bool performLayout = true);
    }
}
