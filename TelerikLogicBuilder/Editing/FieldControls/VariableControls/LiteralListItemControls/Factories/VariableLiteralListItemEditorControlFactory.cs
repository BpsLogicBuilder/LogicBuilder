using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.LiteralListItemControls.Factories
{
    internal class VariableLiteralListItemEditorControlFactory : IVariableLiteralListItemEditorControlFactory
    {
        private readonly Func<ListOfLiteralsVariable, IListOfLiteralsVariableItemDomainAutoCompleteControl> _getListOfLiteralsVariableItemDomainAutoCompleteControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsVariable, IListOfLiteralsVariableItemDomainMultilineControl> _getListOfLiteralsVariableItemDomainMultilineControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsVariable, IListOfLiteralsVariableItemDomainRichInputBoxControl> _getListOfLiteralsVariableItemDomainRichInputBoxControl;
        private readonly Func<ListOfLiteralsVariable, IListOfLiteralsVariableItemDropDownListControl> _getListOfLiteralsVariableItemDropDownListControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsVariable, IListOfLiteralsVariableItemMultilineControl> _getListOfLiteralsVariableItemMultilineControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsVariable, IListOfLiteralsVariableItemPropertyInputRichInputBoxControl> _getListOfLiteralsVariableItemPropertyInputRichInputBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralListVariableElementInfo, IListOfLiteralsVariableItemRichInputBoxControl> _getListOfLiteralsVariableItemRichInputBoxControl;
        private readonly Func<IListOfLiteralsVariableItemTypeAutoCompleteControl> _getListOfLiteralsVariableItemTypeAutoCompleteControl;

        public VariableLiteralListItemEditorControlFactory(
            Func<ListOfLiteralsVariable, IListOfLiteralsVariableItemDomainAutoCompleteControl> getListOfLiteralsVariableItemDomainAutoCompleteControl,
            Func<IDataGraphEditingControl, ListOfLiteralsVariable, IListOfLiteralsVariableItemDomainMultilineControl> getListOfLiteralsVariableItemDomainMultilineControl,
            Func<IDataGraphEditingControl, ListOfLiteralsVariable, IListOfLiteralsVariableItemDomainRichInputBoxControl> getListOfLiteralsVariableItemDomainRichInputBoxControl,
            Func<ListOfLiteralsVariable, IListOfLiteralsVariableItemDropDownListControl> getListOfLiteralsVariableItemDropDownListControl,
            Func<IDataGraphEditingControl, ListOfLiteralsVariable, IListOfLiteralsVariableItemMultilineControl> getListOfLiteralsVariableItemMultilineControl,
            Func<IDataGraphEditingControl, ListOfLiteralsVariable, IListOfLiteralsVariableItemPropertyInputRichInputBoxControl> getListOfLiteralsVariableItemPropertyInputRichInputBoxControl,
            Func<IDataGraphEditingControl, LiteralListVariableElementInfo, IListOfLiteralsVariableItemRichInputBoxControl> getListOfLiteralsVariableItemRichInputBoxControl,
            Func<IListOfLiteralsVariableItemTypeAutoCompleteControl> getListOfLiteralsVariableItemTypeAutoCompleteControl)
        {
            _getListOfLiteralsVariableItemDomainAutoCompleteControl = getListOfLiteralsVariableItemDomainAutoCompleteControl;
            _getListOfLiteralsVariableItemDomainMultilineControl = getListOfLiteralsVariableItemDomainMultilineControl;
            _getListOfLiteralsVariableItemDomainRichInputBoxControl = getListOfLiteralsVariableItemDomainRichInputBoxControl;
            _getListOfLiteralsVariableItemDropDownListControl = getListOfLiteralsVariableItemDropDownListControl;
            _getListOfLiteralsVariableItemMultilineControl = getListOfLiteralsVariableItemMultilineControl;
            _getListOfLiteralsVariableItemPropertyInputRichInputBoxControl = getListOfLiteralsVariableItemPropertyInputRichInputBoxControl;
            _getListOfLiteralsVariableItemRichInputBoxControl = getListOfLiteralsVariableItemRichInputBoxControl;
            _getListOfLiteralsVariableItemTypeAutoCompleteControl = getListOfLiteralsVariableItemTypeAutoCompleteControl;
        }

        public IListOfLiteralsVariableItemDomainAutoCompleteControl GetListOfLiteralsVariableItemDomainAutoCompleteControl(ListOfLiteralsVariable literalListVariable)
            => _getListOfLiteralsVariableItemDomainAutoCompleteControl(literalListVariable);

        public IListOfLiteralsVariableItemDomainMultilineControl GetListOfLiteralsVariableItemDomainMultilineControl(IDataGraphEditingControl editingControl, ListOfLiteralsVariable literalListVariable)
            => _getListOfLiteralsVariableItemDomainMultilineControl(editingControl, literalListVariable);

        public IListOfLiteralsVariableItemDomainRichInputBoxControl GetListOfLiteralsVariableItemDomainRichInputBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsVariable literalListVariable)
            => _getListOfLiteralsVariableItemDomainRichInputBoxControl(editingControl, literalListVariable);

        public IListOfLiteralsVariableItemDropDownListControl GetListOfLiteralsVariableItemDropDownListControl(ListOfLiteralsVariable literalListVariable)
            => _getListOfLiteralsVariableItemDropDownListControl(literalListVariable);

        public IListOfLiteralsVariableItemMultilineControl GetListOfLiteralsVariableItemMultilineControl(IDataGraphEditingControl editingControl, ListOfLiteralsVariable literalListVariable)
            => _getListOfLiteralsVariableItemMultilineControl(editingControl, literalListVariable);

        public IListOfLiteralsVariableItemPropertyInputRichInputBoxControl GetListOfLiteralsVariableItemPropertyInputRichInputBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsVariable literalListVariable)
            => _getListOfLiteralsVariableItemPropertyInputRichInputBoxControl(editingControl, literalListVariable);

        public IListOfLiteralsVariableItemRichInputBoxControl GetListOfLiteralsVariableItemRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralListVariableElementInfo listInfo)
            => _getListOfLiteralsVariableItemRichInputBoxControl(editingControl, listInfo);

        public IListOfLiteralsVariableItemTypeAutoCompleteControl GetListOfLiteralsVariableItemTypeAutoCompleteControl()
            => _getListOfLiteralsVariableItemTypeAutoCompleteControl();
    }
}
