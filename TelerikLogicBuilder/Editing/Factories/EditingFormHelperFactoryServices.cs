using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingFormHelperFactoryServices
    {
        internal static IServiceCollection AddEditingFormHelperFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IDataGraphEditingForm, IDataGraphEditingFormEventsHelper>>
                (
                    provider =>
                    dataGraphEditingForm => new DataGraphEditingFormEventsHelper
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IEditFormFieldSetHelper>(),
                        provider.GetRequiredService<IEditingControlFactory>(),
                        provider.GetRequiredService<IEditingFormCommandFactory>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingForm
                    )
                )
                .AddTransient<Func<IEditingForm, IParametersDataTreeBuilder>>
                (
                    provider =>
                    editingForm => new ParametersDataTreeBuilder
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IConstructorDataParser>(),
                        provider.GetRequiredService<IDataGraphTreeViewHelper>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IGetValidConfigurationFromData>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILiteralListDataParser>(),
                        provider.GetRequiredService<ILiteralListParameterElementInfoHelper>(),
                        provider.GetRequiredService<IObjectListDataParser>(),
                        provider.GetRequiredService<IObjectListParameterElementInfoHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IVariableDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editingForm
                    )
                )
                .AddTransient<IEditingFormHelperFactory, EditingFormHelperFactory>();
        }
    }
}
