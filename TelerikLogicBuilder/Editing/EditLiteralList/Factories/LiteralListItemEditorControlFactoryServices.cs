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
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterDomainAutoCompleteControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsParameterDomainAutoCompleteControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalListParameter
                    )
                )
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterDomainMultilineControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsParameterDomainMultilineControl
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
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterDomainRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsParameterDomainRichInputBoxControl
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
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterDropDownListControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsParameterDropDownListControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalListParameter
                    )
                )
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterMultilineControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsParameterMultilineControl
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
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterPropertyInputRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalListParameter) => new ListOfLiteralsParameterPropertyInputRichInputBoxControl
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
                .AddTransient<Func<IEditingControl, LiteralListParameterElementInfo, IListOfLiteralsParameterRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, listInfo) => new ListOfLiteralsParameterRichInputBoxControl
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
                .AddTransient<Func<IEditingControl, ListOfLiteralsParameter, IDictionary<string, ParameterControlSet>, IListOfLiteralsParameterSourcedPropertyRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalListParameter, editControlsSet) => new ListOfLiteralsParameterSourcedPropertyRichInputBoxControl
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
                        literalListParameter,
                        editControlsSet
                    )
                )
                .AddTransient<IListOfLiteralsParameterTypeAutoCompleteControl, ListOfLiteralsParameterTypeAutoCompleteControl>()
                .AddTransient<ILiteralListItemEditorControlFactory, LiteralListItemEditorControlFactory>();
        }
    }
}
