using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal class TryGetSelectedDocuments : ITryGetSelectedDocuments
    {
        public bool Try(out IList<string> selectedDocuments, IWin32Window dialogOwner)
        {
            using IScopedDisposableManager<SelectDocumentsForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<SelectDocumentsForm>>();
            SelectDocumentsForm selectDocunentsForm = disposableManager.ScopedService;
            selectDocunentsForm.ShowDialog(dialogOwner);

            if (selectDocunentsForm.DialogResult != DialogResult.OK
                || selectDocunentsForm.SourceFiles.Count == 0)
            {
                selectedDocuments = Array.Empty<string>();
                return false;
            }

            selectedDocuments = selectDocunentsForm.SourceFiles;
            return true;
        }
    }
}
