using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal class DataGraphEditingFormEventsHelper : IDataGraphEditingFormEventsHelper
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEditingControlFactory _editingControlFactory;
        private readonly IExceptionHelper _exceptionHelper;

        private readonly IDataGraphEditingForm dataGraphEditingForm;

        public DataGraphEditingFormEventsHelper(
            IConfigurationService configurationService,
            IEditingControlFactory editingControlFactory,
            IExceptionHelper exceptionHelper,
            IDataGraphEditingForm dataGraphEditingForm)
        {
            _configurationService = configurationService;
            _editingControlFactory = editingControlFactory;
            _exceptionHelper = exceptionHelper;
            this.dataGraphEditingForm = dataGraphEditingForm;
        }

        private RadPanel RadPanelFields => dataGraphEditingForm.RadPanelFields;
        private RadTreeView TreeView => dataGraphEditingForm.TreeView;

        public void Setup()
        {
            TreeView.MouseDown += TreeView_MouseDown;
            TreeView.NodeExpandedChanged += TreeView_NodeExpandedChanged;
            TreeView.NodeExpandedChanging += TreeView_NodeExpandedChanging;
            TreeView.NodeMouseClick += TreeView_NodeMouseClick;
            TreeView.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;
            TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;
            TreeView.SelectedNodeChanging += TreeView_SelectedNodeChanging;
        }

        private EditFormFieldSet GetEditFormFieldSet(ParametersDataTreeNode selectedNode)
        {
            switch (selectedNode)
            {
                case ConstructorElementTreeNode:
                    return EditFormFieldSet.Constructor;
                case FunctionElementTreeNode functionElementTreeNode:
                    return GetFieldSetForFunction(_configurationService.FunctionList.Functions[functionElementTreeNode.Text]);
                case LiteralElementTreeNode literalElementTreeNode:
                    if (literalElementTreeNode.Parent is LiteralListElementTreeNode)
                        return EditFormFieldSet.LiteralList;

                    throw _exceptionHelper.CriticalException("{CF8A117A-81FB-4745-B17F-7CE0978C3C52}");
                case LiteralListElementTreeNode:
                    return EditFormFieldSet.LiteralList;
                case LiteralListParameterElementTreeNode:
                case LiteralParameterElementTreeNode:
                case ObjectListParameterElementTreeNode:
                case ObjectParameterElementTreeNode:
                    return selectedNode.Parent switch
                    {
                        ConstructorElementTreeNode => EditFormFieldSet.Constructor,
                        FunctionElementTreeNode functionElementTreeNode => GetFieldSetForFunction(_configurationService.FunctionList.Functions[functionElementTreeNode.Text]),
                        _ => throw _exceptionHelper.CriticalException("{16317437-3740-43E2-8C12-C49612F081E2}"),
                    };
                case ObjectElementTreeNode objectElementTreeNode:
                    if (objectElementTreeNode.Parent is ObjectListElementTreeNode)
                        return EditFormFieldSet.ObjectList;

                    throw _exceptionHelper.CriticalException("{0CD15472-4879-4AED-B2C9-288328A11A94}");
                case ObjectListElementTreeNode:
                    return EditFormFieldSet.ObjectList;
                case VariableElementTreeNode:
                    return EditFormFieldSet.Variable;
                default:
                    throw _exceptionHelper.CriticalException("{54AB80FD-B8CA-48A2-AD5E-56314EC35CBA}");
            }
        }

        private EditFormFieldSet GetFieldSetForFunction(Function function)
        {
            switch (function.FunctionCategory)
            {
                case FunctionCategories.Assert:
                case FunctionCategories.DialogForm:
                case FunctionCategories.Retract:
                case FunctionCategories.RuleChainingUpdate:
                    throw _exceptionHelper.CriticalException("{6DE5BBBE-615F-4911-BE0F-DC2902B38206}");
                default:
                    break;
            }

            return function.ParametersLayout == ParametersLayout.Binary
                ? EditFormFieldSet.BinaryFunction
                : EditFormFieldSet.StandardFunction;
        }

        private void Navigate(IEditingControl editingControl)
        {
            Control newControl = (Control)editingControl;
            Native.NativeMethods.LockWindowUpdate(((Control)dataGraphEditingForm).Handle);
            ((ISupportInitialize)RadPanelFields).BeginInit();
            RadPanelFields.SuspendLayout();

            ClearFieldControls();
            newControl.Dock = DockStyle.Fill;
            newControl.Location = new Point(0, 0);
            RadPanelFields.Controls.Add(newControl);

            ((ISupportInitialize)RadPanelFields).EndInit();
            RadPanelFields.ResumeLayout(false);
            RadPanelFields.PerformLayout();

            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);

            void ClearFieldControls()
            {
                foreach (Control control in RadPanelFields.Controls)
                    control.Visible = false;

                RadPanelFields.Controls.Clear();
            }
        }

        private void SetControlValues(ParametersDataTreeNode selectedNode)
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
                case EditFormFieldSet.Variable:
                    Navigate(GetVariableControl());
                    break;
                case EditFormFieldSet.LiteralList:
                    Navigate(GetLiteralListControl());
                    break;
                case EditFormFieldSet.ObjectList:
                    Navigate(GetObjectListControl());
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
                    dataGraphEditingForm,
                    _configurationService.ConstructorList.Constructors[constructorTreeNodeNode.ConstructorName],
                    constructorTreeNodeNode.AssignedToType,
                    dataGraphEditingForm.XmlDocument,
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
                    dataGraphEditingForm,
                    _configurationService.FunctionList.Functions[functionTreeNodeNode.FunctionName],
                    functionTreeNodeNode.AssignedToType,
                    dataGraphEditingForm.XmlDocument,
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
                    dataGraphEditingForm,
                    _configurationService.FunctionList.Functions[functionTreeNodeNode.FunctionName],
                    functionTreeNodeNode.AssignedToType,
                    dataGraphEditingForm.XmlDocument,
                    functionTreeNodeNode.Name,
                    selectedParameter
                );
            }

            IEditingControl GetVariableControl()
            {
                VariableElementTreeNode variableElementTreeNode = (VariableElementTreeNode)selectedNode;
                IEditVariableControl variableControl = _editingControlFactory.GetEditVariableControl
                (
                    dataGraphEditingForm,
                    variableElementTreeNode.AssignedToType
                );
                variableControl.SetVariable(variableElementTreeNode.VariableName);
                return variableControl;
            }

            IEditingControl GetLiteralListControl()
            {
                LiteralListElementTreeNode? literalListElementTreeNode;
                int? selectedIndex;

                if (selectedNode is LiteralElementTreeNode itemNode)
                {
                    literalListElementTreeNode = (LiteralListElementTreeNode)selectedNode.Parent;
                    selectedIndex = itemNode.NodeIndex;
                }
                else
                {
                    literalListElementTreeNode = (LiteralListElementTreeNode)selectedNode;
                    selectedIndex = null;
                }

                return _editingControlFactory.GetEditLiteralListControl
                (
                    dataGraphEditingForm,
                    literalListElementTreeNode.ListInfo,
                    literalListElementTreeNode.AssignedToType,
                    dataGraphEditingForm.XmlDocument,
                    literalListElementTreeNode.Name,
                    selectedIndex
                );
            }

            IEditingControl GetObjectListControl()
            {
                ObjectListElementTreeNode? objectListElementTreeNode;
                int? selectedIndex;

                if (selectedNode is ObjectElementTreeNode itemNode)
                {
                    objectListElementTreeNode = (ObjectListElementTreeNode)selectedNode.Parent;
                    selectedIndex = itemNode.NodeIndex;
                }
                else
                {
                    objectListElementTreeNode = (ObjectListElementTreeNode)selectedNode;
                    selectedIndex = null;
                }

                return _editingControlFactory.GetEditObjectListControl
                (
                    dataGraphEditingForm,
                    objectListElementTreeNode.ListInfo,
                    objectListElementTreeNode.AssignedToType,
                    dataGraphEditingForm.XmlDocument,
                    objectListElementTreeNode.Name,
                    selectedIndex
                );
            }
        }

        private void UpdateXmlDocument(RadTreeNode selectedNode)
        {
        }

        private void TreeView_MouseDown(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            RadTreeNode treeNode = this.TreeView.GetNodeAt(e.Location);
            if (treeNode == null && this.TreeView.Nodes.Count > 0)
            {
                TreeView.SelectedNode = TreeView.Nodes[0];
                //SetContextMenuState
            }
        }

        private void TreeView_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            if (e.Node.Expanded)
            {
                if (!dataGraphEditingForm.ExpandedNodes.ContainsKey(e.Node.Name))
                    dataGraphEditingForm.ExpandedNodes.Add(e.Node.Name, e.Node.Text);
            }
            else
            {
                if (dataGraphEditingForm.ExpandedNodes.ContainsKey(e.Node.Name))
                    dataGraphEditingForm.ExpandedNodes.Remove(e.Node.Name);
            }
        }

        private void TreeView_NodeExpandedChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            if (!e.Node.Expanded)
            {
                if (TreeView.SelectedNode == null && e.Action == RadTreeViewAction.ByMouse)//Select the expanding node if it is null
                    TreeView.SelectedNode = e.Node;//Prevents treeview1.Nodes[0] from being selected instead
            }
        }

        private void TreeView_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            //SetContextMenuState
        }

        private void TreeView_NodeMouseDoubleClick(object sender, RadTreeViewEventArgs e)
        {
            //treeView1.SelectedNode = null;//takes focus off the treeview and allows a text box 
            TreeView.SelectedNode = e.Node;//in the right panel to be selected when AfterSelect runs.
        }

        private void TreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            if (TreeView.SelectedNode == null)
            {
                //this.UnLockWindowUpdate();
                return;
            }

            SetControlValues((ParametersDataTreeNode)e.Node);
        }

        private void TreeView_SelectedNodeChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            try
            {
                if (TreeView.SelectedNode == null
                    || e.Node == null)//Don't update if e.Node is null because
                    return;             //1) The selected node may have been deleted
                                        //2) There is no navigation (i.e. e.Node == null)

                dataGraphEditingForm.ClearMessage();
                UpdateXmlDocument(TreeView.SelectedNode);
            }
            catch (XmlException ex)
            {
                e.Cancel = true;
                dataGraphEditingForm.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                e.Cancel = true;
                dataGraphEditingForm.SetErrorMessage(ex.Message);
            }
        }
    }
}
