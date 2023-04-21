using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBinaryFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueToNullFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditStandardFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.LiteralListItemControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.ObjectListItemControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.LiteralListItemControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.ObjectListItemControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Xml;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingControlFactoryServices
    {
        internal static IServiceCollection AddEditingControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IDataGraphEditingHost, Function, Type, XmlDocument, string, string?, IEditBinaryFunctionControl>>
                (
                    provider =>
                    (dataGraphEditingHost, function, assignedTo, formDocument, treeNodeXPath, selectedParameter) => new EditBinaryFunctionControl
                    (
                        provider.GetRequiredService<IBinaryFunctionTableLayoutPanelHelper>(),
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionElementValidator>(),
                        provider.GetRequiredService<IFunctionHelper>(),
                        provider.GetRequiredService<IFunctionParameterControlSetValidator>(),
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IGenericFunctionHelper>(),
                        provider.GetRequiredService<IParameterFieldControlFactory>(),
                        provider.GetRequiredService<IRadCheckBoxHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingHost,
                        function,
                        assignedTo,
                        formDocument,
                        treeNodeXPath,
                        selectedParameter
                    )
                )
                .AddTransient<Func<IApplicationForm, IEditConditionFunctionControl>>
                (
                    provider =>
                    parentForm => new EditConditionFunctionControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IEditConditionFunctionCommandFactory>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        parentForm
                    )
                )
                .AddTransient<Func<IDataGraphEditingHost, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl>>
                (
                    provider =>
                    (dataGraphEditingHost, constructor, assignedTo, formDocument, treeNodeXPath, selectedParameter) => new EditConstructorControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IConstructorDataParser>(),
                        provider.GetRequiredService<IConstructorElementValidator>(),
                        provider.GetRequiredService<IConstructorGenericsConfigrationValidator>(),
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IGenericConstructorHelper>(),
                        provider.GetRequiredService<IParameterElementValidator>(),
                        provider.GetRequiredService<IParameterFieldControlFactory>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<ITableLayoutPanelHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IUpdateParameterControlValues>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingHost,
                        constructor,
                        assignedTo, 
                        formDocument, 
                        treeNodeXPath, 
                        selectedParameter
                    )
                )
                .AddTransient<IEditingControlFactory, EditingControlFactory>()
                .AddTransient<Func<IDataGraphEditingHost, LiteralListParameterElementInfo, Type, XmlDocument, string, int?, IEditParameterLiteralListControl>>
                (
                    provider =>
                    (dataGraphEditingHost, literalListElementInfo, assignedTo, formDocument, treeNodeXPath, selectedParameter) => new EditParameterLiteralListControl
                    (
                        provider.GetRequiredService<IEditLiteralListCommandFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ILiteralListBoxItemFactory>(),
                        provider.GetRequiredService<ILiteralListDataParser>(),
                        provider.GetRequiredService<IParameterLiteralListItemEditorControlFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingHost,
                        literalListElementInfo,
                        assignedTo,
                        formDocument,
                        treeNodeXPath,
                        selectedParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingHost, ObjectListParameterElementInfo, Type, XmlDocument, string, int?, IEditParameterObjectListControl>>
                (
                    provider =>
                    (dataGraphEditingHost, objectListElementInfo, assignedTo, formDocument, treeNodeXPath, selectedParameter) => new EditParameterObjectListControl
                    (
                        provider.GetRequiredService<IEditObjectListCommandFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                        provider.GetRequiredService<IObjectListBoxItemFactory>(),
                        provider.GetRequiredService<IObjectListDataParser>(),
                        provider.GetRequiredService<IParameterObjectListItemEditorControlFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingHost,
                        objectListElementInfo,
                        assignedTo,
                        formDocument,
                        treeNodeXPath,
                        selectedParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingHost, Function, XmlDocument, string, IEditSetValueFunctionControl>>
                (
                    provider =>
                    (dataGraphEditingHost, function, formDocument, treeNodeXPath) => new EditSetValueFunctionControl
                    (
                        provider.GetRequiredService<IAssertFunctionDataParser>(),
                        provider.GetRequiredService<IAssertFunctionElementValidator>(),
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IEditSetValueFunctionCommandFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<ISetValueFunctionTableLayoutPanelHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IVariableValueControlFactory>(),
                        provider.GetRequiredService<IVariableValueDataParser>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingHost,
                        function,
                        formDocument,
                        treeNodeXPath
                    )
                )
                .AddTransient<Func<IDataGraphEditingHost, Function, XmlDocument, string, IEditSetValueToNullFunctionControl>>
                (
                    provider =>
                    (dataGraphEditingHost, function, formDocument, treeNodeXPath) => new EditSetValueToNullFunctionControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IEditSetValueFunctionCommandFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IRetractFunctionDataParser>(),
                        provider.GetRequiredService<IRetractFunctionElementValidator>(),
                        provider.GetRequiredService<ISetValueFunctionTableLayoutPanelHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingHost,
                        function,
                        formDocument,
                        treeNodeXPath
                    )
                )
                .AddTransient<Func<IDataGraphEditingHost, Function, Type, XmlDocument, string, string?, IEditStandardFunctionControl>>
                (
                    provider =>
                    (dataGraphEditingHost, function, assignedTo, formDocument, treeNodeXPath, selectedParameter) => new EditStandardFunctionControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionElementValidator>(),
                        provider.GetRequiredService<IFunctionHelper>(),
                        provider.GetRequiredService<IFunctionParameterControlSetValidator>(),
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IGenericFunctionHelper>(),
                        provider.GetRequiredService<IParameterFieldControlFactory>(),
                        provider.GetRequiredService<IRadCheckBoxHelper>(),
                        provider.GetRequiredService<ITableLayoutPanelHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingHost,
                        function,
                        assignedTo,
                        formDocument,
                        treeNodeXPath,
                        selectedParameter
                    )
            )
                .AddTransient<Func<IEditVariableHost, Type, IEditVariableControl>>
                (
                    provider =>
                    (editVariableHost, assignedToType) => new EditVariableControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IEditVariableViewControlFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editVariableHost,
                        assignedToType
                    )
                )
                .AddTransient<Func<IDataGraphEditingHost, LiteralListVariableElementInfo, Type, XmlDocument, string, int?, IEditVariableLiteralListControl>>
                (
                    provider =>
                    (dataGraphEditingHost, literalListElementInfo, assignedTo, formDocument, treeNodeXPath, selectedParameter) => new EditVariableLiteralListControl
                    (
                        provider.GetRequiredService<IEditLiteralListCommandFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ILiteralListBoxItemFactory>(),
                        provider.GetRequiredService<ILiteralListDataParser>(),
                        provider.GetRequiredService<IVariableLiteralListItemEditorControlFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingHost,
                        literalListElementInfo,
                        assignedTo,
                        formDocument,
                        treeNodeXPath,
                        selectedParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingHost, ObjectListVariableElementInfo, Type, XmlDocument, string, int?, IEditVariableObjectListControl>>
                (
                    provider =>
                    (dataGraphEditingHost, objectListElementInfo, assignedTo, formDocument, treeNodeXPath, selectedParameter) => new EditVariableObjectListControl
                    (
                        provider.GetRequiredService<IEditObjectListCommandFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                        provider.GetRequiredService<IObjectListBoxItemFactory>(),
                        provider.GetRequiredService<IObjectListDataParser>(),
                        provider.GetRequiredService<IVariableObjectListItemEditorControlFactory>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingHost,
                        objectListElementInfo,
                        assignedTo,
                        formDocument,
                        treeNodeXPath,
                        selectedParameter
                    )
                );
        }
    }
}
