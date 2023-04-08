using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Commands
{
    internal class AddXMLToFragmentsConfigurationCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFragmentItemFactory _fragmentItemFactory;
        private readonly IFragmentListInitializer _fragmentListInitializer;
        private readonly ILoadFragments _loadFragments;
        private readonly IStringHelper _stringHelper;
        private readonly IUpdateFragments _updateFragments;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlValidator _xmlValidator;
        private readonly IDataGraphEditingHost dataGraphEditingHost;

        public AddXMLToFragmentsConfigurationCommand(
            IConfigurationService configurationService,
            IFragmentItemFactory fragmentItemFactory,
            IFragmentListInitializer fragmentListInitializer,
            ILoadFragments loadFragments,
            IStringHelper stringHelper,
            IUpdateFragments updateFragments,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlValidatorFactory xmlValidatorFactory,
            IDataGraphEditingHost dataGraphEditingHost)
        {
            _configurationService = configurationService;
            _fragmentItemFactory = fragmentItemFactory;
            _fragmentListInitializer = fragmentListInitializer;
            _loadFragments = loadFragments;
            _stringHelper = stringHelper;
            _updateFragments = updateFragments;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlValidator = xmlValidatorFactory.GetXmlValidator(SchemaName.FragmentsSchema);
            this.dataGraphEditingHost = dataGraphEditingHost;
        }

        private static readonly string FRAGMENTNAMES_NODEXPATH = $"//{XmlDataConstants.FRAGMENTELEMENT}/@{XmlDataConstants.NAMEATTRIBUTE}";
        private HashSet<string> FragmentNames => dataGraphEditingHost.XmlDocument.SelectNodes(FRAGMENTNAMES_NODEXPATH)?.OfType<XmlAttribute>()
                                                    .Select(a => a.Value)
                                                    .ToHashSet() ?? new HashSet<string>();

        public override void Execute()
        {
            try
            {
                dataGraphEditingHost.ClearMessage();
                AddFragment();
            }
            catch (LogicBuilderException ex)
            {
                dataGraphEditingHost.SetErrorMessage(ex.Message);
            }
        }

        private void AddFragment()
        {
            RadTreeNode? selectedNode = dataGraphEditingHost.TreeView.SelectedNode;
            if (selectedNode == null)
                return;

            using IScopedDisposableManager<IInputBoxForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<IInputBoxForm>>();
            IInputBoxForm inputBox = disposableManager.ScopedService;
            inputBox.SetTitles(RegularExpressions.XMLNAMEATTRIBUTE, Strings.inputNewFragmentNameCaption, Strings.inputNewFragmentNamePrompt);
            inputBox.ShowDialog((IWin32Window)dataGraphEditingHost);

            if (inputBox.DialogResult != DialogResult.OK)
                return;

            XmlDocument dataDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(dataGraphEditingHost.XmlDocument, selectedNode.Name)
            );
            XmlDocument fragmentDocument = _loadFragments.Load();
            string newFragmentName = _stringHelper.EnsureUniqueName
            (
                inputBox.Input,
                FragmentNames
            );

            _xmlDocumentHelpers
                .SelectSingleElement(fragmentDocument, $"/{XmlDataConstants.FOLDERELEMENT}")
                .AppendChild
                (
                    _xmlDocumentHelpers.MakeElement
                    (
                        fragmentDocument,
                        XmlDataConstants.FOLDERELEMENT,
                        _fragmentItemFactory.GetFragment
                        (
                            newFragmentName,
                            _xmlDocumentHelpers.GetXmlString(dataDocument)
                        )
                        .ToXml,
                        new Dictionary<string, string> { { XmlDataConstants.NAMEATTRIBUTE, DateTime.Now.ToString() } }
                    )
                );

            XmlValidationResponse response = _xmlValidator.Validate(fragmentDocument.OuterXml);
            if (response.Success)
            {
                _updateFragments.Update(fragmentDocument);
                _configurationService.FragmentList = _fragmentListInitializer.InitializeList();
                dataGraphEditingHost.SetMessage(string.Format(CultureInfo.CurrentCulture, Strings.savedFragmentMessageFormat, newFragmentName));
            }
            else
            {
                dataGraphEditingHost.SetErrorMessage(string.Join(Environment.NewLine, response.Errors));
            }
        }
    }
}
