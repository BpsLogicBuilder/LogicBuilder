using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TelerikLogicBuilder.Tests.Mocks
{
    internal class SelectFragmentControlMock : ISelectFragmentControl
    {
        public SelectFragmentControlMock(IDictionary<string, string> expandedNodes)
        {
            ExpandedNodes = expandedNodes;
        }

        public IDictionary<string, string> ExpandedNodes { get; }

        public DockStyle Dock { set => throw new NotImplementedException(); }

        public bool IsValid => throw new NotImplementedException();

        public bool ItemSelected => throw new NotImplementedException();

        public Point Location { set => throw new NotImplementedException(); }

        public string? FragmentName => throw new NotImplementedException();

        event EventHandler? ISelectFragmentControl.Changed
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

        public void SetErrorMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void SetFragment(string fragmentName)
        {
            throw new NotImplementedException();
        }

        public void SetMessage(string message, string title = "")
        {
            throw new NotImplementedException();
        }
    }
}
