using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFromDomain;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class SelectItemFromPropertyListCommand : ClickCommandBase
    {
        private readonly IIntellisenseHelper _intellisenseHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        private readonly IPropertyInputRichInputBoxControl propertyInputRichInputBoxControl;

        public SelectItemFromPropertyListCommand(
            IIntellisenseHelper intellisenseHelper,
            ITypeLoadHelper typeLoadHelper,
            IPropertyInputRichInputBoxControl propertyInputRichInputBoxControl)
        {
            _intellisenseHelper = intellisenseHelper;
            _typeLoadHelper = typeLoadHelper;
            this.propertyInputRichInputBoxControl = propertyInputRichInputBoxControl;
        }

        private ApplicationTypeInfo Application => propertyInputRichInputBoxControl.Application;
        private IRichInputBox RichInputBox => propertyInputRichInputBoxControl.RichInputBox;
        private string? SourceClassName => propertyInputRichInputBoxControl.SourceClassName;

        public override void Execute()
        {
            string? sourceClass = SourceClassName;//We don't want SourceClassName running multiple times.
            if (!RichInputBox.IsSelectionEligibleForLink()
                || string.IsNullOrEmpty(sourceClass)
                || !Application.AssemblyAvailable)
                return;

            if (!_typeLoadHelper.TryGetSystemType(sourceClass, Application, out Type? sourceType))
                return;

            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using ISelectFromDomainForm selectFromDomainForm = disposableManager.GetSelectFromDomainForm(GetDomain(), propertyInputRichInputBoxControl.Comments);

            selectFromDomainForm.ShowDialog((IWin32Window)RichInputBox);
            if (selectFromDomainForm.DialogResult != DialogResult.OK)
                return;

            RichInputBox.Clear();
            RichInputBox.InsertText(selectFromDomainForm.SelectedItem);
            propertyInputRichInputBoxControl.Focus();

            IList<string> GetDomain() => sourceType.GetProperties
            (
                _intellisenseHelper.GetBindingFlags(BindingFlagCategory.Instance)
            )
            .Select(p => p.Name)
            .Order()
            .ToArray();
        }
    }
}
