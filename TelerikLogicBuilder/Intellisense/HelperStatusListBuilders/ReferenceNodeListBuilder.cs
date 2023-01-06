using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal class ReferenceNodeListBuilder : IReferenceNodeListBuilder
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IIntellisenseHelper _intellisenseHelper;
        private readonly IIntellisenseTreeNodeFactory _intellisenseTreeNodeFactory;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        private readonly IConfigurationForm configurationForm;

        public ReferenceNodeListBuilder(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IIntellisenseHelper intellisenseHelper,
            IIntellisenseTreeNodeFactory intellisenseTreeNodeFactory,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IConfigurationForm configurationForm)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _intellisenseHelper = intellisenseHelper;
            _intellisenseTreeNodeFactory = intellisenseTreeNodeFactory;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            this.configurationForm = configurationForm;
        }

        public List<BaseTreeNode> Build(string className, IList<ReferenceInfo> referenceInfoList, BindingFlagCategory rootReferenceBindingFlagCategory)
        {
            if (!_typeLoadHelper.TryGetSystemType(className, configurationForm.Application, out Type? parentType))
                return new List<BaseTreeNode>();

            if (referenceInfoList.Count == 0)
                return new List<BaseTreeNode>();

            List<VariableTreeNode> treeNodeList = new();
            LinkedListNode<ReferenceInfo> firstReferenceNode = new LinkedList<ReferenceInfo>(referenceInfoList).First!;

            BuildReferenceNodeList
            (
                null,
                parentType,
                _intellisenseHelper.GetBindingFlags(rootReferenceBindingFlagCategory),
                firstReferenceNode,
                treeNodeList
            );

            return treeNodeList.Cast<BaseTreeNode>().ToList();
        }

        private void BuildReferenceNodeList(VariableTreeNode? parentNode, Type parentType, BindingFlags bindingFlags, LinkedListNode<ReferenceInfo> referenceNode, List<VariableTreeNode> treeNodeList)
        {
            string referenceName = referenceNode.Value.MemberName;
            ValidIndirectReference referenceDefinition = referenceNode.Value.ValidIndirectReference;
            string castAs = referenceNode.Value.CastAs;
            VariableTreeNode? newNode = null;

            switch (referenceDefinition)
            {
                case ValidIndirectReference.Field:
                    FieldInfo? fInfo = parentType.GetField(referenceName, bindingFlags);
                    if (fInfo != null)
                    {
                        newNode = _intellisenseTreeNodeFactory.GetFieldTreeNode
                        (
                            fInfo,
                            parentNode,
                            configurationForm,
                            new CustomVariableConfiguration
                            (
                                VariableCategory.Field,
                                castAs,
                                referenceName
                            )
                        );
                        treeNodeList.Add(newNode);
                    }
                    break;
                case ValidIndirectReference.Property:
                    PropertyInfo? pInfo = parentType.GetProperty(referenceName, bindingFlags);
                    if (pInfo != null)
                    {
                        newNode = _intellisenseTreeNodeFactory.GetPropertyTreeNode
                        (
                            pInfo,
                            parentNode,
                            configurationForm,
                            new CustomVariableConfiguration
                            (
                                VariableCategory.Property,
                                castAs,
                                referenceName
                            )
                        );
                        treeNodeList.Add(newNode);
                    }
                    break;
                case ValidIndirectReference.StringKeyIndexer:
                case ValidIndirectReference.IntegerKeyIndexer:
                case ValidIndirectReference.BooleanKeyIndexer:
                case ValidIndirectReference.DateTimeKeyIndexer:
                case ValidIndirectReference.DateTimeOffsetKeyIndexer:
                case ValidIndirectReference.DateOnlyKeyIndexer:
                case ValidIndirectReference.DateKeyIndexer:
                case ValidIndirectReference.TimeSpanKeyIndexer:
                case ValidIndirectReference.TimeOnlyKeyIndexer:
                case ValidIndirectReference.TimeOfDayKeyIndexer:
                case ValidIndirectReference.GuidKeyIndexer:
                case ValidIndirectReference.ByteKeyIndexer:
                case ValidIndirectReference.ShortKeyIndexer:
                case ValidIndirectReference.LongKeyIndexer:
                case ValidIndirectReference.FloatKeyIndexer:
                case ValidIndirectReference.DoubleKeyIndexer:
                case ValidIndirectReference.DecimalKeyIndexer:
                case ValidIndirectReference.CharKeyIndexer:
                case ValidIndirectReference.SByteKeyIndexer:
                case ValidIndirectReference.UShortKeyIndexer:
                case ValidIndirectReference.UIntegerKeyIndexer:
                case ValidIndirectReference.ULongKeyIndexer:
                case ValidIndirectReference.VariableKeyIndexer:
                    PropertyInfo? indexerPropertyInfo = parentType.GetProperties(bindingFlags).Where(p => p.Name == IntellisenseConstants.INDEXREFERENCENAME).FirstOrDefault();
                    if (indexerPropertyInfo != null)
                    {
                        ParameterInfo[] pArray = indexerPropertyInfo.GetIndexParameters();
                        if (pArray.Length == 1 && _typeHelper.IsValidIndex(pArray[0].ParameterType))
                        {
                            newNode = _intellisenseTreeNodeFactory.GetIndexerTreeNode
                            (
                                indexerPropertyInfo,
                                parentNode,
                                pArray[0].ParameterType,
                                configurationForm,
                                new CustomVariableConfiguration
                                (
                                    referenceDefinition == ValidIndirectReference.VariableKeyIndexer
                                            ? VariableCategory.VariableKeyIndexer
                                            : _enumHelper.GetVariableCategory(referenceDefinition),
                                    castAs,
                                    referenceName
                                )
                            );
                            treeNodeList.Add(newNode);
                        }
                    }
                    break;
                case ValidIndirectReference.ArrayIndexer:
                case ValidIndirectReference.VariableArrayIndexer:
                    if (parentType.IsArray)
                    {
                        newNode = _intellisenseTreeNodeFactory.GetArrayIndexerTreeNode
                        (
                            parentType.GetMethod(IntellisenseConstants.ARRAYGETMETHODNAME) ?? throw _exceptionHelper.CriticalException("{3C164B25-407D-4F40-8A32-CE0679BF2045}"),
                            parentType.GetArrayRank(),
                            parentNode,
                            configurationForm,
                            new CustomVariableConfiguration
                            (
                                referenceDefinition == ValidIndirectReference.VariableArrayIndexer
                                            ? VariableCategory.VariableArrayIndexer
                                            : VariableCategory.ArrayIndexer,
                                castAs,
                                referenceName
                            )
                        );
                    }
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{03119EF3-5994-4DE3-B2FE-479528058EC2}");

            }

            if (newNode != null && referenceNode.Next != null)
            {
                BuildReferenceNodeList
                (
                    treeNodeList[^1],
                    newNode.MemberType,
                    _intellisenseHelper.GetBindingFlags(BindingFlagCategory.Instance),
                    referenceNode.Next,
                    treeNodeList
                );
            }
        }
    }
}
