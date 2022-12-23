using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands
{
    internal class ConfigureVariablesAddFolderCommand : ClickCommandBase
    {
        private readonly IConfigureVariablesXmlTreeViewSynchronizer _configureVariablesXmlTreeViewSynchronizer;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesAddFolderCommand(
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureVariablesForm configureVariablesForm)
        {
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _configureVariablesXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureVariablesXmlTreeViewSynchronizer
            (
                configureVariablesForm
            );
            this.configureVariablesForm = configureVariablesForm;
        }

        public override void Execute()
        {
            try
            {
                if (configureVariablesForm.TreeView.SelectedNode == null)
                    return;

                RadTreeNode destinationFolderTreeNode = configureVariablesForm.TreeView.SelectedNode;
                if (_treeViewService.IsVariableTypeNode(destinationFolderTreeNode))
                    destinationFolderTreeNode = destinationFolderTreeNode.Parent;

                _configureVariablesXmlTreeViewSynchronizer.AddFolder
                (
                    (StateImageRadTreeNode)destinationFolderTreeNode,
                    _stringHelper.EnsureUniqueName(Strings.defaultNewFolderName, GetFolderNames())
                );

                configureVariablesForm.CheckEnableImportButton();

                HashSet<string> GetFolderNames() 
                    => _xmlDocumentHelpers.ToXmlDocument
                    (
                        _xmlDocumentHelpers.SelectSingleElement
                        (
                            configureVariablesForm.XmlDocument, destinationFolderTreeNode.Name
                        )
                    )
                    .SelectNodes($"/{XmlDataConstants.FOLDERELEMENT}/{XmlDataConstants.FOLDERELEMENT}/@{XmlDataConstants.NAMEATTRIBUTE}")?.OfType<XmlAttribute>()
                    .Select(a => a.Value)
                    .ToHashSet() ?? new HashSet<string>();
            }
            catch (LogicBuilderException ex)
            {
                configureVariablesForm.SetErrorMessage(ex.Message);
            }
        }
    }
}
