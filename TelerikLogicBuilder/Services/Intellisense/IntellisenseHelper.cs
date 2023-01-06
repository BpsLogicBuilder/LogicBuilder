using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense
{
    internal class IntellisenseHelper : IIntellisenseHelper
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IIntellisenseTreeNodeFactory _intellisenseTreeNodeFactory;
        private readonly ITypeHelper _typeHelper;

        public IntellisenseHelper(
            IExceptionHelper exceptionHelper,
            IIntellisenseTreeNodeFactory intellisenseTreeNodeFactory,
            ITypeHelper typeHelper)
        {
            _exceptionHelper = exceptionHelper;
            _intellisenseTreeNodeFactory = intellisenseTreeNodeFactory;
            _typeHelper = typeHelper;
        }

        public void AddArrayIndexer(List<BaseTreeNode> infoList, Type parentType, VariableTreeNode parentNode, IApplicationForm applicationForm)
        {
            if (!parentType.IsArray)
                return;

            infoList.Add
            (
                _intellisenseTreeNodeFactory.GetArrayIndexerTreeNode
                (
                    parentType.GetMethod(IntellisenseConstants.ARRAYGETMETHODNAME) ?? throw _exceptionHelper.CriticalException("{933710FD-33CA-4540-8D8B-D9C5978BC898}"),
                    parentType.GetArrayRank(), 
                    parentNode, 
                    applicationForm
                )
            );
        }

        public void AddFields(List<BaseTreeNode> infoList, Type parentType, VariableTreeNode? parentNode, BindingFlagCategory bindingFlagCategory, IApplicationForm applicationForm, bool root = false)
        {
            foreach(FieldInfo fInfo in parentType.GetFields(GetBindingFlags(bindingFlagCategory)))
            {
                if (fInfo.Name.Contains(MiscellaneousConstants.PERIODSTRING))
                    continue;

                if (!Regex.IsMatch(fInfo.Name, RegularExpressions.VARIABLEORFUNCTIONNAME))
                    continue;

                if (!root && fInfo.IsPrivate)
                    continue;

                
                infoList.Add
                (
                    _intellisenseTreeNodeFactory.GetFieldTreeNode(fInfo, parentNode, applicationForm)
                );
            }
        }

        public void AddMethods(List<BaseTreeNode> infoList, Type parentType, VariableTreeNode? parentNode, BindingFlagCategory bindingFlagCategory, IApplicationForm applicationForm, bool root = false)
        {
            foreach (MethodInfo mInfo in parentType.GetMethods(GetBindingFlags(bindingFlagCategory)))
            {
                if (mInfo.Name.Contains(MiscellaneousConstants.PERIODSTRING))
                    continue;

                if (!Regex.IsMatch(mInfo.Name, RegularExpressions.VARIABLEORFUNCTIONNAME))
                    continue;

                if (!root && mInfo.IsPrivate)
                    continue;

                if (mInfo.Name.StartsWith(IntellisenseConstants.GET, true, CultureInfo.InvariantCulture) 
                    || mInfo.Name.StartsWith(IntellisenseConstants.SET, true, CultureInfo.InvariantCulture))
                    continue;

                infoList.Add
                (
                    _intellisenseTreeNodeFactory.GetFunctionTreeNode(mInfo, parentNode)
                );
            }
        }

        public void AddProperties(List<BaseTreeNode> infoList, Type parentType, VariableTreeNode? parentNode, BindingFlagCategory bindingFlagCategory, IApplicationForm applicationForm, bool root = false)
        {
            foreach (PropertyInfo pInfo in parentType.GetProperties(GetBindingFlags(bindingFlagCategory)))
            {
                if (pInfo.Name.Contains(MiscellaneousConstants.PERIODSTRING))
                    continue;

                if (!Regex.IsMatch(pInfo.Name, RegularExpressions.VARIABLEORFUNCTIONNAME))
                    continue;

                if (!root && (pInfo.GetMethod?.IsPrivate ?? true) && (pInfo.SetMethod?.IsPrivate ?? true))
                    continue;

                ParameterInfo[] pArray = pInfo.GetIndexParameters();
                if (pArray.Length == 1 && _typeHelper.IsValidIndex(pArray[0].ParameterType))
                {
                    infoList.Add
                    (
                        _intellisenseTreeNodeFactory.GetIndexerTreeNode(pInfo, parentNode, pArray[0].ParameterType, applicationForm)
                    );
                }
                else
                {
                    infoList.Add
                    (
                        _intellisenseTreeNodeFactory.GetPropertyTreeNode(pInfo, parentNode, applicationForm)
                    );
                }
            }
        }

        public BindingFlags GetBindingFlags(BindingFlagCategory bindingFlagCategory) 
            => bindingFlagCategory switch
            {
                BindingFlagCategory.Instance => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy,
                BindingFlagCategory.Static => BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy,
                _ => throw _exceptionHelper.CriticalException("{5EE5A0E2-4D45-4B77-B7B1-F527FC77F4B4}"),
            };

        public BindingFlags GetConstructorBindingFlags()
            => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
    }
}
