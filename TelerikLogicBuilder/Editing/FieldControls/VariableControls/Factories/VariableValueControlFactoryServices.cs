using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class VariableValueControlFactoryServices
    {
        internal static IServiceCollection AddVariableValueControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsVariable, ILiteralListVariableRichTextBoxControl>>
                (
                    provider =>
                    (editigControl, listOfLiteralsVariable) => new LiteralListVariableRichTextBoxControl
                    (
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListVariableElementInfoHelper>(),
                        provider.GetRequiredService<IObjectListDataParser>(),
                        provider.GetRequiredService<IObjectListVariableElementInfoHelper>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        listOfLiteralsVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableDomainAutoCompleteControl>>
                (
                    provider =>
                    (editigControl, literalVariable) => new LiteralVariableDomainAutoCompleteControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableDomainMultilineControl>>
                (
                    provider =>
                    (editigControl, literalVariable) => new LiteralVariableDomainMultilineControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableDomainRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalVariable) => new LiteralVariableDomainRichInputBoxControl
                    (
                       provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableDropDownListControl>>
                (
                    provider =>
                    (editigControl, literalVariable) => new LiteralVariableDropDownListControl
                    (
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableMultilineControl>>
                (
                    provider =>
                    (editigControl, literalVariable) => new LiteralVariableMultilineControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariablePropertyInputRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalVariable) => new LiteralVariablePropertyInputRichInputBoxControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalVariable) => new LiteralVariableRichInputBoxControl
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableTypeAutoCompleteControl>>
                (
                    provider =>
                    (editigControl, literalVariable) => new LiteralVariableTypeAutoCompleteControl
                    (
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfObjectsVariable, IObjectListVariableRichTextBoxControl>>
                (
                    provider =>
                    (editigControl, listOfObjectsVariable) => new ObjectListVariableRichTextBoxControl
                    (
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListDataParser>(),
                        provider.GetRequiredService<ILiteralListVariableElementInfoHelper>(),
                        provider.GetRequiredService<IObjectListVariableElementInfoHelper>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        listOfObjectsVariable
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ObjectVariable, IObjectVariableRichTextBoxControl>>
                (
                    provider =>
                    (editigControl, objectVariable) => new ObjectVariableRichTextBoxControl
                    (
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListDataParser>(),
                        provider.GetRequiredService<ILiteralListVariableElementInfoHelper>(),
                        provider.GetRequiredService<IObjectListDataParser>(),
                        provider.GetRequiredService<IObjectListVariableElementInfoHelper>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        objectVariable
                    )
                )
                .AddTransient<IVariableValueControlFactory, VariableValueControlFactory>();
        }
    }
}
