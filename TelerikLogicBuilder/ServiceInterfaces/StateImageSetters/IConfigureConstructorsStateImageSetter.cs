﻿using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters
{
    internal interface IConfigureConstructorsStateImageSetter
    {
        void SetImage(XmlElement constructorElement, StateImageRadTreeNode treeNode, ApplicationTypeInfo application);
    }
}
