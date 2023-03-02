using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingFormHelperFactory : IEditingFormHelperFactory
    {
        private readonly Func<IDataGraphEditingForm, IDataGraphEditingFormEventsHelper> _getDataGraphEditingFormEventsHelper;
        private readonly Func<IEditingForm, IParametersDataTreeBuilder> _getParametersDataTreeBuilder;

        public EditingFormHelperFactory(
            Func<IDataGraphEditingForm, IDataGraphEditingFormEventsHelper> getDataGraphEditingFormEventsHelper,
            Func<IEditingForm, IParametersDataTreeBuilder> getParametersDataTreeBuilder)
        {
            _getDataGraphEditingFormEventsHelper = getDataGraphEditingFormEventsHelper;
            _getParametersDataTreeBuilder = getParametersDataTreeBuilder;
        }

        public IDataGraphEditingFormEventsHelper GetDataGraphEditingFormEventsHelper(IDataGraphEditingForm dataGraphEditingForm)
            => _getDataGraphEditingFormEventsHelper(dataGraphEditingForm);

        public IParametersDataTreeBuilder GetParametersDataTreeBuilder(IEditingForm editingForm)
            => _getParametersDataTreeBuilder(editingForm);
    }
}
