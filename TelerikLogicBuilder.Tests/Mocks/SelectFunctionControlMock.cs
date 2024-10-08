﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
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
        public SelectFunctionControlMock(IDictionary<string, string> expandedNodes, IDictionary<string, Function> functionDictionary, IList<TreeFolder> treeFolders)
        {
            ExpandedNodes = expandedNodes;
            FunctionDictionary = functionDictionary;
            TreeFolders = treeFolders;
        }

        public IDictionary<string, string> ExpandedNodes { get; }

        public DockStyle Dock { set => throw new NotImplementedException(); }

        public string? FunctionName => throw new NotImplementedException();

        public bool IsValid => throw new NotImplementedException();

        public bool ItemSelected => throw new NotImplementedException();

        public Point Location { set => throw new NotImplementedException(); }

        public IList<TreeFolder> TreeFolders { get; }

        public IDictionary<string, Function> FunctionDictionary { get; }

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

        public void SetFunction(string functionName)
        {
            throw new NotImplementedException();
        }

        public void SetMessage(string message, string title = "")
        {
            throw new NotImplementedException();
        }
    }
}
