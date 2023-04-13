using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.LiteralListItemEditor.Factories
{
    internal class ParameterLiteralListItemEditorControlFactory : IParameterLiteralListItemEditorControlFactory
    {
        private readonly Func<ListOfLiteralsParameter, IListOfLiteralsParameterItemDomainAutoCompleteControl> _getListOfLiteralsParameterItemDomainAutoCompleteControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterItemDomainMultilineControl> _getListOfLiteralsParameterItemDomainMultilineControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterItemDomainRichInputBoxControl> _getListOfLiteralsParameterItemDomainRichInputBoxControl;
        private readonly Func<ListOfLiteralsParameter, IListOfLiteralsParameterItemDropDownListControl> _getListOfLiteralsParameterItemDropDownListControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterItemMultilineControl> _getListOfLiteralsParameterItemMultilineControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterItemPropertyInputRichInputBoxControl> _getListOfLiteralsParameterItemPropertyInputRichInputBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralListParameterElementInfo, IListOfLiteralsParameterItemRichInputBoxControl> _getListOfLiteralsParameterItemRichInputBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralListParameterElementInfo, IListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl> _getListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl;
        private readonly IListOfLiteralsParameterItemTypeAutoCompleteControl _listOfLiteralsParameterItemTypeAutoCompleteControl;

        public ParameterLiteralListItemEditorControlFactory(
            Func<ListOfLiteralsParameter, IListOfLiteralsParameterItemDomainAutoCompleteControl> getListOfLiteralsParameterItemDomainAutoCompleteControl,
            Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterItemDomainMultilineControl> getListOfLiteralsParameterItemDomainMultilineControl,
            Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterItemDomainRichInputBoxControl> getListOfLiteralsParameterItemDomainRichInputBoxControl,
            Func<ListOfLiteralsParameter, IListOfLiteralsParameterItemDropDownListControl> getListOfLiteralsParameterItemDropDownListControl,
            Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterItemMultilineControl> getListOfLiteralsParameterItemMultilineControl,
            Func<IDataGraphEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterItemPropertyInputRichInputBoxControl> getListOfLiteralsParameterItemPropertyInputRichInputBoxControl,
            Func<IDataGraphEditingControl, LiteralListParameterElementInfo, IListOfLiteralsParameterItemRichInputBoxControl> getListOfLiteralsParameterItemRichInputBoxControl,
            Func<IDataGraphEditingControl, LiteralListParameterElementInfo, IListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl> getListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl,
            IListOfLiteralsParameterItemTypeAutoCompleteControl listOfLiteralsParameterItemTypeAutoCompleteControl)
        {
            _getListOfLiteralsParameterItemDomainAutoCompleteControl = getListOfLiteralsParameterItemDomainAutoCompleteControl;
            _getListOfLiteralsParameterItemDomainMultilineControl = getListOfLiteralsParameterItemDomainMultilineControl;
            _getListOfLiteralsParameterItemDomainRichInputBoxControl = getListOfLiteralsParameterItemDomainRichInputBoxControl;
            _getListOfLiteralsParameterItemDropDownListControl = getListOfLiteralsParameterItemDropDownListControl;
            _getListOfLiteralsParameterItemMultilineControl = getListOfLiteralsParameterItemMultilineControl;
            _getListOfLiteralsParameterItemPropertyInputRichInputBoxControl = getListOfLiteralsParameterItemPropertyInputRichInputBoxControl;
            _getListOfLiteralsParameterItemRichInputBoxControl = getListOfLiteralsParameterItemRichInputBoxControl;
            _getListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl = getListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl;
            _listOfLiteralsParameterItemTypeAutoCompleteControl = listOfLiteralsParameterItemTypeAutoCompleteControl;
        }

        public IListOfLiteralsParameterItemDomainAutoCompleteControl GetListOfLiteralsParameterItemDomainAutoCompleteControl(ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterItemDomainAutoCompleteControl(literalListParameter);

        public IListOfLiteralsParameterItemDomainMultilineControl GetListOfLiteralsParameterItemDomainMultilineControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterItemDomainMultilineControl(editingControl, literalListParameter);

        public IListOfLiteralsParameterItemDomainRichInputBoxControl GetListOfLiteralsParameterItemDomainRichInputBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterItemDomainRichInputBoxControl(editingControl, literalListParameter);

        public IListOfLiteralsParameterItemDropDownListControl GetListOfLiteralsParameterItemDropDownListControl(ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterItemDropDownListControl(literalListParameter);

        public IListOfLiteralsParameterItemMultilineControl GetListOfLiteralsParameterItemMultilineControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterItemMultilineControl(editingControl, literalListParameter);

        public IListOfLiteralsParameterItemPropertyInputRichInputBoxControl GetListOfLiteralsParameterItemPropertyInputRichInputBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterItemPropertyInputRichInputBoxControl(editingControl, literalListParameter);

        public IListOfLiteralsParameterItemRichInputBoxControl GetListOfLiteralsParameterItemRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralListParameterElementInfo listInfo)
            => _getListOfLiteralsParameterItemRichInputBoxControl(editingControl, listInfo);

        public IListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl GetListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralListParameterElementInfo listInfo)
            => _getListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl(editingControl, listInfo);

        public IListOfLiteralsParameterItemTypeAutoCompleteControl GetListOfLiteralsParameterItemTypeAutoCompleteControl()
            => _listOfLiteralsParameterItemTypeAutoCompleteControl;
    }
}
