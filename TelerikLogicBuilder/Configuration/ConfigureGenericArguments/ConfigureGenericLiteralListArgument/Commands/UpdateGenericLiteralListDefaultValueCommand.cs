using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument.Commands
{
    internal class UpdateGenericLiteralListDefaultValueCommand : ClickCommandBase
    {
        private readonly RadTreeView treeView;
        private readonly HelperButtonTextBox txtListLpDefaultValue;
        private readonly XmlDocument xmlDocument;

        public UpdateGenericLiteralListDefaultValueCommand(
            IConfigureGenericLiteralListArgumentControl configureGenericLiteralListArgumentControl)
        {
            treeView = configureGenericLiteralListArgumentControl.TreeView;
            txtListLpDefaultValue = configureGenericLiteralListArgumentControl.TxtListLpDefaultValue;
            xmlDocument = configureGenericLiteralListArgumentControl.XmlDocument;
        }

        public override void Execute()
        {
            DisplayMessage.Show("UpdateGenericLiteralListDefaultValueCommand");
        }
    }
}
