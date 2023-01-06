using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using System;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal class VariableNodeBuilder : IVariableNodeBuilder
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IIntellisenseHelper _intellisenseHelper;
        private readonly IIntellisenseTreeNodeFactory _intellisenseTreeNodeFactory;
        private readonly ITypeHelper _typeHelper;

        private readonly IConfigurationForm configurationForm;

        public VariableNodeBuilder(
            IExceptionHelper exceptionHelper,
            IIntellisenseHelper intellisenseHelper,
            IIntellisenseTreeNodeFactory intellisenseTreeNodeFactory,
            ITypeHelper typeHelper,
            IConfigurationForm configurationForm)
        {
            _exceptionHelper = exceptionHelper;
            _intellisenseHelper = intellisenseHelper;
            _intellisenseTreeNodeFactory = intellisenseTreeNodeFactory;
            _typeHelper = typeHelper;
            this.configurationForm = configurationForm;
        }

        public BaseTreeNode? Build(VariableCategory variableCategory, VariableTreeNode? parentNode, Type parentType, string memberName, string castVariableAs, BindingFlagCategory bindingFlagCategory)
        {
            BindingFlags bindingFlags = _intellisenseHelper.GetBindingFlags(bindingFlagCategory);

            switch (variableCategory)
            {
                case VariableCategory.Field:
                    FieldInfo? fInfo = parentType.GetField(memberName, bindingFlags);
                    if (fInfo == null)
                        return null;

                    return _intellisenseTreeNodeFactory.GetFieldTreeNode
                    (
                        fInfo,
                        parentNode,
                        configurationForm,
                        new CustomVariableConfiguration
                        (
                            VariableCategory.Field,
                            castVariableAs,
                            memberName
                        )
                    );
                case VariableCategory.Property:
                    PropertyInfo? pInfo = parentType.GetProperty(memberName, bindingFlags);
                    if (pInfo == null)
                        return null;

                    return _intellisenseTreeNodeFactory.GetPropertyTreeNode
                    (
                        pInfo,
                        parentNode,
                        configurationForm,
                        new CustomVariableConfiguration
                        (
                            VariableCategory.Property,
                            castVariableAs,
                            memberName
                        )
                    );
                case VariableCategory.StringKeyIndexer:
                case VariableCategory.IntegerKeyIndexer:
                case VariableCategory.BooleanKeyIndexer:
                case VariableCategory.DateTimeKeyIndexer:
                case VariableCategory.DateTimeOffsetKeyIndexer:
                case VariableCategory.DateOnlyKeyIndexer:
                case VariableCategory.DateKeyIndexer:
                case VariableCategory.TimeSpanKeyIndexer:
                case VariableCategory.TimeOnlyKeyIndexer:
                case VariableCategory.TimeOfDayKeyIndexer:
                case VariableCategory.GuidKeyIndexer:
                case VariableCategory.ByteKeyIndexer:
                case VariableCategory.ShortKeyIndexer:
                case VariableCategory.LongKeyIndexer:
                case VariableCategory.FloatKeyIndexer:
                case VariableCategory.DoubleKeyIndexer:
                case VariableCategory.DecimalKeyIndexer:
                case VariableCategory.CharKeyIndexer:
                case VariableCategory.SByteKeyIndexer:
                case VariableCategory.UShortKeyIndexer:
                case VariableCategory.UIntegerKeyIndexer:
                case VariableCategory.ULongKeyIndexer:
                case VariableCategory.VariableKeyIndexer:
                    PropertyInfo? indexerPropertyInfo = parentType.GetProperties(bindingFlags).Where(p => p.Name == IntellisenseConstants.INDEXREFERENCENAME).FirstOrDefault();
                    if (indexerPropertyInfo == null)
                        return null;

                    ParameterInfo[] pArray = indexerPropertyInfo.GetIndexParameters();
                    if (pArray.Length != 1 || !_typeHelper.IsValidIndex(pArray[0].ParameterType))
                        return null;

                    return _intellisenseTreeNodeFactory.GetIndexerTreeNode
                    (
                        indexerPropertyInfo,
                        parentNode,
                        pArray[0].ParameterType,
                        configurationForm,
                        new CustomVariableConfiguration
                        (
                            variableCategory,
                            castVariableAs,
                            memberName
                        )
                    );
                case VariableCategory.ArrayIndexer:
                case VariableCategory.VariableArrayIndexer:
                    if (!parentType.IsArray)
                        return null;

                    return _intellisenseTreeNodeFactory.GetArrayIndexerTreeNode
                    (
                        parentType.GetMethod(IntellisenseConstants.ARRAYGETMETHODNAME) ?? throw _exceptionHelper.CriticalException("{68FE45D2-C64C-4B19-A08B-5800F0FF2E52}"),
                        parentType.GetArrayRank(),
                        parentNode,
                        configurationForm,
                        new CustomVariableConfiguration
                        (
                            variableCategory,
                            castVariableAs,
                            memberName
                        )
                    );
                default:
                    throw _exceptionHelper.CriticalException("{1901C930-616E-4B99-AFCD-B3A0AC0BEC7A}");
            }
        }
    }
}
