using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal class FunctionHelperStatusBuilder : IFunctionHelperStatusBuilder
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionNodeBuilder _functionNodeBuilder;
        private readonly IFunctionXmlParser _functionXmlParser;
        private readonly IReferenceInfoListBuilder _referenceInfoListBuilder;
        private readonly IReferenceNodeListBuilder _referenceNodeListBuilder;
        private readonly IStringHelper _stringHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public FunctionHelperStatusBuilder(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFunctionNodeBuilder functionNodeBuilder,
            IFunctionXmlParser functionXmlParser,
            IHelperStatusBuilderFactory helperStatusBuilderFactory,
            IReferenceInfoListBuilder referenceInfoListBuilder,
            IStringHelper stringHelper,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _functionNodeBuilder = functionNodeBuilder;
            _functionXmlParser = functionXmlParser;
            _referenceInfoListBuilder = referenceInfoListBuilder;
            _referenceNodeListBuilder = helperStatusBuilderFactory.GetReferenceNodeListBuilder(configureFunctionsForm);
            _stringHelper = stringHelper;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public HelperStatus? Build()
        {
            RadTreeNode? selectedNode = configureFunctionsForm.TreeView.SelectedNode;

            if (!this.configureFunctionsForm.Application.AssemblyAvailable
                || selectedNode == null
                || configureFunctionsForm.CurrentTreeNodeControl is not IConfigureFunctionControl configureFunctionControl)
            {
                return null;
            }

            try
            {
                configureFunctionControl.ValidateFields();
            }
            catch (LogicBuilderException)
            {
                return null;
            }

            return BuildHelperStatus
            (
                configureFunctionControl.CmbReferenceCategory.SelectedValue,
                configureFunctionControl.CmbFunctionCategory.SelectedValue,
                configureFunctionControl.TxtTypeName.Text.Trim(),
                configureFunctionControl.CmbReferenceDefinition.Text.Trim(),
                configureFunctionControl.TxtReferenceName.Text.Trim(),
                configureFunctionControl.TxtCastReferenceAs.Text.Trim(),
                configureFunctionControl.TxtMemberName.Text.Trim()
            );

            HelperStatus? BuildHelperStatus
            (
                object cmbReferenceCategoryValue,
                object cmbFunctionCategoryValue,
                string txtTypeNameValue,
                string cmbReferenceDefinitionValue,
                string txtReferenceNameValue,
                string txtCastReferenceAsValue,
                string txtMemberNameValue
            )
            {
                string[] referenceNameArray = _stringHelper.SplitWithQuoteQualifier
                (
                    txtReferenceNameValue,
                    MiscellaneousConstants.PERIODSTRING
                );

                return Build
                (
                    (ReferenceCategories)cmbReferenceCategoryValue,
                    (FunctionCategories)cmbFunctionCategoryValue,
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
                    GetFunction().Parameters
                );
            }

            Function GetFunction()
                => _functionXmlParser.Parse(_xmlDocumentHelpers.SelectSingleElement(configureFunctionControl.XmlDocument, selectedNode.Name));
        }

        private HelperStatus? Build(ReferenceCategories referenceCategory, FunctionCategories functionCategory, string typeName, IList<string> referenceDefinitionArray, IList<string> referenceNameArray, IList<string> castReferenceAsArray, string methodName, IList<ParameterBase> configuredParameters)
        {
            if (new HashSet<FunctionCategories> {
                        FunctionCategories.Assert,
                        FunctionCategories.Retract,
                        FunctionCategories.BinaryOperator,
                        FunctionCategories.RuleChainingUpdate,
                        FunctionCategories.Unknown,
                        FunctionCategories.Cast
                    }.Contains(functionCategory))
                return null;

            if (!configureFunctionsForm.Application.AssemblyAvailable) return null;
            Type? helperClassType = GetHelperClassType();
            if (helperClassType == null) return null;

            return referenceCategory switch
            {
                ReferenceCategories.InstanceReference or ReferenceCategories.StaticReference => GetHelperStatus(GetListForReference()),
                ReferenceCategories.This or ReferenceCategories.Type => GetHelperStatus(GetListForMember()),
                _ => throw _exceptionHelper.CriticalException("{9CF8999E-94B5-4EA2-8A88-737FA1EEAF6C}"),
            };

            HelperStatus? GetHelperStatus(List<BaseTreeNode> treeNodes)
            {
                if (treeNodes.Count == 0) return null;

                return new
                (
                    configureFunctionsForm.Application.Application,
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
                        return configureFunctionsForm.Application.ActivityType;
                    case ReferenceCategories.StaticReference:
                    case ReferenceCategories.Type:
                        _typeLoadHelper.TryGetSystemType(typeName, configureFunctionsForm.Application, out Type? type);
                        return type;
                    default:
                        throw _exceptionHelper.CriticalException("{4D0672EB-C671-4E9B-8A67-00DCE73E10B4}");
                }
            }

            List<BaseTreeNode> GetListForMember()
            {
                BaseTreeNode? treeNode = _functionNodeBuilder.Build
                (
                    functionCategory,
                    null,
                    helperClassType!,
                    methodName,
                    configuredParameters,
                    referenceCategory == ReferenceCategories.This//can be This or Type at this point
                        ? BindingFlagCategory.Instance
                        : BindingFlagCategory.Static
                );

                return treeNode == null
                    ? new List<BaseTreeNode>()
                    : new List<BaseTreeNode> { treeNode };
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
                BaseTreeNode? leafNode = _functionNodeBuilder.Build
                (
                    functionCategory,
                    (VariableTreeNode)lastNode,
                    lastNode.MemberType,
                    methodName,
                    configuredParameters,
                    BindingFlagCategory.Instance//always a refrence here
                );
                if (leafNode != null) treeNodes.Add(leafNode);

                return treeNodes;
            }
        }
    }
}
