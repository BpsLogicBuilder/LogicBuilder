using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Commands
{
    internal class SelectFragmentCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly RichTextBox richTextBox;

        public SelectFragmentCommand(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            RichTextBox richTextBox)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            this.richTextBox = richTextBox;
        }

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            ISelectFragmentForm selectFragmentForm = disposableManager.GetSelectFragmentForm();
            selectFragmentForm.ShowDialog(richTextBox);
            if (selectFragmentForm.DialogResult != DialogResult.OK)
                return;

            if (!_configurationService.FragmentList.Fragments.TryGetValue(selectFragmentForm.FragmentName, out Fragment? fragment))
                throw _exceptionHelper.CriticalException("{E219CBF5-8D9E-4850-965F-92B8FE908162}");

            this.richTextBox.SelectedText = fragment.Xml;
        }
    }
}
