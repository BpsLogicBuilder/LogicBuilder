using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class AddUpdateConstructorGenericArgumentsCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IGenericConfigManager _genericConfigManager;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IConstructorGenericParametersControl constructorGenericParametersControl;

        public AddUpdateConstructorGenericArgumentsCommand(
            IConfigurationService configurationService,
            IConstructorDataParser constructorDataParser,
            IGenericConfigManager genericConfigManager,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConstructorGenericParametersControl constructorGenericParametersControl)
        {
            _configurationService = configurationService;
            _constructorDataParser = constructorDataParser;
            _genericConfigManager = genericConfigManager;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.constructorGenericParametersControl = constructorGenericParametersControl;
        }

        private static readonly string XmlParentXPath = $"/{XmlDataConstants.CONSTRUCTORELEMENT}";
        private static readonly string GenericArgumentsXPath = $"{XmlParentXPath}/{XmlDataConstants.GENERICARGUMENTSELEMENT}";

        private ApplicationTypeInfo Application => constructorGenericParametersControl.Application;
        private Constructor Constructor => constructorGenericParametersControl.Constructor;
        private XmlDocument XmlDocument => constructorGenericParametersControl.XmlDocument;

        public override void Execute()
        {
            Constructor constructor = _configurationService.ConstructorList.Constructors[Constructor.Name];
            if (!_typeLoadHelper.TryGetSystemType(constructor.TypeName, Application, out Type? constructorType))
            {
                constructorGenericParametersControl.UpdateValidState();
                return;
            }

            if (!constructorType.IsGenericTypeDefinition)
            {
                constructorGenericParametersControl.UpdateValidState();
                return;
            }

            string[] genericArguments = constructorType.GetGenericArguments()
                                                            .Select(a => a.Name)
                                                            .ToArray();

            ConstructorData constructorData = _constructorDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument)); ;
            IList<GenericConfigBase> configs = _genericConfigManager.ReconcileGenericArguments(genericArguments, constructorData.GenericArguments);

            GenericConfigListResult result = UpdateGenericArgs(constructorType, genericArguments, configs);
            if (result.DialogResult != DialogResult.OK)
                return;

            _xmlDocumentHelpers
                .SelectSingleElement(XmlDocument, GenericArgumentsXPath)
                .InnerXml = BuildGenericArgumentsXml(result.GenericConfigs);

            constructorGenericParametersControl.ResetControls();
        }

        string BuildGenericArgumentsXml(IList<GenericConfigBase> cenericConfigs)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
            {
                foreach (GenericConfigBase config in cenericConfigs)
                    xmlTextWriter.WriteRaw(config.ToXml);
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
            return stringBuilder.ToString();
        }

        private GenericConfigListResult UpdateGenericArgs(Type genericTypeDefinition, IList<string> genericArguments, IList<GenericConfigBase> genericArgs)
        {
            _xmlDocumentHelpers
                .SelectSingleElement(XmlDocument, GenericArgumentsXPath)
                .InnerXml = BuildGenericArgumentsXml(genericArgs);

            using IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
            IConfigureGenericArgumentsForm configureConstructorGenericArgumentsForm = disposableManager.GetConfigureConstructorGenericArgumentsForm
            (
                XmlDocument,
                genericArguments,
                Constructor.Parameters,
                genericTypeDefinition
            );

            configureConstructorGenericArgumentsForm.ShowDialog((IWin32Window)constructorGenericParametersControl);

            return new GenericConfigListResult
            (
                configureConstructorGenericArgumentsForm.DialogResult,
                configureConstructorGenericArgumentsForm.DialogResult == DialogResult.OK
                    ? configureConstructorGenericArgumentsForm.GenericArguments
                    : Array.Empty<GenericConfigBase>()
            );
        }
    }
}
