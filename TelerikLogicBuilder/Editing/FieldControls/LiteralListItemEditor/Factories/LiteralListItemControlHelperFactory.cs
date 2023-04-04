using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.LiteralListItemEditor.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.LiteralListItemEditor.Factories
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
