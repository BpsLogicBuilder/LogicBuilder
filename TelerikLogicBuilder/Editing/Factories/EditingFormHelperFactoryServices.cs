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
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        dataGraphEditingForm
                    )
                )
                .AddTransient<Func<IDataGraphEditingHost, IDataGraphEditingHostEventsHelper>>
                (
                    provider =>
                    dataGraphEditingHost => new DataGraphEditingHostEventsHelper
                    (
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        dataGraphEditingHost
                    )
                )
                .AddTransient<Func<IDataGraphEditingHost, IDataGraphEditingManager>>
                (
                    provider =>
                    dataGraphEditingHost => new DataGraphEditingManager
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IEditFormFieldSetHelper>(),
                        provider.GetRequiredService<IEditingControlFactory>(),
                        provider.GetRequiredService<IEditingFormCommandFactory>(),
                        provider.GetRequiredService<IEditingFormHelperFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingHost
                    )
                )
                .AddTransient<Func<IDataGraphEditingHost, IParametersDataTreeBuilder>>
                (
                    provider =>
                    dataGraphEditingHost => new ParametersDataTreeBuilder
                    (
                        provider.GetRequiredService<IAssertFunctionDataParser>(),
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
                        provider.GetRequiredService<ILiteralListVariableElementInfoHelper>(),
                        provider.GetRequiredService<IObjectListDataParser>(),
                        provider.GetRequiredService<IObjectListParameterElementInfoHelper>(),
                        provider.GetRequiredService<IObjectListVariableElementInfoHelper>(),
                        provider.GetRequiredService<IRetractFunctionDataParser>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IVariableDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingHost
                    )
                )
                .AddTransient<IEditingFormHelperFactory, EditingFormHelperFactory>();
        }
    }
}
