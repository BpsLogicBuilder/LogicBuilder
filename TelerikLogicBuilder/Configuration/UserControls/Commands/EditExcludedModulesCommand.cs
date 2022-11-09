using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands
{
    internal class EditExcludedModulesCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;
        private readonly IApplicationControl applicationControl;

        public EditExcludedModulesCommand(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IApplicationControl applicationControl)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _treeViewService = treeViewService;
            this.treeView = applicationControl.TreeView;
            this.xmlDocument = applicationControl.XmlDocument;
            this.applicationControl = applicationControl;
        }

        public override void Execute()
        {
            RadTreeNode selecteNode = this.treeView.SelectedNode;
            if (selecteNode == null)
                return;

            if (!_treeViewService.IsApplicationNode(selecteNode))
                throw _exceptionHelper.CriticalException("{BBD9B576-5AB9-402E-87E8-148B8CC62E5A}");

            using IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
            ConfigureExcludedModules configureExcludedModules = disposableManager.GetConfigureExcludedModules
            (
                _xmlDocumentHelpers.GetChildElements
                (
                    _xmlDocumentHelpers.SelectSingleElement
                    (
                        this.xmlDocument, 
                        $"{selecteNode.Name}/{XmlDataConstants.EXCLUDEDMODULESELEMENT}"
                    )
                )
                .Select(e => e.InnerText)
                .ToArray()
            );
            configureExcludedModules.ShowDialog((Control)applicationControl);

            if (configureExcludedModules.DialogResult != DialogResult.OK)
                return;

            XmlElement elementToUpdate = _xmlDocumentHelpers.ToXmlElement
            (
                BuildExcludedModulesXml(configureExcludedModules.ExcludedModules)
            );

            _xmlDocumentHelpers
                .SelectSingleElement(this.xmlDocument, $"{selecteNode.Name}/{XmlDataConstants.EXCLUDEDMODULESELEMENT}")
                .InnerXml = elementToUpdate.InnerXml;
        }

        private string BuildExcludedModulesXml(IList<string> excludedModules)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.EXCLUDEDMODULESELEMENT);

                foreach (string module in excludedModules)
                {
                    xmlTextWriter.WriteStartElement(XmlDataConstants.MODULEELEMENT);
                    xmlTextWriter.WriteString(module);
                    xmlTextWriter.WriteEndElement();
                }
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
            return stringBuilder.ToString();
        }
    }
}
