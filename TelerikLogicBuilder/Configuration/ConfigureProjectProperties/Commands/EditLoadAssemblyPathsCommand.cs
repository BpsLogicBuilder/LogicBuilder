using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Commands
{
    internal class EditLoadAssemblyPathsCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;
        private readonly IApplicationControl applicationControl;

        public EditLoadAssemblyPathsCommand(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IApplicationControl applicationControl)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _treeViewService = treeViewService;
            treeView = applicationControl.TreeView;
            xmlDocument = applicationControl.XmlDocument;
            this.applicationControl = applicationControl;
        }

        public override void Execute()
        {
            RadTreeNode selecteNode = treeView.SelectedNode;
            if (selecteNode == null)
                return;

            if (!_treeViewService.IsApplicationNode(selecteNode))
                throw _exceptionHelper.CriticalException("{F51FEA03-D7DC-4514-93BC-61DCB915E8B4}");

            IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
            using IConfigureLoadAssemblyPathsForm configureLoadAssemblyPaths = disposableManager.GetConfigureLoadAssemblyPaths
            (
                _xmlDocumentHelpers.GetChildElements
                (
                    _xmlDocumentHelpers.SelectSingleElement
                    (
                        xmlDocument,
                        $"{selecteNode.Name}/{XmlDataConstants.LOADASSEMBLYPATHSELEMENT}"
                    )
                )
                .Select(e => e.InnerText)
                .ToArray()
            );

            configureLoadAssemblyPaths.ShowDialog((Control)applicationControl);
            if (configureLoadAssemblyPaths.DialogResult != DialogResult.OK)
                return;

            XmlElement elementToUpdate = _xmlDocumentHelpers.ToXmlElement
            (
                BuildLoadAssemblyPathsXml(configureLoadAssemblyPaths.Paths)
            );

            _xmlDocumentHelpers
                .SelectSingleElement(xmlDocument, $"{selecteNode.Name}/{XmlDataConstants.LOADASSEMBLYPATHSELEMENT}")
                .InnerXml = elementToUpdate.InnerXml;
        }

        private string BuildLoadAssemblyPathsXml(IList<string> paths)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.LOADASSEMBLYPATHSELEMENT);
                foreach (string path in paths)
                    xmlTextWriter.WriteElementString(XmlDataConstants.PATHELEMENT, path);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
            return stringBuilder.ToString();
        }
    }
}
