using ABIS.LogicBuilder.FlowBuilder.Components.Factories;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector.Factories
{
    internal class EditDialogConnectorFieldControlFactory : IEditDialogConnectorFieldControlFactory
    {
        public IConnectorObjectRichTextBoxControl GetConnectorObjectRichTextBoxControl(IEditingControl editingControl)
            => new ConnectorObjectRichTextBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IConstructorTypeHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListDataParser>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListParameterElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<IObjectListDataParser>(),
                Program.ServiceProvider.GetRequiredService<IObjectListParameterElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl
            );

        public IConnectorTextRichInputBoxControl GetConnectorTextRichInputBoxControl(IEditingControl editingControl)
            => new ConnectorTextRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl
            );
    }
}
