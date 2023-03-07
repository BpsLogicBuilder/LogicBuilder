using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal class LiteralListItemEditorControlFactory : ILiteralListItemEditorControlFactory
    {
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainAutoCompleteControl> _getListOfLiteralsItemDomainAutoCompleteControl;
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainMultilineControl> _getListOfLiteralsItemDomainMultilineControl;
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainRichInputBoxControl> _getListOfLiteralsItemDomainRichInputBoxControl;
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDropDownListControl> _getListOfLiteralsItemDropDownListControl;
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemMultilineControl> _getListOfLiteralsItemMultilineControl;
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemPropertyInputRichInputBoxControl> _getListOfLiteralsItemPropertyInputRichInputBoxControl;
        private readonly Func<IEditingControl, LiteralListParameterElementInfo, IListOfLiteralsItemRichInputBoxControl> _getListOfLiteralsItemRichInputBoxControl;
        private readonly Func<IEditingControl, LiteralListParameterElementInfo, IListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl> _getListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl;
        private readonly IListOfLiteralsItemTypeAutoCompleteControl _listOfLiteralsParameterTypeAutoCompleteControl;

        public LiteralListItemEditorControlFactory(
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainAutoCompleteControl> getListOfLiteralsItemDomainAutoCompleteControl,
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainMultilineControl> getListOfLiteralsItemDomainMultilineControl,
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDomainRichInputBoxControl> getListOfLiteralsItemDomainRichInputBoxControl,
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemDropDownListControl> getListOfLiteralsItemDropDownListControl,
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemMultilineControl> getListOfLiteralsItemMultilineControl,
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsItemPropertyInputRichInputBoxControl> getListOfLiteralsItemPropertyInputRichInputBoxControl,
            Func<IEditingControl, LiteralListParameterElementInfo, IListOfLiteralsItemRichInputBoxControl> getListOfLiteralsItemRichInputBoxControl,
            Func<IEditingControl, LiteralListParameterElementInfo, IListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl> getListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl,
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

        public IListOfLiteralsItemDomainAutoCompleteControl GetListOfLiteralsItemDomainAutoCompleteControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsItemDomainAutoCompleteControl(editingControl, literalListParameter);

        public IListOfLiteralsItemDomainMultilineControl GetListOfLiteralsItemDomainMultilineControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsItemDomainMultilineControl(editingControl, literalListParameter);

        public IListOfLiteralsItemDomainRichInputBoxControl GetListOfLiteralsItemDomainRichInputBoxControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsItemDomainRichInputBoxControl(editingControl, literalListParameter);

        public IListOfLiteralsItemDropDownListControl GetListOfLiteralsItemDropDownListControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsItemDropDownListControl(editingControl, literalListParameter);

        public IListOfLiteralsItemMultilineControl GetListOfLiteralsItemMultilineControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsItemMultilineControl(editingControl, literalListParameter);

        public IListOfLiteralsItemPropertyInputRichInputBoxControl GetListOfLiteralsItemPropertyInputRichInputBoxControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsItemPropertyInputRichInputBoxControl(editingControl, literalListParameter);

        public IListOfLiteralsItemRichInputBoxControl GetListOfLiteralsItemRichInputBoxControl(IEditingControl editingControl, LiteralListParameterElementInfo listInfo)
            => _getListOfLiteralsItemRichInputBoxControl(editingControl, listInfo);

        public IListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl GetListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl(IEditingControl editingControl, LiteralListParameterElementInfo listInfo)
            => _getListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl(editingControl, listInfo);

        public IListOfLiteralsItemTypeAutoCompleteControl GetListOfLiteralsItemTypeAutoCompleteControl()
            => _listOfLiteralsParameterTypeAutoCompleteControl;
    }
}
