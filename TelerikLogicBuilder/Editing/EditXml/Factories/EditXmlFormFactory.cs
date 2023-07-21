using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories
{
    internal class EditXmlFormFactory : IEditXmlFormFactory
    {
        public IEditBooleanFunctionFormXml GetEditBooleanFunctionFormXml(string xml, Type assignedTo)
        {
            return new EditBooleanFunctionFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml,
                assignedTo
            );
        }

        public IEditBuildDecisionFormXml GetEditBuildDecisionFormXml(string xml)
        {
            return new EditBuildDecisionFormXml
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDecisionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml
            );
        }

        public IEditConditionsFormXml GetEditConditionsFormXml(string xml)
        {
            return new EditConditionsFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConditionsElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml
            );
        }

        public IEditConstructorFormXml GetEditConstructorFormXml(string xml, Type assignedTo)
        {
            return new EditConstructorFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConstructorElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml,
                assignedTo
            );
        }

        public IEditDecisionsFormXml GetEditDecisionsFormXml(string xml)
        {
            return new EditDecisionsFormXml
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDecisionsElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml
            );
        }

        public IEditDialogFunctionFormXml GetEditDialogFunctionFormXml(string xml)
        {
            return new EditDialogFunctionFormXml
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
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml
            );
        }

        public IEditFunctionsFormXml GetEditFunctionsFormXml(string xml)
        {
            return new EditFunctionsFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFunctionsDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionsElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml
            );
        }

        public IEditLiteralListFormXml GetEditLiteralListFormXml(string xml, Type assignedTo)
        {
            return new EditLiteralListFormXml
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml,
                assignedTo
            );
        }

        public IEditObjectListFormXml GetEditObjectListFormXml(string xml, Type assignedTo)
        {
            return new EditObjectListFormXml
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IObjectListElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml,
                assignedTo
            );
        }

        public IEditTableFunctionsFormXml GetEditTableFunctionsFormXml(string xml)
        {
            return new EditTableFunctionsFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFunctionsDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionsElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml
            );
        }

        public IEditValueFunctionFormXml GetEditValueFunctionFormXml(string xml, Type assignedTo)
        {
            return new EditValueFunctionFormXml
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditXmlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IUserControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                xml,
                assignedTo
            );
        }
    }
}
