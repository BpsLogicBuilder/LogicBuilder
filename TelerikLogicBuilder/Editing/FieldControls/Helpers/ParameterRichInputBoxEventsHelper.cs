﻿using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class ParameterRichInputBoxEventsHelper : IParameterRichInputBoxEventsHelper
    {
        private readonly IRichInputBoxEventsHelper _richInputBoxEventsHelper;
        private readonly IParameterRichInputBoxValueControl richInputBoxValueControl;

        public ParameterRichInputBoxEventsHelper(
            IEditingControlHelperFactory editingControlHelperFactory,
            IParameterRichInputBoxValueControl richInputBoxValueControl)
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
