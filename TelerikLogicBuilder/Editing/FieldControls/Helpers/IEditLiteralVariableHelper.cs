using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IEditLiteralVariableHelper
    {
        void Edit(Type assignedTo, XmlElement? variableElement = null);
    }
}
