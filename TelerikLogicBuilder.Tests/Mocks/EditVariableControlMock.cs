using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

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

        public ApplicationTypeInfo Application => throw new NotImplementedException();

        public bool IsValid => throw new NotImplementedException();

        public DockStyle Dock { set => throw new NotImplementedException(); }
        public Point Location { set => throw new NotImplementedException(); }

        event EventHandler? IEditVariableControl.Changed
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public void ClearMessage()
        {
            throw new NotImplementedException();
        }

        public void RequestDocumentUpdate()
        {
            throw new NotImplementedException();
        }

        public void SetErrorMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void SetMessage(string message, string title = "")
        {
            throw new NotImplementedException();
        }

        public void SetVariable(string variableName)
        {
            throw new NotImplementedException();
        }
    }
}
