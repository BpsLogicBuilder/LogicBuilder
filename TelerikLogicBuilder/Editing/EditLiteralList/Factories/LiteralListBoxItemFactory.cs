using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal class LiteralListBoxItemFactory : ILiteralListBoxItemFactory
    {
        private readonly Func<string, string, Type, IApplicationForm, ListParameterInputStyle, ILiteralListBoxItem> _getParameterLiteralListBoxItem;

        public LiteralListBoxItemFactory(Func<string, string, Type, IApplicationForm, ListParameterInputStyle, ILiteralListBoxItem> getParameterLiteralListBoxItem)
        {
            _getParameterLiteralListBoxItem = getParameterLiteralListBoxItem;
        }

        public ILiteralListBoxItem GetParameterLiteralListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationForm applicationForm, ListParameterInputStyle listControl)
            => _getParameterLiteralListBoxItem(visibleText, hiddenText, assignedTo, applicationForm, listControl);
    }
}
