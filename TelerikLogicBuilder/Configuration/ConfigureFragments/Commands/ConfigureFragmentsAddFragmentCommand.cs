using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Commands
{
    internal class ConfigureFragmentsAddFragmentCommand : ClickCommandBase
    {
        private readonly IConfigureFragmentsXmlTreeViewSynchronizer _configureFragmentsXmlTreeViewSynchronizer;
        private readonly IFragmentItemFactory _fragmentFactory;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFragmentsForm configureFragmentsForm;
        private HashSet<string> FragmentNames => configureFragmentsForm.FragmentNames;
        private RadTreeView TreeView => configureFragmentsForm.TreeView;
        private XmlDocument XmlDocument => configureFragmentsForm.XmlDocument;

        public ConfigureFragmentsAddFragmentCommand(
            IFragmentItemFactory fragmentFactory,
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureFragmentsForm configureFragmentsForm)
        {
            _configureFragmentsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureFragmentsXmlTreeViewSynchronizer
            (
                configureFragmentsForm
            );
            _fragmentFactory = fragmentFactory;
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFragmentsForm = configureFragmentsForm;
        }

        public override void Execute()
        {
            try
            {
                configureFragmentsForm.ClearMessage();
                TreeView.BeginUpdate();
                AddFragment();
            }
            catch (LogicBuilderException ex)
            {
                configureFragmentsForm.SetErrorMessage(ex.Message);
            }
            finally
            {
                TreeView.EndUpdate();
            }
        }

        private void AddFragment()
        {
            RadTreeNode? selectedNode = TreeView.SelectedNode;
            if (selectedNode == null)
                return;

            if (!_treeViewService.IsFolderNode(selectedNode) && !_treeViewService.IsFileNode(selectedNode))
                return;

            RadTreeNode destinationFolderNode = _treeViewService.IsFileNode(selectedNode) ? selectedNode.Parent : selectedNode;

            string fragmentName = _stringHelper.EnsureUniqueName(Strings.defaultNewFragmentName, FragmentNames);

            _configureFragmentsXmlTreeViewSynchronizer.AddFragmentNode
            (
                destinationFolderNode,
                _xmlDocumentHelpers.AddElementToDoc
                (
                    XmlDocument,
                    _xmlDocumentHelpers.ToXmlElement
                    (
                        _fragmentFactory.GetFragment
                        (
                            _stringHelper.EnsureUniqueName(fragmentName, FragmentNames),
                            BuildFragmentXml(),
                            string.Empty
                        ).ToXml
                    )
                )
            );

            string BuildFragmentXml()
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
                {
                    xmlTextWriter.WriteStartElement(XmlDataConstants.VARIABLEELEMENT);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, fragmentName);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.VISIBLETEXTATTRIBUTE, fragmentName);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.Flush();
                }

                return stringBuilder.ToString();
            }
        }
    }
}
