using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal class FieldControlHelperFactory : IFieldControlHelperFactory
    {
        private readonly Func<IRichInputBoxValueControl, IEditLiteralConstructorHelper> _getEditLiteralConstructorHelper;
        private readonly Func<IRichInputBoxValueControl, IEditLiteralFunctionHelper> _getEditLiteralFunctionHelper;
        private readonly Func<IRichInputBoxValueControl, IEditLiteralVariableHelper> _getEditLiteralVariableHelper;
        private readonly Func<IObjectRichTextBoxValueControl, IEditObjectConstructorHelper> _getEditObjectConstructorHelper;
        private readonly Func<IObjectRichTextBoxValueControl, IEditObjectFunctionHelper> _getEditObjectFunctionHelper;
        private readonly Func<IObjectRichTextBoxValueControl, IEditObjectVariableHelper> _getEditObjectVariableHelper;
        private readonly Func<IObjectRichTextBoxValueControl, IObjectRichTextBoxEventsHelper> _getObjectRichTextBoxEventsHelper;
        private readonly Func<IParameterRichInputBoxValueControl, IParameterRichInputBoxEventsHelper> _getParameterRichInputBoxEventsHelper;
        private readonly Func<IRichInputBoxValueControl, IRichInputBoxEventsHelper> _getRichInputBoxEventsHelper;
        private readonly Func<IObjectRichTextBoxValueControl, IUpdateObjectRichTextBoxXml> _getUpdateObjectRichTextBoxXml;

        public FieldControlHelperFactory(
            Func<IRichInputBoxValueControl, IEditLiteralConstructorHelper> getEditLiteralConstructorHelper,
            Func<IRichInputBoxValueControl, IEditLiteralFunctionHelper> getEditLiteralFunctionHelper,
            Func<IRichInputBoxValueControl, IEditLiteralVariableHelper> getEditLiteralVariableHelper,
            Func<IObjectRichTextBoxValueControl, IEditObjectConstructorHelper> getEditObjectConstructorHelper,
            Func<IObjectRichTextBoxValueControl, IEditObjectFunctionHelper> getEditObjectFunctionHelper,
            Func<IObjectRichTextBoxValueControl, IEditObjectVariableHelper> getEditObjectVariableHelper,
            Func<IObjectRichTextBoxValueControl, IObjectRichTextBoxEventsHelper> getObjectRichTextBoxEventsHelper,
            Func<IParameterRichInputBoxValueControl, IParameterRichInputBoxEventsHelper> getParameterRichInputBoxEventsHelper,
            Func<IRichInputBoxValueControl, IRichInputBoxEventsHelper> getRichInputBoxEventsHelper,
            Func<IObjectRichTextBoxValueControl, IUpdateObjectRichTextBoxXml> getUpdateObjectRichTextBoxXml)
        {
            _getEditLiteralConstructorHelper = getEditLiteralConstructorHelper;
            _getEditLiteralFunctionHelper = getEditLiteralFunctionHelper;
            _getEditLiteralVariableHelper = getEditLiteralVariableHelper;
            _getEditObjectConstructorHelper = getEditObjectConstructorHelper;
            _getEditObjectFunctionHelper = getEditObjectFunctionHelper;
            _getEditObjectVariableHelper = getEditObjectVariableHelper;
            _getObjectRichTextBoxEventsHelper = getObjectRichTextBoxEventsHelper;
            _getParameterRichInputBoxEventsHelper = getParameterRichInputBoxEventsHelper;
            _getRichInputBoxEventsHelper = getRichInputBoxEventsHelper;
            _getUpdateObjectRichTextBoxXml = getUpdateObjectRichTextBoxXml;
        }

        public IEditLiteralConstructorHelper GetEditLiteralConstructorHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => _getEditLiteralConstructorHelper(richInputBoxValueControl);

        public IEditLiteralFunctionHelper GetEditLiteralFunctionHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => _getEditLiteralFunctionHelper(richInputBoxValueControl);

        public IEditLiteralVariableHelper GetEditLiteralVariableHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => _getEditLiteralVariableHelper(richInputBoxValueControl);

        public IEditObjectConstructorHelper GetEditObjectConstructorHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => _getEditObjectConstructorHelper(objectRichTextBoxValueControl);

        public IEditObjectFunctionHelper GetEditObjectFunctionHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => _getEditObjectFunctionHelper(objectRichTextBoxValueControl);

        public IEditObjectVariableHelper GetEditObjectVariableHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => _getEditObjectVariableHelper(objectRichTextBoxValueControl);

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
