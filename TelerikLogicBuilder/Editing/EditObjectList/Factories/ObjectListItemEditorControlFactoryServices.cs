﻿using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.ItemEditorControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ObjectListItemEditorControlFactoryServices
    {
        internal static IServiceCollection AddObjectListItemEditorControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditingControl, ObjectListParameterElementInfo, IListOfObjectsItemRichTextBoxControl>>
                (
                    provider =>
                    (editigControl, listInfo) => new ListOfObjectsItemRichTextBoxControl
                    (
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        listInfo
                    )
                )
                .AddTransient<IObjectListItemEditorControlFactory, ObjectListItemEditorControlFactory>();
        }
    }
}
