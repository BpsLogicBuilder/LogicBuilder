using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingFormHelperFactory : IEditingFormHelperFactory
    {
        public IDataGraphEditingFormEventsHelper GetDataGraphEditingFormEventsHelper(IDataGraphEditingForm dataGraphEditingForm)
            => new DataGraphEditingFormEventsHelper
            (
                Program.ServiceProvider.GetRequiredService<IEditingFormHelperFactory>(),
                dataGraphEditingForm
            );

        public IDataGraphEditingHostEventsHelper GetDataGraphEditingHostEventsHelper(IDataGraphEditingHost dataGraphEditingHost)
            => new DataGraphEditingHostEventsHelper
            (
                Program.ServiceProvider.GetRequiredService<IEditingFormHelperFactory>(),
                dataGraphEditingHost
            );

        public IDataGraphEditingManager GetDataGraphEditingManager(IDataGraphEditingHost dataGraphEditingHost)
            => new DataGraphEditingManager
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IEditFormFieldSetHelper>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingFormCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingFormHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                dataGraphEditingHost
            );

        public IParametersDataTreeBuilder GetParametersDataTreeBuilder(IDataGraphEditingHost dataGraphEditingHost)
            => new ParametersDataTreeBuilder
            (
                Program.ServiceProvider.GetRequiredService<IAssertFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IConstructorDataParser>(),
                Program.ServiceProvider.GetRequiredService<IDataGraphTreeViewHelper>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListDataParser>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListParameterElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListVariableElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<IObjectListDataParser>(),
                Program.ServiceProvider.GetRequiredService<IObjectListParameterElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<IObjectListVariableElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IVariableDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                dataGraphEditingHost
            );
    }
}
