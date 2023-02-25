using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
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
                .AddTransient<IEditingControlFactory, EditingControlFactory>()
                .AddTransient<Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditStandardFunctionControl>>
                (
                    provider =>
                    (editingForm, function, assignedTo, formDocument, treeNodeXPath, selectedParameter) => new EditStandardFunctionControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionGenericsConfigrationValidator>(),
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IFieldControlFactory>(),
                        provider.GetRequiredService<IGenericFunctionHelper>(),
                        provider.GetRequiredService<ITableLayoutPanelHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IUpdateParameterControlValues>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editingForm,
                        function,
                        assignedTo,
                        formDocument,
                        treeNodeXPath,
                        selectedParameter
                    )
                )
                .AddTransient<Func<IEditingForm, Type, IEditVariableControl>>
                (
                    provider =>
                    (editingForm, assignedToType) => new EditVariableControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<IEditVariableViewControlFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        editingForm,
                        assignedToType
                    )
                );
        }
    }
}
