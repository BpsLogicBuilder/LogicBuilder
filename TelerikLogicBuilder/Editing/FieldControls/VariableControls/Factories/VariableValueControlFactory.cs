using ABIS.LogicBuilder.FlowBuilder.Components.Factories;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.Factories
{
    internal class VariableValueControlFactory : IVariableValueControlFactory
    {
        public ILiteralListVariableRichTextBoxControl GetiteralListVariableRichTextBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsVariable listOfLiteralsVariable)
            => new LiteralListVariableRichTextBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListVariableElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<IObjectListDataParser>(),
                Program.ServiceProvider.GetRequiredService<IObjectListVariableElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                listOfLiteralsVariable
            );

        public ILiteralVariableDomainAutoCompleteControl GetLiteralVariableDomainAutoCompleteControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => new LiteralVariableDomainAutoCompleteControl
            (
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalVariable
            );

        public ILiteralVariableDomainMultilineControl GetLiteralVariableDomainMultilineControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => new LiteralVariableDomainMultilineControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalVariable
            );

        public ILiteralVariableDomainRichInputBoxControl GetLiteralVariableDomainRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => new LiteralVariableDomainRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalVariable
            );

        public ILiteralVariableDropDownListControl GetLiteralVariableDropDownListControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => new LiteralVariableDropDownListControl
            (
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalVariable
            );

        public ILiteralVariableMultilineControl GetLiteralVariableMultilineControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => new LiteralVariableMultilineControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalVariable
            );

        public ILiteralVariablePropertyInputRichInputBoxControl GetLiteralVariablePropertyInputRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => new LiteralVariablePropertyInputRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalVariable
            );

        public ILiteralVariableRichInputBoxControl GetLiteralVariableRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => new LiteralVariableRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalVariable
            );

        public ILiteralVariableTypeAutoCompleteControl GetLiteralVariableTypeAutoCompleteControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => new LiteralVariableTypeAutoCompleteControl
            (
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalVariable
            );

        public IObjectListVariableRichTextBoxControl GetObjectListVariableRichTextBoxControl(IDataGraphEditingControl editingControl, ListOfObjectsVariable listOfObjectsVariable)
            => new ObjectListVariableRichTextBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListDataParser>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListVariableElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<IObjectListVariableElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                listOfObjectsVariable
            );

        public IObjectVariableRichTextBoxControl GetObjectVariableRichTextBoxControl(IDataGraphEditingControl editingControl, ObjectVariable objectVariable)
            => new ObjectVariableRichTextBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListDataParser>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListVariableElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<IObjectListDataParser>(),
                Program.ServiceProvider.GetRequiredService<IObjectListVariableElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                objectVariable
            );
    }
}
