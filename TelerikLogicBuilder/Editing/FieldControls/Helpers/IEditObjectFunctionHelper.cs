﻿using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IEditObjectFunctionHelper
    {
        void Edit(Type assignedTo, XmlElement? functionElement = null);
    }
}
