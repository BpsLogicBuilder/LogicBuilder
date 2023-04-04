using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IEditLiteralListHelper
    {
        void Edit(Type assignedTo, XmlElement? literalListElement = null);
    }
}
