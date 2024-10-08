﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
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
    internal interface IEditingFormFactory
    {
        IEditBooleanFunctionForm GetEditBooleanFunctionForm(XmlDocument? functionXmlDocument);
        IEditConditionFunctionsForm GetEditConditionFunctionsForm(XmlDocument? conditionsXmlDocument);
        IEditConstructorForm GetEditConstructorForm(Type assignedTo, XmlDocument constructorXmlDocument, HashSet<string> constructorNames, string selectedConstructor, bool denySpecialCharacters);
        IEditDecisionConnectorForm GetEditDecisionConnectorForm(short connectorIndexToSelect, XmlDocument? connectorXmlDocument);
        IEditDecisionForm GetEditDecisionForm(XmlDocument? decisionXmlDocument);
        IEditDecisionsForm GetEditDecisionsForm(XmlDocument? decisionsXmlDocument);
        IEditDialogConnectorForm GetEditDialogConnectorForm(short connectorIndexToSelect, XmlDocument? connectorXmlDocument);
        IEditDialogFunctionForm GetEditDialogFunctionForm(XmlDocument? functionsXmlDocument);
        IEditFunctionsForm GetEditFunctionsForm(IDictionary<string, Function> functionDictionary, IList<TreeFolder> treeFolders, XmlDocument? functionsXmlDocument);
        IEditJumpForm GetEditJumpForm(XmlDocument? jumpXmlDocument);
        IEditModuleShapeForm GetEditModuleShapeForm(XmlDocument? moduleXmlDocument);
        IEditParameterLiteralListForm GetEditParameterLiteralListForm(Type assignedTo, LiteralListParameterElementInfo literalListInfo, XmlDocument literalListXmlDocument);
        IEditParameterObjectListForm GetEditParameterObjectListForm(Type assignedTo, ObjectListParameterElementInfo objectListInfo, XmlDocument literalListXmlDocument);
        IEditPriorityForm GetEditPriorityForm(XmlDocument? priorityXmlDocument);
        IEditValueFunctionForm GetEditValueFunctionForm(Type assignedTo, XmlDocument? functionXmlDocument);
        IEditVariableForm GetEditVariableForm(Type assignedTo);
        IEditVariableLiteralListForm GetEditVariableLiteralListForm(Type assignedTo, LiteralListVariableElementInfo literalListInfo, XmlDocument literalListXmlDocument);
        IEditVariableObjectListForm GetEditVariableObjectListForm(Type assignedTo, ObjectListVariableElementInfo objectListInfo, XmlDocument literalListXmlDocument);
        ISelectConstructorForm GetSelectConstructorForm(Type assignedTo);
        ISelectFragmentForm GetSelectFragmentForm();
        ISelectFromDomainForm GetSelectFromDomainForm(IList<string> domain, string comments);
        ISelectFunctionForm GetSelectFunctionForm(Type assignedTo, IDictionary<string, Function> functionDisctionary, IList<TreeFolder> treeFolders);
    }
}
