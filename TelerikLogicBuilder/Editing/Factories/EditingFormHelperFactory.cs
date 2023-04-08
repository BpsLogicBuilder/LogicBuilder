using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingFormHelperFactory : IEditingFormHelperFactory
    {
        private readonly Func<IDataGraphEditingForm, IDataGraphEditingFormEventsHelper> _getDataGraphEditingFormEventsHelper;
        private readonly Func<IDataGraphEditingHost, IDataGraphEditingHostEventsHelper> _getDataGraphEditingHostEventsHelper;
        private readonly Func<IDataGraphEditingHost, IDataGraphEditingManager> _getDataGraphEditingManager;
        private readonly Func<IDataGraphEditingHost, IParametersDataTreeBuilder> _getParametersDataTreeBuilder;

        public EditingFormHelperFactory(
            Func<IDataGraphEditingForm, IDataGraphEditingFormEventsHelper> getDataGraphEditingFormEventsHelper,
            Func<IDataGraphEditingHost, IDataGraphEditingHostEventsHelper> getDataGraphEditingHostEventsHelper,
            Func<IDataGraphEditingHost, IDataGraphEditingManager> getDataGraphEditingManager,
            Func<IDataGraphEditingHost, IParametersDataTreeBuilder> getParametersDataTreeBuilder)
        {
            _getDataGraphEditingFormEventsHelper = getDataGraphEditingFormEventsHelper;
            _getDataGraphEditingHostEventsHelper = getDataGraphEditingHostEventsHelper;
            _getDataGraphEditingManager = getDataGraphEditingManager;
            _getParametersDataTreeBuilder = getParametersDataTreeBuilder;
        }

        public IDataGraphEditingFormEventsHelper GetDataGraphEditingFormEventsHelper(IDataGraphEditingForm dataGraphEditingForm)
            => _getDataGraphEditingFormEventsHelper(dataGraphEditingForm);

        public IDataGraphEditingHostEventsHelper GetDataGraphEditingHostEventsHelper(IDataGraphEditingHost dataGraphEditingHost)
            => _getDataGraphEditingHostEventsHelper(dataGraphEditingHost);

        public IDataGraphEditingManager GetDataGraphEditingManager(IDataGraphEditingHost dataGraphEditingHost)
            => _getDataGraphEditingManager(dataGraphEditingHost);

        public IParametersDataTreeBuilder GetParametersDataTreeBuilder(IDataGraphEditingHost dataGraphEditingHost)
            => _getParametersDataTreeBuilder(dataGraphEditingHost);
    }
}
