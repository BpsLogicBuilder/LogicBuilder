using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Xml;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingControlFactoryServices
    {
        internal static IServiceCollection AddEditingControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditingForm, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl>>
                (
                    provider =>
                    (editingForm, constructor, assignedTo, formDocument, treeNodeXPath, selectedParameter) => new EditConstructorControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IConstructorDataParser>(),
                        provider.GetRequiredService<IConstructorGenericsConfigrationValidator>(),
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IFieldControlFactory>(),
                        provider.GetRequiredService<IGenericConstructorHelper>(),
                        provider.GetRequiredService<ITableLayoutPanelHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IUpdateParameterControlValues>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editingForm,
                        constructor,
                        assignedTo, 
                        formDocument, 
                        treeNodeXPath, 
                        selectedParameter
                    )
                )
                .AddTransient<IEditingControlFactory, EditingControlFactory>();
        }
    }
}
