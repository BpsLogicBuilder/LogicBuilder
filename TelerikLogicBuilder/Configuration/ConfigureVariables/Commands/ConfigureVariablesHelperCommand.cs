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
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands
{
    internal class ConfigureVariablesHelperCommand : ClickCommandBase
    {
        private readonly IConfigureVariablesXmlTreeViewSynchronizer _configureVariablesXmlTreeViewSynchronizer;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesHelperCommand(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureVariablesForm configureVariablesForm)
        {
            _configureVariablesXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureVariablesXmlTreeViewSynchronizer
            (
                configureVariablesForm
            );

            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureVariablesForm = configureVariablesForm;
        }

        public override void Execute()
        {
            if (configureVariablesForm.CurrentTreeNodeControl is not IConfigureVariableControl configureVariableControl)
                throw _exceptionHelper.CriticalException("{AE80AA16-6CB1-4422-89F9-A4BC691E54FB}");

            using IIntellisenseFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IIntellisenseFormFactory>();
            IConfigureVariablesHelperForm configureVariablesHelperForm = disposableManager.GetConfigureVariablesHelperForm
            (
                configureVariablesForm.VariablesDictionary,
                configureVariablesForm.HelperStatus
            );
            configureVariablesHelperForm.ShowDialog((Control)configureVariablesForm);
            if (configureVariablesHelperForm.DialogResult != DialogResult.OK)
                return;

            configureVariablesHelperForm.Variable.Name = configureVariableControl.TxtName.Text.Trim();//keep the existing variable name
            UpdateVariableXml(configureVariablesForm.TreeView.SelectedNode, configureVariablesHelperForm.Variable);
        }

        private void UpdateVariableXml(RadTreeNode selectedNode, VariableBase variable)
        {
            if (!_treeViewService.IsVariableTypeNode(selectedNode))
                throw _exceptionHelper.CriticalException("{5B8F3114-80C0-414E-88E3-5F6857E94765}");

            try
            {
                _configureVariablesXmlTreeViewSynchronizer.ReplaceVariableNode
                (
                    (StateImageRadTreeNode)selectedNode,
                    _xmlDocumentHelpers.AddElementToDoc
                    (
                        configureVariablesForm.XmlDocument,
                        _xmlDocumentHelpers.ToXmlElement(variable.ToXml)
                    )
                );
            }
            catch(LogicBuilderException ex)
            {
                configureVariablesForm.SetErrorMessage(ex.Message);
            }
        }
    }
}
