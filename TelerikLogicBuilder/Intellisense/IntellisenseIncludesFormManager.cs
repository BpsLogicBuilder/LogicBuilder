using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.IncludesHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense
{
    internal partial class IntellisenseIncludesFormManager : IIntellisenseIncludesFormManager
    {
        private readonly IImageListService _imageListService;
        private readonly IIntellisenseHelper _intellisenseHelper;
        private readonly IIntellisenseTreeNodeFactory _intellisenseTreeNodeFactory;
        private readonly ITypeAutoCompleteManager _cmbClassTypeAutoCompleteManager;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IIncludesHelperForm includesHelperForm;

        private ApplicationTypeInfo Application => includesHelperForm.Application;
        private AutoCompleteRadDropDownList CmbClass => includesHelperForm.CmbClass;
        private VariableTreeNode ReferenceTreeNode => includesHelperForm.ReferenceTreeNode;
        private RadTreeView TreeView => includesHelperForm.TreeView;

        public IntellisenseIncludesFormManager(
            IImageListService imageListService,
            IIntellisenseHelper intellisenseHelper,
            IIntellisenseTreeNodeFactory intellisenseTreeNodeFactory,
            IServiceFactory serviceFactory,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IIncludesHelperForm includesHelperForm)
        {
            _imageListService = imageListService;
            _intellisenseHelper = intellisenseHelper;
            _intellisenseTreeNodeFactory = intellisenseTreeNodeFactory;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            this.includesHelperForm = includesHelperForm;
            _cmbClassTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                includesHelperForm,
                CmbClass
            );
        }

        public string ReferenceDefinition => ReferenceTreeNode.ReferenceDefinition;

        public string ReferenceName => ReferenceTreeNode.ReferenceName;

        public void AddChildren(VariableTreeNode treeNode)
        {
            if (treeNode.Nodes.Count > 0)
                return;

            List<BaseTreeNode> infoList = new();
            _intellisenseHelper.AddArrayIndexer(infoList, treeNode.MemberType, treeNode, includesHelperForm);
            _intellisenseHelper.AddFields(infoList, treeNode.MemberType, treeNode, BindingFlagCategory.Instance, includesHelperForm);
            _intellisenseHelper.AddProperties(infoList, treeNode.MemberType, treeNode, BindingFlagCategory.Instance, includesHelperForm);
            treeNode.Nodes.AddRange(infoList.OrderBy(n => n.Text));
        }

        public void ApplicationChanged()
        {
            ClearTreeView();

            if (!Application.AssemblyAvailable)
            {
                includesHelperForm.SetErrorMessage(Application.UnavailableMessage);
                return;
            }

            BuildTreeView(CmbClass.Text.Trim());
        }

        public void BeforeCollapse(BaseTreeNode treeNode)
        {
            TreeView.BeginUpdate();
            foreach (RadTreeNode childNode in treeNode.Nodes)
            {
                if (childNode.Expanded)
                    childNode.Collapse();
                childNode.Nodes.Clear();
            }
            TreeView.EndUpdate();
        }

        public void BeforeExpand(BaseTreeNode treeNode)
        {
            if (!Application.AssemblyAvailable)
                return;

            TreeView.SelectedNode = treeNode;
            TreeView.BeginUpdate();
            foreach (VariableTreeNode childNode in treeNode.Nodes.OfType<VariableTreeNode>())
                AddChildren(childNode);

            TreeView.EndUpdate();
        }

        public void BuildTreeView(string classFullName)
        {
            includesHelperForm.ClearMessage();
            if (!_typeLoadHelper.TryGetSystemType(classFullName, Application, out Type? type))
            {
                includesHelperForm.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadClassFormat, classFullName));
                return;
            }

            TreeView.Nodes.Clear();
            BuildReferencesTreeView(type);

            if (TreeView.Nodes.Count > 0)
                TreeView.SelectedNode = TreeView.Nodes[0];
        }

        public void ClearTreeView()
        {
            includesHelperForm.ClearTreeView();
        }

        public void CmbClassTextChanged()
        {
            ClearTreeView();

            if (!Application.AssemblyAvailable)
            {
                includesHelperForm.SetErrorMessage(Application.UnavailableMessage);
                return;
            }

            BuildTreeView(CmbClass.Text.Trim());
        }

        public void Initialize()
        {
            TreeView.ImageList = _imageListService.ImageList;
            TreeView.TreeViewElement.ShowNodeToolTips = true;
            TreeView.ShowRootLines = true;
            TreeView.HideSelection = false;
            _cmbClassTypeAutoCompleteManager.Setup();
            includesHelperForm.ValidateOk();
        }

        private void BuildReferencesTreeView(Type type)
        {
            TreeView.BeginUpdate();
            List<BaseTreeNode> infoList = new();
            BindingFlagCategory bindingFlagCategory = GetBindingFlagCategory();
            foreach (FieldInfo fInfo in type.GetFields(_intellisenseHelper.GetBindingFlags(bindingFlagCategory)))
            {
                if (fInfo.Name.Contains(MiscellaneousConstants.PERIODSTRING))
                    continue;
                if (!VariableOrFunctionNameRegex().IsMatch(fInfo.Name))
                    continue;

                //Don't check for private members - we know this is the root.

                AddTreeNode
                (
                    _intellisenseTreeNodeFactory.GetFieldTreeNode(fInfo, null, includesHelperForm)
                );
            }

            foreach (PropertyInfo pInfo in type.GetProperties(_intellisenseHelper.GetBindingFlags(bindingFlagCategory)))
            {
                if (pInfo.Name.Contains(MiscellaneousConstants.PERIODSTRING))
                    continue;
                if (!VariableOrFunctionNameRegex().IsMatch(pInfo.Name))
                    continue;

                //Don't check for private members - we know this is the root.

                ParameterInfo[] pArray = pInfo.GetIndexParameters();
                if (pArray.Length == 1 && _typeHelper.IsValidIndex(pArray[0].ParameterType))
                {
                    AddTreeNode
                    (
                        _intellisenseTreeNodeFactory.GetIndexerTreeNode(pInfo, null, pArray[0].ParameterType, includesHelperForm)
                    );
                }
                else
                {
                    AddTreeNode
                    (
                        _intellisenseTreeNodeFactory.GetPropertyTreeNode(pInfo, null, includesHelperForm)
                    );
                }
            }

            TreeView.Nodes.AddRange
            (
                infoList.OrderBy(n => n.Text)
            );

            TreeView.EndUpdate();
            void AddTreeNode(VariableTreeNode treeNode)
            {
                infoList.Add(treeNode);
                AddChildren(treeNode);
            }

            BindingFlagCategory GetBindingFlagCategory() => BindingFlagCategory.Instance;
        }

        [GeneratedRegex(RegularExpressions.VARIABLEORFUNCTIONNAME)]
        private static partial Regex VariableOrFunctionNameRegex();
    }
}
