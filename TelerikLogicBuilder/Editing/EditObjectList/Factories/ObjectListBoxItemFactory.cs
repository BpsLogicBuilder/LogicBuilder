using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal class ObjectListBoxItemFactory : IObjectListBoxItemFactory
    {
        private readonly Func<string, string, Type, IApplicationForm, ListParameterInputStyle, IObjectListBoxItem> _getParameterObjectListBoxItem;

        public ObjectListBoxItemFactory(
            Func<string, string, Type, IApplicationForm, ListParameterInputStyle, IObjectListBoxItem> getParameterObjectListBoxItem)
        {
            _getParameterObjectListBoxItem = getParameterObjectListBoxItem;
        }

        public IObjectListBoxItem GetParameterObjectListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationForm applicationForm, ListParameterInputStyle listControl)
            => _getParameterObjectListBoxItem(visibleText, hiddenText, assignedTo, applicationForm, listControl);
    }
}
