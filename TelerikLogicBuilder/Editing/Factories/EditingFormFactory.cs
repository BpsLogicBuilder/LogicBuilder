using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDecisionConnector;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditJump;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditModuleShape;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditPriority;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFromDomain;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingFormFactory : IEditingFormFactory
    {
        private IDisposable? _scopedService;

        public IEditBooleanFunctionForm GetEditBooleanFunctionForm(XmlDocument? functionXmlDocument)
        {
            _scopedService = new EditBooleanFunctionForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IEditBooleanFunctionCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingFormHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                functionXmlDocument
            );
            return (IEditBooleanFunctionForm)_scopedService;
        }

        public IEditConditionFunctionsForm GetEditConditionFunctionsForm(XmlDocument? conditionsXmlDocument)
        {
            _scopedService = new EditConditionFunctionsForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IConditionFunctionListBoxItemFactory>(),
                Program.ServiceProvider.GetRequiredService<IConditionsDataParser>(),
                Program.ServiceProvider.GetRequiredService<IEditConditionFunctionsFormCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                new ObjectRichTextBox(),
                conditionsXmlDocument
            );
            return (IEditConditionFunctionsForm)_scopedService;
        }

        public IEditConstructorForm GetEditConstructorForm(Type assignedTo, XmlDocument constructorXmlDocument, HashSet<string> constructorNames, string selectedConstructor, bool denySpecialCharacters)
        {
            _scopedService = new EditConstructorForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IEditConstructorCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingFormHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                assignedTo,
                constructorXmlDocument,
                constructorNames,
                selectedConstructor,
                denySpecialCharacters
            );
            return (IEditConstructorForm)_scopedService;
        }

        public IEditDecisionConnectorForm GetEditDecisionConnectorForm(short connectorIndexToSelect, XmlDocument? connectorXmlDocument)
        {
            _scopedService = new EditDecisionConnectorForm
            (
                Program.ServiceProvider.GetRequiredService<IConnectorDataParser>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                connectorIndexToSelect,
                connectorXmlDocument
            );
            return (IEditDecisionConnectorForm)_scopedService;
        }

        public IEditDecisionForm GetEditDecisionForm(XmlDocument? decisionXmlDocument)
        {
            _scopedService = new EditDecisionForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IDecisionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IDecisionFunctionListBoxItemFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditDecisionFormCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<ObjectRichTextBox>(),
                decisionXmlDocument
            );
            return (IEditDecisionForm)_scopedService;
        }

        public IEditDecisionsForm GetEditDecisionsForm(XmlDocument? decisionsXmlDocument)
        {
            _scopedService = new EditDecisionsForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IDecisionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IDecisionsDataParser>(),
                Program.ServiceProvider.GetRequiredService<IDecisionListBoxItemFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditDecisionsFormCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                decisionsXmlDocument
            );
            return (IEditDecisionsForm)_scopedService;
        }

        public IEditDialogConnectorForm GetEditDialogConnectorForm(short connectorIndexToSelect, XmlDocument? connectorXmlDocument)
        {
            _scopedService = new EditDialogConnectorForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IEditDialogConnectorControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                connectorIndexToSelect,
                connectorXmlDocument
            );
            return (IEditDialogConnectorForm)_scopedService;
        }

        public IEditDialogFunctionForm GetEditDialogFunctionForm(XmlDocument? functionsXmlDocument)
        {
            _scopedService = new EditDialogFunctionForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IEditDialogFunctionCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingFormHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFunctionsDataParser>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                functionsXmlDocument
            );
            return (IEditDialogFunctionForm)_scopedService;
        }

        public IEditFunctionsForm GetEditFunctionsForm(IDictionary<string, Function> functionDictionary, IList<TreeFolder> treeFolders, XmlDocument? functionsXmlDocument)
        {
            _scopedService = new EditFunctionsForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IEditFunctionsCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditFunctionsControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionListBoxItemFactory>(),
                Program.ServiceProvider.GetRequiredService<IFunctionsDataParser>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                new ObjectRichTextBox(),
                functionDictionary,
                treeFolders,
                functionsXmlDocument
            );
            return (IEditFunctionsForm)_scopedService;
        }

        public IEditJumpForm GetEditJumpForm(XmlDocument? jumpXmlDocument)
        {
            _scopedService = new EditJumpForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IJumpDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                jumpXmlDocument
            );
            return (IEditJumpForm)_scopedService;
        }

        public IEditModuleShapeForm GetEditModuleShapeForm(XmlDocument? moduleXmlDocument)
        {
            _scopedService = new EditModuleShapeForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IEditModuleShapeTreeViewBuilder>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IModuleDataParser>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                moduleXmlDocument
            );
            return (IEditModuleShapeForm)_scopedService;
        }

        public IEditParameterLiteralListForm GetEditParameterLiteralListForm(Type assignedTo, LiteralListParameterElementInfo literalListInfo, XmlDocument literalListXmlDocument)
        {
            _scopedService = new EditParameterLiteralListForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IEditingFormHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditLiteralListCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                assignedTo,
                literalListInfo,
                literalListXmlDocument
            );
            return (IEditParameterLiteralListForm)_scopedService;
        }

        public IEditParameterObjectListForm GetEditParameterObjectListForm(Type assignedTo, ObjectListParameterElementInfo objectListInfo, XmlDocument objectListXmlDocument)
        {
            _scopedService = new EditParameterObjectListForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IEditingFormHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditObjectListCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                assignedTo,
                objectListInfo,
                objectListXmlDocument
            );
            return (IEditParameterObjectListForm)_scopedService;
        }

        public IEditPriorityForm GetEditPriorityForm(XmlDocument? priorityXmlDocument)
        {
            _scopedService = new EditPriorityForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IPriorityDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                priorityXmlDocument
            );
            return (IEditPriorityForm)_scopedService;
        }

        public IEditValueFunctionForm GetEditValueFunctionForm(Type assignedTo, XmlDocument? functionXmlDocument)
        {
            _scopedService = new EditValueFunctionForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IEditValueFunctionCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingFormHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                assignedTo,
                functionXmlDocument
            );
            return (IEditValueFunctionForm)_scopedService;
        }

        public IEditVariableForm GetEditVariableForm(Type assignedTo)
        {
            _scopedService = new EditVariableForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                assignedTo
            );
            return (IEditVariableForm)_scopedService;
        }

        public IEditVariableLiteralListForm GetEditVariableLiteralListForm(Type assignedTo, LiteralListVariableElementInfo literalListInfo, XmlDocument literalListXmlDocument)
        {
            _scopedService = new EditVariableLiteralListForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IEditingFormHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditLiteralListCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                assignedTo,
                literalListInfo,
                literalListXmlDocument
            );
            return (IEditVariableLiteralListForm)_scopedService;
        }

        public IEditVariableObjectListForm GetEditVariableObjectListForm(Type assignedTo, ObjectListVariableElementInfo objectListInfo, XmlDocument objectListXmlDocument)
        {
            _scopedService = new EditVariableObjectListForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IEditingFormHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditObjectListCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                assignedTo,
                objectListInfo,
                objectListXmlDocument
            );
            return (IEditVariableObjectListForm)_scopedService;
        }

        public ISelectConstructorForm GetSelectConstructorForm(Type assignedTo)
        {
            _scopedService = new SelectConstructorForm
            (
                Program.ServiceProvider.GetRequiredService<IConfiguredItemControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                assignedTo
            );
            return (ISelectConstructorForm)_scopedService;
        }

        public ISelectFragmentForm GetSelectFragmentForm()
        {
            _scopedService = new SelectFragmentForm
            (
                Program.ServiceProvider.GetRequiredService<IConfiguredItemControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>()
            );
            return (ISelectFragmentForm)_scopedService;
        }

        public ISelectFromDomainForm GetSelectFromDomainForm(IList<string> domain, string comments)
        {
            _scopedService = new SelectFromDomainForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                domain,
                comments
            );
            return (ISelectFromDomainForm)_scopedService;
        }

        public ISelectFunctionForm GetSelectFunctionForm(Type assignedTo, IDictionary<string, Function> functionDictionary, IList<TreeFolder> treeFolders)
        {
            _scopedService = new SelectFunctionForm
            (
                Program.ServiceProvider.GetRequiredService<IConfiguredItemControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControl>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                assignedTo,
                functionDictionary,
                treeFolders
            );
            return (ISelectFunctionForm)_scopedService;
        }

        public void Dispose()
        {
            _scopedService?.Dispose();
        }
    }
}
