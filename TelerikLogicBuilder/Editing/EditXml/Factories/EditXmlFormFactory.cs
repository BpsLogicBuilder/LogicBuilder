using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories
{
    internal class EditXmlFormFactory : IEditXmlFormFactory
    {
        private IDisposable? _scopedService;
        public IEditBooleanFunctionFormXml GetEditBooleanFunctionFormXml(string xml, Type assignedTo)
        {
            _scopedService = new EditBooleanFunctionFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                new EditXmlRichTextBoxPanel(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml,
                assignedTo
            );
            return (IEditBooleanFunctionFormXml)_scopedService;
        }

        public IEditBuildDecisionFormXml GetEditBuildDecisionFormXml(string xml)
        {
            _scopedService = new EditBuildDecisionFormXml
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDecisionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                new EditXmlRichTextBoxPanel(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml
            );
            return (IEditBuildDecisionFormXml)_scopedService;
        }

        public IEditConditionsFormXml GetEditConditionsFormXml(string xml)
        {
            _scopedService = new EditConditionsFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConditionsElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                new EditXmlRichTextBoxPanel(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml
            );
            return (IEditConditionsFormXml)_scopedService;
        }

        public IEditConstructorFormXml GetEditConstructorFormXml(string xml, Type assignedTo)
        {
            _scopedService = new EditConstructorFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConstructorElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                new EditXmlRichTextBoxPanel(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml,
                assignedTo
            );
            return (IEditConstructorFormXml)_scopedService;
        }

        public IEditDecisionsFormXml GetEditDecisionsFormXml(string xml)
        {
            _scopedService = new EditDecisionsFormXml
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDecisionsElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                new EditXmlRichTextBoxPanel(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml
            );
            return (IEditDecisionsFormXml)_scopedService;
        }

        public IEditDialogFunctionFormXml GetEditDialogFunctionFormXml(string xml)
        {
            _scopedService = new EditDialogFunctionFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFunctionsDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                new EditXmlRichTextBoxPanel(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml
            );
            return (IEditDialogFunctionFormXml)_scopedService;
        }

        public IEditFunctionsFormXml GetEditFunctionsFormXml(string xml)
        {
            _scopedService = new EditFunctionsFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFunctionsDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionsElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                new EditXmlRichTextBoxPanel(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml
            );
            return (IEditFunctionsFormXml)_scopedService;
        }

        public IEditLiteralListFormXml GetEditLiteralListFormXml(string xml, Type assignedTo)
        {
            _scopedService = new EditLiteralListFormXml
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                new EditXmlRichTextBoxPanel(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml,
                assignedTo
            );
            return (IEditLiteralListFormXml)_scopedService;
        }

        public IEditObjectListFormXml GetEditObjectListFormXml(string xml, Type assignedTo)
        {
            _scopedService = new EditObjectListFormXml
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IObjectListElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                new EditXmlRichTextBoxPanel(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml,
                assignedTo
            );
            return (IEditObjectListFormXml)_scopedService;
        }

        public IEditTableFunctionsFormXml GetEditTableFunctionsFormXml(string xml)
        {
            _scopedService = new EditTableFunctionsFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFunctionsDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionsElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                new EditXmlRichTextBoxPanel(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml
            );
            return (IEditTableFunctionsFormXml)_scopedService;
        }

        public IEditValueFunctionFormXml GetEditValueFunctionFormXml(string xml, Type assignedTo)
        {
            _scopedService = new EditValueFunctionFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                new EditXmlRichTextBoxPanel(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml,
                assignedTo
            );
            return (IEditValueFunctionFormXml)_scopedService;
        }

        public void Dispose()
        {
            _scopedService?.Dispose();
        }
    }
}
