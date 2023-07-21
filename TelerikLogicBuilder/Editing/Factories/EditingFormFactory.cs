using ABIS.LogicBuilder.FlowBuilder.Components.Factories;
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
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingFormFactory : IEditingFormFactory
    {
        public IEditBooleanFunctionForm GetEditBooleanFunctionForm(XmlDocument? functionXmlDocument)
        {
            return new EditBooleanFunctionForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
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
        }

        public IEditConditionFunctionsForm GetEditConditionFunctionsForm(XmlDocument? conditionsXmlDocument)
        {
            return new EditConditionFunctionsForm
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
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
                conditionsXmlDocument
            );
        }

        public IEditConstructorForm GetEditConstructorForm(Type assignedTo, XmlDocument constructorXmlDocument, HashSet<string> constructorNames, string selectedConstructor, bool denySpecialCharacters)
        {
            return new EditConstructorForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
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
        }

        public IEditDecisionConnectorForm GetEditDecisionConnectorForm(short connectorIndexToSelect, XmlDocument? connectorXmlDocument)
        {
            return new EditDecisionConnectorForm
            (
                Program.ServiceProvider.GetRequiredService<IConnectorDataParser>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                connectorIndexToSelect,
                connectorXmlDocument
            );
        }

        public IEditDecisionForm GetEditDecisionForm(XmlDocument? decisionXmlDocument)
        {
            return new EditDecisionForm
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
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
                decisionXmlDocument
            );
        }

        public IEditDecisionsForm GetEditDecisionsForm(XmlDocument? decisionsXmlDocument)
        {
            return new EditDecisionsForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
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
        }

        public IEditDialogConnectorForm GetEditDialogConnectorForm(short connectorIndexToSelect, XmlDocument? connectorXmlDocument)
        {
            return new EditDialogConnectorForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditDialogConnectorControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                connectorIndexToSelect,
                connectorXmlDocument
            );
        }

        public IEditDialogFunctionForm GetEditDialogFunctionForm(XmlDocument? functionsXmlDocument)
        {
            return new EditDialogFunctionForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
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
        }

        public IEditFunctionsForm GetEditFunctionsForm(IDictionary<string, Function> functionDictionary, IList<TreeFolder> treeFolders, XmlDocument? functionsXmlDocument)
        {
            return new EditFunctionsForm
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditFunctionsCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditFunctionsControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionListBoxItemFactory>(),
                Program.ServiceProvider.GetRequiredService<IFunctionsDataParser>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                functionDictionary,
                treeFolders,
                functionsXmlDocument
            );
        }

        public IEditJumpForm GetEditJumpForm(XmlDocument? jumpXmlDocument)
        {
            return new EditJumpForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IJumpDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                jumpXmlDocument
            );
        }

        public IEditModuleShapeForm GetEditModuleShapeForm(XmlDocument? moduleXmlDocument)
        {
            return new EditModuleShapeForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditModuleShapeTreeViewBuilder>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IModuleDataParser>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                moduleXmlDocument
            );
        }

        public IEditParameterLiteralListForm GetEditParameterLiteralListForm(Type assignedTo, LiteralListParameterElementInfo literalListInfo, XmlDocument literalListXmlDocument)
        {
            return new EditParameterLiteralListForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
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
        }

        public IEditParameterObjectListForm GetEditParameterObjectListForm(Type assignedTo, ObjectListParameterElementInfo objectListInfo, XmlDocument objectListXmlDocument)
        {
            return new EditParameterObjectListForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
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
        }

        public IEditPriorityForm GetEditPriorityForm(XmlDocument? priorityXmlDocument)
        {
            return new EditPriorityForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IPriorityDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                priorityXmlDocument
            );
        }

        public IEditValueFunctionForm GetEditValueFunctionForm(Type assignedTo, XmlDocument? functionXmlDocument)
        {
            return new EditValueFunctionForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
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
        }

        public IEditVariableForm GetEditVariableForm(Type assignedTo)
        {
            return new EditVariableForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                assignedTo
            );
        }

        public IEditVariableLiteralListForm GetEditVariableLiteralListForm(Type assignedTo, LiteralListVariableElementInfo literalListInfo, XmlDocument literalListXmlDocument)
        {
            return new EditVariableLiteralListForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
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
        }

        public IEditVariableObjectListForm GetEditVariableObjectListForm(Type assignedTo, ObjectListVariableElementInfo objectListInfo, XmlDocument objectListXmlDocument)
        {
            return new EditVariableObjectListForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
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
        }

        public ISelectConstructorForm GetSelectConstructorForm(Type assignedTo)
        {
            return new SelectConstructorForm
            (
                Program.ServiceProvider.GetRequiredService<IConfiguredItemControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                assignedTo
            );
        }

        public ISelectFragmentForm GetSelectFragmentForm()
        {
            return new SelectFragmentForm
            (
                Program.ServiceProvider.GetRequiredService<IConfiguredItemControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>()
            );
        }

        public ISelectFromDomainForm GetSelectFromDomainForm(IList<string> domain, string comments)
        {
            return new SelectFromDomainForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                domain,
                comments
            );
        }

        public ISelectFunctionForm GetSelectFunctionForm(Type assignedTo, IDictionary<string, Function> functionDictionary, IList<TreeFolder> treeFolders)
        {
            return new SelectFunctionForm
            (
                Program.ServiceProvider.GetRequiredService<IConfiguredItemControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                assignedTo,
                functionDictionary,
                treeFolders
            );
        }
    }
}
