using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.ObjectListItemControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.ObjectListItemControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class VariableObjectListItemEditorControlFactoryServices
    {
        internal static IServiceCollection AddVariableObjectListItemEditorControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditingControl, ObjectListVariableElementInfo, IListOfObjectsVariableItemRichTextBoxControl>>
                (
                    provider =>
                    (editigControl, listInfo) => new ListOfObjectsVariableItemRichTextBoxControl
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
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        listInfo
                    )
                )
                .AddTransient<IVariableObjectListItemEditorControlFactory, VariableObjectListItemEditorControlFactory>();
        }
    }
}
