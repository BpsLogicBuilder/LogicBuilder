using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal class FieldControlCommandFactory : IFieldControlCommandFactory
    {
        public AddUpdateConstructorGenericArgumentsCommand GetAddUpdateConstructorGenericArgumentsCommand(IConstructorGenericParametersControl constructorGenericParametersControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IConstructorDataParser>(),
                Program.ServiceProvider.GetRequiredService<IGenericConfigManager>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                constructorGenericParametersControl
            );

        public AddUpdateFunctionGenericArgumentsCommand GetAddUpdateFunctionGenericArgumentsCommand(IFunctionGenericParametersControl functionGenericParametersControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IGenericConfigManager>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                functionGenericParametersControl
            );

        public ClearRichInputBoxTextCommand GetClearRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => new
            (
                richInputBoxValueControl
            );

        public CopyRichInputBoxTextCommand GetCopyRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => new
            (
                richInputBoxValueControl
            );

        public CutRichInputBoxTextCommand GetCutRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => new
            (
                richInputBoxValueControl
            );

        public DeleteRichInputBoxTextCommand GetDeleteRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => new
            (
                richInputBoxValueControl
            );

        public EditObjectRichTextBoxConstructorCommand GetEditObjectRichTextBoxConstructorCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                objectRichTextBoxValueControl
            );

        public EditObjectRichTextBoxFunctionCommand GetEditObjectRichTextBoxFunctionCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => new
                (
                    Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                    Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                    Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                    objectRichTextBoxValueControl
                );

        public EditParameterObjectRichTextBoxLiteralListCommand GetEditParameterObjectRichTextBoxLiteralListCommand(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                parameterRichTextBoxValueControl
            );

        public EditParameterObjectRichTextBoxObjectListCommand GetEditParameterObjectRichTextBoxObjectListCommand(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                parameterRichTextBoxValueControl
            );

        public EditObjectRichTextBoxVariableCommand GetEditObjectRichTextBoxVariableCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                objectRichTextBoxValueControl
            );

        public EditRichInputBoxConstructorCommand GetEditRichInputBoxConstructorCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                richInputBoxValueControl
            );

        public EditRichInputBoxFunctionCommand GetEditRichInputBoxFunctionCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                richInputBoxValueControl
            );

        public EditRichInputBoxVariableCommand GetEditRichInputBoxVariableCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                richInputBoxValueControl
            );

        public EditVariableObjectRichTextBoxLiteralListCommand GetEditVariableObjectRichTextBoxLiteralListCommand(IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                variableRichTextBoxValueControl
            );

        public EditVariableObjectRichTextBoxObjectListCommand GetEditVariableObjectRichTextBoxObjectListCommand(IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                variableRichTextBoxValueControl
            );

        public PasteRichInputBoxTextCommand GetPasteRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => new
            (
                richInputBoxValueControl
            );

        public SelectDomainItemCommand GetSelectDomainItemCommand(IDomainRichInputBoxValueControl richInputBoxValueControl)
            => new
            (
                richInputBoxValueControl
            );

        public SelectItemFromPropertyListCommand GetSelectItemFromPropertyListCommand(IPropertyInputRichInputBoxControl propertyInputRichInputBoxControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IIntellisenseHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                propertyInputRichInputBoxControl
            );

        public SelectItemFromReferencesTreeViewCommand GetSelectItemFromReferencesTreeViewCommand(IPropertyInputRichInputBoxControl propertyInputRichInputBoxControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                propertyInputRichInputBoxControl
            );

        public ToCamelCaseRichInputBoxCommand GetToCamelCaseRichInputBoxCommand(IRichInputBoxValueControl richInputBoxValueControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IStringHelper>(),
                richInputBoxValueControl
            );
    }
}
