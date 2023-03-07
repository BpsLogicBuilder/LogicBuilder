using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class LiteralListItemEditorControlFactoryServices
    {
        internal static IServiceCollection AddLiteralListItemEditorControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainAutoCompleteControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsItemDomainAutoCompleteControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalListParameter
                    )
                )
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainMultilineControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsItemDomainMultilineControl
                    (
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
                        literalListParameter
                    )
                )
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsItemDomainRichInputBoxControl
                    (
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
                        literalListParameter
                    )
                )
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDropDownListControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsItemDropDownListControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalListParameter
                    )
                )
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemMultilineControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsItemMultilineControl
                    (
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
                        literalListParameter
                    )
                )
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemPropertyInputRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsItemPropertyInputRichInputBoxControl
                    (
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
                        literalListParameter
                    )
                )
                .AddTransient<Func<IEditingControl, LiteralListParameterElementInfo, IListOfLiteralsItemRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, listInfo) => new ListOfLiteralsItemRichInputBoxControl
                    (
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
                .AddTransient<Func<IEditingControl, LiteralListParameterElementInfo, IListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, listInfo) => new ListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl
                    (
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
                .AddTransient<IListOfLiteralsItemTypeAutoCompleteControl, ListOfLiteralsItemTypeAutoCompleteControl>()
                .AddTransient<ILiteralListItemEditorControlFactory, LiteralListItemEditorControlFactory>();
        }
    }
}
