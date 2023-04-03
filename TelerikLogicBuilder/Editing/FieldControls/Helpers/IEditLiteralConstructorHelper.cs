using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IEditLiteralConstructorHelper
    {
        void Edit(Type assignedTo, XmlElement? constructorElement = null);
    }
}
