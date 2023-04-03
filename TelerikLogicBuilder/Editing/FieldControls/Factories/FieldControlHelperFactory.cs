﻿using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal class FieldControlHelperFactory : IFieldControlHelperFactory
    {
        private readonly Func<IObjectRichTextBoxValueControl, IEditObjectVariableHelper> _getEditObjectVariableHelper;
        private readonly Func<IRichInputBoxValueControl, IEditVariableHelper> _getEditVariableHelper;
        private readonly Func<IObjectRichTextBoxValueControl, IObjectRichTextBoxEventsHelper> _getObjectRichTextBoxEventsHelper;
        private readonly Func<IParameterRichInputBoxValueControl, IParameterRichInputBoxEventsHelper> _getParameterRichInputBoxEventsHelper;
        private readonly Func<IRichInputBoxValueControl, IRichInputBoxEventsHelper> _getRichInputBoxEventsHelper;
        private readonly Func<IObjectRichTextBoxValueControl, IUpdateObjectRichTextBoxXml> _getUpdateObjectRichTextBoxXml;

        public FieldControlHelperFactory(
            Func<IObjectRichTextBoxValueControl, IEditObjectVariableHelper> getEditObjectVariableHelper,
            Func<IRichInputBoxValueControl, IEditVariableHelper> getEditVariableHelper,
            Func<IObjectRichTextBoxValueControl, IObjectRichTextBoxEventsHelper> getObjectRichTextBoxEventsHelper,
            Func<IParameterRichInputBoxValueControl, IParameterRichInputBoxEventsHelper> getParameterRichInputBoxEventsHelper,
            Func<IRichInputBoxValueControl, IRichInputBoxEventsHelper> getRichInputBoxEventsHelper,
            Func<IObjectRichTextBoxValueControl, IUpdateObjectRichTextBoxXml> getUpdateObjectRichTextBoxXml)
        {
            _getEditObjectVariableHelper = getEditObjectVariableHelper;
            _getEditVariableHelper = getEditVariableHelper;
            _getObjectRichTextBoxEventsHelper = getObjectRichTextBoxEventsHelper;
            _getParameterRichInputBoxEventsHelper = getParameterRichInputBoxEventsHelper;
            _getRichInputBoxEventsHelper = getRichInputBoxEventsHelper;
            _getUpdateObjectRichTextBoxXml = getUpdateObjectRichTextBoxXml;
        }

        public IEditObjectVariableHelper GetEditObjectVariableHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => _getEditObjectVariableHelper(objectRichTextBoxValueControl);

        public IEditVariableHelper GetEditVariableHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => _getEditVariableHelper(richInputBoxValueControl);

        public IObjectRichTextBoxEventsHelper GetObjectRichTextBoxEventsHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => _getObjectRichTextBoxEventsHelper(objectRichTextBoxValueControl);

        public IParameterRichInputBoxEventsHelper GetParameterRichInputBoxEventsHelper(IParameterRichInputBoxValueControl richInputBoxValueControl)
            => _getParameterRichInputBoxEventsHelper(richInputBoxValueControl);

        public IRichInputBoxEventsHelper GetRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => _getRichInputBoxEventsHelper(richInputBoxValueControl);

        public IUpdateObjectRichTextBoxXml GetUpdateObjectRichTextBoxXml(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => _getUpdateObjectRichTextBoxXml(objectRichTextBoxValueControl);
    }
}
