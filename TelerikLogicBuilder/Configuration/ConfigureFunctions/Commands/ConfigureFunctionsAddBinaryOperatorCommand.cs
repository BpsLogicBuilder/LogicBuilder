using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands
{
    internal partial class ConfigureFunctionsAddBinaryOperatorCommand : ClickCommandBase
    {
        private readonly IConfigureFunctionsXmlTreeViewSynchronizer _configureFunctionsXmlTreeViewSynchronizer;
        private readonly IFunctionFactory _functionFactory;
        private readonly IParameterFactory _parameterFactory;
        private readonly IReturnTypeFactory _returnTypeFactory;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFunctionsForm configureFunctionsForm;
        private HashSet<string> FunctionNames => configureFunctionsForm.FunctionNames;
        private RadTreeView TreeView => configureFunctionsForm.TreeView;
        private XmlDocument XmlDocument => configureFunctionsForm.XmlDocument;

        public ConfigureFunctionsAddBinaryOperatorCommand(
            IFunctionFactory functionFactory,
            IParameterFactory parameterFactory,
            IReturnTypeFactory returnTypeFactory,
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _configureFunctionsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureFunctionsXmlTreeViewSynchronizer
            (
                configureFunctionsForm
            );
            _functionFactory = functionFactory;
            _parameterFactory = parameterFactory;
            _returnTypeFactory = returnTypeFactory;
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public override void Execute()
        {
            try
            {
                configureFunctionsForm.ClearMessage();
                TreeView.BeginUpdate();
                AddBinaryOperator();
            }
            catch (LogicBuilderException ex)
            {
                configureFunctionsForm.SetErrorMessage(ex.Message);
            }
            finally
            {
                TreeView.EndUpdate();
            }
        }

        private void AddBinaryOperator()
        {
            RadTreeNode? selectedNode = TreeView.SelectedNode;
            if (selectedNode == null)
                return;

            if (!_treeViewService.IsFolderNode(selectedNode) && !_treeViewService.IsMethodNode(selectedNode))
                return;

            RadTreeNode destinationFolderNode = _treeViewService.IsMethodNode(selectedNode) ? selectedNode.Parent : selectedNode;

            using IScopedDisposableManager<IInputBoxForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<IInputBoxForm>>();
            IInputBoxForm inputBox = disposableManager.ScopedService;
            inputBox.SetTitles(RegularExpressions.XMLNAMEATTRIBUTE, Strings.inputNewActionNameCaption, Strings.inputNewActionNamePrompt);
            inputBox.ShowDialog((Control)configureFunctionsForm);

            if (inputBox.DialogResult != DialogResult.OK)
                return;

            string functionName = inputBox.Input;

            _configureFunctionsXmlTreeViewSynchronizer.AddFunctionNode
            (
                (StateImageRadTreeNode)destinationFolderNode,
                _xmlDocumentHelpers.AddElementToDoc
                (
                    XmlDocument,
                    _xmlDocumentHelpers.ToXmlElement
                    (
                        _functionFactory.GetFunction
                        (
                            _stringHelper.EnsureUniqueName(functionName, FunctionNames),
                            RemoveNumericAndNonWordRegex().Replace(functionName[..1], string.Empty) + RemoveNonWordRegex().Replace(functionName[1..], string.Empty),
                            FunctionCategories.BinaryOperator,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            ReferenceCategories.None,
                            ParametersLayout.Binary,
                            new List<ParameterBase>
                            {
                                _parameterFactory.GetLiteralParameter(Strings.builtInParameterNameValue1, false, string.Empty, LiteralParameterType.Any, LiteralParameterInputStyle.SingleLineTextBox, false, false, true, string.Empty, string.Empty, string.Empty, new List<string>()),
                                _parameterFactory.GetLiteralParameter(Strings.builtInParameterNameValue2, false, string.Empty, LiteralParameterType.Any, LiteralParameterInputStyle.SingleLineTextBox, false, false, true, string.Empty, string.Empty, string.Empty, new List<string>())
                            },
                            new List<string>(),
                            _returnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                            string.Empty
                        ).ToXml
                    )
                )
            );
        }

        [GeneratedRegex(RegularExpressions.REMOVENUMERICANDNONWORD)]
        private static partial Regex RemoveNumericAndNonWordRegex();
        [GeneratedRegex(RegularExpressions.REMOVENONWORD)]
        private static partial Regex RemoveNonWordRegex();
    }
}
