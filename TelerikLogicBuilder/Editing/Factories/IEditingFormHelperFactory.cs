using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingFormHelperFactory
    {
        IDataGraphEditingFormEventsHelper GetDataGraphEditingFormEventsHelper(IDataGraphEditingForm dataGraphEditingForm);
        IDataGraphEditingHostEventsHelper GetDataGraphEditingHostEventsHelper(IDataGraphEditingHost dataGraphEditingHost);
        IDataGraphEditingManager GetDataGraphEditingManager(IDataGraphEditingHost dataGraphEditingHost);
        IParametersDataTreeBuilder GetParametersDataTreeBuilder(IDataGraphEditingHost dataGraphEditingHost);
    }
}
