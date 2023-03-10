using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls.Factories
{
    internal class LiteralListItemControlHelperFactory : ILiteralListItemControlHelperFactory
    {
        private readonly Func<IRichInputBoxValueControl, ILiteralListItemRichInputBoxEventsHelper> _getLiteralListItemRichInputBoxEventsHelper;

        public LiteralListItemControlHelperFactory(
            Func<IRichInputBoxValueControl, ILiteralListItemRichInputBoxEventsHelper> getLiteralListItemRichInputBoxEventsHelper)
        {
            _getLiteralListItemRichInputBoxEventsHelper = getLiteralListItemRichInputBoxEventsHelper;
        }

        public ILiteralListItemRichInputBoxEventsHelper GetLiteralListItemRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => _getLiteralListItemRichInputBoxEventsHelper(richInputBoxValueControl);
    }
}
