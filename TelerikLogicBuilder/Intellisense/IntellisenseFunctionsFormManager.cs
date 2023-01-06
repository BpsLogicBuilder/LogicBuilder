using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense
{
    internal class IntellisenseFunctionsFormManager : IntellisenseConfiguredItemFormManager, IIntellisenseFunctionsFormManager
    {
        protected override IExceptionHelper ExceptionHelper { get; }
        protected override IImageListService ImageListService { get; }
        protected override IIntellisenseHelper IntellisenseHelper { get; }
        protected override IIntellisenseTreeNodeFactory IntellisenseTreeNodeFactory { get; }
        protected override IRadDropDownListHelper RadDropDownListHelper { get; }
        protected override ITypeAutoCompleteManager CmbClassTypeAutoCompleteManager { get; }
        protected override ITypeHelper TypeHelper { get; }
        protected override ITypeLoadHelper TypeLoadHelper { get; }

        protected override IConfiguredItemHelperForm ConfiguredItemHelperForm { get; }

        public IntellisenseFunctionsFormManager(
            IExceptionHelper exceptionHelper,
            IImageListService imageListService,
            IIntellisenseHelper intellisenseHelper,
            IIntellisenseTreeNodeFactory intellisenseTreeNodeFactory,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IConfiguredItemHelperForm configuredItemHelperForm)
        {
            ExceptionHelper = exceptionHelper;
            ImageListService = imageListService;
            IntellisenseHelper = intellisenseHelper;
            IntellisenseTreeNodeFactory = intellisenseTreeNodeFactory;
            RadDropDownListHelper = radDropDownListHelper;
            TypeHelper = typeHelper;
            TypeLoadHelper = typeLoadHelper;
            ConfiguredItemHelperForm = configuredItemHelperForm;
            CmbClassTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configuredItemHelperForm,
                CmbClass
            );
        }

        public override void AddChildren(VariableTreeNode treeNode)
        {
            if (treeNode.Nodes.Count > 0)
                return;

            List<BaseTreeNode> infoList = new();
            IntellisenseHelper.AddArrayIndexer(infoList, treeNode.MemberType, treeNode, ConfiguredItemHelperForm);
            IntellisenseHelper.AddFields(infoList, treeNode.MemberType, treeNode, BindingFlagCategory.Instance, ConfiguredItemHelperForm);
            IntellisenseHelper.AddMethods(infoList, treeNode.MemberType, treeNode, BindingFlagCategory.Instance, ConfiguredItemHelperForm);
            IntellisenseHelper.AddProperties(infoList, treeNode.MemberType, treeNode, BindingFlagCategory.Instance, ConfiguredItemHelperForm);
            treeNode.Nodes.AddRange(infoList.OrderBy(n => n.Text));
        }

        protected override void BuildMembersTreeView(Type type)
        {
            ConfiguredItemHelperForm.TreeView.BeginUpdate();
            List<BaseTreeNode> infoList = new();
            IntellisenseHelper.AddMethods(infoList, type, null, GetBindingFlagCategory(), ConfiguredItemHelperForm);
            ConfiguredItemHelperForm.TreeView.Nodes.AddRange
            (
                infoList.OrderBy(n => n.Text)
            );
            ConfiguredItemHelperForm.TreeView.EndUpdate();

            BindingFlagCategory GetBindingFlagCategory()
            {
                return ConfiguredItemHelperForm.ReferenceCategory switch
                {
                    ReferenceCategories.This => BindingFlagCategory.Instance,
                    ReferenceCategories.Type => BindingFlagCategory.Static,
                    _ => throw ExceptionHelper.CriticalException("{68210392-3C03-42DE-9F7C-F36050FAF412}"),
                };
            }
        }
    }
}
