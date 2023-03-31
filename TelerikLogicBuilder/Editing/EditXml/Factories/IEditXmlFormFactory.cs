using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories
{
    internal interface IEditXmlFormFactory : IDisposable
    {
        IEditBooleanFunctionFormXml GetEditBooleanFunctionFormXml(string xml, Type assignedTo);
        IEditBuildDecisionFormXml GetEditBuildDecisionFormXml(string xml);
        IEditConditionsFormXml GetEditConditionsFormXml(string xml);
        IEditConstructorFormXml GetEditConstructorFormXml(string xml, Type assignedTo);
        IEditDecisionsFormXml GetEditDecisionsFormXml(string xml);
        IEditDialogFunctionFormXml GetEditDialogFunctionFormXml(string xml);
        IEditFunctionsFormXml GetEditFunctionsFormXml(string xml);
        IEditLiteralListFormXml GetEditLiteralListFormXml(string xml, Type assignedTo);
        IEditObjectListFormXml GetEditObjectListFormXml(string xml, Type assignedTo);
        IEditTableFunctionsFormXml GetEditTableFunctionsFormXml(string xml);
        IEditValueFunctionFormXml GetEditValueFunctionFormXml(string xml, Type assignedTo);
    }
}
