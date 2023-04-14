﻿using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ParameterFieldControlFactoryServices
    {
        internal static IServiceCollection AddParameterFieldControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditConstructorControl, IConstructorGenericParametersControl>>
                (
                    provider =>
                    editigConstructorControl => new ConstructorGenericParametersControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IConstructorDataParser>(),
                        provider.GetRequiredService<IConstructorGenericsConfigrationValidator>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigConstructorControl
                    )
                )
                .AddTransient<Func<IEditFunctionControl, IFunctionGenericParametersControl>>
                (
                    provider =>
                    editigFunctionControl => new FunctionGenericParametersControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionGenericsConfigrationValidator>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigFunctionControl
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfLiteralsParameter, IDictionary<string, ParameterControlSet>, ILiteralListParameterRichTextBoxControl>>
                (
                    provider =>
                    (editigControl, listOfLiteralsParameter, editControlsSet) => new LiteralListParameterRichTextBoxControl
                    (
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListParameterElementInfoHelper>(),
                        provider.GetRequiredService<IObjectListDataParser>(),
                        provider.GetRequiredService<IObjectListParameterElementInfoHelper>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        listOfLiteralsParameter,
                        editControlsSet
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterDomainAutoCompleteControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterDomainAutoCompleteControl
                    (
                        provider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterDomainMultilineControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterDomainMultilineControl
                    (
                        provider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterDomainRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterDomainRichInputBoxControl
                    (
                        provider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterDropDownListControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterDropDownListControl
                    (
                        provider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        literalParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterMultilineControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterMultilineControl
                    (
                        provider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterPropertyInputRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterPropertyInputRichInputBoxControl
                    (
                        provider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterRichInputBoxControl
                    (
                        provider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl, 
                        literalParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralParameter, IDictionary<string, ParameterControlSet>, ILiteralParameterSourcedPropertyRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalParameter, editControlsSet) => new LiteralParameterSourcedPropertyRichInputBoxControl
                    (
                        provider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl,
                        literalParameter,
                        editControlsSet
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterTypeAutoCompleteControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterTypeAutoCompleteControl
                    (
                        provider.GetRequiredService<ICreateLiteralParameterXmlElement>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl, 
                        literalParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ListOfObjectsParameter, IObjectListParameterRichTextBoxControl>>
                (
                    provider =>
                    (editigControl, listOfObjectsParameter) => new ObjectListParameterRichTextBoxControl
                    (
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListDataParser>(),
                        provider.GetRequiredService<ILiteralListParameterElementInfoHelper>(),
                        provider.GetRequiredService<IObjectListParameterElementInfoHelper>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        listOfObjectsParameter
                    )
                )
                .AddTransient<Func<IDataGraphEditingControl, ObjectParameter, IObjectParameterRichTextBoxControl>>
                (
                    provider =>
                    (editigControl, objectParameter) => new ObjectParameterRichTextBoxControl
                    (
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<ILiteralListDataParser>(),
                        provider.GetRequiredService<ILiteralListParameterElementInfoHelper>(),
                        provider.GetRequiredService<IObjectListDataParser>(),
                        provider.GetRequiredService<IObjectListParameterElementInfoHelper>(),
                        provider.GetRequiredService<ObjectRichTextBox>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editigControl,
                        objectParameter
                    )
                )
                .AddTransient<IParameterFieldControlFactory, ParameterFieldControlFactory>();
        }
    }
}
