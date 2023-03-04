using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal class LiteralListItemEditorControlFactory : ILiteralListItemEditorControlFactory
    {
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterDomainAutoCompleteControl> _getListOfLiteralsParameterDomainAutoCompleteControl;
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterDomainMultilineControl> _getListOfLiteralsParameterDomainMultilineControl;
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterDomainRichInputBoxControl> _getListOfLiteralsParameterDomainRichInputBoxControl;
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterDropDownListControl> _getListOfLiteralsParameterDropDownListControl;
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterMultilineControl> _getListOfLiteralsParameterMultilineControl;
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterPropertyInputRichInputBoxControl> _getListOfLiteralsParameterPropertyInputRichInputBoxControl;
        private readonly Func<IEditingControl, LiteralListParameterElementInfo, IListOfLiteralsParameterRichInputBoxControl> _getListOfLiteralsParameterRichInputBoxControl;
        private readonly Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterSourcedPropertyRichInputBoxControl> _getListOfLiteralsParameterSourcedPropertyRichInputBoxControl;
        private readonly IListOfLiteralsParameterTypeAutoCompleteControl _listOfLiteralsParameterTypeAutoCompleteControl;

        public LiteralListItemEditorControlFactory(
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterDomainAutoCompleteControl> getListOfLiteralsParameterDomainAutoCompleteControl,
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterDomainMultilineControl> getListOfLiteralsParameterDomainMultilineControl,
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterDomainRichInputBoxControl> getListOfLiteralsParameterDomainRichInputBoxControl,
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterDropDownListControl> getListOfLiteralsParameterDropDownListControl,
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterMultilineControl> getListOfLiteralsParameterMultilineControl,
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterPropertyInputRichInputBoxControl> getListOfLiteralsParameterPropertyInputRichInputBoxControl,
            Func<IEditingControl, LiteralListParameterElementInfo, IListOfLiteralsParameterRichInputBoxControl> getListOfLiteralsParameterRichInputBoxControl,
            Func<IEditingControl, ListOfLiteralsParameter, IListOfLiteralsParameterSourcedPropertyRichInputBoxControl> getListOfLiteralsParameterSourcedPropertyRichInputBoxControl,
            IListOfLiteralsParameterTypeAutoCompleteControl listOfLiteralsParameterTypeAutoCompleteControl)
        {
            _getListOfLiteralsParameterDomainAutoCompleteControl = getListOfLiteralsParameterDomainAutoCompleteControl;
            _getListOfLiteralsParameterDomainMultilineControl = getListOfLiteralsParameterDomainMultilineControl;
            _getListOfLiteralsParameterDomainRichInputBoxControl = getListOfLiteralsParameterDomainRichInputBoxControl;
            _getListOfLiteralsParameterDropDownListControl = getListOfLiteralsParameterDropDownListControl;
            _getListOfLiteralsParameterMultilineControl = getListOfLiteralsParameterMultilineControl;
            _getListOfLiteralsParameterPropertyInputRichInputBoxControl = getListOfLiteralsParameterPropertyInputRichInputBoxControl;
            _getListOfLiteralsParameterRichInputBoxControl = getListOfLiteralsParameterRichInputBoxControl;
            _getListOfLiteralsParameterSourcedPropertyRichInputBoxControl = getListOfLiteralsParameterSourcedPropertyRichInputBoxControl;
            _listOfLiteralsParameterTypeAutoCompleteControl = listOfLiteralsParameterTypeAutoCompleteControl;
        }

        public IListOfLiteralsParameterDomainAutoCompleteControl GetListOfLiteralsParameterDomainAutoCompleteControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterDomainAutoCompleteControl(editingControl, literalListParameter);

        public IListOfLiteralsParameterDomainMultilineControl GetListOfLiteralsParameterDomainMultilineControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterDomainMultilineControl(editingControl, literalListParameter);

        public IListOfLiteralsParameterDomainRichInputBoxControl GetListOfLiteralsParameterDomainRichInputBoxControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterDomainRichInputBoxControl(editingControl, literalListParameter);

        public IListOfLiteralsParameterDropDownListControl GetListOfLiteralsParameterDropDownListControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterDropDownListControl(editingControl, literalListParameter);

        public IListOfLiteralsParameterMultilineControl GetListOfLiteralsParameterMultilineControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterMultilineControl(editingControl, literalListParameter);

        public IListOfLiteralsParameterPropertyInputRichInputBoxControl GetListOfLiteralsParameterPropertyInputRichInputBoxControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterPropertyInputRichInputBoxControl(editingControl, literalListParameter);

        public IListOfLiteralsParameterRichInputBoxControl GetListOfLiteralsParameterRichInputBoxControl(IEditingControl editingControl, LiteralListParameterElementInfo listInfo)
            => _getListOfLiteralsParameterRichInputBoxControl(editingControl, listInfo);

        public IListOfLiteralsParameterSourcedPropertyRichInputBoxControl GetListOfLiteralsParameterSourcedPropertyRichInputBoxControl(IEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => _getListOfLiteralsParameterSourcedPropertyRichInputBoxControl(editingControl, literalListParameter);

        public IListOfLiteralsParameterTypeAutoCompleteControl GetListOfLiteralsParameterTypeAutoCompleteControl()
            => _listOfLiteralsParameterTypeAutoCompleteControl;
    }
}
