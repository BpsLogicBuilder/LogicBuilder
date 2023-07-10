using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.LiteralListItemControls.Factories
{
    internal class VariableLiteralListItemEditorControlFactory : IVariableLiteralListItemEditorControlFactory
    {
        public IListOfLiteralsVariableItemDomainAutoCompleteControl GetListOfLiteralsVariableItemDomainAutoCompleteControl(ListOfLiteralsVariable literalListVariable)
            => new ListOfLiteralsVariableItemDomainAutoCompleteControl
            (
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                literalListVariable
            );

        public IListOfLiteralsVariableItemDomainMultilineControl GetListOfLiteralsVariableItemDomainMultilineControl(IDataGraphEditingControl editingControl, ListOfLiteralsVariable literalListVariable)
            => new ListOfLiteralsVariableItemDomainMultilineControl
            (
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                Program.ServiceProvider.GetRequiredService<RichInputBox>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalListVariable
            );

        public IListOfLiteralsVariableItemDomainRichInputBoxControl GetListOfLiteralsVariableItemDomainRichInputBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsVariable literalListVariable)
            => new ListOfLiteralsVariableItemDomainRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                Program.ServiceProvider.GetRequiredService<RichInputBox>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalListVariable
            );

        public IListOfLiteralsVariableItemDropDownListControl GetListOfLiteralsVariableItemDropDownListControl(ListOfLiteralsVariable literalListVariable)
            => new ListOfLiteralsVariableItemDropDownListControl
            (
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                literalListVariable
            );

        public IListOfLiteralsVariableItemMultilineControl GetListOfLiteralsVariableItemMultilineControl(IDataGraphEditingControl editingControl, ListOfLiteralsVariable literalListVariable)
            => new ListOfLiteralsVariableItemMultilineControl
            (
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                Program.ServiceProvider.GetRequiredService<RichInputBox>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalListVariable
            );

        public IListOfLiteralsVariableItemPropertyInputRichInputBoxControl GetListOfLiteralsVariableItemPropertyInputRichInputBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsVariable literalListVariable)
            => new ListOfLiteralsVariableItemPropertyInputRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                Program.ServiceProvider.GetRequiredService<RichInputBox>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalListVariable
            );

        public IListOfLiteralsVariableItemRichInputBoxControl GetListOfLiteralsVariableItemRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralListVariableElementInfo listInfo)
            => new ListOfLiteralsVariableItemRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                Program.ServiceProvider.GetRequiredService<RichInputBox>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                listInfo
            );

        public IListOfLiteralsVariableItemTypeAutoCompleteControl GetListOfLiteralsVariableItemTypeAutoCompleteControl()
            => new ListOfLiteralsVariableItemTypeAutoCompleteControl
            (
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>()
            );
    }
}
