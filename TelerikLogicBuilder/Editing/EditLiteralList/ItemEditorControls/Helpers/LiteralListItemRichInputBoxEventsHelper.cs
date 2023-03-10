using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls.Helpers
{
    internal class LiteralListItemRichInputBoxEventsHelper : ILiteralListItemRichInputBoxEventsHelper
    {
        private readonly IRichInputBoxEventsHelper _richInputBoxEventsHelper;
        private readonly IRichInputBoxValueControl richInputBoxValueControl;

        public LiteralListItemRichInputBoxEventsHelper(
            IEditingControlHelperFactory editingControlHelperFactory,
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            _richInputBoxEventsHelper = editingControlHelperFactory.GetRichInputBoxEventsHelper(richInputBoxValueControl);
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
