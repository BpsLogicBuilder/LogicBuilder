﻿using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal class TryGetSelectedDocuments : ITryGetSelectedDocuments
    {
        private readonly IMainWindow _mainWindow;

        public TryGetSelectedDocuments(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool Try(out IList<string> selectedDocuments)
        {
            using IScopedDisposableManager<ISelectDocumentsForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<ISelectDocumentsForm>>();
            ISelectDocumentsForm selectDocunentsForm = disposableManager.ScopedService;
            selectDocunentsForm.ShowDialog(_mainWindow.Instance);

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
