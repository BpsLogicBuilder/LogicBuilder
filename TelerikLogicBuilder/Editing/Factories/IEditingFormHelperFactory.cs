using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingFormHelperFactory
    {
        IDataGraphEditingFormEventsHelper GetDataGraphEditingFormEventsHelper(IDataGraphEditingForm dataGraphEditingForm);
        IParametersDataTreeBuilder GetParametersDataTreeBuilder(IEditingForm editingForm);
    }
}
