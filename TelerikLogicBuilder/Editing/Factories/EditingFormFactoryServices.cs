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
using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
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
using System;
using System.Collections.Generic;
using System.Xml;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingFormFactoryServices
    {
        internal static IServiceCollection AddEditingFormFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<XmlDocument?, IEditBooleanFunctionForm>>
                (
                    provider =>
                    functionXmlDocument => new EditBooleanFunctionForm
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditBooleanFunctionCommandFactory>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        functionXmlDocument
                    )
                )
                .AddTransient<Func<XmlDocument?, IEditConditionFunctionsForm>>
                (
                    provider =>
                    conditionsXmlDocument => new EditConditionFunctionsForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IConditionFunctionListBoxItemFactory>(),
                        provider.GetRequiredService<IConditionsDataParser>(),
                        provider.GetRequiredService<IEditConditionFunctionsFormCommandFactory>(),
                        provider.GetRequiredService<IEditingControlFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        conditionsXmlDocument
                    )
                )
                .AddTransient<Func<Type, XmlDocument, HashSet<string>, string, bool, IEditConstructorForm>>
                (
                    provider =>
                    (assignedTo, constructorXmlDocument, constructorNames, selectedConstructor, denySpecialCharacters) => new EditConstructorForm
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditConstructorCommandFactory>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        assignedTo,
                        constructorXmlDocument,
                        constructorNames,
                        selectedConstructor,
                        denySpecialCharacters
                    )
                )
                .AddTransient<Func<short, XmlDocument?, IEditDecisionConnectorForm>>
                (
                    provider =>
                    (connectorIndexToSelect, connectorXmlDocument) => new EditDecisionConnectorForm
                    (
                        provider.GetRequiredService<IConnectorDataParser>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        connectorIndexToSelect,
                        connectorXmlDocument
                    )
                )
                .AddTransient<Func<XmlDocument?, IEditDecisionForm>>
                (
                    provider =>
                    conditionsXmlDocument => new EditDecisionForm
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IDecisionDataParser>(),
                        provider.GetRequiredService<IDecisionFunctionListBoxItemFactory>(),
                        provider.GetRequiredService<IEditDecisionFormCommandFactory>(),
                        provider.GetRequiredService<IEditingControlFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        conditionsXmlDocument
                    )
                )
                .AddTransient<Func<XmlDocument?, IEditDecisionsForm>>
                (
                    provider =>
                    decisionsXmlDocument => new EditDecisionsForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IDecisionDataParser>(),
                        provider.GetRequiredService<IDecisionsDataParser>(),
                        provider.GetRequiredService<IDecisionListBoxItemFactory>(),
                        provider.GetRequiredService<IEditDecisionsFormCommandFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        decisionsXmlDocument
                    )
                )
                .AddTransient<Func<short, XmlDocument?, IEditDialogConnectorForm>>
                (
                    provider =>
                    (connectorIndexToSelect, connectorXmlDocument) => new EditDialogConnectorForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditDialogConnectorControlFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        connectorIndexToSelect,
                        connectorXmlDocument
                    )
                )
                .AddTransient<Func<XmlDocument?, IEditDialogFunctionForm>>
                (
                    provider =>
                    functionsXmlDocument => new EditDialogFunctionForm
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditDialogFunctionCommandFactory>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionHelper>(),
                        provider.GetRequiredService<IFunctionsDataParser>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        functionsXmlDocument
                    )
                )
                .AddTransient<Func<IDictionary<string, Function>, IList<TreeFolder>, XmlDocument?, IEditFunctionsForm>>
                (
                    provider =>
                    (functionDictionary, treeFolders, functionsXmlDocument) => new EditFunctionsForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditFunctionsCommandFactory>(),
                        provider.GetRequiredService<IEditFunctionsControlFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IFunctionListBoxItemFactory>(),
                        provider.GetRequiredService<IFunctionsDataParser>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        functionDictionary, 
                        treeFolders, 
                        functionsXmlDocument
                    )
                )
                .AddTransient<IEditingFormFactory, EditingFormFactory>()
                .AddTransient<Func<XmlDocument?, IEditJumpForm>>
                (
                    provider =>
                    jumpXmlDocument => new EditJumpForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IJumpDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        jumpXmlDocument
                    )
                )
                .AddTransient<Func<XmlDocument?, IEditModuleShapeForm>>
                (
                    provider =>
                    moduleXmlDocument => new EditModuleShapeForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditModuleShapeTreeViewBuilder>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IModuleDataParser>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        moduleXmlDocument
                    )
                )
                .AddTransient<Func<Type, LiteralListParameterElementInfo, XmlDocument, IEditParameterLiteralListForm>>
                (
                    provider =>
                    (assignedTo, literalListInfo, literalListXmlDocument) => new EditParameterLiteralListForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IEditLiteralListCommandFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        assignedTo,
                        literalListInfo,
                        literalListXmlDocument
                    )
                )
                .AddTransient<Func<Type, ObjectListParameterElementInfo, XmlDocument, IEditParameterObjectListForm>>
                (
                    provider =>
                    (assignedTo, literalListInfo, objectListXmlDocument) => new EditParameterObjectListForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IEditObjectListCommandFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        assignedTo,
                        literalListInfo,
                        objectListXmlDocument
                    )
                )
                .AddTransient<Func<Type, XmlDocument?, IEditValueFunctionForm>>
                (
                    provider =>
                    (assignedTo, functionXmlDocument) => new EditValueFunctionForm
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditValueFunctionCommandFactory>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        assignedTo,
                        functionXmlDocument
                    )
                )
                .AddTransient<Func<Type, IEditVariableForm>>
                (
                    provider =>
                    assignedTo => new EditVariableForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditingControlFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        assignedTo
                    )
                )
                .AddTransient<Func<Type, LiteralListVariableElementInfo, XmlDocument, IEditVariableLiteralListForm>>
                (
                    provider =>
                    (assignedTo, literalListInfo, literalListXmlDocument) => new EditVariableLiteralListForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IEditLiteralListCommandFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        assignedTo,
                        literalListInfo,
                        literalListXmlDocument
                    )
                )
                .AddTransient<Func<Type, ObjectListVariableElementInfo, XmlDocument, IEditVariableObjectListForm>>
                (
                    provider =>
                    (assignedTo, literalListInfo, objectListXmlDocument) => new EditVariableObjectListForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IEditObjectListCommandFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        assignedTo,
                        literalListInfo,
                        objectListXmlDocument
                    )
                )
                .AddTransient<Func<Type, ISelectConstructorForm>>
                (
                    provider =>
                    assignedTo => new SelectConstructorForm
                    (
                        provider.GetRequiredService<IConfiguredItemControlFactory>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        assignedTo
                    )
                )
                .AddTransient<Func<ISelectFragmentForm>>//the factory should not pass on the same injected instance if requested more than once
                (
                    provider =>
                    () => new SelectFragmentForm
                    (
                        provider.GetRequiredService<IConfiguredItemControlFactory>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IServiceFactory>()
                    )
                )
                .AddTransient<Func<IList<string>, string, ISelectFromDomainForm>>
                (
                    provider =>
                    (domain, comments) => new SelectFromDomainForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        domain, 
                        comments
                    )
                )
                .AddTransient<Func<Type, IDictionary<string, Function>, IList<TreeFolder>, ISelectFunctionForm>>
                (
                    provider =>
                    (assignedTo, functionDeictionary, treeFolders) => new SelectFunctionForm
                    (
                        provider.GetRequiredService<IConfiguredItemControlFactory>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        assignedTo,
                        functionDeictionary,
                        treeFolders
                    )
                );
        }
    }
}
