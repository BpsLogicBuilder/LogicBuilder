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
        private readonly Func<IParameterRichTextBoxValueControl, IEditParameterLiteralListHelper> _getEditParameterLiteralListHelper;
        private readonly Func<IParameterRichTextBoxValueControl, IEditParameterObjectListHelper> _getEditParameterObjectListHelper;
        private readonly Func<IVariableRichTextBoxValueControl, IEditVariableLiteralListHelper> _getEditVariableLiteralListHelper;
        private readonly Func<IVariableRichTextBoxValueControl, IEditVariableObjectListHelper> _getEditVariableObjectListHelper;
        private readonly Func<IRichInputBoxValueControl, ILiteralListItemRichInputBoxEventsHelper> _getLiteralListItemRichInputBoxEventsHelper;
        private readonly Func<IParameterRichTextBoxValueControl, IParameterObjectRichTextBoxEventsHelper> _getParameterObjectRichTextBoxEventsHelper;
        private readonly Func<IParameterRichInputBoxValueControl, IParameterRichInputBoxEventsHelper> _getParameterRichInputBoxEventsHelper;
        private readonly Func<IRichInputBoxValueControl, IRichInputBoxEventsHelper> _getRichInputBoxEventsHelper;
        private readonly Func<IObjectRichTextBoxValueControl, IUpdateObjectRichTextBoxXml> _getUpdateObjectRichTextBoxXml;
        private readonly Func<IVariableRichTextBoxValueControl, IVariableObjectRichTextBoxEventsHelper> _getVariableObjectRichTextBoxEventsHelper;
        private readonly Func<IVariableRichInputBoxValueControl, IVariableRichInputBoxEventsHelper> _getVariableRichInputBoxEventsHelper;

        public FieldControlHelperFactory(
            Func<IRichInputBoxValueControl, IEditLiteralConstructorHelper> getEditLiteralConstructorHelper,
            Func<IRichInputBoxValueControl, IEditLiteralFunctionHelper> getEditLiteralFunctionHelper,
            Func<IRichInputBoxValueControl, IEditLiteralVariableHelper> getEditLiteralVariableHelper,
            Func<IObjectRichTextBoxValueControl, IEditObjectConstructorHelper> getEditObjectConstructorHelper,
            Func<IObjectRichTextBoxValueControl, IEditObjectFunctionHelper> getEditObjectFunctionHelper,
            Func<IObjectRichTextBoxValueControl, IEditObjectVariableHelper> getEditObjectVariableHelper,
            Func<IParameterRichTextBoxValueControl, IEditParameterLiteralListHelper> getEditParameterLiteralListHelper,
            Func<IParameterRichTextBoxValueControl, IEditParameterObjectListHelper> getEditParameterObjectListHelper,
            Func<IVariableRichTextBoxValueControl, IEditVariableLiteralListHelper> getEditVariableLiteralListHelper,
            Func<IVariableRichTextBoxValueControl, IEditVariableObjectListHelper> getEditVariableObjectListHelper,
            Func<IRichInputBoxValueControl, ILiteralListItemRichInputBoxEventsHelper> getLiteralListItemRichInputBoxEventsHelper,
            Func<IParameterRichTextBoxValueControl, IParameterObjectRichTextBoxEventsHelper> getParameterObjectRichTextBoxEventsHelper,
            Func<IParameterRichInputBoxValueControl, IParameterRichInputBoxEventsHelper> getParameterRichInputBoxEventsHelper,
            Func<IRichInputBoxValueControl, IRichInputBoxEventsHelper> getRichInputBoxEventsHelper,
            Func<IObjectRichTextBoxValueControl, IUpdateObjectRichTextBoxXml> getUpdateObjectRichTextBoxXml,
            Func<IVariableRichTextBoxValueControl, IVariableObjectRichTextBoxEventsHelper> getVariableObjectRichTextBoxEventsHelper,
            Func<IVariableRichInputBoxValueControl, IVariableRichInputBoxEventsHelper> getVariableRichInputBoxEventsHelper)
        {
            _getEditLiteralConstructorHelper = getEditLiteralConstructorHelper;
            _getEditLiteralFunctionHelper = getEditLiteralFunctionHelper;
            _getEditLiteralVariableHelper = getEditLiteralVariableHelper;
            _getEditObjectConstructorHelper = getEditObjectConstructorHelper;
            _getEditObjectFunctionHelper = getEditObjectFunctionHelper;
            _getEditObjectVariableHelper = getEditObjectVariableHelper;
            _getEditParameterLiteralListHelper = getEditParameterLiteralListHelper;
            _getEditParameterObjectListHelper = getEditParameterObjectListHelper;
            _getEditVariableLiteralListHelper = getEditVariableLiteralListHelper;
            _getEditVariableObjectListHelper = getEditVariableObjectListHelper;
            _getLiteralListItemRichInputBoxEventsHelper = getLiteralListItemRichInputBoxEventsHelper;
            _getParameterObjectRichTextBoxEventsHelper = getParameterObjectRichTextBoxEventsHelper;
            _getParameterRichInputBoxEventsHelper = getParameterRichInputBoxEventsHelper;
            _getRichInputBoxEventsHelper = getRichInputBoxEventsHelper;
            _getUpdateObjectRichTextBoxXml = getUpdateObjectRichTextBoxXml;
            _getVariableObjectRichTextBoxEventsHelper = getVariableObjectRichTextBoxEventsHelper;
            _getVariableRichInputBoxEventsHelper = getVariableRichInputBoxEventsHelper;
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

        public IEditParameterLiteralListHelper GetEditParameterLiteralListHelper(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
            => _getEditParameterLiteralListHelper(parameterRichTextBoxValueControl);

        public IEditParameterObjectListHelper GetEditParameterObjectListHelper(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
            => _getEditParameterObjectListHelper(parameterRichTextBoxValueControl);

        public IEditVariableLiteralListHelper GetEditVariableLiteralListHelper(IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
            => _getEditVariableLiteralListHelper(variableRichTextBoxValueControl);

        public IEditVariableObjectListHelper GetEditVariableObjectListHelper(IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
            => _getEditVariableObjectListHelper(variableRichTextBoxValueControl);

        public ILiteralListItemRichInputBoxEventsHelper GetLiteralListItemRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => _getLiteralListItemRichInputBoxEventsHelper(richInputBoxValueControl);

        public IParameterObjectRichTextBoxEventsHelper GetParameterObjectRichTextBoxEventsHelper(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
            => _getParameterObjectRichTextBoxEventsHelper(parameterRichTextBoxValueControl);

        public IParameterRichInputBoxEventsHelper GetParameterRichInputBoxEventsHelper(IParameterRichInputBoxValueControl richInputBoxValueControl)
            => _getParameterRichInputBoxEventsHelper(richInputBoxValueControl);

        public IRichInputBoxEventsHelper GetRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => _getRichInputBoxEventsHelper(richInputBoxValueControl);

        public IUpdateObjectRichTextBoxXml GetUpdateObjectRichTextBoxXml(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => _getUpdateObjectRichTextBoxXml(objectRichTextBoxValueControl);

        public IVariableObjectRichTextBoxEventsHelper GetVariableObjectRichTextBoxEventsHelper(IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
            => _getVariableObjectRichTextBoxEventsHelper(variableRichTextBoxValueControl);

        public IVariableRichInputBoxEventsHelper GetVariableRichInputBoxEventsHelper(IVariableRichInputBoxValueControl variableRichInputBoxValueControl)
            => _getVariableRichInputBoxEventsHelper(variableRichInputBoxValueControl);
    }
}
