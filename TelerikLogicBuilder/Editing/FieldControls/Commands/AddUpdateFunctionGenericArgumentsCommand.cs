using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
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
    internal class AddUpdateFunctionGenericArgumentsCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IGenericConfigManager _genericConfigManager;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IFunctionGenericParametersControl functionGenericParametersControl;

        public AddUpdateFunctionGenericArgumentsCommand(
            IConfigurationService configurationService,
            IFunctionDataParser functionDataParser,
            IGenericConfigManager genericConfigManager,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IFunctionGenericParametersControl functionGenericParametersControl)
        {
            _configurationService = configurationService;
            _functionDataParser = functionDataParser;
            _genericConfigManager = genericConfigManager;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.functionGenericParametersControl = functionGenericParametersControl;
        }

        private static readonly string XmlParentXPath = $"/{XmlDataConstants.NOTELEMENT}|/{XmlDataConstants.FUNCTIONELEMENT}";
        private static readonly string GenericArgumentsXPath = $"{XmlParentXPath}/{XmlDataConstants.GENERICARGUMENTSELEMENT}";

        private ApplicationTypeInfo Application => functionGenericParametersControl.Application;
        private Function Function => functionGenericParametersControl.Function;
        private XmlDocument XmlDocument => functionGenericParametersControl.XmlDocument;

        public override void Execute()
        {
            Function function = _configurationService.FunctionList.Functions[Function.Name];
            if (!_typeLoadHelper.TryGetSystemType(function.TypeName, Application, out Type? functionType))
            {
                functionGenericParametersControl.UpdateValidState();
                return;
            }

            if (!functionType.IsGenericTypeDefinition)
            {
                functionGenericParametersControl.UpdateValidState();
                return;
            }

            string[] genericArguments = functionType.GetGenericArguments()
                                                            .Select(a => a.Name)
                                                            .ToArray();

            FunctionData functionData = _functionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument)); ;
            IList<GenericConfigBase> configs = _genericConfigManager.ReconcileGenericArguments(genericArguments, functionData.GenericArguments);

            GenericConfigListResult result = UpdateGenericArgs(functionType, genericArguments, configs);
            if (result.DialogResult != DialogResult.OK)
                return;

            _xmlDocumentHelpers
                .SelectSingleElement(XmlDocument, GenericArgumentsXPath)
                .InnerXml = BuildGenericArgumentsXml(result.GenericConfigs);

            functionGenericParametersControl.ResetControls();
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
            IConfigureGenericArgumentsForm configureFunctionGenericArgumentsForm = disposableManager.GetConfigureFunctionGenericArgumentsForm
            (
                GetSourceDocument(_xmlDocumentHelpers.GetDocumentElement(XmlDocument)),
                genericArguments,
                Function.Parameters,
                genericTypeDefinition
            );

            configureFunctionGenericArgumentsForm.ShowDialog((IWin32Window)functionGenericParametersControl);

            return new GenericConfigListResult
            (
                configureFunctionGenericArgumentsForm.DialogResult,
                configureFunctionGenericArgumentsForm.DialogResult == DialogResult.OK
                    ? configureFunctionGenericArgumentsForm.GenericArguments
                    : Array.Empty<GenericConfigBase>()
            );

            XmlDocument GetSourceDocument(XmlElement documentElement)//ParametersDataSchema (used in ConfigureConstructorGenericArgumentsForm and ConfigureFunctionGenericArgumentsForm)
                => documentElement.Name == XmlDataConstants.NOTELEMENT//does not support <not /> at the root.  <not /> is supported for ConditionsDataSchema and DecisionsDataSchema (ConditionsForm and DecisionsForm only)
                        ? _xmlDocumentHelpers.ToXmlDocument(_xmlDocumentHelpers.GetSingleChildElement(documentElement))
                        : XmlDocument;
        }
    }
}
