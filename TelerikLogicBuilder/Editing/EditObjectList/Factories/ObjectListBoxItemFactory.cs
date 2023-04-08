using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal class ObjectListBoxItemFactory : IObjectListBoxItemFactory
    {
        private readonly Func<string, string, Type, IApplicationControl, ListParameterInputStyle, IObjectListBoxItem> _getParameterObjectListBoxItem;
        private readonly Func<string, string, Type, IApplicationControl, ListVariableInputStyle, IObjectListBoxItem> _getVariableObjectListBoxItem;

        public ObjectListBoxItemFactory(
            Func<string, string, Type, IApplicationControl, ListParameterInputStyle, IObjectListBoxItem> getParameterObjectListBoxItem, 
            Func<string, string, Type, IApplicationControl, ListVariableInputStyle, IObjectListBoxItem> getVariableObjectListBoxItem)
        {
            _getParameterObjectListBoxItem = getParameterObjectListBoxItem;
            _getVariableObjectListBoxItem = getVariableObjectListBoxItem;
        }

        public IObjectListBoxItem GetParameterObjectListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationControl applicationControl, ListParameterInputStyle listControl)
            => _getParameterObjectListBoxItem(visibleText, hiddenText, assignedTo, applicationControl, listControl);

        public IObjectListBoxItem GetVariableObjectListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationControl applicationControl, ListVariableInputStyle listControl)
            => _getVariableObjectListBoxItem(visibleText, hiddenText, assignedTo, applicationControl, listControl);
    }
}
