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
        private readonly Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxConstructorCommand> _getEditObjectRichTextBoxConstructorCommand;
        private readonly Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxFunctionCommand> _getEditObjectRichTextBoxFunctionCommand;
        private readonly Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxLiteralListCommand> _getEditObjectRichTextBoxLiteralListCommand;
        private readonly Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxObjectListCommand> _getEditObjectRichTextBoxObjectListCommand;
        private readonly Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxVariableCommand> _getEditObjectRichTextBoxVariableCommand;
        private readonly Func<IRichInputBoxValueControl, EditRichInputBoxConstructorCommand> _getEditRichInputBoxConstructorCommand;
        private readonly Func<IRichInputBoxValueControl, EditRichInputBoxFunctionCommand> _getEditRichInputBoxFunctionCommand;
        private readonly Func<IRichInputBoxValueControl, EditRichInputBoxVariableCommand> _getEditRichInputBoxVariableCommand;
        private readonly Func<IRichInputBoxValueControl, PasteRichInputBoxTextCommand> _getPasteRichInputBoxTextCommand;
        private readonly Func<IDomainRichInputBoxValueControl, SelectDomainItemCommand> _getSelectDomainItemCommand;
        private readonly Func<IPropertyInputRichInputBoxControl, SelectItemFromPropertyListCommand> _getSelectItemFromPropertyListCommand;
        private readonly Func<IPropertyInputRichInputBoxControl, SelectItemFromReferencesTreeViewCommand> _getSelectItemFromReferencesTreeViewCommand;
        private readonly Func<IRichInputBoxValueControl, ToCamelCaseRichInputBoxCommand> _getToCamelCaseRichInputBoxCommand;

        public FieldControlCommandFactory(
            Func<IRichInputBoxValueControl, ClearRichInputBoxTextCommand> getClearRichInputBoxTextCommand,
            Func<IRichInputBoxValueControl, CopyRichInputBoxTextCommand> getCopyRichInputBoxTextCommand,
            Func<IRichInputBoxValueControl, CutRichInputBoxTextCommand> getCutRichInputBoxTextCommand,
            Func<IRichInputBoxValueControl, DeleteRichInputBoxTextCommand> getDeleteRichInputBoxTextCommand,
            Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxConstructorCommand> getEditObjectRichTextBoxConstructorCommand,
            Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxFunctionCommand> getEditObjectRichTextBoxFunctionCommand,
            Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxLiteralListCommand> getEditObjectRichTextBoxLiteralListCommand,
            Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxObjectListCommand> getEditObjectRichTextBoxObjectListCommand,
            Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxVariableCommand> getEditObjectRichTextBoxVariableCommand,
            Func<IRichInputBoxValueControl, EditRichInputBoxConstructorCommand> getEditRichInputBoxConstructorCommand,
            Func<IRichInputBoxValueControl, EditRichInputBoxFunctionCommand> getEditRichInputBoxFunctionCommand,
            Func<IRichInputBoxValueControl, EditRichInputBoxVariableCommand> getEditRichInputBoxVariableCommand,
            Func<IRichInputBoxValueControl, PasteRichInputBoxTextCommand> getPasteRichInputBoxTextCommand,
            Func<IDomainRichInputBoxValueControl, SelectDomainItemCommand> getSelectDomainItemCommand,
            Func<IPropertyInputRichInputBoxControl, SelectItemFromPropertyListCommand> getSelectItemFromPropertyListCommand,
            Func<IPropertyInputRichInputBoxControl, SelectItemFromReferencesTreeViewCommand> getSelectItemFromReferencesTreeViewCommand,
            Func<IRichInputBoxValueControl, ToCamelCaseRichInputBoxCommand> getToCamelCaseRichInputBoxCommand)
        {
            _getClearRichInputBoxTextCommand = getClearRichInputBoxTextCommand;
            _getCopyRichInputBoxTextCommand = getCopyRichInputBoxTextCommand;
            _getCutRichInputBoxTextCommand = getCutRichInputBoxTextCommand;
            _getDeleteRichInputBoxTextCommand = getDeleteRichInputBoxTextCommand;
            _getEditObjectRichTextBoxConstructorCommand = getEditObjectRichTextBoxConstructorCommand;
            _getEditObjectRichTextBoxFunctionCommand = getEditObjectRichTextBoxFunctionCommand;
            _getEditObjectRichTextBoxLiteralListCommand = getEditObjectRichTextBoxLiteralListCommand;
            _getEditObjectRichTextBoxObjectListCommand = getEditObjectRichTextBoxObjectListCommand;
            _getEditObjectRichTextBoxVariableCommand = getEditObjectRichTextBoxVariableCommand;
            _getEditRichInputBoxConstructorCommand = getEditRichInputBoxConstructorCommand;
            _getEditRichInputBoxFunctionCommand = getEditRichInputBoxFunctionCommand;
            _getEditRichInputBoxVariableCommand = getEditRichInputBoxVariableCommand;
            _getPasteRichInputBoxTextCommand = getPasteRichInputBoxTextCommand;
            _getSelectDomainItemCommand = getSelectDomainItemCommand;
            _getSelectItemFromPropertyListCommand = getSelectItemFromPropertyListCommand;
            _getSelectItemFromReferencesTreeViewCommand = getSelectItemFromReferencesTreeViewCommand;
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

        public EditObjectRichTextBoxConstructorCommand GetEditObjectRichTextBoxConstructorCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => _getEditObjectRichTextBoxConstructorCommand(objectRichTextBoxValueControl);

        public EditObjectRichTextBoxFunctionCommand GetEditObjectRichTextBoxFunctionCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => _getEditObjectRichTextBoxFunctionCommand(objectRichTextBoxValueControl);

        public EditObjectRichTextBoxLiteralListCommand GetEditObjectRichTextBoxLiteralListCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => _getEditObjectRichTextBoxLiteralListCommand(objectRichTextBoxValueControl);

        public EditObjectRichTextBoxObjectListCommand GetEditObjectRichTextBoxObjectListCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => _getEditObjectRichTextBoxObjectListCommand(objectRichTextBoxValueControl);

        public EditObjectRichTextBoxVariableCommand GetEditObjectRichTextBoxVariableCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => _getEditObjectRichTextBoxVariableCommand(objectRichTextBoxValueControl);

        public EditRichInputBoxConstructorCommand GetEditRichInputBoxConstructorCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getEditRichInputBoxConstructorCommand(richInputBoxValueControl);

        public EditRichInputBoxFunctionCommand GetEditRichInputBoxFunctionCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getEditRichInputBoxFunctionCommand(richInputBoxValueControl);

        public EditRichInputBoxVariableCommand GetEditRichInputBoxVariableCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getEditRichInputBoxVariableCommand(richInputBoxValueControl);

        public PasteRichInputBoxTextCommand GetPasteRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getPasteRichInputBoxTextCommand(richInputBoxValueControl);

        public SelectDomainItemCommand GetSelectDomainItemCommand(IDomainRichInputBoxValueControl richInputBoxValueControl)
            => _getSelectDomainItemCommand(richInputBoxValueControl);

        public SelectItemFromPropertyListCommand GetSelectItemFromPropertyListCommand(IPropertyInputRichInputBoxControl propertyInputRichInputBoxControl)
            => _getSelectItemFromPropertyListCommand(propertyInputRichInputBoxControl);

        public SelectItemFromReferencesTreeViewCommand GetSelectItemFromReferencesTreeViewCommand(IPropertyInputRichInputBoxControl propertyInputRichInputBoxControl)
            => _getSelectItemFromReferencesTreeViewCommand(propertyInputRichInputBoxControl);

        public ToCamelCaseRichInputBoxCommand GetToCamelCaseRichInputBoxCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => _getToCamelCaseRichInputBoxCommand(richInputBoxValueControl);
    }
}
