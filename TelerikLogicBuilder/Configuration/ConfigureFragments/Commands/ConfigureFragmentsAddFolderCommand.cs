using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Commands
{
    internal class ConfigureFragmentsAddFolderCommand : ClickCommandBase
    {
        private readonly IConfigureFragmentsXmlTreeViewSynchronizer _configureFragmentsXmlTreeViewSynchronizer;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IConfigureFragmentsForm configureFragmentsForm;

        public ConfigureFragmentsAddFolderCommand(
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureFragmentsForm configureFragmentsForm)
        {
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _configureFragmentsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureFragmentsXmlTreeViewSynchronizer
            (
                configureFragmentsForm
            );
            this.configureFragmentsForm = configureFragmentsForm;
        }

        public override void Execute()
        {
            try
            {
                RadTreeNode destinationFolderTreeNode = configureFragmentsForm.TreeView.SelectedNode;
                if (destinationFolderTreeNode == null)
                    return;

                if (!_treeViewService.IsFolderNode(destinationFolderTreeNode)
                    && !_treeViewService.IsFileNode(destinationFolderTreeNode))
                    return;

                if (_treeViewService.IsFileNode(destinationFolderTreeNode))
                    destinationFolderTreeNode = destinationFolderTreeNode.Parent;

                _configureFragmentsXmlTreeViewSynchronizer.AddFolder
                (
                    destinationFolderTreeNode,
                    _stringHelper.EnsureUniqueName(Strings.defaultNewFolderName, GetFolderNames())
                );
                configureFragmentsForm.CheckEnableImportButton();

                HashSet<string> GetFolderNames()
                    => _xmlDocumentHelpers.ToXmlDocument
                    (
                        _xmlDocumentHelpers.SelectSingleElement
                        (
                            configureFragmentsForm.XmlDocument, destinationFolderTreeNode.Name
                        )
                    )
                    .SelectNodes($"/{XmlDataConstants.FOLDERELEMENT}/{XmlDataConstants.FOLDERELEMENT}/@{XmlDataConstants.NAMEATTRIBUTE}")?.OfType<XmlAttribute>()
                    .Select(a => a.Value)
                    .ToHashSet() ?? new HashSet<string>();
            }
            catch (LogicBuilderException ex)
            {
                configureFragmentsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}
