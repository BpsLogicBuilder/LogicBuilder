using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IEditObjectVariableHelper
    {
        void Edit(Type assignedTo, XmlElement? variableElement = null);
    }
}
