using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor;
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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands
{
    internal class ConfigureConstructorsHelperCommand : ClickCommandBase
    {
        private readonly IConfigureConstructorsXmlTreeViewSynchronizer _configureConstructorsXmlTreeViewSynchronizer;
        private readonly IConstructorXmlParser _constructorXmlParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers; 
        
        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsHelperCommand(
            IConstructorXmlParser constructorXmlParser,
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _constructorXmlParser = constructorXmlParser;
            _configureConstructorsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureConstructorsXmlTreeViewSynchronizer
            (
                configureConstructorsForm
            );
            _exceptionHelper = exceptionHelper;
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
                UpdateConstructor();
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

        private void UpdateConstructor()
        {
            if (configureConstructorsForm.CurrentTreeNodeControl is not IConfigureConstructorControl)
                throw _exceptionHelper.CriticalException("{EFFF7F69-C303-4B2E-9175-3D34D9F67654}");

            RadTreeNode? selectedNode = configureConstructorsForm.TreeView.SelectedNode;
            if (selectedNode == null || !_treeViewService.IsConstructorNode(selectedNode))
                return;

            Constructor constructor = _constructorXmlParser.Parse
            (
                _xmlDocumentHelpers.SelectSingleElement
                (
                    configureConstructorsForm.XmlDocument,
                    selectedNode.Name
                )
            );

            IIntellisenseFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IIntellisenseFormFactory>();
            using IConfigureConstructorsHelperForm configureConstructorsHelperForm = disposableManager.GetConfigureConstructorsHelperForm
            (
                configureConstructorsForm.ConstructorsDictionary,
                configureConstructorsForm.HelperStatus,
                constructor.Name
            );
            configureConstructorsHelperForm.ShowDialog((Control)configureConstructorsForm);
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
                    configureConstructorsForm.XmlDocument,
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
                        configureConstructorsForm.XmlDocument,
                        _xmlDocumentHelpers.ToXmlElement(c.ToXml)
                    )
                )
                .ToArray()
            );
        }
    }
}
