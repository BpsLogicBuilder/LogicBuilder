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
    internal static class ParameterLiteralListItemEditorControlFactoryServices
    {
        internal static IServiceCollection AddParameterLiteralListItemEditorControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<ListOfLiteralsParameter, IListOfLiteralsParameterItemDomainAutoCompleteControl>>
                (
                    provider =>
                    literalListParameter => new ListOfLiteralsParameterItemDomainAutoCompleteControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalListParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterItemDomainMultilineControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsParameterItemDomainMultilineControl
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
                        literalListParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterItemDomainRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsParameterItemDomainRichInputBoxControl
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
                        literalListParameter
                    )
                )
                .AddTransient<Func<ListOfLiteralsParameter, IListOfLiteralsParameterItemDropDownListControl>>
                (
                    provider =>
                    literalListParameter => new ListOfLiteralsParameterItemDropDownListControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalListParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterItemMultilineControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsParameterItemMultilineControl
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
                        literalListParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterItemPropertyInputRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsParameterItemPropertyInputRichInputBoxControl
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
                        literalListParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralListParameterElementInfo, IListOfLiteralsParameterItemRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, listInfo) => new ListOfLiteralsParameterItemRichInputBoxControl
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
                .AddTransient<Func<IDataGraphEditingControl, LiteralListParameterElementInfo, IListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, listInfo) => new ListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl
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
                .AddTransient<IListOfLiteralsParameterItemTypeAutoCompleteControl, ListOfLiteralsParameterItemTypeAutoCompleteControl>()
                .AddTransient<IParameterLiteralListItemEditorControlFactory, ParameterLiteralListItemEditorControlFactory>();
        }
    }
}
