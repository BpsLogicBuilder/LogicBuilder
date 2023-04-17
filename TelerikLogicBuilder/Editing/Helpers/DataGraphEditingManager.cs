using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal class DataGraphEditingManager : IDataGraphEditingManager
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEditingControlFactory _editingControlFactory;
        private readonly IEditingFormCommandFactory _editingFormCommandFactory;
        private readonly IParametersDataTreeBuilder _parametersDataTreeBuilder;
        private readonly IEditFormFieldSetHelper _editFormFieldSetHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingHost dataGraphEditingHost;

        private readonly RadMenuItem mnuItemCopyXml = new(Strings.mnuItemCopyXml);
        private readonly RadMenuItem mnuItemAddXmlToFragments = new(Strings.mnuItemAddXmlToFragments);

        private static readonly HashSet<ParametersDataElementType> eligibleForFragmentCopy = new()
        {
            ParametersDataElementType.Constructor,
            ParametersDataElementType.AssertFunction,
            ParametersDataElementType.Function,
            ParametersDataElementType.NotFunction,
            ParametersDataElementType.ParameterLiteralList,
            ParametersDataElementType.ParameterObjectList,
            ParametersDataElementType.RetractFunction,
            ParametersDataElementType.VariableLiteralList,
            ParametersDataElementType.VariableObjectList
        };

        public DataGraphEditingManager(
            IConfigurationService configurationService,
            IEditFormFieldSetHelper editFormFieldSetHelper,
            IEditingControlFactory editingControlFactory,
            IEditingFormCommandFactory editingFormCommandFactory,
            IEditingFormHelperFactory editingFormHelperFactory,
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingHost dataGraphEditingHost)
        {
            _configurationService = configurationService;
            _editFormFieldSetHelper = editFormFieldSetHelper;
            _editingControlFactory = editingControlFactory;
            _editingFormCommandFactory = editingFormCommandFactory;
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _parametersDataTreeBuilder = editingFormHelperFactory.GetParametersDataTreeBuilder(dataGraphEditingHost);
            this.dataGraphEditingHost = dataGraphEditingHost;
        }

        private RadPanel RadPanelFields => dataGraphEditingHost.RadPanelFields;
        private RadTreeView TreeView => dataGraphEditingHost.TreeView;
        private IEditingControl CurrentEditingControl
        {
            get
            {
                if (RadPanelFields.Controls.Count != 1)
                    throw _exceptionHelper.CriticalException("{202AA525-3FA7-4B24-A1E8-4C52662B2005}");

                return (IEditingControl)RadPanelFields.Controls[0];
            }
        }
        private XmlDocument XmlDocument => dataGraphEditingHost.XmlDocument;

        public void CreateContextMenus()
        {
            AddContextMenuClickCommand(mnuItemAddXmlToFragments, _editingFormCommandFactory.GetAddXMLToFragmentsConfigurationCommand(dataGraphEditingHost));
            AddContextMenuClickCommand(mnuItemCopyXml, _editingFormCommandFactory.GetCopyXmlToClipboardCommand(dataGraphEditingHost));
            TreeView.RadContextMenu = new()
            {
                Items =
                {
                    new RadMenuSeparatorItem(),
                    mnuItemCopyXml,
                    new RadMenuSeparatorItem(),
                    mnuItemAddXmlToFragments,
                    new RadMenuSeparatorItem(),
                }
            };
        }

        public void RequestDocumentUpdate(IEditingControl editingControl)
        {
            if (TreeView.SelectedNode == null
                || !object.ReferenceEquals(CurrentEditingControl, editingControl))//may need to use an event like changed (may run before the control is loaded/activated)
                return;                                                             //instead of validated

            try
            {
                dataGraphEditingHost.ClearMessage();
                UpdateXmlDocument(TreeView.SelectedNode);
                RefreshTreeNode((ParametersDataTreeNode)TreeView.SelectedNode);
            }
            catch (XmlException ex)
            {
                dataGraphEditingHost.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                dataGraphEditingHost.SetErrorMessage(ex.Message);
            }
        }

        public void SetContextMenuState(ParametersDataTreeNode treeNode)
        {
            bool enabled = eligibleForFragmentCopy.Contains(treeNode.XmlElementType);
            mnuItemAddXmlToFragments.Enabled = enabled;
            mnuItemCopyXml.Enabled = enabled;
        }

        public void SetControlValues(ParametersDataTreeNode selectedNode)
        {
            EditFormFieldSet fieldSet = GetEditFormFieldSet(selectedNode);
            switch (fieldSet)
            {
                case EditFormFieldSet.Constructor:
                    Navigate(GetConstructorControl());
                    break;
                case EditFormFieldSet.StandardFunction:
                    Navigate(GetStandardFunctionControl());
                    break;
                case EditFormFieldSet.BinaryFunction:
                    Navigate(GetBinaryFunctionControl());
                    break;
                case EditFormFieldSet.SetValueFunction:
                    Navigate(GetSetValueFunctionControl());
                    break;
                case EditFormFieldSet.SetValueToNullFunction:
                    Navigate(GetSetValueToNullFunctionControl());
                    break;
                case EditFormFieldSet.Variable:
                    Navigate(GetVariableControl());
                    break;
                case EditFormFieldSet.ParameterLiteralList:
                    Navigate(GetParameterLiteralListControl());
                    break;
                case EditFormFieldSet.ParameterObjectList:
                    Navigate(GetParameterObjectListControl());
                    break;
                case EditFormFieldSet.VariableLiteralList:
                    Navigate(GetVariableLiteralListControl());
                    break;
                case EditFormFieldSet.VariableObjectList:
                    Navigate(GetVariableObjectListControl());
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{21C63ED5-DAC3-49A1-9447-C99FD2DCAD1D}");
            }

            IEditingControl GetConstructorControl()
            {
                ConstructorElementTreeNode? constructorTreeNodeNode;
                string? selectedParameter;
                if (selectedNode is IParameterElementTreeNode parameterNode)
                {
                    constructorTreeNodeNode = (ConstructorElementTreeNode)selectedNode.Parent;
                    selectedParameter = parameterNode.ParameterName;
                }
                else
                {
                    constructorTreeNodeNode = (ConstructorElementTreeNode)selectedNode;
                    selectedParameter = null;
                }

                return _editingControlFactory.GetEditConstructorControl
                (
                    dataGraphEditingHost,
                    _configurationService.ConstructorList.Constructors[constructorTreeNodeNode.ConstructorName],
                    constructorTreeNodeNode.AssignedToType,
                    dataGraphEditingHost.XmlDocument,
                    constructorTreeNodeNode.Name,
                    selectedParameter
                );
            }

            IEditingControl GetStandardFunctionControl()
            {
                FunctionElementTreeNode? functionTreeNodeNode;
                string? selectedParameter;
                if (selectedNode is IParameterElementTreeNode parameterNode)
                {
                    functionTreeNodeNode = (FunctionElementTreeNode)selectedNode.Parent;
                    selectedParameter = parameterNode.ParameterName;
                }
                else
                {
                    functionTreeNodeNode = (FunctionElementTreeNode)selectedNode;
                    selectedParameter = null;
                }

                return _editingControlFactory.GetEditStandardFunctionControl
                (
                    dataGraphEditingHost,
                    _configurationService.FunctionList.Functions[functionTreeNodeNode.FunctionName],
                    functionTreeNodeNode.AssignedToType,
                    dataGraphEditingHost.XmlDocument,
                    functionTreeNodeNode.Name,
                    selectedParameter
                );
            }

            IEditingControl GetBinaryFunctionControl()
            {
                FunctionElementTreeNode? functionTreeNodeNode;
                string? selectedParameter;
                if (selectedNode is IParameterElementTreeNode parameterNode)
                {
                    functionTreeNodeNode = (FunctionElementTreeNode)selectedNode.Parent;
                    selectedParameter = parameterNode.ParameterName;
                }
                else
                {
                    functionTreeNodeNode = (FunctionElementTreeNode)selectedNode;
                    selectedParameter = null;
                }

                return _editingControlFactory.GetEditBinaryFunctionControl
                (
                    dataGraphEditingHost,
                    _configurationService.FunctionList.Functions[functionTreeNodeNode.FunctionName],
                    functionTreeNodeNode.AssignedToType,
                    dataGraphEditingHost.XmlDocument,
                    functionTreeNodeNode.Name,
                    selectedParameter
                );
            }

            IEditingControl GetSetValueFunctionControl()
            {
                AssertFunctionElementTreeNode? functionTreeNodeNode;
                if (selectedNode is IVariableElementTreeNode)
                {
                    functionTreeNodeNode = (AssertFunctionElementTreeNode)selectedNode.Parent;
                }
                else
                {
                    functionTreeNodeNode = (AssertFunctionElementTreeNode)selectedNode;
                }

                return _editingControlFactory.GetEditSetValueFunctionControl
                (
                    dataGraphEditingHost,
                    _configurationService.FunctionList.Functions[functionTreeNodeNode.FunctionName],
                    dataGraphEditingHost.XmlDocument,
                    functionTreeNodeNode.Name
                );
            }

            IEditingControl GetSetValueToNullFunctionControl()
            {
                RetractFunctionElementTreeNode functionTreeNodeNode = (RetractFunctionElementTreeNode)selectedNode;

                return _editingControlFactory.GetEditSetValueToNullFunctionControl
                (
                    dataGraphEditingHost,
                    _configurationService.FunctionList.Functions[functionTreeNodeNode.FunctionName],
                    functionTreeNodeNode.AssignedToType,
                    dataGraphEditingHost.XmlDocument,
                    functionTreeNodeNode.Name
                );
            }

            IEditingControl GetVariableControl()
            {
                VariableElementTreeNode variableElementTreeNode = (VariableElementTreeNode)selectedNode;
                IEditVariableControl variableControl = _editingControlFactory.GetEditVariableControl
                (
                    dataGraphEditingHost,
                    variableElementTreeNode.AssignedToType
                );
                variableControl.SetVariable(variableElementTreeNode.VariableName);
                return variableControl;
            }

            IEditingControl GetParameterLiteralListControl()
            {
                ParameterLiteralListElementTreeNode? literalListElementTreeNode;
                int? selectedIndex;

                if (selectedNode is LiteralElementTreeNode itemNode)
                {
                    literalListElementTreeNode = (ParameterLiteralListElementTreeNode)selectedNode.Parent;
                    selectedIndex = itemNode.NodeIndex;
                }
                else
                {
                    literalListElementTreeNode = (ParameterLiteralListElementTreeNode)selectedNode;
                    selectedIndex = null;
                }

                return _editingControlFactory.GetEditParameterLiteralListControl
                (
                    dataGraphEditingHost,
                    literalListElementTreeNode.ListInfo,
                    literalListElementTreeNode.AssignedToType,
                    dataGraphEditingHost.XmlDocument,
                    literalListElementTreeNode.Name,
                    selectedIndex
                );
            }

            IEditingControl GetParameterObjectListControl()
            {
                ParameterObjectListElementTreeNode? objectListElementTreeNode;
                int? selectedIndex;

                if (selectedNode is ObjectElementTreeNode itemNode)
                {
                    objectListElementTreeNode = (ParameterObjectListElementTreeNode)selectedNode.Parent;
                    selectedIndex = itemNode.NodeIndex;
                }
                else
                {
                    objectListElementTreeNode = (ParameterObjectListElementTreeNode)selectedNode;
                    selectedIndex = null;
                }

                return _editingControlFactory.GetEditParameterObjectListControl
                (
                    dataGraphEditingHost,
                    objectListElementTreeNode.ListInfo,
                    objectListElementTreeNode.AssignedToType,
                    dataGraphEditingHost.XmlDocument,
                    objectListElementTreeNode.Name,
                    selectedIndex
                );
            }

            IEditingControl GetVariableLiteralListControl()
            {
                VariableLiteralListElementTreeNode? literalListElementTreeNode;
                int? selectedIndex;

                if (selectedNode is LiteralElementTreeNode itemNode)
                {
                    literalListElementTreeNode = (VariableLiteralListElementTreeNode)selectedNode.Parent;
                    selectedIndex = itemNode.NodeIndex;
                }
                else
                {
                    literalListElementTreeNode = (VariableLiteralListElementTreeNode)selectedNode;
                    selectedIndex = null;
                }

                return _editingControlFactory.GetEditVariableLiteralListControl
                (
                    dataGraphEditingHost,
                    literalListElementTreeNode.ListInfo,
                    literalListElementTreeNode.AssignedToType,
                    dataGraphEditingHost.XmlDocument,
                    literalListElementTreeNode.Name,
                    selectedIndex
                );
            }

            IEditingControl GetVariableObjectListControl()
            {
                VariableObjectListElementTreeNode? objectListElementTreeNode;
                int? selectedIndex;

                if (selectedNode is ObjectElementTreeNode itemNode)
                {
                    objectListElementTreeNode = (VariableObjectListElementTreeNode)selectedNode.Parent;
                    selectedIndex = itemNode.NodeIndex;
                }
                else
                {
                    objectListElementTreeNode = (VariableObjectListElementTreeNode)selectedNode;
                    selectedIndex = null;
                }

                return _editingControlFactory.GetEditVariableObjectListControl
                (
                    dataGraphEditingHost,
                    objectListElementTreeNode.ListInfo,
                    objectListElementTreeNode.AssignedToType,
                    dataGraphEditingHost.XmlDocument,
                    objectListElementTreeNode.Name,
                    selectedIndex
                );
            }
        }

        public void UpdateXmlDocument(RadTreeNode selectedNode)
        {
            CurrentEditingControl.ValidateFields();

            _xmlDocumentHelpers.ReplaceElement
            (
                _xmlDocumentHelpers.SelectSingleElement
                (
                    XmlDocument,
                    GetControlXPathTreeNode((ParametersDataTreeNode)selectedNode).Name
                ),
                CurrentEditingControl.XmlResult
            );

            dataGraphEditingHost.ValidateXmlDocument();
        }

        private static void AddContextMenuClickCommand(RadMenuItem radMenuItem, IClickCommand command)
        {
            radMenuItem.Click += (sender, args) => command.Execute();
        }

        private ParametersDataTreeNode GetControlXPathTreeNode(ParametersDataTreeNode selectedNode)
            => GetEditFormFieldSet(selectedNode) switch
            {
                EditFormFieldSet.Constructor 
                    or EditFormFieldSet.StandardFunction 
                    or EditFormFieldSet.BinaryFunction 
                    or EditFormFieldSet.SetValueFunction 
                    or EditFormFieldSet.SetValueToNullFunction 
                    => selectedNode is IParameterElementTreeNode 
                        || selectedNode is IVariableElementTreeNode 
                        ? selectedNode.Parent 
                        : selectedNode,
                EditFormFieldSet.Variable => selectedNode,
                EditFormFieldSet.ParameterLiteralList => selectedNode is LiteralElementTreeNode ? selectedNode.Parent : selectedNode,
                EditFormFieldSet.ParameterObjectList => selectedNode is ObjectElementTreeNode ? selectedNode.Parent : selectedNode,
                EditFormFieldSet.VariableLiteralList => selectedNode is LiteralElementTreeNode ? selectedNode.Parent : selectedNode,
                EditFormFieldSet.VariableObjectList => selectedNode is ObjectElementTreeNode ? selectedNode.Parent : selectedNode,
                _ => throw _exceptionHelper.CriticalException("{21C63ED5-DAC3-49A1-9447-C99FD2DCAD1D}"),
            };

        private EditFormFieldSet GetEditFormFieldSet(ParametersDataTreeNode selectedNode)
        {
            switch (selectedNode)
            {
                case AssertFunctionElementTreeNode:
                    return EditFormFieldSet.SetValueFunction;
                case ConstructorElementTreeNode:
                    return EditFormFieldSet.Constructor;
                case FunctionElementTreeNode functionElementTreeNode:
                    return _editFormFieldSetHelper.GetFieldSetForFunction(_configurationService.FunctionList.Functions[functionElementTreeNode.Text]);
                case LiteralElementTreeNode literalElementTreeNode:
                    if (literalElementTreeNode.Parent is ParameterLiteralListElementTreeNode)
                        return EditFormFieldSet.ParameterLiteralList;
                    else if (literalElementTreeNode.Parent is VariableLiteralListElementTreeNode)
                        return EditFormFieldSet.VariableLiteralList;

                    throw _exceptionHelper.CriticalException("{CF8A117A-81FB-4745-B17F-7CE0978C3C52}");
                case ListOfLiteralsParameterElementTreeNode:
                case LiteralParameterElementTreeNode:
                case ListOfObjectsParameterElementTreeNode:
                case ObjectParameterElementTreeNode:
                    return selectedNode.Parent switch
                    {
                        ConstructorElementTreeNode => EditFormFieldSet.Constructor,
                        FunctionElementTreeNode functionElementTreeNode => _editFormFieldSetHelper.GetFieldSetForFunction(_configurationService.FunctionList.Functions[functionElementTreeNode.Text]),
                        _ => throw _exceptionHelper.CriticalException("{16317437-3740-43E2-8C12-C49612F081E2}"),
                    };
                case ListOfLiteralsVariableElementTreeNode:
                case LiteralVariableElementTreeNode:
                case ListOfObjectsVariableElementTreeNode:
                case ObjectVariableElementTreeNode:
                    return selectedNode.Parent switch
                    {
                        AssertFunctionElementTreeNode => EditFormFieldSet.SetValueFunction,
                        _ => throw _exceptionHelper.CriticalException("{16317437-3740-43E2-8C12-C49612F081E2}"),
                    };
                case ObjectElementTreeNode objectElementTreeNode:
                    if (objectElementTreeNode.Parent is ParameterObjectListElementTreeNode)
                        return EditFormFieldSet.ParameterObjectList;
                    else if (objectElementTreeNode.Parent is VariableObjectListElementTreeNode)
                        return EditFormFieldSet.VariableObjectList;

                    throw _exceptionHelper.CriticalException("{0CD15472-4879-4AED-B2C9-288328A11A94}");
                case ParameterLiteralListElementTreeNode:
                    return EditFormFieldSet.ParameterLiteralList;
                case ParameterObjectListElementTreeNode:
                    return EditFormFieldSet.ParameterObjectList;
                case RetractFunctionElementTreeNode:
                    return EditFormFieldSet.SetValueToNullFunction;
                case VariableElementTreeNode:
                    return EditFormFieldSet.Variable;
                case VariableLiteralListElementTreeNode:
                    return EditFormFieldSet.VariableLiteralList;
                case VariableObjectListElementTreeNode:
                    return EditFormFieldSet.VariableObjectList;
                default:
                    throw _exceptionHelper.CriticalException("{54AB80FD-B8CA-48A2-AD5E-56314EC35CBA}");
            }
        }

        private void Navigate(IEditingControl editingControl)
        {
            NavigationUtility.Navigate(((Control)dataGraphEditingHost).Handle, RadPanelFields, (Control)editingControl);
        }

        private void RefreshTreeNode(ParametersDataTreeNode selectedNode)
        {
            ParametersDataTreeNode xPathTreeNode = GetControlXPathTreeNode(selectedNode);
            if (xPathTreeNode is VariableElementTreeNode variableElementTree)
            {
                RefreshVariableTreeNode(variableElementTree);
                return;
            }

            _parametersDataTreeBuilder.RefreshTreeNode(TreeView, XmlDocument, xPathTreeNode);
            if (object.ReferenceEquals(selectedNode, xPathTreeNode))
                return;

            //TreeView.SelectedNodeChanged -= TreeView_SelectedNodeChanged;
            _treeViewService.SelectTreeNode(TreeView, selectedNode.Name);
            //TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;
            if (TreeView.SelectedNode != null)
                _treeViewService.MakeVisible(TreeView.SelectedNode);
        }

        private void RefreshVariableTreeNode(VariableElementTreeNode treeNode)
        {
            Refresh(CurrentEditingControl.XmlResult.GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            void Refresh(string variableName)
            {
                treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.VARIABLEELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{variableName}\"]";
                treeNode.Text = variableName;
                treeNode.ToolTipText = variableName;
            }
        }
    }
}
