using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories
{
    internal class EditXmlFormFactory : IEditXmlFormFactory
    {
        private IDisposable? _scopedService;
        private readonly Func<string, Type, IEditBooleanFunctionFormXml> _getEditBooleanFunctionsFormXml;
        private readonly Func<string, IEditBuildDecisionFormXml> _getEditBuildDecisionFormXml;
        private readonly Func<string, IEditConditionsFormXml> _getEditConditionsFormXml;
        private readonly Func<string, Type, IEditConstructorFormXml> _getEditConstructorFormXml;
        private readonly Func<string, IEditDecisionsFormXml> _getEditDecisionsFormXml;
        private readonly Func<string, IEditDialogFunctionFormXml> _getEditDialogFunctionFormXml;
        private readonly Func<string, IEditFunctionsFormXml> _getEditFunctionsFormXml;
        private readonly Func<string, Type, IEditLiteralListFormXml> _getEditLiteralListFormXml;
        private readonly Func<string, Type, IEditObjectListFormXml> _getEditObjectListFormXml;
        private readonly Func<string, IEditTableFunctionsFormXml> _getEditTableFunctionsFormXml;
        private readonly Func<string, Type, IEditValueFunctionFormXml> _getEditValueFunctionsFormXml;

        public EditXmlFormFactory(
            Func<string, Type, IEditBooleanFunctionFormXml> getEditBooleanFunctionsFormXml,
            Func<string, IEditBuildDecisionFormXml> getEditBuildDecisionFormXml,
            Func<string, IEditConditionsFormXml> getEditConditionsFormXml,
            Func<string, Type, IEditConstructorFormXml> getEditConstructorFormXml,
            Func<string, IEditDecisionsFormXml> getEditDecisionsFormXml,
            Func<string, IEditDialogFunctionFormXml> getEditDialogFunctionFormXml,
            Func<string, IEditFunctionsFormXml> getEditFunctionsFormXml,
            Func<string, Type, IEditLiteralListFormXml> getEditLiteralListFormXml,
            Func<string, Type, IEditObjectListFormXml> getEditObjectListFormXml,
            Func<string, IEditTableFunctionsFormXml> getEditTableFunctionsFormXml,
            Func<string, Type, IEditValueFunctionFormXml> getEditValueFunctionsFormXml)
        {
            _getEditBooleanFunctionsFormXml = getEditBooleanFunctionsFormXml;
            _getEditBuildDecisionFormXml = getEditBuildDecisionFormXml;
            _getEditConditionsFormXml = getEditConditionsFormXml;
            _getEditConstructorFormXml = getEditConstructorFormXml;
            _getEditDecisionsFormXml = getEditDecisionsFormXml;
            _getEditDialogFunctionFormXml = getEditDialogFunctionFormXml;
            _getEditFunctionsFormXml = getEditFunctionsFormXml;
            _getEditLiteralListFormXml = getEditLiteralListFormXml;
            _getEditObjectListFormXml = getEditObjectListFormXml;
            _getEditTableFunctionsFormXml = getEditTableFunctionsFormXml;
            _getEditValueFunctionsFormXml = getEditValueFunctionsFormXml;
        }

        public IEditBooleanFunctionFormXml GetEditBooleanFunctionFormXml(string xml, Type assignedTo)
        {
            _scopedService = _getEditBooleanFunctionsFormXml(xml, assignedTo);
            return (IEditBooleanFunctionFormXml)_scopedService;
        }

        public IEditBuildDecisionFormXml GetEditBuildDecisionFormXml(string xml)
        {
            _scopedService = _getEditBuildDecisionFormXml(xml);
            return (IEditBuildDecisionFormXml)_scopedService;
        }

        public IEditConditionsFormXml GetEditConditionsFormXml(string xml)
        {
            _scopedService = _getEditConditionsFormXml(xml);
            return (IEditConditionsFormXml)_scopedService;
        }

        public IEditConstructorFormXml GetEditConstructorFormXml(string xml, Type assignedTo)
        {
            _scopedService = _getEditConstructorFormXml(xml, assignedTo);
            return (IEditConstructorFormXml)_scopedService;
        }

        public IEditDialogFunctionFormXml GetEditDialogFunctionFormXml(string xml)
        {
            _scopedService = _getEditDialogFunctionFormXml(xml);
            return (IEditDialogFunctionFormXml)_scopedService;
        }

        public IEditDecisionsFormXml GetEditDecisionsFormXml(string xml)
        {
            _scopedService = _getEditDecisionsFormXml(xml);
            return (IEditDecisionsFormXml)_scopedService;
        }

        public IEditFunctionsFormXml GetEditFunctionsFormXml(string xml)
        {
            _scopedService = _getEditFunctionsFormXml(xml);
            return (IEditFunctionsFormXml)_scopedService;
        }

        public IEditLiteralListFormXml GetEditLiteralListFormXml(string xml, Type assignedTo)
        {
            _scopedService = _getEditLiteralListFormXml(xml, assignedTo);
            return (IEditLiteralListFormXml)_scopedService;
        }

        public IEditObjectListFormXml GetEditObjectListFormXml(string xml, Type assignedTo)
        {
            _scopedService = _getEditObjectListFormXml(xml, assignedTo);
            return (IEditObjectListFormXml)_scopedService;
        }

        public IEditTableFunctionsFormXml GetEditTableFunctionsFormXml(string xml)
        {
            _scopedService = _getEditTableFunctionsFormXml(xml);
            return (IEditTableFunctionsFormXml)_scopedService;
        }

        public IEditValueFunctionFormXml GetEditValueFunctionFormXml(string xml, Type assignedTo)
        {
            _scopedService = _getEditValueFunctionsFormXml(xml, assignedTo);
            return (IEditValueFunctionFormXml)_scopedService;
        }

        public void Dispose()
        {
            _scopedService?.Dispose();
        }
    }
}
