using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IEditObjectConstructorHelper
    {
        void Edit(Type assignedTo, XmlElement? constructorElement = null);
    }
}
