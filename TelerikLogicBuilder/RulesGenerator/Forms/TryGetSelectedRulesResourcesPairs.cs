using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal class TryGetSelectedRulesResourcesPairs : ITryGetSelectedRulesResourcesPairs
    {
        private readonly IMainWindow _mainWindow;

        public TryGetSelectedRulesResourcesPairs(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool Try(string selctedApplication, string title, out IList<RulesResourcesPair> sourceFiles)
        {
            using ISelectRulesResourcesPairFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<ISelectRulesResourcesPairFormFactory>();
            SelectRulesResourcesPairForm selectRulesForm = disposableManager.GetScopedService(selctedApplication);
            selectRulesForm.SetTitle(title);
            selectRulesForm.ShowDialog(_mainWindow.Instance);

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
