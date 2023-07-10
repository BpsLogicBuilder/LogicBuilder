using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.ObjectListItemControls.Factories
{
    internal class VariableObjectListItemEditorControlFactory : IVariableObjectListItemEditorControlFactory
    {
        public IListOfObjectsVariableItemRichTextBoxControl GetListOfObjectsVariableItemRichTextBoxControl(IEditingControl editingControl, ObjectListVariableElementInfo listInfo)
            => new ListOfObjectsVariableItemRichTextBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListDataParser>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListVariableElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<IObjectListDataParser>(),
                Program.ServiceProvider.GetRequiredService<IObjectListVariableElementInfoHelper>(),
                new ObjectRichTextBox(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                listInfo
            );
    }
}
