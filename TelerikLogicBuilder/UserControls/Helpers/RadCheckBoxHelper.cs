using System.Windows.Forms;
using Telerik.WinControls.Layouts;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers
{
    internal class RadCheckBoxHelper : IRadCheckBoxHelper
    {
        public void SetLabelMargin(RadCheckBox radCheckBox, int left = 5, int top = 0, int right = 0, int bottom = 0)
        {
            ((ImageAndTextLayoutPanel)radCheckBox.RootElement.Children[0].Children[1].Children[0]).Margin = new Padding(left, top, right, bottom);
        }
    }
}
