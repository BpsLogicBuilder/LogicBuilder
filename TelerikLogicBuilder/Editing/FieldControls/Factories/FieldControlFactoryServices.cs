using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FieldControlFactoryServices
    {
        internal static IServiceCollection AddFieldControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IFieldControlFactory, FieldControlFactory>()
                .AddTransient<Func<IEditingControl, LiteralParameter, ILiteralParameterDomainAutoCompleteControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterDomainAutoCompleteControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalParameter
                    )
                )
                .AddTransient<Func<IEditingControl, LiteralParameter, ILiteralParameterDomainMultilineControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterDomainMultilineControl
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalParameter
                    )
                )
                .AddTransient<Func<IEditingControl, LiteralParameter, ILiteralParameterDomainRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterDomainRichInputBoxControl
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalParameter
                    )
                )
                .AddTransient<Func<IEditingControl, LiteralParameter, ILiteralParameterDropDownListControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterDropDownListControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalParameter
                    )
                )
                .AddTransient<Func<IEditingControl, LiteralParameter, ILiteralParameterMultilineControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterMultilineControl
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalParameter
                    )
                )
                .AddTransient<Func<IEditingControl, LiteralParameter, ILiteralParameterPropertyInputRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterPropertyInputRichInputBoxControl
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalParameter
                    )
                )
                .AddTransient<Func<IEditingControl, LiteralParameter, ILiteralParameterRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterRichInputBoxControl
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl, 
                        literalParameter
                    )
                )
                .AddTransient<Func<IEditingControl, LiteralParameter, IDictionary<string, ParameterControlSet>, ILiteralParameterSourcedPropertyRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalParameter, editControlsSet) => new LiteralParameterSourcedPropertyRichInputBoxControl
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalParameter,
                        editControlsSet
                    )
                )
                .AddTransient<Func<IEditingControl, LiteralParameter, ILiteralParameterTypeAutoCompleteControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterTypeAutoCompleteControl
                    (
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IUiNotificationService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl, 
                        literalParameter
                    )
                );
        }
    }
}
