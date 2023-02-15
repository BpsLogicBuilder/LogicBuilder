using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense
{
    internal abstract partial class IntellisenseConfiguredItemFormManager
    {
        protected abstract IExceptionHelper ExceptionHelper { get; }
        protected abstract IImageListService ImageListService { get; }
        protected abstract IIntellisenseHelper IntellisenseHelper { get; }
        protected abstract IIntellisenseTreeNodeFactory IntellisenseTreeNodeFactory { get; }
        protected abstract IRadDropDownListHelper RadDropDownListHelper { get; }
        protected abstract ITypeAutoCompleteManager CmbClassTypeAutoCompleteManager { get; }
        protected abstract ITypeHelper TypeHelper { get; }
        protected abstract ITypeLoadHelper TypeLoadHelper { get; }

        protected abstract IConfiguredItemHelperForm ConfiguredItemHelperForm { get; }

        private ApplicationTypeInfo Application => ConfiguredItemHelperForm.Application;
        private ApplicationDropDownList CmbApplication => ConfiguredItemHelperForm.CmbApplication;
        protected AutoCompleteRadDropDownList CmbClass => ConfiguredItemHelperForm.CmbClass;
        private RadDropDownList CmbReferenceCategory => ConfiguredItemHelperForm.CmbReferenceCategory;
        private VariableTreeNode ReferenceTreeNode => ConfiguredItemHelperForm.ReferenceTreeNode;
        private RadTreeView TreeView => ConfiguredItemHelperForm.TreeView;

        public string CastReferenceAs
            => ReferenceCategory == ReferenceCategories.InstanceReference || ReferenceCategory == ReferenceCategories.StaticReference
                ? ReferenceTreeNode.CastReferenceDefinition
                : string.Empty;

        public HelperStatus HelperStatus
        {
            get
            {
                List<BaseTreeNode> list = new();
                BaseTreeNode? treeNode = (BaseTreeNode)TreeView.SelectedNode;
                if (treeNode == null)
                    return GetHelperStatus();

                while (treeNode != null)
                {
                    list.Add(treeNode);
                    treeNode = (BaseTreeNode?)treeNode.ParentNode;
                }

                list.Reverse();
                return GetHelperStatus();

                HelperStatus GetHelperStatus()
                    => new
                    (
                        Application.Application,
                        new LinkedList<BaseTreeNode>(list),
                        ReferenceCategory,
                        CmbClass.Text
                    );
            }
        }

        public ReferenceCategories ReferenceCategory => ConfiguredItemHelperForm.ReferenceCategory;

        public string ReferenceDefinition
            => ReferenceCategory == ReferenceCategories.InstanceReference || ReferenceCategory == ReferenceCategories.StaticReference
                ? ReferenceTreeNode.ReferenceDefinition
                : string.Empty;

        public string ReferenceName
            => ReferenceCategory == ReferenceCategories.InstanceReference || ReferenceCategory == ReferenceCategories.StaticReference
                ? ReferenceTreeNode.ReferenceName
                : string.Empty;

        public string TypeName
            => ReferenceCategory == ReferenceCategories.Type || ReferenceCategory == ReferenceCategories.StaticReference
                ? CmbClass.Text.Trim()
                : string.Empty;

        public abstract void AddChildren(VariableTreeNode treeNode);
        protected abstract void BuildMembersTreeView(Type type);

        public void ApplicationChanged() => HandleSourceChanged();

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
            ConfiguredItemHelperForm.ClearMessage();
            if (!TypeLoadHelper.TryGetSystemType(classFullName, Application, out Type? type))
            {
                ConfiguredItemHelperForm.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadClassFormat, classFullName));
                return;
            }

            TreeView.Nodes.Clear();
            switch (ReferenceCategory)
            {
                case ReferenceCategories.InstanceReference:
                case ReferenceCategories.StaticReference:
                    BuildReferencesTreeView(type);
                    break;
                case ReferenceCategories.This:
                case ReferenceCategories.Type:
                    BuildMembersTreeView(type);
                    break;
                default:
                    throw ExceptionHelper.CriticalException("{CE978AC3-B0D1-4F7A-80D4-2EB20948BBE3}");
            }

            if (TreeView.Nodes.Count > 0)
                TreeView.SelectedNode = TreeView.Nodes[0];
        }

        public void ClearTreeView()
        {
            ConfiguredItemHelperForm.ClearTreeView();
        }

        public void CmbClassTextChanged()
        {
            ClearTreeView();

            if (!Application.AssemblyAvailable)
            {
                ConfiguredItemHelperForm.SetErrorMessage(Application.UnavailableMessage);
                return;
            }

            if (CmbReferenceCategory.Items.Count == 0)
                return;

            BuildTreeView(CmbClass.Text.Trim());
        }

        public void CmbReferenceCategorySelectedIndexChanged() => HandleSourceChanged();

        public void Initialize()
        {
            TreeView.ImageList = ImageListService.ImageList;
            TreeView.TreeViewElement.ShowNodeToolTips = true;
            TreeView.ShowRootLines = true;
            TreeView.HideSelection = false;
            CmbClassTypeAutoCompleteManager.Setup();
            LoadReferenceCategoryComboBox();
            ConfiguredItemHelperForm.ValidateOk();
        }

        public void UpdateSelectedVariableConfiguration(CustomVariableConfiguration customVariableConfiguration)
        {
            if (!Application.AssemblyAvailable)
                return;

            if (TreeView.SelectedNode is not VariableTreeNode treeNode)
                throw ExceptionHelper.CriticalException("{5DDBB387-904D-4010-87EA-0C0B864AE5C3}");

            if (treeNode.CustomVariableConfiguration?.Equals(customVariableConfiguration) == true)
            {
                ConfiguredItemHelperForm.ValidateOk();
                return;
            }

            TreeView.BeginUpdate();
            ReplaceReferenceTreeMode(treeNode, customVariableConfiguration);
            TreeView.TreeViewElement.Update(RadTreeViewElement.UpdateActions.Reset);
            TreeView.EndUpdate();
            
            ConfiguredItemHelperForm.ValidateOk();
        }

        public void UpdateSelection(HelperStatus? helperStatus)
        {
            if (helperStatus == null || helperStatus.Path.Count == 0)
                return;

            CmbApplication.SelectedValue = helperStatus.Application.Name;
            CmbReferenceCategory.SelectedValue = helperStatus.ReferenceCategory;
            if (helperStatus.ReferenceCategory == ReferenceCategories.StaticReference
                || helperStatus.ReferenceCategory == ReferenceCategories.Type)
                CmbClass.Text = helperStatus.ClassName;

            LinkedListNode<BaseTreeNode> firstLinkedNode = helperStatus.Path.First ?? throw ExceptionHelper.CriticalException("{8F326C4F-8067-4937-A42F-2499102B6390}");
            BaseTreeNode? treeNode = ReplaceReferenceTreeMode
            (
                firstLinkedNode,
                TreeView.Nodes.OfType<BaseTreeNode>().FirstOrDefault(item => item.Equals(firstLinkedNode.Value))
            );

            if (treeNode == null)
                return;

            TreeView.BeginUpdate();
            UpdateSelection(treeNode, firstLinkedNode);
            TreeView.EndUpdate();
        }

        private void BuildReferencesTreeView(Type type)
        {
            TreeView.BeginUpdate();
            List<BaseTreeNode> infoList = new();
            BindingFlagCategory bindingFlagCategory = GetBindingFlagCategory();
            foreach (FieldInfo fInfo in type.GetFields(IntellisenseHelper.GetBindingFlags(bindingFlagCategory)))
            {
                if (fInfo.Name.Contains(MiscellaneousConstants.PERIODSTRING))
                    continue;
                if (!VariableOrFunctionNameRegex().IsMatch(fInfo.Name))
                    continue;

                //Don't check for private members - we know this is the root.

                AddTreeNode
                (
                    IntellisenseTreeNodeFactory.GetFieldTreeNode(fInfo, null, ConfiguredItemHelperForm)
                );
            }

            foreach (PropertyInfo pInfo in type.GetProperties(IntellisenseHelper.GetBindingFlags(bindingFlagCategory)))
            {
                if (pInfo.Name.Contains(MiscellaneousConstants.PERIODSTRING))
                    continue;
                if (!VariableOrFunctionNameRegex().IsMatch(pInfo.Name))
                    continue;

                //Don't check for private members - we know this is the root.

                ParameterInfo[] pArray = pInfo.GetIndexParameters();
                if (pArray.Length == 1 && TypeHelper.IsValidIndex(pArray[0].ParameterType))
                {
                    AddTreeNode
                    (
                        IntellisenseTreeNodeFactory.GetIndexerTreeNode(pInfo, null, pArray[0].ParameterType, ConfiguredItemHelperForm)
                    );
                }
                else
                {
                    AddTreeNode
                    (
                        IntellisenseTreeNodeFactory.GetPropertyTreeNode(pInfo, null, ConfiguredItemHelperForm)
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

            BindingFlagCategory GetBindingFlagCategory()
            {
                return ConfiguredItemHelperForm.ReferenceCategory switch
                {
                    ReferenceCategories.InstanceReference => BindingFlagCategory.Instance,
                    ReferenceCategories.StaticReference => BindingFlagCategory.Static,
                    _ => throw ExceptionHelper.CriticalException("{2434CF0C-8428-468B-AEC5-992B3A34804C}"),
                };
            }
        }

        private void HandleSourceChanged()
        {
            ClearTreeView();

            if (!Application.AssemblyAvailable)
            {
                ConfiguredItemHelperForm.SetErrorMessage(Application.UnavailableMessage);
                return;
            }

            if (CmbReferenceCategory.Items.Count == 0)
                return;

            if (ReferenceCategory == ReferenceCategories.This || ReferenceCategory == ReferenceCategories.InstanceReference)
            {
                string activityType = TypeHelper.ToId(Application.ActivityType);
                if (CmbClass.Text != activityType)
                {
                    CmbClass.Text = activityType;//CmbClass_TextChanged
                }
                else
                    BuildTreeView(CmbClass.Text.Trim());

                CmbClass.Enabled = false;
            }
            else
            {
                CmbClass.Enabled = true;
                BuildTreeView(CmbClass.Text.Trim());
            }
        }

        private void LoadReferenceCategoryComboBox()
        {
            RadDropDownListHelper.LoadComboItems
            (
                CmbReferenceCategory,
                RadDropDownStyle.DropDownList,
                new ReferenceCategories[] { ReferenceCategories.None }
            );
            
            CmbReferenceCategory.SelectedValue = ReferenceCategories.InstanceReference;
        }

        private VariableTreeNode ReplaceReferenceTreeMode(VariableTreeNode currentNode, CustomVariableConfiguration? customVariableConfiguration)
        {
            if (currentNode is FieldTreeNode)
            {
                return ReplaceTreeNode
                (
                    IntellisenseTreeNodeFactory.GetFieldTreeNode
                    (
                        (FieldInfo)currentNode.MemberInfo,
                        currentNode.ParentNode,
                        ConfiguredItemHelperForm,
                        customVariableConfiguration
                    )
                );
            }

            if (currentNode is PropertyTreeNode)
            {
                return ReplaceTreeNode
                (
                    IntellisenseTreeNodeFactory.GetPropertyTreeNode
                    (
                        (PropertyInfo)currentNode.MemberInfo,
                        currentNode.ParentNode,
                        ConfiguredItemHelperForm,
                        customVariableConfiguration
                    )
                );
            }

            if (currentNode is IndexerTreeNode)
            {
                PropertyInfo pInfo = (PropertyInfo)currentNode.MemberInfo;
                return ReplaceTreeNode
                (
                    IntellisenseTreeNodeFactory.GetIndexerTreeNode
                    (
                        pInfo,
                        currentNode.ParentNode,
                        pInfo.GetIndexParameters()[0].ParameterType,
                        ConfiguredItemHelperForm,
                        customVariableConfiguration
                    )
                );
            }

            if (currentNode is ArrayIndexerTreeNode arrayIndexerTreeNode)
            {
                return ReplaceTreeNode
                (
                    IntellisenseTreeNodeFactory.GetArrayIndexerTreeNode
                    (
                        (MethodInfo)arrayIndexerTreeNode.MemberInfo,
                        arrayIndexerTreeNode.Rank,
                        currentNode.ParentNode,
                        ConfiguredItemHelperForm,
                        customVariableConfiguration
                    )
                );
            }

            throw ExceptionHelper.CriticalException("{E2A382D1-FD13-47E4-A7DE-7A3CC515D1DB}");

            VariableTreeNode ReplaceTreeNode(VariableTreeNode newTreeNode)
            {
                if (currentNode.ParentNode != null)
                    currentNode.ParentNode.Nodes.Insert(currentNode.Index, newTreeNode);
                else
                    TreeView.Nodes.Insert(currentNode.Index, newTreeNode);

                TreeView.SelectedNode = null;
                currentNode.Remove();
                AddChildren(newTreeNode);
                newTreeNode.Expand();

                return newTreeNode;
            }
        }

        private BaseTreeNode? ReplaceReferenceTreeMode(LinkedListNode<BaseTreeNode> linkedNode, BaseTreeNode? treeNode)
        {
            if (treeNode == null)
                return null;

            if (treeNode is VariableTreeNode variableNode
                && linkedNode.Value is VariableTreeNode linkedVariableNode
                && variableNode.CustomVariableConfiguration?.Equals(linkedVariableNode.CustomVariableConfiguration) != true)
            {
                if (variableNode.CustomVariableConfiguration == null
                    && linkedVariableNode.CustomVariableConfiguration == null)
                    return treeNode;

                if (variableNode.CustomVariableConfiguration?.Equals(linkedVariableNode.CustomVariableConfiguration) != true)
                    return ReplaceReferenceTreeMode(variableNode, linkedVariableNode.CustomVariableConfiguration);
            }

            return treeNode;
        }

        private void UpdateSelection(BaseTreeNode treeNode, LinkedListNode<BaseTreeNode> linkedNode)
        {
            if (linkedNode.Next == null)
            {
                TreeView.SelectedNode = treeNode;
                return;
            }

            if (treeNode.Nodes.Count < 1)
                return;

            foreach (VariableTreeNode childNode in treeNode.Nodes.OfType<VariableTreeNode>())
                AddChildren(childNode);

            treeNode.Expand();

            BaseTreeNode? childTreeNode = ReplaceReferenceTreeMode
            (
                linkedNode.Next,
                treeNode.Nodes.OfType<BaseTreeNode>().FirstOrDefault(item => item.Equals(linkedNode.Next.Value))
            );

            if (childTreeNode == null)
                return;

            UpdateSelection(childTreeNode, linkedNode.Next);
        }

        [GeneratedRegex(RegularExpressions.VARIABLEORFUNCTIONNAME)]
        private static partial Regex VariableOrFunctionNameRegex();
    }
}