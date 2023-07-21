using ABIS.LogicBuilder.FlowBuilder.Components.Factories;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.Factories
{
    internal class ParameterFieldControlFactory : IParameterFieldControlFactory
    {
        public IConstructorGenericParametersControl GetConstructorGenericParametersControl(IEditConstructorControl editConstructorControl)
            => new ConstructorGenericParametersControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IConstructorDataParser>(),
                Program.ServiceProvider.GetRequiredService<IConstructorGenericsConfigrationValidator>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editConstructorControl
            );

        public IFunctionGenericParametersControl GetFunctionGenericParametersControl(IEditFunctionControl editFunctionControl)
            => new FunctionGenericParametersControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionGenericsConfigrationValidator>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editFunctionControl
            );

        public ILiteralListParameterRichTextBoxControl GetiteralListParameterRichTextBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter listOfLiteralsParameter, IDictionary<string, ParameterControlSet> editControlSet)
           => new LiteralListParameterRichTextBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListParameterElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<IObjectListDataParser>(),
                Program.ServiceProvider.GetRequiredService<IObjectListParameterElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                listOfLiteralsParameter,
                editControlSet
            );

        public ILiteralParameterDomainAutoCompleteControl GetLiteralParameterDomainAutoCompleteControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => new LiteralParameterDomainAutoCompleteControl
            (
                Program.ServiceProvider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalParameter
            );

        public ILiteralParameterDomainMultilineControl GetLiteralParameterDomainMultilineControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => new LiteralParameterDomainMultilineControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                editingControl,
                literalParameter
            );

        public ILiteralParameterDomainRichInputBoxControl GetLiteralParameterDomainRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => new LiteralParameterDomainRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                editingControl,
                literalParameter
            );

        public ILiteralParameterDropDownListControl GetLiteralParameterDropDownListControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => new LiteralParameterDropDownListControl
            (
                Program.ServiceProvider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalParameter
            );

        public ILiteralParameterMultilineControl GetLiteralParameterMultilineControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => new LiteralParameterMultilineControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                editingControl,
                literalParameter
            );

        public ILiteralParameterPropertyInputRichInputBoxControl GetLiteralParameterPropertyInputRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => new LiteralParameterPropertyInputRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                editingControl,
                literalParameter
            );

        public ILiteralParameterRichInputBoxControl GetLiteralParameterRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => new LiteralParameterRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                editingControl,
                literalParameter
            );

        public ILiteralParameterSourcedPropertyRichInputBoxControl GetLiteralParameterSourcedPropertyRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter, IDictionary<string, ParameterControlSet> editControlsSet)
            => new LiteralParameterSourcedPropertyRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                editingControl,
                literalParameter,
                editControlsSet
            );

        public ILiteralParameterTypeAutoCompleteControl GetLiteralParameterTypeAutoCompleteControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => new LiteralParameterTypeAutoCompleteControl
            (
                Program.ServiceProvider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalParameter
            );

        public IObjectListParameterRichTextBoxControl GetObjectListParameterRichTextBoxControl(IDataGraphEditingControl editingControl, ListOfObjectsParameter listOfObjectsParameter)
            => new ObjectListParameterRichTextBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListDataParser>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListParameterElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<IObjectListParameterElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                listOfObjectsParameter
            );

        public IObjectParameterRichTextBoxControl GetObjectParameterRichTextBoxControl(IDataGraphEditingControl editingControl, ObjectParameter objectParameter)
            => new ObjectParameterRichTextBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IComponentFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListDataParser>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListParameterElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<IObjectListDataParser>(),
                Program.ServiceProvider.GetRequiredService<IObjectListParameterElementInfoHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                objectParameter
            );
    }
}
