using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TelerikLogicBuilder.IntegrationTests.Mocks
{
    internal class ConfigureVariablesFormMock : IConfigureVariablesForm
    {
        public ConfigureVariablesFormMock(ApplicationTypeInfo application)
        {
            Application = application;
        }

        public IDictionary<string, string> ExpandedNodes => new Dictionary<string, string>();

        public ApplicationTypeInfo Application { get; }

        public DialogResult DialogResult => throw new NotImplementedException();

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SetErrorMessage(string message)
        {
            ApplicationChanged?.Invoke(this, new ApplicationChangedEventArgs(null!));
            throw new NotImplementedException();
        }

        public void SetMessage(string message, string title = "")
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            throw new NotImplementedException();
        }
    }
}
