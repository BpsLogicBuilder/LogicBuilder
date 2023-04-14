using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.LiteralListItemControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.LiteralListItemControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class VariableLiteralListItemEditorControlFactoryServices
    {
        internal static IServiceCollection AddVariableLiteralListItemEditorControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<ListOfLiteralsVariable, IListOfLiteralsVariableItemDomainAutoCompleteControl>>
                (
                    provider =>
                    literalListVariable => new ListOfLiteralsVariableItemDomainAutoCompleteControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalListVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsVariable, IListOfLiteralsVariableItemDomainMultilineControl>>
                (
                    provider =>
                    (editigControl, literalListVariable) => new ListOfLiteralsVariableItemDomainMultilineControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalListVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsVariable, IListOfLiteralsVariableItemDomainRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalListVariable) => new ListOfLiteralsVariableItemDomainRichInputBoxControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalListVariable
                    )
                )
                .AddTransient<Func<ListOfLiteralsVariable, IListOfLiteralsVariableItemDropDownListControl>>
                (
                    provider =>
                    literalListVariable => new ListOfLiteralsVariableItemDropDownListControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalListVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsVariable, IListOfLiteralsVariableItemMultilineControl>>
                (
                    provider =>
                    (editigControl, literalListVariable) => new ListOfLiteralsVariableItemMultilineControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalListVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsVariable, IListOfLiteralsVariableItemPropertyInputRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalListVariable) => new ListOfLiteralsVariableItemPropertyInputRichInputBoxControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalListVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralListVariableElementInfo, IListOfLiteralsVariableItemRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, listInfo) => new ListOfLiteralsVariableItemRichInputBoxControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        listInfo
                    )
                )
                .AddTransient<Func<IListOfLiteralsVariableItemTypeAutoCompleteControl>>//factory should return a new one on every call
                (
                    provider =>
                    () => new ListOfLiteralsVariableItemTypeAutoCompleteControl
                    (
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>()
                    )
                )
                .AddTransient<IVariableLiteralListItemEditorControlFactory, VariableLiteralListItemEditorControlFactory>();
        }
    }
}
