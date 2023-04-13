﻿using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.ObjectListItemControls.Factories
{
    internal static class ParameterObjectListItemEditorControlFactoryServices
    {
        internal static IServiceCollection AddParameterObjectListItemEditorControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditingControl, ObjectListParameterElementInfo, IListOfObjectsParameterItemRichTextBoxControl>>
                (
                    provider =>
                    (editigControl, listInfo) => new ListOfObjectsParameterItemRichTextBoxControl
                    (
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListDataParser>(),
                        provider.GetRequiredService<ILiteralListParameterElementInfoHelper>(),
                        provider.GetRequiredService<IObjectListDataParser>(),
                        provider.GetRequiredService<IObjectListParameterElementInfoHelper>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        listInfo
                    )
                )
                .AddTransient<IParameterObjectListItemEditorControlFactory, ParameterObjectListItemEditorControlFactory>();
        }
    }
}