using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
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

        IEditStandardFunctionControl GetEditStandardFunctionControl(
            IEditingForm editingForm,
            Function function,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null);

        IEditVariableControl GetEditVariableControl(IEditingForm editingForm, Type assignedTo);
    }
}
