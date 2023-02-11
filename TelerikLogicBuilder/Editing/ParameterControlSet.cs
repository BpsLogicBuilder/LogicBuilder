using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal class ParameterControlSet
    {
        public ParameterControlSet(
            RadLabel imageLabel,
            RadCheckBox chkInclude,
            IValueControl valueControl)
        {
            ImageLabel = imageLabel;
            ChkInclude = chkInclude;
            ValueControl = valueControl;
        }

        public RadLabel ImageLabel { get; }

        public RadCheckBox ChkInclude { get; }

        public UserControl Control => (UserControl)ValueControl;

        public IValueControl ValueControl { get; }
    }
}
