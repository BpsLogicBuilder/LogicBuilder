using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal class TryGetSelectedRules : ITryGetSelectedRules
    {
        private readonly IMainWindow _mainWindow;

        public TryGetSelectedRules(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool Try(string selctedApplication, string title, out IList<string> selectedRules)
        {
            using ISelectRulesFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<ISelectRulesFormFactory>();
            SelectRulesForm selectRulesForm = disposableManager.GetScopedService(selctedApplication);
            selectRulesForm.SetTitle(title);
            selectRulesForm.ShowDialog(_mainWindow.Instance);

            if (selectRulesForm.DialogResult != DialogResult.OK
                || selectRulesForm.SourceFiles.Count == 0)
            {
                selectedRules = Array.Empty<string>();
                return false;
            }

            selectedRules = selectRulesForm.SourceFiles;
            return true;
        }
    }
}
