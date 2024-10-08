﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes
{
    internal class ConstructorElementTreeNode : ParametersDataTreeNode
    {
        public ConstructorElementTreeNode(string constructorName, string name, Type assignedToType) 
            : base(constructorName, name, assignedToType)
        {
            ImageIndex = ImageIndexes.CONSTRUCTORIMAGEINDEX;
        }

        public string ConstructorName => Text;

        public override ParametersDataElementType XmlElementType => ParametersDataElementType.Constructor;
    }
}
