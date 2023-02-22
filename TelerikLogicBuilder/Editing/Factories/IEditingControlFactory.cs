using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingControlFactory
    {
        IEditConstructorControl GetEditConstructorControl(
            IEditingForm editingForm,
            Constructor constructor,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null);
    }
}
