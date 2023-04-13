using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal interface IEditParameterObjectListHelper
    {
        void Edit(Type assignedTo, XmlElement? objectListElement = null);
    }
}
