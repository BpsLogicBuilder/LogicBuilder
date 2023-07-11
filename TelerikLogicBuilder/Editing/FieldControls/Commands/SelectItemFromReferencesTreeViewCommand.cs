using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.IncludesHelper;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class SelectItemFromReferencesTreeViewCommand : ClickCommandBase
    {
        private readonly ITypeLoadHelper _typeLoadHelper;

        private readonly IPropertyInputRichInputBoxControl propertyInputRichInputBoxControl;

        public SelectItemFromReferencesTreeViewCommand(
            ITypeLoadHelper typeLoadHelper,
            IPropertyInputRichInputBoxControl propertyInputRichInputBoxControl)
        {
            _typeLoadHelper = typeLoadHelper;
            this.propertyInputRichInputBoxControl = propertyInputRichInputBoxControl;
        }

        private ApplicationTypeInfo Application => propertyInputRichInputBoxControl.Application;
        private RichInputBox RichInputBox => propertyInputRichInputBoxControl.RichInputBox;
        private string? SourceClassName => propertyInputRichInputBoxControl.SourceClassName;

        public override void Execute()
        {
            string? sourceClass = SourceClassName;//We don't want SourceClassName running multiple times.
            if (!RichInputBox.IsSelectionEligibleForLink()
                || string.IsNullOrEmpty(sourceClass)
                || !Application.AssemblyAvailable)
                return;

            if (!_typeLoadHelper.TryGetSystemType(sourceClass, Application, out Type? _))
                return;

            IIntellisenseFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IIntellisenseFormFactory>();
            using IIncludesHelperForm includesHelperForm = disposableManager.GetIncludesHelperForm(sourceClass);

            includesHelperForm.ShowDialog(RichInputBox);
            if (includesHelperForm.DialogResult != DialogResult.OK)
                return;

            RichInputBox.Clear();
            RichInputBox.InsertText(includesHelperForm.Includes);
            propertyInputRichInputBoxControl.Focus();
        }
    }
}
