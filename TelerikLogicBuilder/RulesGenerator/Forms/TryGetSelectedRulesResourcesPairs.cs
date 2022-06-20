using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal class TryGetSelectedRulesResourcesPairs : ITryGetSelectedRulesResourcesPairs
    {
        public bool Try(string selctedApplication, string title, out IList<RulesResourcesPair> sourceFiles, IWin32Window dialogOwner)
        {
            using ISelectRulesResourcesPairFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<ISelectRulesResourcesPairFormFactory>();
            SelectRulesResourcesPairForm selectRulesForm = disposableManager.GetScopedService(selctedApplication);
            selectRulesForm.SetTitle(title);
            selectRulesForm.ShowDialog(dialogOwner);

            if (selectRulesForm.DialogResult != DialogResult.OK
                || selectRulesForm.SourceFiles.Count == 0)
            {
                sourceFiles = Array.Empty<RulesResourcesPair>();
                return false;
            }

            sourceFiles = selectRulesForm.SourceFiles;
            return true;
        }
    }
}
