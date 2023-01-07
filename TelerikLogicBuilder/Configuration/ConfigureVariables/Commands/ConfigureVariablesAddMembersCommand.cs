using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureVariablesHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands
{
    internal class ConfigureVariablesAddMembersCommand : ClickCommandBase
    {
        private readonly IConfigureVariablesXmlTreeViewSynchronizer _configureVariablesXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesAddMembersCommand(
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureVariablesForm configureVariablesForm)
        {
            _configureVariablesXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureVariablesXmlTreeViewSynchronizer
            (
                configureVariablesForm
            );

            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureVariablesForm = configureVariablesForm;
        }

        public override void Execute()
        {
            RadTreeNode? selectedNode = configureVariablesForm.TreeView.SelectedNode;
            if (selectedNode == null)
                return;

            using IIntellisenseFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IIntellisenseFormFactory>();
            IConfigureClassVariablesHelperForm configureClassVariablesHelperForm = disposableManager.GetConfigureClassVariablesHelperForm
            (
                configureVariablesForm.VariablesDictionary,
                configureVariablesForm.HelperStatus
            );

            configureClassVariablesHelperForm.ShowDialog((Control)configureVariablesForm);
            if (configureClassVariablesHelperForm.DialogResult != DialogResult.OK)
                return;

            AddVariablesXml
            (
                _treeViewService.IsFolderNode(selectedNode) ? selectedNode : selectedNode.Parent, 
                configureClassVariablesHelperForm.Variables
            );
        }

        private void AddVariablesXml(RadTreeNode destinationTreeNode, IList<VariableBase> variables)
        {
            List<string> errors = new();
            HashSet<string> excistingVariables = configureVariablesForm.VariableNames;
            IList<XmlElement> validVariables = variables.Aggregate
            (
                new List<XmlElement>(),
                (list, variable) =>
                {
                    if (excistingVariables.Contains(variable.Name))
                    {
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableAlreadyConfiguredFormat, variable.Name));
                        return list;
                    }

                    list.Add
                    (
                        _xmlDocumentHelpers.AddElementToDoc
                        (
                            configureVariablesForm.XmlDocument,
                            _xmlDocumentHelpers.ToXmlElement(variable.ToXml)
                        )
                    );

                    return list;
                }
            );

            try
            {
                _configureVariablesXmlTreeViewSynchronizer.AddVariableNodes
                (
                    (StateImageRadTreeNode)destinationTreeNode,
                    validVariables
                );

                if (errors.Count > 0)
                {
                    configureVariablesForm.SetErrorMessage(string.Join(Environment.NewLine, errors));
                }
            }
            catch (LogicBuilderException ex)
            {
                configureVariablesForm.SetErrorMessage(ex.Message);
            }
        }
    }
}
