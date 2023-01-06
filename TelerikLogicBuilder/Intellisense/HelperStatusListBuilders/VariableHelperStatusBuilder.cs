using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal class VariableHelperStatusBuilder : IVariableHelperStatusBuilder
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IReferenceInfoListBuilder _referenceInfoListBuilder;
        private readonly IReferenceNodeListBuilder _referenceNodeListBuilder;
        private readonly IStringHelper _stringHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariableNodeBuilder _variableNodeBuilder;

        private readonly IConfigureVariablesForm configureVariablesForm;

        public VariableHelperStatusBuilder(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IHelperStatusBuilderFactory helperStatusBuilderFactory,
            IReferenceInfoListBuilder referenceInfoListBuilder,
            IStringHelper stringHelper,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IConfigureVariablesForm configureVariablesForm)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _referenceInfoListBuilder = referenceInfoListBuilder;
            _referenceNodeListBuilder = helperStatusBuilderFactory.GetReferenceNodeListBuilder(configureVariablesForm);
            _stringHelper = stringHelper;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            _variableNodeBuilder = helperStatusBuilderFactory.GetVariableNodeBuilder(configureVariablesForm);
            this.configureVariablesForm = configureVariablesForm;
        }

        public HelperStatus? Build()
        {
            if (!this.configureVariablesForm.Application.AssemblyAvailable
                || configureVariablesForm.TreeView.SelectedNode == null
                || configureVariablesForm.CurrentTreeNodeControl is not IConfigureVariableControl configureVariableControl)
            {
                return null;
            }

            try
            {
                configureVariableControl.ValidateFields();
            }
            catch (LogicBuilderException)
            {
                return null;
            }

            return Build
            (
                configureVariableControl.CmbReferenceCategory.SelectedValue,
                configureVariableControl.CmbVariableCategory.SelectedValue,
                configureVariableControl.TxtTypeName.Text.Trim(),
                configureVariableControl.CmbReferenceDefinition.Text.Trim(),
                configureVariableControl.TxtReferenceName.Text.Trim(),
                configureVariableControl.TxtCastReferenceAs.Text.Trim(),
                configureVariableControl.TxtMemberName.Text.Trim(),
                configureVariableControl.TxtCastVariableAs.Text.Trim()
            );
        }

        private HelperStatus? Build(
                object referenceCategoryValue,
                object variableCategoryValue,
                string txtTypeNameValue,
                string cmbReferenceDefinitionValue,
                string txtReferenceNameValue,
                string txtCastReferenceAsValue,
                string txtMemberNameValue,
                string txtCastVariableAsValue)
        {
            string[] referenceNameArray = _stringHelper.SplitWithQuoteQualifier
            (
                txtReferenceNameValue,
                MiscellaneousConstants.PERIODSTRING
            );

            return Build
            (
                (ReferenceCategories)referenceCategoryValue,
                (VariableCategory)variableCategoryValue,
                txtTypeNameValue,
                _stringHelper.SplitWithQuoteQualifier
                (
                    _enumHelper.BuildValidReferenceDefinition(cmbReferenceDefinitionValue),
                    MiscellaneousConstants.PERIODSTRING
                ),
                referenceNameArray,
                txtCastReferenceAsValue.Length == 0
                    ? referenceNameArray.Select(r => MiscellaneousConstants.TILDE).ToArray()
                    : _stringHelper.SplitWithQuoteQualifier(txtCastReferenceAsValue, MiscellaneousConstants.PERIODSTRING),
                txtMemberNameValue,
                txtCastVariableAsValue
            );
        }

        private HelperStatus? Build(ReferenceCategories referenceCategory, VariableCategory variableCategory, string typeName, IList<string> referenceDefinitionArray, IList<string> referenceNameArray, IList<string> castReferenceAsArray, string memberName, string castVariableAs)
        {
            if (!configureVariablesForm.Application.AssemblyAvailable) return null;
            Type? helperClassType = GetHelperClassType();
            if (helperClassType == null) return null;

            return referenceCategory switch
            {
                ReferenceCategories.InstanceReference or ReferenceCategories.StaticReference => GetHelperStatus(GetListForReference()),
                ReferenceCategories.This or ReferenceCategories.Type => GetHelperStatus(GetListForMember()),
                _ => throw _exceptionHelper.CriticalException("{3334E224-B012-4158-A764-10B759B84AAC}"),
            };

            HelperStatus? GetHelperStatus(List<BaseTreeNode> treeNodes)
            {
                if (treeNodes.Count == 0) return null;

                return new
                (
                    configureVariablesForm.Application.Application,
                    new LinkedList<BaseTreeNode>(treeNodes),
                    referenceCategory,
                    _typeHelper.ToId(helperClassType)
                );
            }

            Type? GetHelperClassType()
            {
                switch (referenceCategory)
                {
                    case ReferenceCategories.InstanceReference:
                    case ReferenceCategories.This:
                        return configureVariablesForm.Application.ActivityType;
                    case ReferenceCategories.StaticReference:
                    case ReferenceCategories.Type:
                        _typeLoadHelper.TryGetSystemType(typeName, configureVariablesForm.Application, out Type? type);
                        return type;
                    default:
                        throw _exceptionHelper.CriticalException("{4A7D5A05-88BA-4494-897A-E2EC0221D567}");
                }
            }

            List<BaseTreeNode> GetListForMember()
            {
                BaseTreeNode? treeNode = _variableNodeBuilder.Build
                (
                    variableCategory,
                    null,
                    helperClassType!,
                    memberName,
                    castVariableAs,
                    referenceCategory == ReferenceCategories.This//can be This or Type at this point
                        ? BindingFlagCategory.Instance
                        : BindingFlagCategory.Static
                );

                return treeNode == null 
                    ? new List<BaseTreeNode>() 
                    : new List<BaseTreeNode>{ treeNode };
            }

            List<BaseTreeNode> GetListForReference()
            {
                IList<ReferenceInfo> referenceInfos = _referenceInfoListBuilder.BuildList
                (
                    referenceDefinitionArray,
                    referenceNameArray,
                    castReferenceAsArray
                );

                List<BaseTreeNode> treeNodes = _referenceNodeListBuilder.Build
                (
                    _typeHelper.ToId(helperClassType),
                    referenceInfos,
                    referenceCategory == ReferenceCategories.InstanceReference//can be InstanceReference/ or StaticReference/ at this point
                        ? BindingFlagCategory.Instance
                        : BindingFlagCategory.Static
                );

                if (treeNodes.Count != referenceInfos.Count)
                    return new List<BaseTreeNode>();

                BaseTreeNode lastNode = treeNodes.Last();
                BaseTreeNode? leafNode = _variableNodeBuilder.Build
                (
                    variableCategory,
                    (VariableTreeNode)lastNode,
                    lastNode.MemberType,
                    memberName,
                    castVariableAs,
                    BindingFlagCategory.Instance//always a refrence here
                );
                if (leafNode != null) treeNodes.Add(leafNode);

                return treeNodes;
            }
        }
    }
}
