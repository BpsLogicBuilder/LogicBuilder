using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IDataGraphEditingForm : IDataGraphEditingHost, IEditingForm
    {
        event FormClosingEventHandler? FormClosing;
    }
}
