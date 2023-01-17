using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands
{
    internal class ConfigureConstructorsAddConstructorCommand : ClickCommandBase
    {
        private readonly IConfigureConstructorsXmlTreeViewSynchronizer _configureConstructorsXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsAddConstructorCommand(
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _configureConstructorsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureConstructorsXmlTreeViewSynchronizer
            (
                configureConstructorsForm
            );
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public override void Execute()
        {
            try
            {
                configureConstructorsForm.ClearMessage();
                configureConstructorsForm.TreeView.BeginUpdate();
                AddConstructor();
            }
            catch (LogicBuilderException ex)
            {
                configureConstructorsForm.SetErrorMessage(ex.Message);
            }
            finally
            {
                configureConstructorsForm.TreeView.EndUpdate();
            }
        }

        public void AddConstructor()
        {
            RadTreeNode? selectedNode = configureConstructorsForm.TreeView.SelectedNode;
            if (selectedNode == null)
                return;

            if (!_treeViewService.IsFolderNode(selectedNode) && !_treeViewService.IsConstructorNode(selectedNode))
                return;

            RadTreeNode destinationFolderNode = _treeViewService.IsConstructorNode(selectedNode) ? selectedNode.Parent : selectedNode;

            using IIntellisenseFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IIntellisenseFormFactory>();
            IConfigureConstructorsHelperForm configureConstructorsHelperForm = disposableManager.GetConfigureConstructorsHelperForm
            (
                configureConstructorsForm.ConstructorsDictionary,
                configureConstructorsForm.HelperStatus
            );
            configureConstructorsHelperForm.ShowDialog((Control)configureConstructorsForm);
            if (configureConstructorsHelperForm.DialogResult != DialogResult.OK)
                return;

            AddChildConstructors(destinationFolderNode, configureConstructorsHelperForm.ChildConstructors);
            AddConstructor(destinationFolderNode, configureConstructorsHelperForm.Constructor);
        }

        private void AddConstructor(RadTreeNode destinationFolderNode, Constructor constructor)
        {
            _configureConstructorsXmlTreeViewSynchronizer.AddConstructorNode
            (
                (StateImageRadTreeNode)destinationFolderNode,
                _xmlDocumentHelpers.AddElementToDoc
                (
                    configureConstructorsForm.XmlDocument,
                    _xmlDocumentHelpers.ToXmlElement(constructor.ToXml)
                )
            );
        }

        private void AddChildConstructors(RadTreeNode destinationFolderNode, ICollection<Constructor> childConstructors)
        {
            _configureConstructorsXmlTreeViewSynchronizer.AddConstructorNodes
            (
                (StateImageRadTreeNode)destinationFolderNode,
                childConstructors.Select
                (
                    c => _xmlDocumentHelpers.AddElementToDoc
                    (
                        configureConstructorsForm.XmlDocument,
                        _xmlDocumentHelpers.ToXmlElement(c.ToXml)
                    )
                )
                .ToArray()
            );
        }
    }
}
