using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDecisionConnector;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditJump;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditModuleShape;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditPriority;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFromDomain;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingFormFactory : IEditingFormFactory
    {
        private IDisposable? _scopedService;
        private readonly Func<XmlDocument?, IEditBooleanFunctionForm> _getEditBooleanFunctionForm;
        private readonly Func<XmlDocument?, IEditConditionFunctionsForm> _getEditConditionFunctionsForm;
        private readonly Func<Type, XmlDocument, HashSet<string>, string, bool, IEditConstructorForm> _getEditConstructorForm;
        private readonly Func<short, XmlDocument?, IEditDecisionConnectorForm> _getEditDecisionConnectorForm;
        private readonly Func<XmlDocument?, IEditDecisionForm> _getEditDecisionForm;
        private readonly Func<XmlDocument?, IEditDecisionsForm> _getEditDecisionsForm;
        private readonly Func<short, XmlDocument?, IEditDialogConnectorForm> _getEditDialogConnectorForm;
        private readonly Func<XmlDocument?, IEditDialogFunctionForm> _getEditDialogFunctionForm;
        private readonly Func<IDictionary<string, Function>, IList<TreeFolder>, XmlDocument?, IEditFunctionsForm> _getEditFunctionsForm;
        private readonly Func<XmlDocument?, IEditJumpForm> _getEditJumpForm;
        private readonly Func<XmlDocument?, IEditModuleShapeForm> _getEditModuleShapeForm;
        private readonly Func<Type, LiteralListParameterElementInfo, XmlDocument, IEditParameterLiteralListForm> _getEditParameterLiteralListForm;
        private readonly Func<Type, ObjectListParameterElementInfo, XmlDocument, IEditParameterObjectListForm> _getEditParameterObjectListForm;
        private readonly Func<XmlDocument?, IEditPriorityForm> _getEditPriorityForm;
        private readonly Func<Type, XmlDocument?, IEditValueFunctionForm> _getEditValueFunctionForm;
        private readonly Func<Type, IEditVariableForm> _getEditVariableForm;
        private readonly Func<Type, LiteralListVariableElementInfo, XmlDocument, IEditVariableLiteralListForm> _getEditVariableLiteralListForm;
        private readonly Func<Type, ObjectListVariableElementInfo, XmlDocument, IEditVariableObjectListForm> _getEditVariableObjectListForm;
        private readonly Func<Type, ISelectConstructorForm> _getSelectConstructorForm;
        private readonly Func<ISelectFragmentForm> _getSelectFragmentForm;
        private readonly Func<IList<string>, string, ISelectFromDomainForm> _getSelectFromDomainForm;
        private readonly Func<Type, IDictionary<string, Function>, IList<TreeFolder>, ISelectFunctionForm> _getSelectFunctionForm;

        public EditingFormFactory(
            Func<XmlDocument?, IEditBooleanFunctionForm> getEditBooleanFunctionForm,
            Func<XmlDocument?, IEditConditionFunctionsForm> getEditConditionFunctionsForm,
            Func<Type, XmlDocument, HashSet<string>, string, bool, IEditConstructorForm> getEditConstructorForm,
            Func<short, XmlDocument?, IEditDecisionConnectorForm> getEditDecisionConnectorForm,
            Func<XmlDocument?, IEditDecisionForm> getEditDecisionForm,
            Func<XmlDocument?, IEditDecisionsForm> getEditDecisionsForm,
            Func<XmlDocument?, IEditDialogFunctionForm> getEditDialogFunctionForm,
            Func<short, XmlDocument?, IEditDialogConnectorForm> getEditDialogConnectorForm,
            Func<IDictionary<string, Function>, IList<TreeFolder>, XmlDocument?, IEditFunctionsForm> getEditFunctionsForm,
            Func<XmlDocument?, IEditJumpForm> getEditJumpForm,
            Func<XmlDocument?, IEditModuleShapeForm> getEditModuleShapeForm,
            Func<Type, LiteralListParameterElementInfo, XmlDocument, IEditParameterLiteralListForm> getEditParameterLiteralListForm,
            Func<Type, ObjectListParameterElementInfo, XmlDocument, IEditParameterObjectListForm> getEditParameterObjectListForm,
            Func<XmlDocument?, IEditPriorityForm> getEditPriorityForm,
            Func<Type, XmlDocument?, IEditValueFunctionForm> getEditValueFunctionForm,
            Func<Type, IEditVariableForm> getEditVariableForm,
            Func<Type, LiteralListVariableElementInfo, XmlDocument, IEditVariableLiteralListForm> getEditVariableLiteralListForm,
            Func<Type, ObjectListVariableElementInfo, XmlDocument, IEditVariableObjectListForm> getEditVariableObjectListForm,
            Func<Type, ISelectConstructorForm> getSelectConstructorForm,
            Func<ISelectFragmentForm> getSelectFragmentForm,
            Func<IList<string>, string, ISelectFromDomainForm> getSelectFromDomainForm,
            Func<Type, IDictionary<string, Function>, IList<TreeFolder>, ISelectFunctionForm> getSelectFunctionForm)
        {
            _getEditBooleanFunctionForm = getEditBooleanFunctionForm;
            _getEditConditionFunctionsForm = getEditConditionFunctionsForm;
            _getEditConstructorForm = getEditConstructorForm;
            _getEditDecisionConnectorForm = getEditDecisionConnectorForm;
            _getEditDecisionForm = getEditDecisionForm;
            _getEditDecisionsForm = getEditDecisionsForm;
            _getEditDialogConnectorForm = getEditDialogConnectorForm;
            _getEditDialogFunctionForm = getEditDialogFunctionForm;
            _getEditFunctionsForm = getEditFunctionsForm;
            _getEditJumpForm = getEditJumpForm;
            _getEditModuleShapeForm = getEditModuleShapeForm;
            _getEditParameterLiteralListForm = getEditParameterLiteralListForm;
            _getEditParameterObjectListForm = getEditParameterObjectListForm;
            _getEditPriorityForm = getEditPriorityForm;
            _getEditValueFunctionForm = getEditValueFunctionForm;
            _getEditVariableForm = getEditVariableForm;
            _getEditVariableLiteralListForm = getEditVariableLiteralListForm;
            _getEditVariableObjectListForm = getEditVariableObjectListForm;
            _getSelectConstructorForm = getSelectConstructorForm;
            _getSelectFragmentForm = getSelectFragmentForm;
            _getSelectFromDomainForm = getSelectFromDomainForm;
            _getSelectFunctionForm = getSelectFunctionForm;
        }

        public IEditBooleanFunctionForm GetEditBooleanFunctionForm(XmlDocument? functionXmlDocument)
        {
            _scopedService = _getEditBooleanFunctionForm(functionXmlDocument);
            return (IEditBooleanFunctionForm)_scopedService;
        }

        public IEditConditionFunctionsForm GetEditConditionFunctionsForm(XmlDocument? conditionsXmlDocument)
        {
            _scopedService = _getEditConditionFunctionsForm(conditionsXmlDocument);
            return (IEditConditionFunctionsForm)_scopedService;
        }

        public IEditConstructorForm GetEditConstructorForm(Type assignedTo, XmlDocument constructorXmlDocument, HashSet<string> constructorNames, string selectedConstructor, bool denySpecialCharacters)
        {
            _scopedService = _getEditConstructorForm(assignedTo, constructorXmlDocument, constructorNames, selectedConstructor, denySpecialCharacters);
            return (IEditConstructorForm)_scopedService;
        }

        public IEditDecisionConnectorForm GetEditDecisionConnectorForm(short connectorIndexToSelect, XmlDocument? connectorXmlDocument)
        {
            _scopedService = _getEditDecisionConnectorForm(connectorIndexToSelect, connectorXmlDocument);
            return (IEditDecisionConnectorForm)_scopedService;
        }

        public IEditDecisionForm GetEditDecisionForm(XmlDocument? decisionXmlDocument)
        {
            _scopedService = _getEditDecisionForm(decisionXmlDocument);
            return (IEditDecisionForm)_scopedService;
        }

        public IEditDecisionsForm GetEditDecisionsForm(XmlDocument? decisionsXmlDocument)
        {
            _scopedService = _getEditDecisionsForm(decisionsXmlDocument);
            return (IEditDecisionsForm)_scopedService;
        }

        public IEditDialogConnectorForm GetEditDialogConnectorForm(short connectorIndexToSelect, XmlDocument? connectorXmlDocument)
        {
            _scopedService = _getEditDialogConnectorForm(connectorIndexToSelect, connectorXmlDocument);
            return (IEditDialogConnectorForm)_scopedService;
        }

        public IEditDialogFunctionForm GetEditDialogFunctionForm(XmlDocument? functionsXmlDocument)
        {
            _scopedService = _getEditDialogFunctionForm(functionsXmlDocument);
            return (IEditDialogFunctionForm)_scopedService;
        }

        public IEditFunctionsForm GetEditFunctionsForm(IDictionary<string, Function> functionDictionary, IList<TreeFolder> treeFolders, XmlDocument? functionsXmlDocument)
        {
            _scopedService = _getEditFunctionsForm(functionDictionary, treeFolders, functionsXmlDocument);
            return (IEditFunctionsForm)_scopedService;
        }

        public IEditJumpForm GetEditJumpForm(XmlDocument? jumpXmlDocument)
        {
            _scopedService = _getEditJumpForm(jumpXmlDocument);
            return (IEditJumpForm)_scopedService;
        }

        public IEditModuleShapeForm GetEditModuleShapeForm(XmlDocument? moduleXmlDocument)
        {
            _scopedService = _getEditModuleShapeForm(moduleXmlDocument);
            return (IEditModuleShapeForm)_scopedService;
        }

        public IEditParameterLiteralListForm GetEditParameterLiteralListForm(Type assignedTo, LiteralListParameterElementInfo literalListInfo, XmlDocument literalListXmlDocument)
        {
            _scopedService = _getEditParameterLiteralListForm(assignedTo, literalListInfo, literalListXmlDocument);
            return (IEditParameterLiteralListForm)_scopedService;
        }

        public IEditParameterObjectListForm GetEditParameterObjectListForm(Type assignedTo, ObjectListParameterElementInfo objectListInfo, XmlDocument literalListXmlDocument)
        {
            _scopedService = _getEditParameterObjectListForm(assignedTo, objectListInfo, literalListXmlDocument);
            return (IEditParameterObjectListForm)_scopedService;
        }

        public IEditPriorityForm GetEditPriorityForm(XmlDocument? priorityXmlDocument)
        {
            _scopedService = _getEditPriorityForm(priorityXmlDocument);
            return (IEditPriorityForm)_scopedService;
        }

        public IEditValueFunctionForm GetEditValueFunctionForm(Type assignedTo, XmlDocument? functionXmlDocument)
        {
            _scopedService = _getEditValueFunctionForm(assignedTo, functionXmlDocument);
            return (IEditValueFunctionForm)_scopedService;
        }

        public IEditVariableForm GetEditVariableForm(Type assignedTo)
        {
            _scopedService = _getEditVariableForm(assignedTo);
            return (IEditVariableForm)_scopedService;
        }

        public IEditVariableLiteralListForm GetEditVariableLiteralListForm(Type assignedTo, LiteralListVariableElementInfo literalListInfo, XmlDocument literalListXmlDocument)
        {
            _scopedService = _getEditVariableLiteralListForm(assignedTo, literalListInfo, literalListXmlDocument);
            return (IEditVariableLiteralListForm)_scopedService;
        }

        public IEditVariableObjectListForm GetEditVariableObjectListForm(Type assignedTo, ObjectListVariableElementInfo objectListInfo, XmlDocument objectListXmlDocument)
        {
            _scopedService = _getEditVariableObjectListForm(assignedTo, objectListInfo, objectListXmlDocument);
            return (IEditVariableObjectListForm)_scopedService;
        }

        public ISelectConstructorForm GetSelectConstructorForm(Type assignedTo)
        {
            _scopedService = _getSelectConstructorForm(assignedTo);
            return (ISelectConstructorForm)_scopedService;
        }

        public ISelectFragmentForm GetSelectFragmentForm()
        {
            _scopedService = _getSelectFragmentForm();
            return (ISelectFragmentForm)_scopedService;
        }

        public ISelectFromDomainForm GetSelectFromDomainForm(IList<string> domain, string comments)
        {
            _scopedService = _getSelectFromDomainForm(domain, comments);
            return (ISelectFromDomainForm)_scopedService;
        }

        public ISelectFunctionForm GetSelectFunctionForm(Type assignedTo, IDictionary<string, Function> functionDisctionary, IList<TreeFolder> treeFolders)
        {
            _scopedService = _getSelectFunctionForm(assignedTo, functionDisctionary, treeFolders);
            return (ISelectFunctionForm)_scopedService;
        }

        public void Dispose()
        {
            _scopedService?.Dispose();
        }
    }
}
