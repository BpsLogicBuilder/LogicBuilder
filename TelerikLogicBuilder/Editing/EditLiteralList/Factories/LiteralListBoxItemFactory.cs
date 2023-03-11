using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal class LiteralListBoxItemFactory : ILiteralListBoxItemFactory
    {
        private readonly Func<string, string, Type, IApplicationForm, ListParameterInputStyle, ILiteralListBoxItem> _getParameterLiteralListBoxItem;
        private readonly Func<string, string, Type, IApplicationForm, ListVariableInputStyle, ILiteralListBoxItem> _getVariableLiteralListBoxItem;

        public LiteralListBoxItemFactory(
            Func<string, string, Type, IApplicationForm, ListParameterInputStyle, ILiteralListBoxItem> getParameterLiteralListBoxItem, 
            Func<string, string, Type, IApplicationForm, ListVariableInputStyle, ILiteralListBoxItem> getVariableLiteralListBoxItem)
        {
            _getParameterLiteralListBoxItem = getParameterLiteralListBoxItem;
            _getVariableLiteralListBoxItem = getVariableLiteralListBoxItem;
        }

        public ILiteralListBoxItem GetParameterLiteralListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationForm applicationForm, ListParameterInputStyle listControl)
            => _getParameterLiteralListBoxItem(visibleText, hiddenText, assignedTo, applicationForm, listControl);

        public ILiteralListBoxItem GetVariableLiteralListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationForm applicationForm, ListVariableInputStyle listControl)
            => _getVariableLiteralListBoxItem(visibleText, hiddenText, assignedTo, applicationForm, listControl);
    }
}
