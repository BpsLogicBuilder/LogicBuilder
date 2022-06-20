using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal class TryGetSelectedRules : ITryGetSelectedRules
    {
        public bool Try(string selctedApplication, string title, out IList<string> selectedRules, IWin32Window dialogOwner)
        {
            using ISelectRulesFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<ISelectRulesFormFactory>();
            SelectRulesForm selectRulesForm = disposableManager.GetScopedService(selctedApplication);
            selectRulesForm.SetTitle(title);
            selectRulesForm.ShowDialog(dialogOwner);

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
