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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands
{
    internal class ConfigureConstructorsAddFolderCommand : ClickCommandBase
    {
        private readonly IConfigureConstructorsXmlTreeViewSynchronizer _configureConstructorsXmlTreeViewSynchronizer;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsAddFolderCommand(
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _configureConstructorsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureConstructorsXmlTreeViewSynchronizer
            (
                configureConstructorsForm
            );
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public override void Execute()
        {
            try
            {
                RadTreeNode destinationFolderTreeNode = configureConstructorsForm.TreeView.SelectedNode;
                if (destinationFolderTreeNode == null)
                    return;

                if (!_treeViewService.IsFolderNode(destinationFolderTreeNode)
                    && !_treeViewService.IsConstructorNode(destinationFolderTreeNode))
                    return;

                if (_treeViewService.IsConstructorNode(destinationFolderTreeNode))
                    destinationFolderTreeNode = destinationFolderTreeNode.Parent;

                _configureConstructorsXmlTreeViewSynchronizer.AddFolder
                (
                    (StateImageRadTreeNode)destinationFolderTreeNode,
                    _stringHelper.EnsureUniqueName(Strings.defaultNewFolderName, GetFolderNames())
                );

                configureConstructorsForm.CheckEnableImportButton();

                HashSet<string> GetFolderNames()
                    => _xmlDocumentHelpers.ToXmlDocument
                    (
                        _xmlDocumentHelpers.SelectSingleElement
                        (
                            configureConstructorsForm.XmlDocument, destinationFolderTreeNode.Name
                        )
                    )
                    .SelectNodes($"/{XmlDataConstants.FOLDERELEMENT}/{XmlDataConstants.FOLDERELEMENT}|/{XmlDataConstants.FORMELEMENT}/{XmlDataConstants.FOLDERELEMENT}/@{XmlDataConstants.NAMEATTRIBUTE}")?.OfType<XmlAttribute>()
                    .Select(a => a.Value)
                    .ToHashSet() ?? new HashSet<string>();
            }
            catch (LogicBuilderException ex)
            {
                configureConstructorsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}
