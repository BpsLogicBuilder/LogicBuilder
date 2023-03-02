using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Helpers
{
    internal interface IObjectHashSetListBoxItemComparer
    {
        bool Compare(XmlElement? first, XmlElement? second, ApplicationTypeInfo application);
    }
}
