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
    internal class UpdateGenericLiteralListDomainCommand : ClickCommandBase
    {
        private readonly RadTreeView treeView;
        private readonly HelperButtonTextBox txtListLpDomain;
        private readonly XmlDocument xmlDocument;

        public UpdateGenericLiteralListDomainCommand(
            IConfigureGenericLiteralListArgumentControl configureGenericLiteralListArgumentControl)
        {
            treeView = configureGenericLiteralListArgumentControl.TreeView;
            txtListLpDomain = configureGenericLiteralListArgumentControl.TxtListLpDomain;
            xmlDocument = configureGenericLiteralListArgumentControl.XmlDocument;
        }

        public override void Execute()
        {
            DisplayMessage.Show("UpdateGenericLiteralListDomainCommand");
        }
    }
}
