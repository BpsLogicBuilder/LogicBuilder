using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
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
            RichInputBox.Disposed += RichInputBox_Disposed;
        }

        private RichInputBox RichInputBox => richInputBoxValueControl.RichInputBox;

        public void Setup()
        {
            RichInputBox.KeyUp += _richInputBoxEventsHelper.RichInputBox_KeyUp;
            RichInputBox.MouseClick += _richInputBoxEventsHelper.RichInputBox_MouseClick;
            RichInputBox.MouseUp += _richInputBoxEventsHelper.RichInputBox_MouseUp;
            RichInputBox.TextChanged += _richInputBoxEventsHelper.RichInputBox_TextChanged;
        }

        private void RichInputBox_Disposed(object? sender, System.EventArgs e)
        {
            RichInputBox.KeyUp -= _richInputBoxEventsHelper.RichInputBox_KeyUp;
            RichInputBox.MouseClick -= _richInputBoxEventsHelper.RichInputBox_MouseClick;
            RichInputBox.MouseUp -= _richInputBoxEventsHelper.RichInputBox_MouseUp;
            RichInputBox.TextChanged -= _richInputBoxEventsHelper.RichInputBox_TextChanged;
        }
    }
}
