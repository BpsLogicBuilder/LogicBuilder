using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal class LiteralListItemEditorControlFactory : ILiteralListItemEditorControlFactory
    {
        private readonly Func<ListOfLiteralsParameter, IListOfLiteralsItemDomainAutoCompleteControl> _getListOfLiteralsItemDomainAutoCompleteControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainMultilineControl> _getListOfLiteralsItemDomainMultilineControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainRichInputBoxControl> _getListOfLiteralsItemDomainRichInputBoxControl;
        private readonly Func<ListOfLiteralsParameter, IListOfLiteralsItemDropDownListControl> _getListOfLiteralsItemDropDownListControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemMultilineControl> _getListOfLiteralsItemMultilineControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemPropertyInputRichInputBoxControl> _getListOfLiteralsItemPropertyInputRichInputBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralListParameterElementInfo, IListOfLiteralsItemRichInputBoxControl> _getListOfLiteralsItemRichInputBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralListParameterElementInfo, IListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl> _getListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl;
        private readonly IListOfLiteralsItemTypeAutoCompleteControl _listOfLiteralsParameterTypeAutoCompleteControl;

        public LiteralListItemEditorControlFactory(
            Func<ListOfLiteralsParameter, IListOfLiteralsItemDomainAutoCompleteControl> getListOfLiteralsItemDomainAutoCompleteControl,
            Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainMultilineControl> getListOfLiteralsItemDomainMultilineControl,
            Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainRichInputBoxControl> getListOfLiteralsItemDomainRichInputBoxControl,
            Func<ListOfLiteralsParameter, IListOfLiteralsItemDropDownListControl> getListOfLiteralsItemDropDownListControl,
            Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemMultilineControl> getListOfLiteralsItemMultilineControl,
            Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemPropertyInputRichInputBoxControl> getListOfLiteralsItemPropertyInputRichInputBoxControl,
            Func<IDataGraphEditingControl, LiteralListParameterElementInfo, IListOfLiteralsItemRichInputBoxControl> getListOfLiteralsItemRichInputBoxControl,
            Func<IDataGraphEditingControl, LiteralListParameterElementInfo, IListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl> getListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl,
            IListOfLiteralsItemTypeAutoCompleteControl listOfLiteralsItemTypeAutoCompleteControl)
        {
            _getListOfLiteralsItemDomainAutoCompleteControl = getListOfLiteralsItemDomainAutoCompleteControl;
            _getListOfLiteralsItemDomainMultilineControl = getListOfLiteralsItemDomainMultilineControl;
            _getListOfLiteralsItemDomainRichInputBoxControl = getListOfLiteralsItemDomainRichInputBoxControl;
            _getListOfLiteralsItemDropDownListControl = getListOfLiteralsItemDropDownListControl;
            _getListOfLiteralsItemMultilineControl = getListOfLiteralsItemMultilineControl;
            _getListOfLiteralsItemPropertyInputRichInputBoxControl = getListOfLiteralsItemPropertyInputRichInputBoxControl;
            _getListOfLiteralsItemRichInputBoxControl = getListOfLiteralsItemRichInputBoxControl;
            _getListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl = getListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl;
            _listOfLiteralsParameterTypeAutoCompleteControl = listOfLiteralsItemTypeAutoCompleteControl;
        }

        public IListOfLiteralsItemDomainAutoCompleteControl GetListOfLiteralsItemDomainAutoCompleteControl(ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsItemDomainAutoCompleteControl(literalListParameter);

        public IListOfLiteralsItemDomainMultilineControl GetListOfLiteralsItemDomainMultilineControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsItemDomainMultilineControl(editingControl, literalListParameter);

        public IListOfLiteralsItemDomainRichInputBoxControl GetListOfLiteralsItemDomainRichInputBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsItemDomainRichInputBoxControl(editingControl, literalListParameter);

        public IListOfLiteralsItemDropDownListControl GetListOfLiteralsItemDropDownListControl(ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsItemDropDownListControl(literalListParameter);

        public IListOfLiteralsItemMultilineControl GetListOfLiteralsItemMultilineControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsItemMultilineControl(editingControl, literalListParameter);

        public IListOfLiteralsItemPropertyInputRichInputBoxControl GetListOfLiteralsItemPropertyInputRichInputBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsItemPropertyInputRichInputBoxControl(editingControl, literalListParameter);

        public IListOfLiteralsItemRichInputBoxControl GetListOfLiteralsItemRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralListParameterElementInfo listInfo)
            => _getListOfLiteralsItemRichInputBoxControl(editingControl, listInfo);

        public IListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl GetListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralListParameterElementInfo listInfo)
            => _getListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl(editingControl, listInfo);

        public IListOfLiteralsItemTypeAutoCompleteControl GetListOfLiteralsItemTypeAutoCompleteControl()
            => _listOfLiteralsParameterTypeAutoCompleteControl;
    }
}
