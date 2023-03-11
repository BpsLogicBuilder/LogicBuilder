using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal class ObjectListBoxItemFactory : IObjectListBoxItemFactory
    {
        private readonly Func<string, string, Type, IApplicationForm, ListParameterInputStyle, IObjectListBoxItem> _getParameterObjectListBoxItem;
        private readonly Func<string, string, Type, IApplicationForm, ListVariableInputStyle, IObjectListBoxItem> _getVariableObjectListBoxItem;

        public ObjectListBoxItemFactory(
            Func<string, string, Type, IApplicationForm, ListParameterInputStyle, IObjectListBoxItem> getParameterObjectListBoxItem, 
            Func<string, string, Type, IApplicationForm, ListVariableInputStyle, IObjectListBoxItem> getVariableObjectListBoxItem)
        {
            _getParameterObjectListBoxItem = getParameterObjectListBoxItem;
            _getVariableObjectListBoxItem = getVariableObjectListBoxItem;
        }

        public IObjectListBoxItem GetParameterObjectListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationForm applicationForm, ListParameterInputStyle listControl)
            => _getParameterObjectListBoxItem(visibleText, hiddenText, assignedTo, applicationForm, listControl);

        public IObjectListBoxItem GetVariableObjectListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationForm applicationForm, ListVariableInputStyle listControl)
            => _getVariableObjectListBoxItem(visibleText, hiddenText, assignedTo, applicationForm, listControl);
    }
}
