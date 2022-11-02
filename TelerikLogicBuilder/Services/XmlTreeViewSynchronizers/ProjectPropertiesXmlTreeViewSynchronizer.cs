using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class ProjectPropertiesXmlTreeViewSynchronizer : IProjectPropertiesXmlTreeViewSynchronizer
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureProjectProperties configureProjectProperties;

        public ProjectPropertiesXmlTreeViewSynchronizer(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureProjectProperties configureProjectProperties)
        {
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureProjectProperties = configureProjectProperties;
        }

        public void AddApplicationNode(RadTreeNode destinationParentNode, XmlElement newXmlApplicationNode)
        {
            _xmlDocumentHelpers
                .SelectSingleElement(this.configureProjectProperties.XmlDocument, destinationParentNode.Name)
                .PrependChild(newXmlApplicationNode);

            this.configureProjectProperties.OnInvalidXmlRestoreDocumentAndThrow();

            RadTreeNode newTreeNode = _treeViewService.GetChildTreeNode
            (
                destinationParentNode,
                newXmlApplicationNode.Name,
                XmlDataConstants.NAMEATTRIBUTE,
                newXmlApplicationNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                ImageIndexes.APPLICATIONIMAGEINDEX
            );

            destinationParentNode.Nodes.Insert
            (
                _treeViewService.GetInsertPosition
                (
                    destinationParentNode.Nodes.ToArray(), 
                    newTreeNode, 
                    new ApplicationTreeNodeComparer(_treeViewService)
                ), 
                newTreeNode
            );

            _treeViewService.MakeVisible(newTreeNode);
            newTreeNode.TreeView.SelectedNode = newTreeNode;
        }

        public void DeleteNode(RadTreeNode treeNode)
        {
            RadTreeNode closestNode = _treeViewService.GetClosestNodeForSelectionAfterDelete(treeNode);
            this.configureProjectProperties.TreeView.SelectedNode = null;

            XmlNode xmlNodeToDelete = _xmlDocumentHelpers.SelectSingleElement
            (
                this.configureProjectProperties.XmlDocument, 
                treeNode.Name
            );
            (
                xmlNodeToDelete.ParentNode ?? throw _exceptionHelper.CriticalException("{94F17B9D-30F3-4140-8AAE-7A7403021070}")
            )
            .RemoveChild(xmlNodeToDelete);

            this.configureProjectProperties.OnInvalidXmlRestoreDocumentAndThrow();

            RadTreeNode parentNode = treeNode.Parent;
            treeNode.Remove();
            if (parentNode.Nodes.Count == 0)
                parentNode.Collapse();

            this.configureProjectProperties.TreeView.SelectedNode = closestNode;
        }
    }

    class ApplicationTreeNodeComparer : IComparer<RadTreeNode>
    {
        readonly ITreeViewService service;

        public ApplicationTreeNodeComparer(ITreeViewService service)
        {
            this.service = service;
        }

        public int Compare(RadTreeNode? treeNodeA, RadTreeNode? treeNodeB)
        {
            if (treeNodeA == null || treeNodeB == null)
                throw new InvalidOperationException("{58A0B075-2FA3-4935-84CC-F1AFDBFF69DC}");

            if (service.IsApplicationNode(treeNodeA) && service.IsApplicationNode(treeNodeB))
                return string.Compare(treeNodeA.Text, treeNodeB.Text);
            else
                throw new InvalidOperationException("{38D434EC-85F1-480F-8A86-C968A6492CD7}");
        }
    }
}
