using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IEditObjectListHelper
    {
        void Edit(Type assignedTo, XmlElement? objectListElement = null);
    }
}
