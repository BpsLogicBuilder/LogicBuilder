using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument.Commands
{
    internal class UpdateGenericLiteralDomainCommand : ClickCommandBase
    {
        private readonly RadTreeView treeView;
        private readonly HelperButtonTextBox txtLpDomain;
        private readonly XmlDocument xmlDocument;

        public UpdateGenericLiteralDomainCommand(
            IConfigureGenericLiteralArgumentControl configureGenericLiteralArgumentControl)
        {
            treeView = configureGenericLiteralArgumentControl.TreeView;
            txtLpDomain = configureGenericLiteralArgumentControl.TxtLpDomain;
            xmlDocument = configureGenericLiteralArgumentControl.XmlDocument;
        }

        public override void Execute()
        {
            DisplayMessage.Show("UpdateGenericLiteralDomainCommand");
        }
    }
}
