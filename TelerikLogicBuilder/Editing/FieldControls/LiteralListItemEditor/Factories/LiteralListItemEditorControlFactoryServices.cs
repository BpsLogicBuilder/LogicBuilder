using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.LiteralListItemEditor.Factories
{
    internal static class LiteralListItemEditorControlFactoryServices
    {
        internal static IServiceCollection AddLiteralListItemEditorControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<ListOfLiteralsParameter, IListOfLiteralsItemDomainAutoCompleteControl>>
                (
                    provider =>
                    literalListParameter => new ListOfLiteralsItemDomainAutoCompleteControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalListParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainMultilineControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsItemDomainMultilineControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListItemControlHelperFactory>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalListParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsItemDomainRichInputBoxControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListItemControlHelperFactory>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalListParameter
                    )
                )
                .AddTransient<Func<ListOfLiteralsParameter, IListOfLiteralsItemDropDownListControl>>
                (
                    provider =>
                    literalListParameter => new ListOfLiteralsItemDropDownListControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalListParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemMultilineControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsItemMultilineControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListItemControlHelperFactory>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalListParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemPropertyInputRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsItemPropertyInputRichInputBoxControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListItemControlHelperFactory>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalListParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralListParameterElementInfo, IListOfLiteralsItemRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, listInfo) => new ListOfLiteralsItemRichInputBoxControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListItemControlHelperFactory>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        listInfo
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralListParameterElementInfo, IListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, listInfo) => new ListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListItemControlHelperFactory>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        listInfo
                    )
                )
                .AddTransient<IListOfLiteralsItemTypeAutoCompleteControl, ListOfLiteralsItemTypeAutoCompleteControl>()
                .AddTransient<ILiteralListItemEditorControlFactory, LiteralListItemEditorControlFactory>();
        }
    }
}
