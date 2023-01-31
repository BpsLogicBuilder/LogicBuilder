using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using System;
using System.Collections.Generic;

namespace TelerikLogicBuilder.Tests.Mocks
{
    internal class EditVariableControlMock : IEditVariableControl
    {
        public EditVariableControlMock(IDictionary<string, string> expandedNodes)
        {
            ExpandedNodes = expandedNodes;
        }

        public IDictionary<string, string> ExpandedNodes { get; }

        public bool ItemSelected => throw new NotImplementedException();

        public string? VariableName => throw new NotImplementedException();
    }
}
