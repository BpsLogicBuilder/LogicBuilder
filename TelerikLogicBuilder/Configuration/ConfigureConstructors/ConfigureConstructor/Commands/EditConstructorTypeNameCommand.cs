using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor.Commands
{
    internal class EditConstructorTypeNameCommand : ClickCommandBase
    {
        private readonly IConfigureConstructorsXmlTreeViewSynchronizer _configureConstructorsXmlTreeViewSynchronizer;
        private readonly IConstructorXmlParser _constructorXmlParser;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureConstructorControl configureConstructorControl;

        public EditConstructorTypeNameCommand(
            IConstructorXmlParser constructorXmlParser,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureConstructorControl configureConstructorControl)
        {
            _constructorXmlParser = constructorXmlParser;
            _configureConstructorsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureConstructorsXmlTreeViewSynchronizer
            (
                configureConstructorControl.Form
            );
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureConstructorControl = configureConstructorControl;
        }

        public override void Execute()
        {
            try
            {
                configureConstructorControl.ClearMessage();
                configureConstructorControl.TreeView.BeginUpdate();
                UpdateConstructor();
            }
            catch (LogicBuilderException ex)
            {
                configureConstructorControl.SetErrorMessage(ex.Message);
            }
            finally
            {
                configureConstructorControl.TreeView.EndUpdate();
            }
        }

        private void UpdateConstructor()
        {
            RadTreeNode? selectedNode = configureConstructorControl.TreeView.SelectedNode;
            if (selectedNode == null || !_treeViewService.IsConstructorNode(selectedNode))
                return;

            Constructor constructor = _constructorXmlParser.Parse
            (
                _xmlDocumentHelpers.SelectSingleElement
                (
                    configureConstructorControl.XmlDocument,
                    selectedNode.Name
                )
            );

            IIntellisenseFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IIntellisenseFormFactory>();
            using IConfigureConstructorsHelperForm configureConstructorsHelperForm = disposableManager.GetConfigureConstructorsHelperForm
            (
                configureConstructorControl.ConstructorsDictionary,
                configureConstructorControl.HelperStatus,
                constructor.Name
            );
            configureConstructorsHelperForm.ShowDialog((Control)configureConstructorControl);
            if (configureConstructorsHelperForm.DialogResult != DialogResult.OK)
                return;

            AddChildConstructors(selectedNode, configureConstructorsHelperForm.ChildConstructors);
            UpdateConstructor(selectedNode, configureConstructorsHelperForm.Constructor);
        }

        private void UpdateConstructor(RadTreeNode selectedNode, Constructor constructor)
        {
            _configureConstructorsXmlTreeViewSynchronizer.ReplaceConstructorNode
            (
                (StateImageRadTreeNode)selectedNode,
                _xmlDocumentHelpers.AddElementToDoc
                (
                    configureConstructorControl.XmlDocument,
                    _xmlDocumentHelpers.ToXmlElement(constructor.ToXml)
                )
            );
        }

        private void AddChildConstructors(RadTreeNode selectedNode, ICollection<Constructor> childConstructors)
        {
            RadTreeNode parentTreeNode = selectedNode.Parent;
            _configureConstructorsXmlTreeViewSynchronizer.AddConstructorNodes
            (
                (StateImageRadTreeNode)parentTreeNode,
                childConstructors.Select
                (
                    c => _xmlDocumentHelpers.AddElementToDoc
                    (
                        configureConstructorControl.XmlDocument,
                        _xmlDocumentHelpers.ToXmlElement(c.ToXml)
                    )
                )
                .ToArray()
            );
        }
    }
}
