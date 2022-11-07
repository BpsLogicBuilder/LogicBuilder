using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System.Globalization;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Forms.Commands
{
    internal class AddApplicationCommand : ClickCommandBase
    {
        private readonly ICreateDefaultApplication _createDefaultApplication;
        private readonly IGetNextApplicationNumber _getNextApplicationNumber;
        private readonly IProjectPropertiesXmlTreeViewSynchronizer _projectPropertiesXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureProjectProperties configureProjectProperties;

        public AddApplicationCommand(
            ICreateDefaultApplication createDefaultApplication,
            IGetNextApplicationNumber getNextApplicationNumber,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureProjectProperties configureProjectProperties)
        {
            _createDefaultApplication = createDefaultApplication;
            _getNextApplicationNumber = getNextApplicationNumber;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            this.configureProjectProperties = configureProjectProperties;

            _projectPropertiesXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetProjectPropertiesXmlTreeViewSynchronizer
            (
                this.configureProjectProperties
            );
        }

        public override void Execute()
        {
            try
            {
                RadTreeNode selecteNode = configureProjectProperties.TreeView.SelectedNode;
                if (selecteNode == null)
                    return;

                RadTreeNode destinationFolderNode = _treeViewService.IsApplicationNode(selecteNode)
                                                            ? selecteNode.Parent
                                                            : selecteNode;

                Application application = _createDefaultApplication.Create
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.applicationNameFormat,
                        _getNextApplicationNumber.Get(destinationFolderNode).ToString("00", CultureInfo.CurrentCulture)
                    )
                );

                XmlElement newApplicationElement = _xmlDocumentHelpers.AddElementToDoc
                (
                    configureProjectProperties.XmlDocument,
                    _xmlDocumentHelpers.ToXmlElement(application.ToXml)
                );

                _projectPropertiesXmlTreeViewSynchronizer.AddApplicationNode
                (
                    destinationFolderNode,
                    newApplicationElement
                );
            }
            catch (LogicBuilderException ex)
            {
                configureProjectProperties.SetErrorMessage(ex.Message);
            }
        }
    }
}
