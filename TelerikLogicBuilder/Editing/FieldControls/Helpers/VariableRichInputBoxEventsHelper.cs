using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class VariableRichInputBoxEventsHelper : IVariableRichInputBoxEventsHelper
    {
        private readonly IRichInputBoxEventsHelper _richInputBoxEventsHelper;
        private readonly IVariableRichInputBoxValueControl richInputBoxValueControl;

        public VariableRichInputBoxEventsHelper(
            IFieldControlHelperFactory fieldControlHelperFactory,
            IVariableRichInputBoxValueControl richInputBoxValueControl)
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
            RichInputBox.Validated += RichInputBox_Validated;
        }

        private void RichInputBox_Validated(object? sender, EventArgs e)
        {
            if (RichInputBox.Modified)
            {
                richInputBoxValueControl.RequestDocumentUpdate();
                RichInputBox.Modified = false;
            }
        }
    }
}
