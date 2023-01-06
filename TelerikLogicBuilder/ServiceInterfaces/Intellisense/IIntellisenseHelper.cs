using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense
{
    internal interface IIntellisenseHelper
    {
        void AddArrayIndexer(List<BaseTreeNode> infoList, Type parentType, VariableTreeNode parentNode, IApplicationForm applicationForm);
        void AddFields(List<BaseTreeNode> infoList, Type parentType, VariableTreeNode? parentNode, BindingFlagCategory bindingFlagCategory, IApplicationForm applicationForm, bool root = false);
        void AddMethods(List<BaseTreeNode> infoList, Type parentType, VariableTreeNode? parentNode, BindingFlagCategory bindingFlagCategory, IApplicationForm applicationForm, bool root = false);
        void AddProperties(List<BaseTreeNode> infoList, Type parentType, VariableTreeNode? parentNode, BindingFlagCategory bindingFlagCategory, IApplicationForm applicationForm, bool root = false);
        BindingFlags GetBindingFlags(BindingFlagCategory bindingFlagCategory);
        BindingFlags GetConstructorBindingFlags();
    }
}
