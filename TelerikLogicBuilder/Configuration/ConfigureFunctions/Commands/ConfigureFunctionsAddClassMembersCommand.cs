using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureFunctionsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands
{
    internal class ConfigureFunctionsAddClassMembersCommand : ClickCommandBase
    {
        private readonly IConfigureFunctionsXmlTreeViewSynchronizer _configureFunctionsXmlTreeViewSynchronizer;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFunctionsForm configureFunctionsForm;
        private XmlDocument ConstructorsDoc => configureFunctionsForm.ConstructorsDoc;
        private XmlDocument XmlDocument => configureFunctionsForm.XmlDocument;
        private RadTreeView TreeView => configureFunctionsForm.TreeView;

        public ConfigureFunctionsAddClassMembersCommand(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _configureFunctionsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureFunctionsXmlTreeViewSynchronizer
            (
                configureFunctionsForm
            );
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public override void Execute()
        {
            XmlDocument functionsBackup = new();
            XmlDocument constructorsBackup = new();
            string selectedNodeName = TreeView.SelectedNode?.Name ?? throw _exceptionHelper.CriticalException("{ED5F0835-CBB2-4879-A012-B8F0EE933BBF}");
            try
            {
                configureFunctionsForm.TreeView.BeginUpdate();
                functionsBackup.LoadXml(XmlDocument.OuterXml);
                constructorsBackup.LoadXml(ConstructorsDoc.OuterXml);
                AddFunctions();
            }
            catch (LogicBuilderException ex)
            {
                configureFunctionsForm.ReloadXmlDocument(functionsBackup.OuterXml);
                ConstructorsDoc.LoadXml(constructorsBackup.OuterXml);
                configureFunctionsForm.RebuildTreeView();
                _treeViewService.SelectTreeNode(TreeView, selectedNodeName);
                if (TreeView.SelectedNode != null)
                    _treeViewService.MakeVisible(TreeView.SelectedNode);

                configureFunctionsForm.SetErrorMessage(ex.Message);
            }
            finally
            {
                configureFunctionsForm.TreeView.EndUpdate();
            }
        }

        private void AddFunctions()
        {
            RadTreeNode? selectedNode = TreeView.SelectedNode;
            if (selectedNode == null)
                return;

            IIntellisenseFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IIntellisenseFormFactory>();
            using IConfigureClassFunctionsHelperForm configureClassFunctionsHelperForm = disposableManager.GetConfigureClassFunctionsHelperForm
            (
                configureFunctionsForm.ConstructorsDictionary,
                configureFunctionsForm.VariablesDictionary,
                configureFunctionsForm.HelperStatus
            );

            configureClassFunctionsHelperForm.ShowDialog((Control)configureFunctionsForm);
            configureFunctionsForm.HelperStatus = configureClassFunctionsHelperForm.HelperStatus;
            if (configureClassFunctionsHelperForm.DialogResult != DialogResult.OK)
                return;

            configureFunctionsForm.UpdateConstructorsConfiguration(configureClassFunctionsHelperForm.NewConstructors);
            AddFunctionsXml
            (
                _treeViewService.IsFolderNode(selectedNode) ? selectedNode : selectedNode.Parent,
                configureClassFunctionsHelperForm.Functions
            );
        }

        private void AddFunctionsXml(RadTreeNode destinationFolderTreeNode, ICollection<Function> functions)
        {
            List<string> errors = new();
            HashSet<string> existingFunctions = configureFunctionsForm.FunctionNames;
            IList<XmlElement> validFunctions = functions.Aggregate
            (
                new List<XmlElement>(), 
                (list, function) =>
                {
                    if (existingFunctions.Contains(function.Name))
                    {
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionAlreadyConfiguredFormat, function.Name));
                        return list;
                    }

                    list.Add
                    (
                        _xmlDocumentHelpers.AddElementToDoc
                        (
                            XmlDocument,
                            _xmlDocumentHelpers.ToXmlElement(function.ToXml)
                        )
                    );

                    return list;
                }
            );

            _configureFunctionsXmlTreeViewSynchronizer.AddFunctionNodes
            (
                (StateImageRadTreeNode)destinationFolderTreeNode,
                validFunctions
            );

            if (errors.Count > 0)
            {
                configureFunctionsForm.SetErrorMessage(string.Join(Environment.NewLine, errors));
            }
        }
    }
}
