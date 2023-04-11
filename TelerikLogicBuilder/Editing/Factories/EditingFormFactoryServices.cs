using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
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
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        functionXmlDocument
                    )
                )
                .AddTransient<Func<Type, XmlDocument, HashSet<string>, string, IEditConstructorForm>>
                (
                    provider =>
                    (assignedTo, constructorXmlDocument, constructorNames, selectedConstructor) => new EditConstructorForm
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditConstructorCommandFactory>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        assignedTo,
                        constructorXmlDocument,
                        constructorNames,
                        selectedConstructor
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
                .AddTransient<Func<XmlDocument?, IEditFunctionsForm>>
                (
                    provider =>
                    functionsXmlDocument => new EditFunctionsForm
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
                        functionsXmlDocument
                    )
                )
                .AddTransient<IEditingFormFactory, EditingFormFactory>()
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
                        provider.GetRequiredService<IServiceFactory>(),
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
                        provider.GetRequiredService<IServiceFactory>(),
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
