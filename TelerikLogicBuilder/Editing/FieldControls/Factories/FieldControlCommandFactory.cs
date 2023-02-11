using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal class FieldControlCommandFactory : IFieldControlCommandFactory
    {
        private readonly Func<IRichInputBoxValueControl, ClearRichInputBoxTextCommand> _getClearRichInputBoxTextCommand;
        private readonly Func<IRichInputBoxValueControl, CopyRichInputBoxTextCommand> _getCopyRichInputBoxTextCommand;
        private readonly Func<IRichInputBoxValueControl, CutRichInputBoxTextCommand> _getCutRichInputBoxTextCommand;
        private readonly Func<IRichInputBoxValueControl, DeleteRichInputBoxTextCommand> _getDeleteRichInputBoxTextCommand;
        private readonly Func<IRichInputBoxValueControl, EditRichInputBoxConstructorCommand> _getEditRichInputBoxConstructorCommand;
        private readonly Func<IRichInputBoxValueControl, EditRichInputBoxFunctionCommand> _getEditRichInputBoxFunctionCommand;
        private readonly Func<IRichInputBoxValueControl, EditRichInputBoxVariableCommand> _getEditRichInputBoxVariableCommand;
        private readonly Func<IRichInputBoxValueControl, PasteRichInputBoxTextCommand> _getPasteRichInputBoxTextCommand;
        private readonly Func<IRichInputBoxValueControl, ToCamelCaseRichInputBoxCommand> _getToCamelCaseRichInputBoxCommand;

        public FieldControlCommandFactory(
            Func<IRichInputBoxValueControl, ClearRichInputBoxTextCommand> getClearRichInputBoxTextCommand,
            Func<IRichInputBoxValueControl, CopyRichInputBoxTextCommand> getCopyRichInputBoxTextCommand,
            Func<IRichInputBoxValueControl, CutRichInputBoxTextCommand> getCutRichInputBoxTextCommand,
            Func<IRichInputBoxValueControl, DeleteRichInputBoxTextCommand> getDeleteRichInputBoxTextCommand,
            Func<IRichInputBoxValueControl, EditRichInputBoxConstructorCommand> getEditRichInputBoxConstructorCommand,
            Func<IRichInputBoxValueControl, EditRichInputBoxFunctionCommand> getEditRichInputBoxFunctionCommand,
            Func<IRichInputBoxValueControl, EditRichInputBoxVariableCommand> getEditRichInputBoxVariableCommand,
            Func<IRichInputBoxValueControl, PasteRichInputBoxTextCommand> getPasteRichInputBoxTextCommand,
            Func<IRichInputBoxValueControl, ToCamelCaseRichInputBoxCommand> getToCamelCaseRichInputBoxCommand)
        {
            _getClearRichInputBoxTextCommand = getClearRichInputBoxTextCommand;
            _getCopyRichInputBoxTextCommand = getCopyRichInputBoxTextCommand;
            _getCutRichInputBoxTextCommand = getCutRichInputBoxTextCommand;
            _getDeleteRichInputBoxTextCommand = getDeleteRichInputBoxTextCommand;
            _getEditRichInputBoxConstructorCommand = getEditRichInputBoxConstructorCommand;
            _getEditRichInputBoxFunctionCommand = getEditRichInputBoxFunctionCommand;
            _getEditRichInputBoxVariableCommand = getEditRichInputBoxVariableCommand;
            _getPasteRichInputBoxTextCommand = getPasteRichInputBoxTextCommand;
            _getToCamelCaseRichInputBoxCommand = getToCamelCaseRichInputBoxCommand;
        }

        public ClearRichInputBoxTextCommand GetClearRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getClearRichInputBoxTextCommand(richInputBoxValueControl);

        public CopyRichInputBoxTextCommand GetCopyRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getCopyRichInputBoxTextCommand(richInputBoxValueControl);

        public CutRichInputBoxTextCommand GetCutRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getCutRichInputBoxTextCommand(richInputBoxValueControl);

        public DeleteRichInputBoxTextCommand GetDeleteRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getDeleteRichInputBoxTextCommand(richInputBoxValueControl);

        public EditRichInputBoxConstructorCommand GetEditRichInputBoxConstructorCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getEditRichInputBoxConstructorCommand(richInputBoxValueControl);

        public EditRichInputBoxFunctionCommand GetEditRichInputBoxFunctionCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getEditRichInputBoxFunctionCommand(richInputBoxValueControl);

        public EditRichInputBoxVariableCommand GetEditRichInputBoxVariableCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getEditRichInputBoxVariableCommand(richInputBoxValueControl);

        public PasteRichInputBoxTextCommand GetPasteRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getPasteRichInputBoxTextCommand(richInputBoxValueControl);

        public ToCamelCaseRichInputBoxCommand GetToCamelCaseRichInputBoxCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getToCamelCaseRichInputBoxCommand(richInputBoxValueControl);
    }
}
