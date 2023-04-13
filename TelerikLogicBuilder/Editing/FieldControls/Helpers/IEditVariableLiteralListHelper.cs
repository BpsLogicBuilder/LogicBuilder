using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IEditVariableLiteralListHelper
    {
        void Edit(Type assignedTo, XmlElement? literalListElement = null);
    }
}
