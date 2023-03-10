using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingControlHelperFactoryServices
    {
        internal static IServiceCollection AddEditingControlHelperFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditFunctionControl, IEditFunctionControlHelper>>
                (
                    provider =>
                    editingFunctionControl => new EditFunctionControlHelper
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionGenericsConfigrationValidator>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IUpdateParameterControlValues>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editingFunctionControl
                    )
                )
                .AddTransient<IEditingControlHelperFactory, EditingControlHelperFactory>()
                .AddTransient<Func<IDataGraphEditingControl, IEditingForm, ILoadParameterControlsDictionary>>
                (
                    provider =>
                    (editingControl, edifForm) => new LoadParameterControlsDictionary
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFieldControlFactory>(),
                        provider.GetRequiredService<IRadCheckBoxHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        editingControl, 
                        edifForm
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, IRichInputBoxEventsHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new RichInputBoxEventsHelper
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IVariableDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        richInputBoxValueControl
                    )
                );
        }
    }
}
