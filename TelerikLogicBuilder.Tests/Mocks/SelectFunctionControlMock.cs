using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelerikLogicBuilder.Tests.Mocks
{
    internal class SelectFunctionControlMock : ISelectFunctionControl
    {
        public SelectFunctionControlMock(IDictionary<string, string> expandedNodes, IList<TreeFolder> treeFolders)
        {
            ExpandedNodes = expandedNodes;
            TreeFolders = treeFolders;
        }

        public IDictionary<string, string> ExpandedNodes { get; }

        public DockStyle Dock { set => throw new NotImplementedException(); }

        public string? FunctionName => throw new NotImplementedException();

        public bool IsValid => throw new NotImplementedException();

        public bool ItemSelected => throw new NotImplementedException();

        public Point Location { set => throw new NotImplementedException(); }

        public IList<TreeFolder> TreeFolders { get; }

        event EventHandler? ISelectFunctionControl.Changed
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
    }
}
