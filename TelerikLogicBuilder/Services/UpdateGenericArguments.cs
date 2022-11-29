using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class UpdateGenericArguments : IUpdateGenericArguments
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGenericConfigManager _genericConfigManager;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IApplicationForm applicationForm;

        public UpdateGenericArguments(
            IExceptionHelper exceptionHelper,
            IGenericConfigManager genericConfigManager,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IApplicationForm applicationForm)
        {
            _exceptionHelper = exceptionHelper;
            _genericConfigManager = genericConfigManager;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.applicationForm = applicationForm;
        }

        public void Update(ITypeAutoCompleteTextControl control)
        {
            if (!_typeLoadHelper.TryGetSystemType(control.Text ?? "", this.applicationForm.Application, out Type? enteredType))
                throw _exceptionHelper.CriticalException("{2A4CE382-6F35-4095-880E-17A9160CB341}");

            if (!enteredType.IsGenericType)
                throw _exceptionHelper.CriticalException("{12A49EB7-0DA9-47D8-9361-E4C1259BBADA}");

            Type genericTypeDefinition = GetGenericTypeDefinition();
            string[] genericArguments = genericTypeDefinition.GetGenericArguments()
                                            .Select(a => a.Name)
                                            .ToArray();
            IList<GenericConfigBase> configs = GetGenericConfigs(genericArguments);
            GenericConfigListResult result = UpdateGenericArgs(genericTypeDefinition, genericArguments, configs);
            if (result.DialogResult != DialogResult.OK)
                return;

            this.applicationForm.ClearMessage();

            try
            {
                control.Text = _typeHelper.ToId
                (
                    genericTypeDefinition.MakeGenericType
                    (
                        result.GenericConfigs.Select(GetTypeFromConfig).ToArray()
                    )
                );
            }
            catch (ArgumentException ex)
            {
                this.applicationForm.SetErrorMessage(ex.Message);
            }

            Type GetTypeFromConfig(GenericConfigBase config)
            {
                if (!_typeLoadHelper.TryGetSystemType(config, this.applicationForm.Application, out Type? type))
                {
                    throw new ArgumentException
                    (
                        string.Format
                        (
                            CultureInfo.CurrentCulture, 
                            Strings.cannotLoadTypeForGenericArgument, 
                            config.GenericArgumentName
                        )
                    );
                }

                return type;
            }

            Type GetGenericTypeDefinition()
                => enteredType.IsGenericTypeDefinition ? enteredType : enteredType.GetGenericTypeDefinition();

            IList<GenericConfigBase> GetGenericConfigs(string[] genericArguments)
            {
                return enteredType.IsGenericTypeDefinition
                    ? _genericConfigManager.ReconcileGenericArguments(genericArguments, new List<GenericConfigBase>())
                    : _genericConfigManager.CreateGenericConfigs(genericArguments, enteredType.GetGenericArguments());
            }
        }

        private GenericConfigListResult UpdateGenericArgs(Type genericTypeDefinition, IList<string> genericArguments, IList<GenericConfigBase> genericArgs)
        {
            using IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
            IConfigureGenericArgumentsForm configureConstructorGenericArgumentsForm = disposableManager.GetConfigureConstructorGenericArgumentsForm
            (
                CreateXmlDocument(),//The type may not have a configured constructor so just create the document
                genericArguments,
                new List<ParameterBase>(),
                genericTypeDefinition
            );

            configureConstructorGenericArgumentsForm.ShowDialog((IWin32Window)applicationForm);

            return new GenericConfigListResult
            (
                configureConstructorGenericArgumentsForm.DialogResult,
                configureConstructorGenericArgumentsForm.DialogResult == DialogResult.OK 
                    ? configureConstructorGenericArgumentsForm.GenericArguments 
                    : Array.Empty<GenericConfigBase>()
            );

            XmlDocument CreateXmlDocument()
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
                {
                    xmlTextWriter.WriteStartElement(XmlDataConstants.CONSTRUCTORELEMENT);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, genericTypeDefinition.Name);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.VISIBLETEXTATTRIBUTE, genericTypeDefinition.Name);
                        xmlTextWriter.WriteStartElement(XmlDataConstants.GENERICARGUMENTSELEMENT);
                        foreach (GenericConfigBase config in genericArgs)
                            xmlTextWriter.WriteRaw(config.ToXml);
                        xmlTextWriter.WriteEndElement();
                        xmlTextWriter.WriteElementString(XmlDataConstants.PARAMETERSELEMENT, string.Empty);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.Flush();
                    xmlTextWriter.Close();
                }

                return _xmlDocumentHelpers.ToXmlDocument(stringBuilder.ToString());
            }
        }
    }
}
