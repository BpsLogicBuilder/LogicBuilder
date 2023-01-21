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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands
{
    internal class ConfigureFunctionsAddFolderCommand : ClickCommandBase
    {
        private readonly IConfigureFunctionsXmlTreeViewSynchronizer _configureFunctionsXmlTreeViewSynchronizer;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionsAddFolderCommand(
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _configureFunctionsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureFunctionsXmlTreeViewSynchronizer
            (
                configureFunctionsForm
            );
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public override void Execute()
        {
            try
            {
                RadTreeNode destinationFolderTreeNode = configureFunctionsForm.TreeView.SelectedNode;
                if (destinationFolderTreeNode == null)
                    return;

                if (!_treeViewService.IsFolderNode(destinationFolderTreeNode)
                    && !_treeViewService.IsMethodNode(destinationFolderTreeNode))
                    return;

                if (_treeViewService.IsMethodNode(destinationFolderTreeNode))
                    destinationFolderTreeNode = destinationFolderTreeNode.Parent;

                _configureFunctionsXmlTreeViewSynchronizer.AddFolder
                (
                    (StateImageRadTreeNode)destinationFolderTreeNode,
                    _stringHelper.EnsureUniqueName(Strings.defaultNewFolderName, GetFolderNames())
                );
                configureFunctionsForm.CheckEnableImportButton();

                HashSet<string> GetFolderNames()
                    => _xmlDocumentHelpers.ToXmlDocument
                    (
                        _xmlDocumentHelpers.SelectSingleElement
                        (
                            configureFunctionsForm.XmlDocument, destinationFolderTreeNode.Name
                        )
                    )
                    .SelectNodes($"/{XmlDataConstants.FOLDERELEMENT}/{XmlDataConstants.FOLDERELEMENT}/@{XmlDataConstants.NAMEATTRIBUTE}")?.OfType<XmlAttribute>()
                    .Select(a => a.Value)
                    .ToHashSet() ?? new HashSet<string>();
            }
            catch (LogicBuilderException ex)
            {
                configureFunctionsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}
