using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal class FieldControlHelperFactory : IFieldControlHelperFactory
    {
        public IConnectorTextRichInputBoxEventsHelper GetConnectorTextRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => new ConnectorTextRichInputBoxEventsHelper
            (
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                richInputBoxValueControl
            );

        public IEditLiteralConstructorHelper GetEditLiteralConstructorHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => new EditLiteralConstructorHelper
            (
                Program.ServiceProvider.GetRequiredService<IConstructorDataParser>(),
                Program.ServiceProvider.GetRequiredService<IConstructorTypeHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                richInputBoxValueControl
            );

        public IEditLiteralFunctionHelper GetEditLiteralFunctionHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => new EditLiteralFunctionHelper
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                richInputBoxValueControl
            );

        public IEditLiteralVariableHelper GetEditLiteralVariableHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => new EditLiteralVariableHelper
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IVariableDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                richInputBoxValueControl
            );

        public IEditObjectConstructorHelper GetEditObjectConstructorHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => new EditObjectConstructorHelper
            (
                Program.ServiceProvider.GetRequiredService<IConstructorDataParser>(),
                Program.ServiceProvider.GetRequiredService<IConstructorTypeHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                objectRichTextBoxValueControl
            );

        public IEditObjectFunctionHelper GetEditObjectFunctionHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => new EditObjectFunctionHelper
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                objectRichTextBoxValueControl
            );

        public IEditObjectVariableHelper GetEditObjectVariableHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => new EditObjectVariableHelper
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IVariableDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                objectRichTextBoxValueControl
            );

        public IEditParameterLiteralListHelper GetEditParameterLiteralListHelper(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
            => new EditParameterLiteralListHelper
            (
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                parameterRichTextBoxValueControl
            );

        public IEditParameterObjectListHelper GetEditParameterObjectListHelper(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
            => new EditParameterObjectListHelper
            (
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                parameterRichTextBoxValueControl
            );

        public IEditVariableLiteralListHelper GetEditVariableLiteralListHelper(IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
            => new EditVariableLiteralListHelper
            (
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                variableRichTextBoxValueControl
            );

        public IEditVariableObjectListHelper GetEditVariableObjectListHelper(IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
            => new EditVariableObjectListHelper
            (
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                variableRichTextBoxValueControl
            );

        public ILiteralListItemRichInputBoxEventsHelper GetLiteralListItemRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => new LiteralListItemRichInputBoxEventsHelper
            (
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                richInputBoxValueControl
            );

        public IParameterObjectRichTextBoxEventsHelper GetParameterObjectRichTextBoxEventsHelper(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
            => new ParameterObjectRichTextBoxEventsHelper
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                parameterRichTextBoxValueControl
            );

        public IParameterRichInputBoxEventsHelper GetParameterRichInputBoxEventsHelper(IParameterRichInputBoxValueControl richInputBoxValueControl)
            => new ParameterRichInputBoxEventsHelper
            (
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                richInputBoxValueControl
            );

        public IRichInputBoxEventsHelper GetRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl)
            => new RichInputBoxEventsHelper
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                richInputBoxValueControl
            );

        public IUpdateObjectRichTextBoxXml GetUpdateObjectRichTextBoxXml(IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
            => new UpdateObjectRichTextBoxXml
            (
                Program.ServiceProvider.GetRequiredService<IConstructorElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IObjectListElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IVariableElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                objectRichTextBoxValueControl
            );

        public IVariableObjectRichTextBoxEventsHelper GetVariableObjectRichTextBoxEventsHelper(IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
            => new VariableObjectRichTextBoxEventsHelper
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                variableRichTextBoxValueControl
            );

        public IVariableRichInputBoxEventsHelper GetVariableRichInputBoxEventsHelper(IVariableRichInputBoxValueControl variableRichInputBoxValueControl)
            => new VariableRichInputBoxEventsHelper
            (
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                variableRichInputBoxValueControl
            );
    }
}
