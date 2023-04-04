using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.LiteralListItemEditor.Helpers
{
    internal class LiteralListItemRichInputBoxEventsHelper : ILiteralListItemRichInputBoxEventsHelper
    {
        private readonly IRichInputBoxEventsHelper _richInputBoxEventsHelper;
        private readonly IRichInputBoxValueControl richInputBoxValueControl;

        public LiteralListItemRichInputBoxEventsHelper(
            IFieldControlHelperFactory fieldControlHelperFactory,
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            _richInputBoxEventsHelper = fieldControlHelperFactory.GetRichInputBoxEventsHelper(richInputBoxValueControl);
            this.richInputBoxValueControl = richInputBoxValueControl;
        }

        private RichInputBox RichInputBox => richInputBoxValueControl.RichInputBox;

        public void Setup()
        {
            RichInputBox.KeyUp += _richInputBoxEventsHelper.RichInputBox_KeyUp;
            RichInputBox.MouseClick += _richInputBoxEventsHelper.RichInputBox_MouseClick;
            RichInputBox.MouseUp += _richInputBoxEventsHelper.RichInputBox_MouseUp;
            RichInputBox.TextChanged += _richInputBoxEventsHelper.RichInputBox_TextChanged;
        }
    }
}
