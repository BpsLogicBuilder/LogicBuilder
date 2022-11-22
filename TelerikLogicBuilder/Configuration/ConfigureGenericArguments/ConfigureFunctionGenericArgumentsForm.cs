using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments
{
    internal partial class ConfigureFunctionGenericArgumentsForm : Telerik.WinControls.UI.RadForm, IConfigureGenericArgumentsForm
    {
        public ConfigureFunctionGenericArgumentsForm(
            XmlDocument xmlDocument,
            IList<string> configuredGenericArgumentNames,
            IList<ParameterBase> memberParameters,
            Type genericTypeDefinition)
        {
            InitializeComponent();
            XmlDocument = xmlDocument;
            ConfiguredGenericArgumentNames = configuredGenericArgumentNames;
            MemberParameters = memberParameters;
            this.genericTypeDefinition = genericTypeDefinition;
            Initialize();
        }

        private readonly Type genericTypeDefinition;

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public ApplicationTypeInfo Application => throw new NotImplementedException();

        public IList<GenericConfigBase> GenericArguments => throw new NotImplementedException();

        public RadTreeView TreeView => throw new NotImplementedException();

        public XmlDocument XmlDocument { get; }

        public IList<string> ConfiguredGenericArgumentNames { get; }

        public IList<ParameterBase> MemberParameters { get; }

        private void Initialize()
        {
            ApplicationChanged?.Invoke(this, new ApplicationChangedEventArgs(null!));
        }

        public void ClearMessage()
        {
            throw new NotImplementedException();
        }

        public void SetErrorMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void SetMessage(string message, string title = "")
        {
            throw new NotImplementedException();
        }

        public void ValidateXmlDocument()
        {
            throw new NotImplementedException();
        }
    }
}
