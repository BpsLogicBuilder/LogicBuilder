using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction;
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
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands
{
    internal class ConfigureFunctionsHelperCommand : ClickCommandBase
    {
        private readonly IConfigureFunctionsXmlTreeViewSynchronizer _configureFunctionsXmlTreeViewSynchronizer;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFunctionsForm configureFunctionsForm;
        private XmlDocument ConstructorsDoc => configureFunctionsForm.ConstructorsDoc;
        private XmlDocument XmlDocument => configureFunctionsForm.XmlDocument;
        private RadTreeView TreeView => configureFunctionsForm.TreeView;

        public ConfigureFunctionsHelperCommand(
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
            string selectedNodeName = TreeView.SelectedNode?.Name ?? throw _exceptionHelper.CriticalException("{D1467C04-8081-43B5-AD68-12A6C8DD7694}");
            try
            {
                functionsBackup.LoadXml(XmlDocument.OuterXml);
                constructorsBackup.LoadXml(ConstructorsDoc.OuterXml);
                ConfigureFunction();
            }
            catch (LogicBuilderException ex)
            {
                configureFunctionsForm.ReloadXmlDocument(functionsBackup.OuterXml);
                ConstructorsDoc.LoadXml(constructorsBackup.OuterXml);
                configureFunctionsForm.RebuildTreeView();
                _treeViewService.SelectTreeNode(TreeView, selectedNodeName);
                if(TreeView.SelectedNode != null)
                    _treeViewService.MakeVisible(TreeView.SelectedNode);

                this.configureFunctionsForm.SetErrorMessage(ex.Message);
            }
        }

        public void ConfigureFunction()
        {
            if (configureFunctionsForm.CurrentTreeNodeControl is not IConfigureFunctionControl configureFunctionControl)
                throw _exceptionHelper.CriticalException("{28D4CCA6-9714-4779-9092-1B0622F76211}");

            using IIntellisenseFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IIntellisenseFormFactory>();
            IConfigureFunctionsHelperForm configureFunctionsHelperForm = disposableManager.GetConfigureFunctionsHelperForm
            (
                configureFunctionsForm.ConstructorsDictionary,
                configureFunctionsForm.VariablesDictionary,
                configureFunctionsForm.HelperStatus
            );
            configureFunctionsHelperForm.ShowDialog((Control)configureFunctionsForm);
            if (configureFunctionsHelperForm.DialogResult != DialogResult.OK)
                return;

            configureFunctionsForm.UpdateConstructorsConfiguration(configureFunctionsHelperForm.NewConstructors);
            configureFunctionsHelperForm.Function.Name = configureFunctionControl.TxtFunctionName.Text;
            UpdateFunctionXml(TreeView.SelectedNode, configureFunctionsHelperForm.Function);
        }

        private void UpdateFunctionXml(RadTreeNode nodeToReplace, Function function)
        {
            if (!_treeViewService.IsMethodNode(nodeToReplace))
                throw _exceptionHelper.CriticalException("{7A7E6FB7-5AD2-4DC7-8957-E48534426934}");

            _configureFunctionsXmlTreeViewSynchronizer.ReplaceFunctionNode
            (
                (StateImageRadTreeNode)nodeToReplace,
                _xmlDocumentHelpers.AddElementToDoc
                (
                    XmlDocument,
                    _xmlDocumentHelpers.ToXmlElement(function.ToXml)
                )
            );
        }
    }
}
